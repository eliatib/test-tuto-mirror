using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 3f;

    [SerializeField]
    private float m_rotationSpeedX = 3f;

    [SerializeField]
    private float m_rotationSpeedY = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Joints Options")]

    [SerializeField]
    private float jointSpring = 20f;

    [SerializeField]
    private float jointMaxForce = 50f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;


    // Start is called before the first frame update
    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        setJointSettings(jointSpring);
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * horizontal;
        Vector3 moveVertical = transform.forward * vertical;
        Vector3 thrusterVelocity = Vector3.zero;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * m_speed;

        motor.Move(velocity);

        //rotation
        float yRot = Input.GetAxisRaw("Mouse X");
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 rotation = new Vector3(0, yRot, 0) * m_rotationSpeedX;

        float cameraRotationX = xRot * m_rotationSpeedY;

        motor.Rotate(rotation);
        motor.RotateCamera(cameraRotationX);

        if (Input.GetButton("Jump")) 
        {
            thrusterVelocity = Vector3.up * thrusterForce;
            setJointSettings(0f);
        }
        else 
        {
            setJointSettings(jointSpring);
        }

        motor.ApplyThruster(thrusterVelocity);
    }

    private void setJointSettings(float _jointSpring) 
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }

}