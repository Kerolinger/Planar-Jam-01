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
    [SerializeField] private Transform cameraPivot_third;
    [SerializeField] private Transform cameraPivot_first;
    [SerializeField] private InputManager inputManager;

    private Vector3 cameraFollowVelocity = Vector3.zero;

    [Header("Debugging")]
    private float lookAngle; //Camera look up and down
    private float pivotAngle; //Camera look left and right
    private cameraMode currentCameraMode = cameraMode.ThirdPerson;

    private enum cameraMode {FirstPerson,ThirdPerson};

    private void Start()
    {
        Cursor.visible = false;

        cameraTransform.parent = cameraPivot_third;
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
        pivotAngle = Mathf.Clamp(pivotAngle, currentCameraMode == cameraMode.FirstPerson ? minimumPivotAngle_first : minimumPivotAngle_third,
                                             currentCameraMode == cameraMode.FirstPerson ? maximumPivotAngle_first : maximumPivotAngle_third);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot_third.localRotation = targetRotation;
        cameraPivot_first.localRotation = targetRotation;
    }
    public void SwitchCameraMode()
    {
        if (currentCameraMode == cameraMode.FirstPerson)
        {
            currentCameraMode = cameraMode.ThirdPerson;
            cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 1, -10f);
        }
        else
        {
            currentCameraMode = cameraMode.FirstPerson;
            cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 0, -4f);
        }

    }
}
