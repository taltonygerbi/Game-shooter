using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 1.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
