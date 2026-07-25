using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject m_cameraUI;
    [SerializeField] private GameObject m_cameraSnapFX;

    public void ToggleFirstPersonUI()
    {
        m_cameraUI.SetActive(!m_cameraUI.activeSelf);
    }

    public void CameraSnapFX()
    {
        m_cameraSnapFX.SetActive(true);
    }
}
