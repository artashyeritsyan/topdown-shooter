using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform GunPivot;
    Ray ray;
    RaycastHit hit;
    public LayerMask mask;

    public float speed = 100f;
    public float dashForce = 200f;


    Rigidbody rb;
    float xVel;
    float zVel;

    public static event Action<Quaternion> OnShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        // TODO Move the isShooting logic to gun code.
        if (Mouse.current.leftButton.isPressed)
        {
            OnShoot?.Invoke(GunPivot.rotation);
        }

    }

    public void OnMove(InputValue input)
    {
        Vector2 move = input.Get<Vector2>();
        xVel = move.x;
        zVel = move.y;
    }

    private void OnDash()
    {
        Debug.Log("Dash!");
        rb.AddForce(new Vector3(xVel, 0, zVel) * dashForce * Time.deltaTime);
        // Play Dash sound
        // Inst Dash Particle
    }

}
