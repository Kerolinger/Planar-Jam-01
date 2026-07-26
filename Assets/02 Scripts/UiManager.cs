using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject m_cameraUI;
    [SerializeField] private GameObject m_cameraSnapFX;
    [SerializeField] private Image m_cameraRing;

    [SerializeField] private float maxCameraZoomValue;
    [SerializeField] private float minCameraZoomValue;

    public GameObject CameraUI { get => m_cameraUI; set => m_cameraUI = value; }

    public void ToggleFirstPersonUI( )
    {
        m_cameraUI.SetActive(!m_cameraUI.activeSelf);
    }

    public void CameraSnapFX()
    {
        m_cameraSnapFX.SetActive(true);
    }

    public void ChangeDetectorColor(bool isCorrect)
    {
        CameraRing.color = isCorrect ? Color.green : Color.white ;
        Debug.Log(isCorrect ? "orb is in screen" : "orb is not visible");
    }

    public void ZoomCircle(float yInput)
    {

    }
}
