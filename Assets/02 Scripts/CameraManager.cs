using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header ("Customizable")]
    [SerializeField] private float cameraFollowSpeed = 0.2f;
    [SerializeField] private float cameraLookSpeed = 0.2f;
    [SerializeField] private float cameraPivotSpeed = 0.2f;
    [SerializeField] private float minimumPivotAngle = -45f;
    [SerializeField] private float maximumPivotAngle = 45;
    [SerializeField] private float thirdPersonDistance = 10;
    [SerializeField] private float firstPersonDistance = -1;

    [Header("References")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform cameraPivot;
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
        cameraPivot.transform.position = new Vector3(cameraPivot.transform.position.x, cameraPivot.transform.position.y, thirdPersonDistance);
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
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

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
        if (currentCameraMode == cameraMode.FirstPerson)
        {
            currentCameraMode = cameraMode.ThirdPerson;
            cameraPivot.transform.position = new Vector3(cameraPivot.transform.position.x, cameraPivot.transform.position.y, thirdPersonDistance);
        }
        else
        {
            currentCameraMode = cameraMode.FirstPerson;
            cameraPivot.transform.position = new Vector3(cameraPivot.transform.position.x, cameraPivot.transform.position.y, firstPersonDistance);
        }

    }
}
