using System.Collections;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] float maxHealth = 1000;
    private float currentHp;

    [SerializeField] float speed = 1.5f;

    [SerializeField] float invincibleDelay = 0.2f;
    private bool isInvincible;
    private Vector3 finishPosition;

    [SerializeField] AudioSource engineSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finishPosition = GameManager.instance.GetFinishPosition();
        currentHp = maxHealth;
        isInvincible = false;
        engineSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x >= finishPosition.x)
        {
            GameManager.instance.GameWin();
            speed = 0;
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float health)
    {
        this.maxHealth = health;
    }

    public float GetDistanceToFinish()
    {
        return finishPosition.x - transform.position.x;
    }

    public void Damaged(float hp)
    {
        if (isInvincible) return;

        StartCoroutine(InvincibleFrames());

        currentHp -= hp;
        //Debug.Log("Vehicle HP = " + currentHp);
        
        GameManager.instance.SetVehicleHpBar(currentHp);

        if (currentHp <= 0)
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
