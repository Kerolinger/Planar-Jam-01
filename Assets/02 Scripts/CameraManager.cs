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
    [SerializeField] private Animator cameraAnimator;

    [SerializeField] private GameObject firstPersonVolume;
    [SerializeField] private GameObject thirdPersonVolume;

    [Header("Debugging")]
    private Vector3 cameraFollowVelocity = Vector3.zero;

    private float lookAngle; //Camera look up and down
    private float pivotAngle; //Camera look left and right
    public cameraMode currentCameraMode = cameraMode.ThirdPerson;

    public cameraMode CurrentCameraMode { get => currentCameraMode; set => currentCameraMode = value; }

    public enum cameraMode {FirstPerson,ThirdPerson};

    private const string isMovingBool = "cameraMoving";
    private OrbDetector m_orbDetector;

    private void Start()
    {
        Cursor.visible = false;
        m_orbDetector = GetComponent<OrbDetector>();

        //setting up third person mode as default
        CurrentCameraMode = cameraMode.ThirdPerson;
        cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 1, -10f);
        thirdPersonVolume.SetActive(true);
        firstPersonVolume.SetActive(false);
        cameraAnimator.SetBool(isMovingBool, false);
        m_orbDetector.IsDetecting = false;
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
            thirdPersonVolume.SetActive(true);
            firstPersonVolume.SetActive(false);
            cameraAnimator.SetBool(isMovingBool, false);
            m_orbDetector.IsDetecting = false;
            AudioManager.instance.Play("SFX_cameraUnequipped");
        }
        else
        {
            CurrentCameraMode = cameraMode.FirstPerson;
            cameraTransform.transform.localPosition = new Vector3(cameraTransform.transform.localPosition.x, 0, -4f);
            thirdPersonVolume.SetActive(false);
            firstPersonVolume.SetActive(true);
            cameraAnimator.SetBool(isMovingBool, true);
            m_orbDetector.IsDetecting = true;
            AudioManager.instance.Play("SFX_cameraEquipped");
        }

    }
}
