using UnityEngine;
using System.Collections.Generic;
using System;

public class InputManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private OrbDetector orbDetector;

    [Header("Customizable")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Debugging")]
    public Vector2 movementInput;
    public Vector2 cameraInput;
    public Vector2 zoomInput;

    public float cam_verticalInput;
    public float cam_horizontalInput;

    private PlayerControls playerControls;
    private float verticalInput;
    private float horizontalInput;

    private Vector3 moveDirection;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.CameraZoom.performed += i => zoomInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.CameraSnap.performed += OnCameraSnap;
            playerControls.PlayerMovement.CameraSnapTaken.performed += OnCameraSnapTaken;
        }

        playerControls.Enable();

        AudioManager.instance.Play("Forest_ambience");
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleInputs()
    {
        //HandleMovementInput
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cam_verticalInput = cameraInput.y;
        cam_horizontalInput = cameraInput.x;
    }

    private void HandleMovement()
    {
        moveDirection = cameraTransform.forward * verticalInput;
        moveDirection = moveDirection + cameraTransform.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed * Time.deltaTime;

        //playerRigidbody.linearVelocity = moveDirection;
        transform.position += moveDirection;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraTransform.forward * verticalInput;
        targetDirection = targetDirection + cameraTransform.right * horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void LateUpdate()
    {
        cameraManager.FollowTarget();
        cameraManager.RotateCamera();
    }

    private void OnCameraSnap(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        cameraManager.SwitchCameraMode();
    }

    private void OnCameraSnapTaken(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (cameraManager.CurrentCameraMode == CameraManager.cameraMode.FirstPerson )
        {
            if(uiManager.OrbIsInDistance)
            {
                uiManager.CameraSnapFX();
                AudioManager.instance.Play("SFX_cameraSnap");
                orbDetector.TryActivateOrb();
            }
            else
            {
                Debug.Log("Orb is not visible!");
            }

        }
    }

    private void HandleZoom()
    {
        if (cameraManager.CurrentCameraMode != CameraManager.cameraMode.FirstPerson)
            return;

        uiManager.ZoomCircle(zoomInput.y);
    }

}
