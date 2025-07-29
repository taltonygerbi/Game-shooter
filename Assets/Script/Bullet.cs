using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy bullet after time
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); // Destroy on impact
    }
}
