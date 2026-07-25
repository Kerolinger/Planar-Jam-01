using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header ("Customizable")]
    [SerializeField] private float cameraFollowSpeed = 0.2f;
    [SerializeField] private float cameraLookSpeed = 0.2f;
    [SerializeField] private float cameraPivotSpeed = 0.2f;
    [SerializeField] private float minimumPivotAngle_third = -45f;
    [SerializeField] private float maximumPivotAngle_third = 45;
    [SerializeField] private float minimumPivotAngle_first = -45f;
    [SerializeField] private float maximumPivotAngle_first = 45;

    [Header("References")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UiManager uiManager;

    private Vector3 cameraFollowVelocity = Vector3.zero;

    [Header("Debugging")]
    private float lookAngle; //Camera look up and down
    private float pivotAngle; //Camera look left and right
    public cameraMode currentCameraMode = cameraMode.ThirdPerson;

    public cameraMode CurrentCameraMode { get => currentCameraMode; set => currentCameraMode = value; }

    public enum cameraMode {FirstPerson,ThirdPerson};

    private void Start()
    {
        Cursor.visible = false;
        cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 1, -10f);
    }
    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    public void RotateCamera()
    {
        lookAngle = lookAngle + (inputManager.cam_horizontalInput * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cam_verticalInput * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, CurrentCameraMode == cameraMode.FirstPerson ? minimumPivotAngle_first : minimumPivotAngle_third,
                                             CurrentCameraMode == cameraMode.FirstPerson ? maximumPivotAngle_first : maximumPivotAngle_third);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
    public void SwitchCameraMode()
    {
        uiManager.ToggleFirstPersonUI();

        if (CurrentCameraMode == cameraMode.FirstPerson)
        {
            CurrentCameraMode = cameraMode.ThirdPerson;
            cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 1, -10f);
        }
        else
        {
            CurrentCameraMode = cameraMode.FirstPerson;
            cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 0, -4f);      
        }

    }
}
