using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class PlayerMove1 : MonoBehaviour
{
    private bool lastIsGrounded = false;
    public CharacterController characterController;
    private float moveY;
    private Vector3 MoveDir;
    private Vector3 realMoveDir;
    [SerializeField]
    private Animator animator;
    public Vector3 addForce = Vector3.zero;
    [SerializeField]
    private float resistance;
    private Vector3 hitPointNormal;

    [Header("PlayerStat")]

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpPow;
    [SerializeField]
    private float RotateDemp;
    [SerializeField]
    private float MoveDemp;
    [SerializeField]
    private float gravity;

    [SerializeField]
    private float slopeSpeed = 8f;

    private bool willSlideOnSlope = true;
    private Vector3 downForce;

    public void MoveTransorm(Vector3 _pos, bool heal){
        characterController.enabled = false;
        transform.position = _pos;
        characterController.enabled = true;
    }
    private bool IsSliding{
        get{
            Debug.DrawRay(transform.position, Vector3.down*2f, Color.blue);
            if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f)){
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up)>characterController.slopeLimit;
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



    public void addForceGo(Vector3 velocity){
        moveY = velocity.y;
        realMoveDir.y = velocity.y;
        addForce.x = velocity.x;
        addForce.z = velocity.z;
    }
    public void SetVelocity(Vector3 velocity){
        addForce.x = velocity.x;
        addForce.z = velocity.z;
    }
    
    private void Start(){


        MoveDir = Vector3.zero;
        moveY = 0f;


    }
    private void Move(){
        moveY = realMoveDir.y;

        Transform CameraTransform = Camera.main.transform;
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        if(((!characterController.isGrounded))){
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");
            MoveDir = horizontal * right + vertical * forward;
        }else{
            MoveDir = Vector3.zero;
        }

        MoveDir.y = 0f;
        MoveDir.Normalize();
        
        
        
        MoveDir *= speed;
        realMoveDir = Vector3.Lerp(realMoveDir, MoveDir, MoveDemp*Time.deltaTime);
        realMoveDir.y = moveY;
        animator.SetFloat("VelocityY", moveY);
        animator.SetBool("IsGround", characterController.isGrounded);
        if(characterController.isGrounded){
            if(!lastIsGrounded&&realMoveDir.y<-0.1f)realMoveDir.y = -0.1f;
            lastIsGrounded = true;
            if(Input.GetButton("Jump")&&!IsSliding){
                realMoveDir.y = jumpPow;
                animator.SetTrigger("Jump");
            }
        }
        else{
            lastIsGrounded = false;
            realMoveDir.y -= gravity * Time.deltaTime;
        }
        addForce.y = 0f;
        


        if(MoveDir != Vector3.zero){
            animator.SetBool("Runing", true);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(MoveDir), RotateDemp * Time.deltaTime);
        }
        else{
            animator.SetBool("Runing", false);
        }
        
        downForce = Vector3.zero;
        if(willSlideOnSlope && IsSliding && characterController.isGrounded){
            realMoveDir += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }else if(realMoveDir.y < 0){
            downForce = -DowmDowm * Vector3.up;
        }
        
        characterController.Move((realMoveDir + addForce + downForce) * Time.deltaTime);
    }



    private void Update() {
        Move();
    }
    private void FixedUpdate() {
        if(addForce.x!=0){
            addForce.x += resistance * ((addForce.x>0)?-1f:1f);
            if(Mathf.Abs(addForce.x)<3)addForce.x=0;
        }
        if(addForce.z!=0){
            addForce.z += resistance * ((addForce.z>0)?-1f:1f);
            if(Mathf.Abs(addForce.z)<3)addForce.z=0;
        }
        addForce.y = moveY;
        

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

    
}