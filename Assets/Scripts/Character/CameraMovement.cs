using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private InputSystemController inputSystemController;
    private DebugController debugController;

    [Header("Camera FOV")]
    [SerializeField] private float fovMove;
    [SerializeField] private float fovStill;
    [SerializeField] private Camera myCam;
    [SerializeField] private float fovSpeed;

    [Header("Camera Follow")]
    [SerializeField] private GameObject objectToFollow, movingObjectToFollow, shootingFollow;
    [SerializeField] private float followSpeed;

    [Header("Claw")]
    [SerializeField] private ClawController clawController;

    [Header("Submarine")]
    [SerializeField] private SubmarineMovement submarineMovement;

    // Start is called before the first frame update
    void Start()
    {
        inputSystemController = InputSystemController.Instance;
        debugController = DebugController.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 target = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, transform.position.z);
        float followingSpeed = followSpeed;

        float fovTarget = fovStill;
        float fovSpeeding = fovSpeed;

        //Targets
        if (submarineMovement.GetMovingStatus())
        {
            fovTarget = fovMove;
            target = new Vector3(movingObjectToFollow.transform.position.x, movingObjectToFollow.transform.position.y, transform.position.z);

            //Jika berjalan sambil sedang menembakkan Trap maka akan mempercepat gerakan kamera
            if (clawController.IsShootingStatus())
            {
                followingSpeed = 30f;
                fovSpeeding = 40f;

                target = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, transform.position.z);
            }
        }

        else if (clawController.IsShootingStatus())
        {
            shootingFollow = GameObject.FindGameObjectWithTag("Trap");
            target = new Vector3(shootingFollow.transform.position.x, shootingFollow.transform.position.y, transform.position.z);
            followingSpeed = 50f;
            fovTarget = 20f;
            fovSpeeding = 40f;
        }

        //Speed
        else if (!inputSystemController.GetMouseButton())
        {
            followingSpeed = 4f;
            fovSpeeding = 20f;
        }

        myCam.fieldOfView = Mathf.MoveTowards(myCam.fieldOfView, fovTarget, fovSpeeding * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target, followingSpeed * Time.deltaTime);
    }
}
