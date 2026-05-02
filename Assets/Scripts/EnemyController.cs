using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform target;

    [SerializeField] float maxHp = 100;
    [SerializeField] float damage = 20;
    private float currentHp;

    [SerializeField] float speed = 10;

    Rigidbody rb;
    MeshRenderer mr;
    [SerializeField] Material damagedMaterial;
    Material initialMaterial;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        initialMaterial = mr.material;
        currentHp = maxHp;
        agent.speed = speed;
    }

    void Update()
    {
        if (agent.enabled)
        {
            agent.SetDestination(target.position);
        }
    }

    private void OnEnable()
    {
        agent.enabled = true;
    }

    private void OnDisable()
    {
        agent.enabled = false;
        StopAllCoroutines();
    }

    public void EnableAgent()
    {
        agent.enabled = true;
    }
    public void DisableAgent()
    {
        agent.enabled = false;
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void Damaged(float damage)
    {
        currentHp -= damage;

        Debug.Log("Enemy Damaged from bullet");
        StopCoroutine(EnemyDrawBack());
        StartCoroutine(EnemyDrawBack());

        if (currentHp <= 0)
        {
            EnemyDied();
        }
    }

    IEnumerator EnemyDrawBack()
    {
        // Also can add the Shining material to enemy (on 0.1 second to show that he damaged)
        agent.speed = 0;
        mr.material = damagedMaterial;

        yield return new WaitForSeconds(0.1f);
        agent.speed = speed;
        mr.material = initialMaterial;
    }

    public void ResetHP(float newHp = 0)
    {
        if (newHp > 0)
        {
            maxHp = newHp;
        }

        currentHp = maxHp;
    }

    void EnemyDied()
    {
        Debug.Log("Enemy Dead");
        gameObject.SetActive(false);
        rb.linearVelocity.Set(0, 0, 0);
        rb.angularVelocity.Set(0, 0, 0);
        ResetHP();
        // Call the Enemy pool and deActivate the object;
        // Drop loot (if needed)
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision Enter");
            collision.gameObject.GetComponent<Player>().Damaged(damage);
        }
        else if (collision.gameObject.CompareTag("Vehicle"))
        {
            Debug.Log("Vehicle Collision Enter");
            collision.gameObject.GetComponent<VehicleController>().Damaged(damage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision Enter");
            collision.gameObject.GetComponent<Player>().Damaged(damage);
        }
        else if (collision.gameObject.CompareTag("Vehicle"))
        {
            Debug.Log("Vehicle Collision Enter");
            collision.gameObject.GetComponent<VehicleController>().Damaged(damage);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Player Collision Enter");
    //        other.GetComponent<Player>().Damaged(damage);
    //    }
    //    else if (other.gameObject.CompareTag("Vehicle"))
    //    {
    //        Debug.Log("Vehicle Collision Enter");
    //        other.GetComponent<VehicleController>().Damaged(damage);
    //    }

    //    //if (other.gameObject.CompareTag("bullet"))
    //    //{
    //    //    Debug.Log("Bullet collider Enter");
    //    //    //other.GetComponent<Player>().Damaged(damage);
    //    //}
    //}


}
