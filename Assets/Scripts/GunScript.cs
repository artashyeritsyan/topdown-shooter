using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class GunScript : MonoBehaviour
{
    [Header("Weapon Params")]
    [SerializeField] bool isSniper = false; 
    [SerializeField] float damage = 10f;
    List<Transform> shootPoints;

    [SerializeField] GameObject muzzle;
    [SerializeField] AudioClip shootSoundEffect;

    [Header ("Bullet Params")]
    [SerializeField] GameObject bullet;
    bool isShooting = false;
    public float shootingSpeed = 10f;
    public float bulletSpeed = 200f;

    bool isMuzzleWorked = true;

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
    
    void HandleShooting()
    {
        if (!isShooting)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(Shoot());
                isShooting = true;
            }
        }
    }

    IEnumerator Shoot()
    {
        isMuzzleWorked = false;
        foreach (Transform shootPoint in shootPoints)
        {
            if (shootPoint.gameObject.activeSelf)
            {
                GameObject spawnedBullet;
                if (isSniper)
                {
                    spawnedBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
                }
                else
                {
                    //Debug.Log("Call Get Bullet");
                    spawnedBullet = BulletPool.instance.GetBullet(bullet, shootPoint.position, shootPoint.rotation);

                }

                Bullet bs = spawnedBullet.GetComponent<Bullet>();
                bs.SetSpeed(bulletSpeed);
                bs.SetDamage(damage);

                if (!isMuzzleWorked)
                {
                    Instantiate(muzzle, shootPoint.position, shootPoint.rotation, shootPoint);
                    float randomPitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    AudioManager.Play(shootSoundEffect, 0.7f, randomPitch);
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
        isShooting = false;
        isMuzzleWorked = true;
    }

    void OnDisable()
    {
        Player.OnShoot -= HandleShooting;
        StopAllCoroutines();
        isShooting = false;
    }

    void HandlePosition(Vector3 pos)
    {
        Debug.Log("Received position: " + pos);
    }


}
