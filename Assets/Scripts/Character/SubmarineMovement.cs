using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubmarineMovement : MonoBehaviour
{
    private InputSystemController inputSystemController;
    private DebugController debugController;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private int followDistance;
    [SerializeField] private ParticleSystem knalpotEffect;
    private Rigidbody myRb;
    private List<Vector3> storedPositions;
    private Vector3 lastPosition;
    private bool isMoving = false;

    [Header("Move Boundaries")]
    [SerializeField] private float xMinBoundaries;
    [SerializeField] private float xMaxBoundaries;
    [SerializeField] private float yMinBoundaries;
    [SerializeField] private float yMaxBoundaries;
    private bool inBoundaries = false;

    [Header("Rotation")]
    [SerializeField] private float damping;

    [Header("Knalpot")]
    [SerializeField] private Animator knalpotAnim;
    private float lastXAngle;

    [Header("Claw / Pencapit")]
    [SerializeField] private ClawController clawController;

    private void Awake()
    {
        storedPositions = new List<Vector3>(); //create a blank list
    }

    // Start is called before the first frame update
    void Start()
    {
        debugController = DebugController.Instance;
        inputSystemController = InputSystemController.Instance;

        myRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        float movementSpeed = speed;

        //Jika mouse tidak digunakan maka Kapal akan mengurangi kecepatan
        if (!inputSystemController.GetMovementJoyStickUseStatus())
            movementSpeed = speed / 1.5f;

        #region Position
        Vector3 destination = inputSystemController.GetMovementJoyStickPosition();

        /*
        //Boundaries
        if (transform.position.x >= xMaxBoundaries && destination.x > 0)
        {
            inBoundaries = true;
            destination.x = 0f;
        }
            
        else if (transform.position.x <= xMinBoundaries && destination.x < 0)
        {
            inBoundaries = true;
            destination.x = 0f;
        }
            
        else if (transform.position.y >= yMaxBoundaries && destination.y > 0)
        {
            inBoundaries = true;
            destination.y = 0f;
        }
            
        else if (transform.position.y <= yMinBoundaries && destination.y < 0)
        {
            inBoundaries = true;
            destination.y = 0f;
        }

        else
        {
            inBoundaries = false;
        }*/


        if (transform.position.y >= yMaxBoundaries && destination.y > 0)
        {
            inBoundaries = true;
            destination.y = 0f;
        }

        else
        {
            inBoundaries = false;
        }

        //Pastikan jika sedang shooting jangkar tidak bergerak
        //if (clawController.IsShootingStatus())
           // destination = Vector3.zero;

        storedPositions.Add(destination); //store the position every frame

        if (storedPositions.Count > followDistance)
        {
            //Position
            myRb.MovePosition(transform.position + (storedPositions[0] * Time.deltaTime * movementSpeed));

            storedPositions.RemoveAt(0); //delete the position that player just move to
        }

        if (transform.position == lastPosition)
        {
            isMoving = false;
            knalpotEffect.Stop();
        }

        else
        {
            isMoving = true;
            knalpotEffect.Play();
        }

        #endregion

        #region Rotation
        if (inBoundaries)
        {
            ResetRotation();
        }

        //Jika sedang Menggunakan Joystick mancing maka reset rotasi
        else if (inputSystemController.GetAnchorJoyStickUseStatus())
        {
            ResetRotation();
        }

        //Jika sedang mancing maka reset rotasi
        /*else if (clawController.IsShootingStatus())
        {
            ResetRotation();
        }
        */
        //Jika sedang menggunakan joystick movement maka akan mengikutii arah rotasi joystick
        else if (inputSystemController.GetMovementJoyStickUseStatus() || transform.position != lastPosition)
        {
            Vector3 lookAngle = inputSystemController.GetMovementJoyStickPosition();
            Quaternion rotation = Quaternion.LookRotation(lookAngle);
            myRb.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping));
        }

        else
        {
            ResetRotation();
        }

        lastPosition = transform.position;
        #endregion

        KnalpotRotation();

    }

    void KnalpotRotation()
    {
        if (lastXAngle - transform.rotation.x <= -0.001f)
        {
            knalpotAnim.SetInteger("State", -1);
        }

        else if (lastXAngle - transform.rotation.x >= 0.001f)
        {
            knalpotAnim.SetInteger("State", 1);
        }

        else
        {
            
            knalpotAnim.SetInteger("State", 0);
        }

        lastXAngle = transform.rotation.x;
    }

    public bool GetMovingStatus()
    {
        return isMoving;
    }

    void ResetRotation()
    {
        float angle1 = Quaternion.Angle(transform.rotation, Quaternion.Euler(0f, 90f, 0f));
        float angle2 = Quaternion.Angle(transform.rotation, Quaternion.Euler(0f, -90f, 0f));

        if (angle1 < angle2)
        {
            myRb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 90f, 0f), Time.deltaTime * damping / 2));
        }

        else
        {
            myRb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, -90f, 0f), Time.deltaTime * damping / 2));
        }
    }
}
