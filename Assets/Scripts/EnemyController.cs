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

    [SerializeField] Animator animator;
    [SerializeField] float walkingAnimSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        initialMaterial = mr.material;
        currentHp = maxHp;
        agent.speed = speed;

        //animator = GetComponentInChildren<Animator>();

        animator.SetFloat("animSpeed", walkingAnimSpeed);


        animator.SetBool("Attack", false);
        animator.SetBool("Running", true);
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
        if (agent == null) return;

        agent.enabled = true;
        if (agent.enabled)
            agent.ResetPath();

        animator.SetFloat("animSpeed", walkingAnimSpeed);
    }

    private void OnDisable()
    {
        if (agent == null) return;

        if (agent.enabled)
            //agent.ResetPath();
        agent.enabled = false;
        //StopAllCoroutines();
    }

    //public void EnableAgent()
    //{
    //    agent.enabled = true;
    //}
    //public void DisableAgent()
    //{
    //    agent.enabled = false;
    //}

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

        //Debug.Log("Enemy Damaged from bullet");
        //StopCoroutine(EnemyDrawBack());
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(EnemyDrawBack());
        }


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

    void ResetAnim()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Running", true);

        animator.SetFloat("animSpeed", walkingAnimSpeed);

        Debug.Log(animator.GetBool("Running"));
        //Debug.Log(speed + " : " + animator.GetFloat("animSpeed"));
        Debug.Log(speed + " : " + walkingAnimSpeed);
    }

    void EnemyDied()
    {
        Debug.Log("Enemy Dead");
        gameObject.SetActive(false);

        rb.linearVelocity.Set(0, 0, 0);
        rb.angularVelocity.Set(0, 0, 0);
        agent.speed = speed;
        mr.material = initialMaterial;
        ResetHP();
        ResetAnim();
        // Call the Enemy pool and deActivate the object;
        // Drop loot (if needed)
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision Enter");
            collision.gameObject.GetComponent<Player>().Damaged(damage);
            agent.speed = 0;
        }

        if (collision.gameObject.CompareTag("Vehicle"))
        {
            Debug.Log("Vehicle Collision Enter");
            collision.gameObject.GetComponent<VehicleController>().Damaged(damage);
            agent.speed = speed / 2;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision Enter");
            collision.gameObject.GetComponent<Player>().Damaged(damage);
            agent.speed = 0;

        }

        if (collision.gameObject.CompareTag("Vehicle"))
        {
            Debug.Log("Vehicle Collision Enter");
            collision.gameObject.GetComponent<VehicleController>().Damaged(damage);
            agent.speed = speed / 2;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Vehicle"))
        {
            agent.speed = speed;

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {

    //    }
    //    else if (other.gameObject.CompareTag("Vehicle"))
    //    {

    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Attack", true);
            animator.SetBool("Running", false);
            agent.speed = 0;
            Debug.Log(speed + " : " + animator.GetFloat("animSpeed"));

        }

        if (other.gameObject.CompareTag("Vehicle"))
        {
            animator.SetBool("Attack", true);
            animator.SetBool("Running", false);
            agent.speed = speed / 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Vehicle"))
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Running", true);
            agent.speed = speed;
        }
    }


}
