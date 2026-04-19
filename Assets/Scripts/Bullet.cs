using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 200f;
    public float LifeTime = 5f;

    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        rb.position += transform.forward * speed * Time.deltaTime;
    }
}
