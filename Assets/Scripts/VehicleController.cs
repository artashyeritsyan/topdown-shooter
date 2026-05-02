using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] float maxHealth = 1000;
    private float currentHealth;

    [SerializeField] float speed = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
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
        currentHealth -= hp;
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game OVer");
    }


}
