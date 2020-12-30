using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    [SerializeField]
    private Camera cam;

    private Vector3 velocity;
    private Vector3 rotation;
    private float cameraRotationX = 0F;
    private float currentCameraRotationX = 0F;
    private Vector3 thrusterVelocity;
    private Rigidbody rb;


    [SerializeField]
    private float cameraRotationLimit = 85f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    public void ApplyThruster(Vector3 _thrusterVelocity)
    {
        thrusterVelocity = _thrusterVelocity;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if(thrusterVelocity != Vector3.zero) 
        {
            rb.AddForce(thrusterVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCameraRotationX += cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX,-cameraRotationLimit,cameraRotationLimit);
        cam.transform.localEulerAngles = new Vector3(-currentCameraRotationX, 0F, 0f);
    }
}
