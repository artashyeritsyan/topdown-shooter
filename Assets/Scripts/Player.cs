using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Vector3 spawnpoint;
    [SerializeField] Transform GunPivot;
    [SerializeField] LayerMask mask;
    RaycastHit hit;
    Ray ray;

    [SerializeField] float maxHp = 100;
    float currentHp;

    [SerializeField] float speed = 100f;

    [SerializeField] float invincibleDelay = 0.2f;
    private bool isInvincible;

    [SerializeField] float dashForce = 200f;
    [SerializeField] GameObject dashParticle;
    [SerializeField] float dashRecoverTime = 1f;
    [SerializeField] AudioClip dashSound;
    private bool canDash = true;

    [SerializeField] AudioClip reloadSoundEffect;

    [Header("Weapons")]
    [SerializeField] GameObject[] weapons;
    [SerializeField] float weaponSwitchDelay = 0.2f;
    private int currentWeaponIdx;
    private bool canSwitchWeapon;

    [Header("Animator")]
    public Animator animator;

    Rigidbody rb;
    float xVel;
    float zVel;

    Vector3 moveDirection;

    public static event Action OnShoot;
    public static event Action<int> OnWeaponSelect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentHp = maxHp;
        currentWeaponIdx = 0;
        canSwitchWeapon = true;
        canDash = true;
        isInvincible = false;
    }

    void Update()
    {
        //rb.linearVelocity = new Vector3(xVel, 0, zVel) * speed * Time.deltaTime;
        rb.AddForce(moveDirection * speed * Time.deltaTime);   

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            //Debug.Log(hit.point);

            Vector3 hitPoint = hit.point;
            hitPoint.y = GunPivot.transform.position.y;
            GunPivot.rotation = Quaternion.LookRotation(hitPoint - GunPivot.position);
        }

        // TODO Move the isShooting logic to gun code.
        if (Mouse.current.leftButton.isPressed)
        {
            OnShoot?.Invoke();
        }

        if ((moveDirection == Vector3.zero))
        {
            animator.SetBool("Running", false);
        } 
        else
        {
            animator.SetBool("Running", true);
        }
    }

    public void Damaged(float damage)
    {
        if (isInvincible) return;

        StartCoroutine(InvincibleFrames());

        currentHp -= damage;
        Debug.Log("Player hp = " + currentHp);

        GameManager.instance.SetPlayerHpBar(currentHp);

        if (currentHp <= 0)
        {
            PlayerDied(); // Death Or minus one life (from 3)
        }
    }

    IEnumerator InvincibleFrames()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleDelay);

        isInvincible = false;
    }

    void PlayerDied()
    {
        Debug.Log("Player Died");
        gameObject.transform.position = spawnpoint;
        currentHp = maxHp;
        GameManager.instance.SetPlayerHpBar(currentHp);
    }

    public void OnMove(InputValue input)
    {
        Vector2 move = input.Get<Vector2>();
        xVel = move.x;
        zVel = move.y;

        moveDirection = new Vector3(xVel, 0, zVel);
    }

    public void OnDash()
    {
        if (moveDirection == Vector3.zero || !canDash) return;

        //animator.SetBool("dashing", true);
        canDash = false;

        float randomPitch = UnityEngine.Random.Range(0.8f, 1.2f);
        // 1 is default value for volume
        AudioManager.Play(dashSound, 1.2f, randomPitch);
        Instantiate(dashParticle, new Vector3(transform.position.x, 0.1f, transform.position.z) , Quaternion.identity);

        rb.AddForce(moveDirection * dashForce, ForceMode.Impulse);

        StartCoroutine(CooldownForDashing());
    }

    IEnumerator CooldownForDashing()
    {
        // This time is a dash animation duration
        yield return new WaitForSeconds(0.17f);
        //animator.SetBool("dashing", false);

        yield return new WaitForSeconds(dashRecoverTime - 0.17f);
        //animator.Play(2);
        canDash = true;

    }

    private void SetWeapon(int weaponIndex)
    {
        // TODO: Add the possibility to set the hand and the weapon for that hand
        // (So just give the index of hand, or bool isRightHand);
        // But need to make 2 currentWeaponsIdx`s for each hand

        if (!canSwitchWeapon) return;

        Transform hand = GunPivot.GetChild(0);

        if (currentWeaponIdx != weaponIndex)
        {
            if (currentWeaponIdx >= 0)
                hand.GetChild(currentWeaponIdx).gameObject.SetActive(false);

            hand.GetChild(weaponIndex).gameObject.SetActive(true);
            currentWeaponIdx = weaponIndex;
            OnWeaponSelect?.Invoke(weaponIndex);

            float randomPitch = UnityEngine.Random.Range(0.8f, 1.2f);
            AudioManager.Play(reloadSoundEffect, 0.5f, randomPitch);
            canSwitchWeapon = false;
            StartCoroutine(CooldownForSwitching());
        }
    }

    IEnumerator CooldownForSwitching()
    {
        yield return new WaitForSeconds(weaponSwitchDelay);
        canSwitchWeapon = true;
    }

    private void OnWeapon1()
    {
        SetWeapon(0);
    }
    private void OnWeapon2()
    {
        SetWeapon(1);
    }
    private void OnWeapon3()
    {
        SetWeapon(2);
    }
    private void OnWeapon4()
    {
        SetWeapon(3);
    }

    private void OnNext()
    {
        Debug.Log("Next");
        int nextWeapon = currentWeaponIdx + 1;
        // Currently the 4 is the count of weapons, but it needs to be refactored to collect them into array
        if (nextWeapon >= 4) nextWeapon = 0;

        SetWeapon(nextWeapon);
    }

}
