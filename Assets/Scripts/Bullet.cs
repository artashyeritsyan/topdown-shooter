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
    private MeshRenderer mr;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

    }

    void Start()
    {
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPool));
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


    public float GetSpeed()
    {
        return speed;
    }

    public float GetDamage()
    {
        return damage;
    }

    public Material GetMaterial()
    {
        if (mr == null)
            mr = GetComponent<MeshRenderer>();

        return mr.sharedMaterial;
    }

    public void SetMaterial(Material material)
    {
        if (mr == null)
            mr = GetComponent<MeshRenderer>();

        mr.sharedMaterial = material;
    }

    public void ResetRigidBody()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            Debug.Log("Get THe RB");
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void MoveBullet()
    {
        rb.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet")) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().Damaged(damage);
        }

        OnBulletDestroy();
    }

    // remove this
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>()?.Damaged(damage);
        }

        OnBulletDestroy();

        
    }

    public void OnBulletDestroy()
    {
        AudioManager.Play(hitSound, hitVolume);
        Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        ReturnToPool();
    }


    public void ReturnToPool()
    {
        BulletPool.instance.ReturnBullet(gameObject);
    }
}
