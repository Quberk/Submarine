using UnityEngine;

public class ClawController : MonoBehaviour
{
    private InputSystemController inputSystemController;
    private DebugController debugController;

    [SerializeField] private SubmarineMovement submarineMovement;
    [SerializeField] private GameObject claw;
    [SerializeField] private Animator clawUiAnim;
    [SerializeField] private GameObject clawShoot;
    [SerializeField] private GameObject clawCaptureSphere;
    [SerializeField] private ClawCapture clawCapture;
    [SerializeField] private GameObject fishCatchPanelUi;
    [SerializeField] private GameObject shootingMeter;
    [SerializeField] private GameObject clawLiftingUi;
    [SerializeField] private GameObject clawLiftingSlider;

    [Header("Claw Shooting Animation")]
    [SerializeField] private float shootSpeed;
    [SerializeField] private GameObject shootPos;
    [SerializeField] private GameObject idlePos;

    [Header("Trap")]
    [SerializeField] private GameObject trap;
    [SerializeField] private float pushForce;

    private float inititalClawRotation;

    private bool isShooting = false;
    private bool isPiercing = false;

    // Start is called before the first frame update
    void Start()
    {
        inputSystemController = InputSystemController.Instance;
        debugController = DebugController.Instance;

        clawCaptureSphere.GetComponent<MeshRenderer>().enabled = false;
        clawCaptureSphere.GetComponent<SphereCollider>().isTrigger = false;

        fishCatchPanelUi.SetActive(false);
        //clawLiftingUi.SetActive(false);
        clawLiftingSlider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ShootingAnim();
        shootingMeter.SetActive(false);

        if (!submarineMovement.GetMovingStatus())
        {
            if (!inputSystemController.GetAnchorJoyStickUseStatus() && inputSystemController.GetAnchorJoyStickBeingDragStatus() && isShooting == false)
            {
                isPiercing = true;
                isShooting = true;
                submarineMovement.gameObject.GetComponent<BoxCollider>().enabled = false;
                /*
                clawCaptureSphere.GetComponent<MeshRenderer>().enabled = true;
                clawCaptureSphere.GetComponent<SphereCollider>().isTrigger = true;*/

                //Trap Coba"
                GameObject myTrap = Instantiate(trap, clawCaptureSphere.transform.position, Quaternion.identity);
                myTrap.GetComponent<Rigidbody>().velocity = new Vector3(inputSystemController.GetLastAnchorJoystickPosition().x,
                                                                        inputSystemController.GetLastAnchorJoystickPosition().y, 0f) * pushForce * Time.deltaTime;

                inputSystemController.SetAnchorJoyStickBeingDragStatus(false);
                clawUiAnim.SetBool("On", false);

                submarineMovement.gameObject.GetComponent<BoxCollider>().enabled = true;

                /*clawLiftingSlider.SetActive(true);
                inputSystemController.SetValueClawSlider(0f);*/

                return;
            }

            if (isShooting)
            {
                //float myXrot = inputSystemController.GetValueClawSlider() + inititalClawRotation;

                //claw.transform.rotation = Quaternion.Euler(myXrot, 90f, 0f);
                return;
            }

            clawUiAnim.gameObject.GetComponent<FixedJoystick>().enabled = true;
            clawUiAnim.SetBool("On", true);

            shootingMeter.SetActive(true);

            //Rotation Claw
            inputSystemController.SetLastAnchorJoystickPosition();
            float heading = Mathf.Atan2(inputSystemController.GetAnchorJoyStickPosition().x, inputSystemController.GetAnchorJoyStickPosition().y);
            float xRot =(heading * Mathf.Rad2Deg) - 90f;
            inititalClawRotation = xRot;

            claw.transform.rotation = Quaternion.Euler(xRot, 90f, 0f);

            //Reset Rotasi Claw
            if (inputSystemController.GetAnchorJoyStickPosition() == Vector3.zero)
            {
                claw.transform.rotation = Quaternion.Euler(40f, 90f, 0f);
                shootingMeter.SetActive(false);
            }

            return;
        }

        clawUiAnim.SetBool("On", false);
    }

    public bool IsShootingStatus()
    {
        return isShooting;
    }

    public bool IsPiercingStatus()
    {
        return isPiercing;
    }

    public void ResetIsShooting()
    {
        clawCaptureSphere.GetComponent<MeshRenderer>().enabled = false;
        clawCaptureSphere.GetComponent<SphereCollider>().isTrigger = false;
        isShooting = false;
        
    }

    public void DonePiercing()
    {
        isPiercing = false;
        //clawLiftingUi.SetActive(true);
    }

    void ShootingAnim()
    {
        if (clawShoot.transform.localPosition == shootPos.transform.localPosition)
            DonePiercing();

        if (isPiercing)
        {
            clawShoot.transform.localPosition = Vector3.MoveTowards(clawShoot.transform.localPosition, shootPos.transform.localPosition, shootSpeed * Time.deltaTime * 2f);
            return;
        }

        if (clawShoot.transform.localPosition == idlePos.transform.localPosition)
        {
            isShooting = false;

            clawLiftingSlider.SetActive(false);

            //Reset Shooting Status
            ResetIsShooting();

            //Reset Lifting Status and Button
            //clawLiftingUi.SetActive(false);
            inputSystemController.ReleaseButtonLift();

            if (clawCapture.GetCapturedCreature() != null)
            {
                clawCapture.DestroyCreature();
                fishCatchPanelUi.SetActive(true);
            }

            return;
        }

        if (!isPiercing)
        clawShoot.transform.localPosition = Vector3.MoveTowards(clawShoot.transform.localPosition, idlePos.transform.localPosition, shootSpeed * 2f * Time.deltaTime);

    }
}
