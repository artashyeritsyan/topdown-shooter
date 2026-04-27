using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class GunScript : MonoBehaviour
{
    [Header("Weapon Params")]
    [SerializeField] float damage = 10f;
    List<Transform> shootPoints;

    [SerializeField] GameObject muzzle;
    [SerializeField] AudioClip shootSoundEffect;

    [Header ("Bullet Params")]
    [SerializeField] GameObject bullet;
    bool isShooting = false;
    public float shootingSpeed = 10f;
    public float bulletSpeed = 200f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("shootpoint"))
            {
                shootPoints.Add(child);
            }
        }

        isShooting = false;
    }

    void Update()
    {

    }
    void HandleShooting(Quaternion rotation)
    {
        if (!isShooting)
        {
            StartCoroutine(Shoot());
            isShooting = true;
        }
    }

    IEnumerator Shoot()
    {
        bool isMuzzleWorked = false;
        foreach (Transform shootPoint in shootPoints)
        {
            if (shootPoint.gameObject.activeSelf)
            {
                Instantiate(bullet, shootPoint.position, shootPoint.rotation)
                    .GetComponent<Bullet>().SetSpeed(bulletSpeed);

                if (!isMuzzleWorked)
                {
                    Instantiate(muzzle, shootPoint.position, shootPoint.rotation, shootPoint);
                    AudioManager.Play(shootSoundEffect);
                    isMuzzleWorked = true;
                }
            }
        }


        // shots count in second
        yield return new WaitForSeconds(1 / shootingSpeed);

        isShooting = false;
    }


    void OnEnable()
    {
        Player.OnShoot += HandleShooting;
    }

    void HandlePosition(Vector3 pos)
    {
        Debug.Log("Received position: " + pos);
    }
}
