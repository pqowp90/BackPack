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
    private Vector3 velocity;
    private PlayerAnimation playerAnimation;
    public bool isGrounded;
    public bool isRunning;

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
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseX;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        myCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, -xRotation, 0f);
        // 캐릭터의 상하 회전을 마우스 입력에 따라 조절합니다.
    }

    private void PositiveMove()
    {
        
        float moveSpeed = speed;
        if(isRunning = Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed *= 1.5f;
        }
        playerAnimation.isRunning = isRunning;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.Normalize();
        // 캐릭터의 이동 방향을 계산합니다.
        float yVelocity = velocity.y;
        float airControll = moveLerp;
        if(!controller.isGrounded)
        {
            airControll *= 0.3f;
        }
        else{

        }
        velocity.y = 0f;
        velocity = Vector3.Lerp(velocity, move * moveSpeed, Time.deltaTime * airControll);
        velocity.y = yVelocity;
        // 중력을 적용합니다.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        isGrounded = controller.isGrounded;
        if (controller.isGrounded)
        {
            // 캐릭터가 땅에 있을 때만 점프 가능하도록 처리합니다.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                // 점프 높이에 따라 점프 속도를 계산합니다.
            }
            else
            {
                // 캐릭터가 땅에 닿아 있을 때만 y 속도를 초기화합니다.
                velocity.y = -2f;
            }
        }
    }

    private void Update()
    {
        RotateMove();
        PositiveMove();
        
    }
}