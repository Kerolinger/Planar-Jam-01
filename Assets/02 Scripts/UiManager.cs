using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject m_cameraUI;
    [SerializeField] private GameObject m_cameraSnapFX;
    [SerializeField] private Image m_cameraRing;
    [SerializeField] private Slider m_zoomSlider;
    [SerializeField] private Image SquareFill;
    [SerializeField] private TextMeshProUGUI zoomText;
    [SerializeField] private TextMeshProUGUI distanceText;

    [SerializeField] private float maxCameraZoomValue;
    [SerializeField] private float minCameraZoomValue;
    [SerializeField] private float cameraZoomSpeed;

    private Animator squareFilledAnimator;

    public GameObject CameraUI { get => m_cameraUI; set => m_cameraUI = value; }

    private const string squareFilledBool = "isSquareFIlled";


    private void Awake()
    {
        squareFilledAnimator = SquareFill.GetComponent<Animator>();
    }
    public void ToggleFirstPersonUI( )
    {
        m_cameraUI.SetActive(!m_cameraUI.activeSelf);
    }

    public void CameraSnapFX()
    {
        m_cameraSnapFX.SetActive(true);
    }

    public void ChangeDetectorColor(float distancePercentage)
    {
        if (distancePercentage == -1)
        {
            distanceText.text = "NO OBJECT FOUND";
            distanceText.color = Color.red;
            return;
        }

        bool isInDistance = 0.8f >= distancePercentage ? true : false ;
        //m_cameraRing.color = isCorrect ? Color.green : Color.white ;
        //Debug.Log(isCorrect ? "orb is in screen" : "orb is not visible");

        float newDistanceDisplay = distancePercentage - 0.75f;
        squareFilledAnimator.SetBool(squareFilledBool, isInDistance);
        distanceText.text = string.Format("{0:0.00}", 0 > newDistanceDisplay ? 0 : newDistanceDisplay);
        distanceText.color = isInDistance ? Color.white : Color.cyan;
    }

    public void ChangeDistance(float newDistance)
    {

    }

    public void ZoomCircle(float yInput)
    {
        float newZoomValue = m_cameraRing.rectTransform.localScale.x + (-yInput * cameraZoomSpeed);
        Debug.Log("zooming! new zoom value:" + newZoomValue);
        m_cameraRing.rectTransform.localScale = new Vector3(newZoomValue, newZoomValue, newZoomValue);

        if (m_cameraRing.rectTransform.localScale.x < minCameraZoomValue)
        {
            m_cameraRing.rectTransform.localScale = new Vector3(minCameraZoomValue, minCameraZoomValue, minCameraZoomValue);
        }
        else if (maxCameraZoomValue < m_cameraRing.rectTransform.localScale.x)
            m_cameraRing.rectTransform.localScale = new Vector3(maxCameraZoomValue, maxCameraZoomValue, maxCameraZoomValue);

        if(newZoomValue == minCameraZoomValue)
        {
            m_zoomSlider.value = 0;
            return;
        }

        m_zoomSlider.value = (newZoomValue - minCameraZoomValue) / (maxCameraZoomValue - minCameraZoomValue);
        zoomText.text = string.Format("{0:0.00}", m_zoomSlider.value);

        //m_cameraRing.rectTransform.localScale = new Vector3(Mathf.Clamp(minCameraZoomValue, maxCameraZoomValue, newZoomValue), Mathf.Clamp(minCameraZoomValue, maxCameraZoomValue, newZoomValue), Mathf.Clamp(minCameraZoomValue, maxCameraZoomValue, newZoomValue));
        //Debug.Log("adjusting local zoom! new zoom value:" + m_cameraRing.rectTransform.localScale.x);
    }
}
