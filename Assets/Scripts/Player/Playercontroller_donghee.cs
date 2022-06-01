using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller_donghee : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed; // 움직임 속도
    [SerializeField]
    private float runSpeed; // 달리기 속도
    [SerializeField]
    private float crouchSpeed; // 앉기 속도


    private float applySpeed; // 함수 하나를 사용하여 달리기속도와 움직임 속도를 제어하기 위해

    [SerializeField]
    private float jumpForce; // 점프의 힘

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false; // 뛰고 있는지 안 뛰고 있는지 확인 처음 기본 값은 false
    private bool isCrouch = false; // 앉았는지 서있는지 여부 기본 값은 false
    private bool isGround = true; // 땅에 있는지 아닌지 기본 값은 true로 (공중에서 또다시 점프하지 못하게)


    // 움직임 체크 변수
    private Vector3 lastPos;



    // 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY; // 앉기 높이
    private float originPosY; // 원래 높이 
    private float applyCrouchPosY; // 앉기 높이와 원래 높이 각각의 값을 더해줄 때 사용
    
    // 땅에 닿았는지 여부
    private CapsuleCollider capsuleCollider;

    [SerializeField]
    private float lookSensitivity; // 카메라 민감도

    [SerializeField]
    private float cameraRotationLimit; // 카메라 회전 제한
    private float currentCameraRotationX = 0; // 카메라가 정면을 보게 0

    [SerializeField]
    private Camera theCamera; // 카메라 컴포넌트를 불러옴
    private Rigidbody myRigid; // 플레이어의 물리적인 몸
    private GunController thegunController;
    private CrossHair theCrosshair;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>(); // CapsuleCollider를 사용해서 플레이어의 CapsuleCollider를 통제
        myRigid = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 값을 myRigid에 넣는다.
        theCrosshair = FindObjectOfType<CrossHair>();

        // 초기화
        applySpeed = walkSpeed; // applySpeed에 기본 값으로 walkSpeed를 넣어줌
        originPosY = theCamera.transform.localPosition.y; // 앉기 위해 카메라를 내려 시점을 밑으로 내림
        applyCrouchPosY = originPosY; // applyCrouchPosY는 기본 서있는 상태
    }

    // Update is called once per frame
    void Update()
    {
        IsGround(); // 땅에 닿아있는지
        TryJump(); // 점프
        TryRun(); // 반드시 Move() 위에 있어야함 뛸지 걸을지 위에서 판단하고 움직임을 제어하기 위해
        TryCrouch(); // 앉기
        Move(); // 캐릭터 방향키 움직임
        MoveCheck(); // 움직임 체크
        CameraRotation(); // 상하 카메라 움직임
        CharacterRotation(); // 좌우 캐릭터 움직임
    }

    /*
    private void FixedUpdate()
    {
        MoveCheck(); // 움직임 체크
    }
    */

    private void TryCrouch() // 앉기 시도
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGround) // LeftControl을 누르면 실행, 땅에 닿지 않으면 앉기 실행 불가능
        {
            Crouch(); // 앉기 실행
        }
    }

    private void Crouch() // 앉기 동작
    {
        isCrouch = !isCrouch; // isCrouch가 true면 false로, false면 true로 바꿔준다.

        if (isCrouch) // isCrouch가 ture일 경우 실행
        {
            applySpeed = crouchSpeed; // applySpeed를 crouchSpeed로 전환
            applyCrouchPosY = crouchPosY; // applyCrouchPosY에 앉은 상태의 위치를 넣어준다.
        }
        
        
        else // isCrouch가 false일 경우 실행
        {
            applySpeed = walkSpeed; //applySpeed를 walkSpeed로 전환
            applyCrouchPosY = originPosY; // applyCrouchPosY에 원래 서있는 상태의 위치를 넣어준다.
        }
        

        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
    }

    IEnumerator CrouchCoroutine() // 부드러운 앉기 // 병렬처리를 위해서 Coroutine을 사용 (왔다갔다 빠르게 처리)
    {

        float _posY = theCamera.transform.localPosition.y; // 카메라의 로컬 포지션을 _posY에 대입
        int count = 0; // count변수 선언

        while(_posY != applyCrouchPosY) // _posY가 원하는 값이 아닐때만 실행
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.01f); // 보관함수 Lerp를 사용, _posY에서 applyCrouchPosY값까지 0.01f의 비율로 증가 (천천히 스무스하게 앉았다 일어나기 위해서)
            theCamera.transform.localPosition = new Vector3(0, _posY, 0); // 로컬포지션 y에 _posY를 넣어줌
            if (count > 15) // count가 15가 넘을 경우 반복문을 빠져나오게 한다.
                break;
            yield return null; // 1프레임 대기

        }

        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f); // 로컬 포지션 y에 applyCrouchPosY를 넣는다
    }

    private void IsGround() // 지면 체크
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.5f); // 레이저를 쏴서 밑으로 쏴서 땅에 착지 했는지 여부를 확인함, +0.8f를 한 이유는 여유를 주기 위해서(석상위에 올라가거나 했을때)
    }

    private void TryJump() // 점프 시도
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround) // isGround가 true일 경우(땅에 닿아있을 경우) space를 누르면 실행.
        {
            Jump(); // 점프
        }
    }

    private void Jump() // 점프
    {
        if (isCrouch) // 앉은 상태에서 점프시 앉은 상태 해제
            Crouch();

        myRigid.velocity = transform.up * jumpForce; // velocity를 바꿔서 순간적으로 공중으로 뛰게한다.
    }

    private void TryRun() // 달리기 시도
    {
        if(Input.GetKey(KeyCode.LeftShift)) // LeftShift를 누를시 실행
        {
            Running(); // 달리기
        }

        if(Input.GetKeyUp(KeyCode.LeftShift)) // LeftShift를 뗄 경우 실행
        {
            RunningCancle(); // 달리기 취소
        }
    }

    private void Running() // 달리기
    {
        if (isCrouch) // 앉은 상태에서 뛸 시 앉은 상태 해제
            Crouch();

        isRun = true; // isRun을 true로
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed; // 걷기속도에서 달리기 속도로 바뀌면서 뛰게 된다.

    }

    private void RunningCancle() // 달리기 취소
    {
        isRun = false; // isRun을 false로
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed; // applySpeed에 walkSpeed 값을 넣어줌으로써 다시 걷게함
    }

    private void Move() // 캐릭터 방향키 움직임
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // a,d 나 좌우 방향키를 누르면 오른쪽 1, 왼쪽 -1이 리턴되며 x에 들어간다
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // 앞 뒤 빼고 위와 같음
         
        Vector3 _moveHorizontal = transform.right * _moveDirX; // transform이 갖고 있는 위치값에 오른쪽을 쓴다.
        Vector3 _moveVertical = transform.forward * _moveDirZ; // 앞 뒤 빼고 위와 같음

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // 삼각함수, 더하면 어차피 방향이 같기 때문에 normalized를 해줌으로써 합을 1로 만들어 정규화 시켜준다.

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); // 1초동안 _velocity만큼 움직이게 함, Time.deltaTime(0.016)을 곱해줌으로써 순간이동 하는 느낌이 없도록 한다.

    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f) // 경사 같은 곳도 체크하기 위해
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
       
    }

    private void CameraRotation() // 상하 마우스 컨트롤 (카메라 회전) 
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // _xRotation으로 Mouse Y 값을 받아옴 (마우스가 상하로 움직이는 경우)
        float _cameraRotationX = _xRotation * lookSensitivity; // 민감도에 따라서 천천히 움직이게 함
        currentCameraRotationX -= _cameraRotationX; // 현재 자신에 _cameraRotationX 만큼 나온 값을 빼준다 (빼주는 이유는 상하 반전 때문에)
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); // 제한인 45도 만큼 위아래가 넘어가지 않도록

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); // localEulerAngles은 Transform에 Rotation xyz임
    }

    private void CharacterRotation() // 좌우 마우스 컨트롤 (캐릭터 회전)
    {
        float _yRotation = Input.GetAxisRaw("Mouse X"); // _yRotation으로 Mouse X 값을 받아옴 (마우스가 좌우로 움직이는 경우)
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity; // 위아래와 좌우 민감도를 똑같이 해준다
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // myRigid.rotation 값과 오일러로 변환시킨 Quaternion 값 _characterRotationY을 곱해주면 플레이어가 회전시킬 수 있다.
    }
}
