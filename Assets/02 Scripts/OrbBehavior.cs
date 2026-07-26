using UnityEngine;

public class OrbBehavior : MonoBehaviour
{
    [SerializeField] private Material unactivatedMaterial;
    [SerializeField] private Material activatedMaterial;
    [SerializeField] private MeshRenderer orbMesh;

    private void Start()
    {
        orbMesh.material = unactivatedMaterial;
    }

    public void ActivateOrb()
    {
        orbMesh.material = activatedMaterial;
    }
}
