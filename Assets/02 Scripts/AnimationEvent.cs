using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
