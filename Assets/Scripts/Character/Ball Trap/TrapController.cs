using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private GameObject forceField;
    [SerializeField] private Rigidbody myRb;
    [SerializeField] private float forceFieldInitiateTime;
    private float forceFieldInitiateCounter = 0f;
    private bool alreadyInitiating = false;

    // Start is called before the first frame update
    void Start()
    {
        forceField.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        forceFieldInitiateCounter += Time.deltaTime;

        if (forceFieldInitiateCounter >= forceFieldInitiateTime && !alreadyInitiating)
        {
            alreadyInitiating = true;
            forceField.SetActive(true);
            //myRb.useGravity = false;
            myRb.isKinematic = true;
        }
    }
}
