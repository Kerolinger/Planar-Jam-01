using System.Collections.Generic;
using UnityEngine;

public class OrbDetector : MonoBehaviour
{
    [SerializeField] private GameObject orbContainer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private UiManager uiManager;

    private CameraManager m_cameraManager;
    private List<OrbBehavior> orbBehaviors;

    private bool isDetecting;

    private Vector3 viewPointPosition;

    public bool IsDetecting { get => isDetecting; set => isDetecting = value; }

    void Awake()
    {
        m_cameraManager = GetComponent<CameraManager>();
        orbBehaviors = new List<OrbBehavior>();

        OrbBehavior[] orbs = orbContainer.GetComponentsInChildren<OrbBehavior>();

        foreach(OrbBehavior o in orbs)
            orbBehaviors.Add(o.GetComponent<OrbBehavior>());
    }

    void Update()
    {
        if (!isDetecting)
            return;

        foreach (OrbBehavior orbBehavior in orbBehaviors)
        {
            viewPointPosition = mainCamera.WorldToViewportPoint(orbBehavior.transform.position);

            if (viewPointPosition.x >= 0.45f && viewPointPosition.x <= 0.55f && viewPointPosition.y >= 0.45f && viewPointPosition.y <= 0.55f && viewPointPosition.z > 0f)
            {
                uiManager.ChangeDetectorColor(true);
            }
            else
                uiManager.ChangeDetectorColor(false);
        }
    }

    private void OnDisable()
    {
        orbBehaviors.Clear();
    }
}
