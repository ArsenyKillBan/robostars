using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerControler : MonoBehaviourPunCallbacks
{
    [SerializeField] float rotateSpeed;
    PlayerInputs inputActions;
    CharacterController controller;
    Animator animator;
    Vector2 movementInput;
    Vector3 currentMovement;
    Quaternion rotateDir;
    bool isRun, isWalk;

    PhotonView pv;

    [SerializeField] CameraFovle myCameraScript;

    void OnMovementActions(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        currentMovement.x = movementInput.x;
        currentMovement.z = movementInput.y;
        isWalk = movementInput.x != 0 || movementInput.y != 0;
    }

    private void Awake()
    {
        pv = GetComponentInParent<PhotonView>();

        if (!pv.IsMine)
        {
            Destroy(myCameraScript.gameObject);
        }

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputs();

        inputActions.CharacterControlls.Movment.started += OnMovementActions;
        inputActions.CharacterControlls.Movment.performed += OnMovementActions;
        inputActions.CharacterControlls.Movment.canceled += OnMovementActions;

        inputActions.CharacterControlls.Movment.started += OnCameraMovment;
        inputActions.CharacterControlls.Movment.performed += OnCameraMovment;
        inputActions.CharacterControlls.Movment.canceled += OnCameraMovment;

        inputActions.CharacterControlls.run.started += OnRun;
        inputActions.CharacterControlls.run.canceled += OnRun;

        
    }

    public override void OnEnable()
    {
        inputActions.CharacterControlls.Enable();

    }

    public override void OnDisable()
    {
        inputActions.CharacterControlls.Disable();
    }

    void PlayerRotate()
    {
        if (isWalk)
        {
             rotateDir = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(currentMovement),
            rotateSpeed * Time.deltaTime);
            transform.rotation = rotateDir;
        }
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRun = context.ReadValueAsButton();
    }
    void AnimationControl()
    {
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isRun", isRun);

    }

    private void Update()
    {
        if (!pv.IsMine) return;
        AnimationControl();
        PlayerRotate();
    }
    private void FixedUpdate()
    {
        if (!pv.IsMine) return;
        controller.Move(currentMovement * Time.fixedDeltaTime);
    }

    void OnCameraMovment(InputAction.CallbackContext context)
    {
        myCameraScript.SetOffset(currentMovement);
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = Vector3.up;
        controller.enabled = true;
    }
}

