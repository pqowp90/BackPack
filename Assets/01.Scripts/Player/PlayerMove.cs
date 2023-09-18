using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 2.5f;
    [SerializeField]
    private float moveLerp = 5f;
    [SerializeField]
    private float gravity = -9.81f;  // 중력 가속도
    [SerializeField]
    private float jumpHeight = 1f;
    [SerializeField]
    private Camera myCamera;
    [SerializeField]
    private float sensitivity;
    private CharacterController controller;
    private float xRotation = 0f;
    private float yRotation = 0f;
    [SerializeField]
    private Vector3 curVelocity;
    public Vector3 addForce = Vector3.zero;
    private PlayerAnimation playerAnimation;
    public bool isGrounded{get{return controller.isGrounded;}private set{}}
    public bool isRunning;
    private Vector3 hitPointNormal;
    [SerializeField]
    private float resistance;
    [SerializeField]
    private float slopeSpeed = 1.1f;

    private bool willSlideOnSlope = true;
    private Vector3 downForce;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        // 컴포넌트를 초기화합니다.
        Cursor.lockState = CursorLockMode.Locked;
        // 마우스 커서를 잠금 상태로 설정하여 화면을 클릭해도 마우스가 움직이지 않도록 합니다.
    }

    private void RotateMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * 2f;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * 2f;

        xRotation -= mouseX;
        yRotation -= mouseY;

        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        myCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, -xRotation, 0f);
        // 캐릭터의 상하 회전을 마우스 입력에 따라 조절합니다.
    }

    public void AddForce(Vector3 velocity){
        addForce.x += velocity.x;
        addForce.z += velocity.z;
        curVelocity.y += velocity.y;
    }

    private bool IsSliding{
        get{
            Debug.DrawRay(transform.position, Vector3.down*2f, Color.blue);
            if(controller.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f)){
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up)>controller.slopeLimit;
            }else{
                return false;
            }
        }
    }
    private float DowmDowm{
        get{
            Debug.DrawRay(transform.position, Vector3.down*2f, Color.blue);
            if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 0.2f)){
                return Vector3.Angle(slopeHit.normal, Vector3.down)/9f;
            }else{
                return 0f;
            }
        }
    }

    private void PositiveMove()
    {


        float moveSpeed = speed;
        if(isRunning = Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed *= 1.5f;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        playerAnimation.isRunning = isRunning && (Mathf.Abs(moveX) + Mathf.Abs(moveZ) > 0);

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.Normalize();
        playerAnimation.moveDir = (Vector3.right * moveX + Vector3.forward * moveZ).normalized;
        // 캐릭터의 이동 방향을 계산합니다.
        float yVelocity = curVelocity.y;
        float airControll = moveLerp;
        if(!controller.isGrounded)
        {
            airControll *= 0.3f;
        }
        else{

        }
        curVelocity.y = 0f;
        curVelocity = Vector3.Lerp(curVelocity, move * moveSpeed, Time.deltaTime * airControll);
        curVelocity.y = yVelocity;
        // 중력을 적용합니다.
        curVelocity.y += gravity * Time.deltaTime;

        downForce = Vector3.zero;
        if(willSlideOnSlope && IsSliding && controller.isGrounded){
            curVelocity += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }else if(curVelocity.y < 0){
            downForce = -DowmDowm * Vector3.up;
        }

        controller.Move((curVelocity + addForce + downForce) * Time.deltaTime);
        if (controller.isGrounded)
        {
            // 캐릭터가 땅에 있을 때만 점프 가능하도록 처리합니다.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                // 점프 높이에 따라 점프 속도를 계산합니다.
                playerAnimation.Jump();
            }
            else
            {
                // 캐릭터가 땅에 닿아 있을 때만 y 속도를 초기화합니다.
                curVelocity.y = -2f;
            }
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 충돌된 물체의 릿지드 바디를 가져옴
        Rigidbody body = hit.collider.attachedRigidbody;

        // 만약에 충돌된 물체에 콜라이더가 없거나, isKinematic이 켜저있으면 리턴
        if (body == null || body.isKinematic) return;

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        // pushDir이라는 벡터값에 새로운 백터값 저장. 부딪힌 물체의 x의 방향과 y의 방향을 저장
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        // 부딪힌 물체의 릿지드바디의 velocity에 위에 저장한 백터 값과 힘을 곱해줌
        body.velocity = pushDir * 4f;
    }

    private void Update()
    {
        RotateMove();
        PositiveMove();
        if(Input.GetKeyDown(KeyCode.G))
        {
            AddForce(new Vector3(0f, 10f, 10f));
        }
    }
    private void FixedUpdate() {
        if(addForce.x!=0){
            addForce.x += resistance * ((addForce.x>0)?-1f:1f);
            if(Mathf.Abs(addForce.x)<0.5)addForce.x=0;
        }
        if(addForce.z!=0){
            addForce.z += resistance * ((addForce.z>0)?-1f:1f);
            if(Mathf.Abs(addForce.z)<0.5)addForce.z=0;
        }
        

    }
    
}