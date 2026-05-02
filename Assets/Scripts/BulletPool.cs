using System.Collections.Generic;
using UnityEngine;


public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    public GameObject[] bulletsVariations;
    //public GameObject BulletPrefab;
    public int PoolSize = 30;

    List<GameObject> pool = new List<GameObject>();
    private void Awake()
    {
        instance = this;

        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletsVariations[0]);
            bullet.SetActive(false);
            pool.Add(bullet);
        }
    }

    public GameObject GetBullet(GameObject bulletOriginal, Vector3 position, Quaternion rotation)
    {
        foreach (GameObject bullet in pool)
        {
            if (!bullet.activeInHierarchy)
            {
                Bullet bulletScript = bulletOriginal.GetComponent<Bullet>();
                bullet.transform.SetPositionAndRotation(position, rotation);
                Bullet bs = bullet.GetComponent<Bullet>();
                bs.SetMaterial(bulletScript.GetMaterial()); 

                bs.SetSpeed(bulletScript.GetSpeed());
                bs.SetDamage(bulletScript.GetDamage());
                bs.ResetRigidBody();

                bullet.gameObject.transform.localScale = bulletScript.gameObject.transform.localScale;

                bullet.SetActive(true);

                //Debug.Log(bullet.GetComponent<Rigidbody>().useGravity);?
                return bullet;

            }
        }

        GameObject newBullet = Instantiate(bulletOriginal, position, rotation);
        pool.Add(newBullet);
        return newBullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

}
