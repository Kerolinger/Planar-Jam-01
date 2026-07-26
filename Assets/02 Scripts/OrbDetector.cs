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

    private OrbBehavior currentOrb;

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

            if (viewPointPosition.x >= 0 && viewPointPosition.x <= 1f && viewPointPosition.y >= 0f && viewPointPosition.y <= 1f && viewPointPosition.z > 0f)
            {
                uiManager.ChangeDetectorColor((viewPointPosition.x / 1) + (viewPointPosition.y / 1) / 2);
                currentOrb = orbBehavior;
            }
            else
            {
                uiManager.ChangeDetectorColor(-1);
                currentOrb = null;
            }
        }
    }

    private void OnDisable()
    {
        orbBehaviors.Clear();
    }

    public void TryActivateOrb()
    {
        if (currentOrb == null)
            return;

        currentOrb.ActivateOrb();
    }
}
