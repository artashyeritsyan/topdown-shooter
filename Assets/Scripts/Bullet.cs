using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 5f;
    private float speed = 100f;
    private float damage = 10f;

    public GameObject DestroyEffect;

    public AudioClip hitSound;
    public float hitVolume = 0.3f;

    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void MoveBullet()
    {
        rb.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet")) return;
        OnBulletDestroy();
    }

    // remove this
    private void OnCollisionEnter(Collision collision)
    {
        OnBulletDestroy();
    }

    public void OnBulletDestroy()
    {
        AudioManager.Play(hitSound, hitVolume);
        Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
