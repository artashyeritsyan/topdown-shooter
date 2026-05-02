using System.Collections;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] float maxHealth = 1000;
    private float currentHealth;

    [SerializeField] float speed = 1;

    [SerializeField] float invincibleDelay = 0.2f;
    private bool isInvincible;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float health)
    {
        this.maxHealth = health;
    }

    public void Damaged(float hp)
    {
        if (isInvincible) return;

        StartCoroutine(InvincibleFrames());

        currentHealth -= hp;
        Debug.Log("Vehicle HP = " + currentHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    IEnumerator InvincibleFrames()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleDelay);

        isInvincible = false;
    }

    void GameOver()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        Debug.Log("Game OVer");
    }


}
