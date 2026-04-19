using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform GunPivot;
    Ray ray;
    RaycastHit hit;
    public LayerMask mask;

    Rigidbody rb;
    public float speed = 100f;
    float xVel;
    float zVel;

    public GameObject bullet;
    public Transform shootPoint;

    public bool isShooting = false;
    public float AttackSpeed = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isShooting = false;
    }



    void Update()
    {
        //rb.linearVelocity = new Vector3(xVel, 0, zVel) * speed * Time.deltaTime;
        rb.AddForce(new Vector3(xVel, 0, zVel) * speed * Time.deltaTime);   

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            Debug.Log(hit.point);

            Vector3 hitPoint = hit.point;
            hitPoint.y = GunPivot.transform.position.y;
            GunPivot.rotation = Quaternion.LookRotation(hitPoint - GunPivot.position);
        }


        if (Mouse.current.leftButton.isPressed && !isShooting)
        {
            StartCoroutine(Shoot());
            isShooting = true;
        }

    }

    IEnumerator Shoot()
    {
        Instantiate(bullet, shootPoint.position, GunPivot.rotation);
        
        // shots count in second
        yield return new WaitForSeconds(1 / AttackSpeed);

        isShooting = false;
    }

    public void OnMove(InputValue input)
    {
        Vector2 move = input.Get<Vector2>();
        xVel = move.x;
        zVel = move.y;
    }
}
