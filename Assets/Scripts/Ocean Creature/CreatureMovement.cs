using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureMovement : MonoBehaviour
{
    [SerializeField] private CreatureSpesification creatureSpesification;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float agressiveSpeed;
    private float speed;

    [Header("Delay Movement")]
    [SerializeField] private int delayPos;
    private List<Vector3> storedPositions;
    private Vector3 destination;

    [Header("Movement Boundaries")]
    private float xMinNormal, xMaxNormal, yMinNormal, yMaxNormal;
    private float xMinPanic, xMaxPanic, yMinPanic, yMaxPanic;
    private float xMinimum, xMaximum, yMinimum, yMaximum;

    [Header("Target To Go")]
    //[SerializeField] private float radiusPoint;
    //private Vector3 centerPoint;
    private Vector3 pointToGo;

    [Header("Rotation")]
    [SerializeField] private float damping;

    [Header("Panic")]
    [SerializeField] private ParticleSystem fearFx;
    [SerializeField] private ParticleSystem curiousFx;
    [SerializeField] private float panicTime;
    [SerializeField] private SphereCollider submarineDetector;
    private float panicCounter;
    private bool panic;

    [Header("Captured")]
    private bool captured = false;
    private GameObject capturedPos;

    private bool alreadyInDestination = false;

    private void Awake()
    {
        storedPositions = new List<Vector3>(); //create a blank list
    }

    // Start is called before the first frame update
    void Start()
    {
        xMinimum = xMinNormal;
        xMaximum = xMaxNormal;
        yMinimum = yMinNormal;
        yMaximum = yMaxNormal;

        fearFx.Stop();
        curiousFx.Stop();

        //centerPoint = transform.position;
        pointToGo = PickAPointToGo();
        destination = transform.position;
        speed = normalSpeed;

        float fearness = 100f - creatureSpesification.GetFearCuriousLevel();
        submarineDetector.radius = fearness / 100f;

        //Sementara maksimal radius Trigger 0.25
        if (submarineDetector.radius >= 0.25f)
            submarineDetector.radius = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        SwimmingToThePoint(PickAPointToGo());

        if (transform.position.x > xMaximum || transform.position.x < xMinimum || transform.position.y > yMaximum || transform.position.y < yMinimum)
        {
            pointToGo = PickAPointToGo();
        }

        //Rotation
        Vector3 lookPos = pointToGo - transform.position;
        lookPos.z = 0f;
        Quaternion rot = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);

        //Panic
        if (panic)
        {
            speed = agressiveSpeed;
            panicCounter += Time.deltaTime;

            xMinimum = xMinPanic;
            xMaximum = xMaxPanic;
            yMinimum = yMinPanic;
            yMaximum = yMaxPanic;

            if (panicCounter >= panicTime)
            {
                panic = false;
                panicCounter = 0f;
            }
            return;
        }

        speed = normalSpeed;

        xMinimum = xMinNormal;
        xMaximum = xMaxNormal;
        yMinimum = yMinNormal;
        yMaximum = yMaxNormal;
    }

    private Vector3 PickAPointToGo()
    {
        float xPos = Random.Range(xMinimum, xMaximum);
        float yPos = Random.Range(yMinimum, yMaximum);
        Vector3 targetPos = new Vector3(xPos, yPos, 9f);

        return targetPos;
    }

    private void SwimmingToThePoint(Vector3 point)
    {
        if (captured)
        {
            destination = Vector3.MoveTowards(destination, capturedPos.transform.position, speed * Time.deltaTime);
        }

        else if (alreadyInDestination)
        {
            destination = Vector3.MoveTowards(destination, point, speed * Time.deltaTime);
            pointToGo = point;
        }

        else
        {
            destination = Vector3.MoveTowards(destination, pointToGo, speed * Time.deltaTime);
        }

        storedPositions.Add(destination);

        //Delay Move
        if (storedPositions.Count > delayPos)
        {
            transform.position = storedPositions[0];
            storedPositions.RemoveAt(0); //delete the position that player just move to
        }


        if (Vector3.Distance(destination, pointToGo) <= 0.2f)
        {
            alreadyInDestination = true;
            return;
        }

        alreadyInDestination = false;
    }

    public void GetCaptured(GameObject trap)
    {
        captured = true;
        capturedPos = trap;
    }

    public void DetectingSubmarine(GameObject submarine)
    {
        if (captured)
        {
            return;
        }

        float randomNum = Random.Range(0, 100f);
        float fearLevel = (float)creatureSpesification.GetFearCuriousLevel();

        //Ikan takut jika kapal bergerak atau Lagi takut aja
        if (randomNum >= fearLevel || submarine.GetComponent<SubmarineMovement>().GetMovingStatus())
        {
            fearFx.Play();

            //Submarine berada di Kiri
            pointToGo = new Vector3(transform.position.x + 7f, Random.Range(yMinimum, yMaximum), transform.position.z);
            alreadyInDestination = false;
            panic = true;
            panicCounter = 0f;

            //Submarine berada di Kanan
            if ((submarine.transform.position.x - transform.position.x) > 0)
            {
                pointToGo = new Vector3(transform.position.x - 7f, Random.Range(yMinimum, yMaximum), transform.position.z);
            }

            return;
        }

        //Ikan penasaran
        curiousFx.Play();
        panic = false;
        panicCounter = 0f;
    }

    public void SetTheBoundariesPosition(float xMinNorm, float xMaxNorm, float yMinNorm, float yMaxNorm, float xMinPan, float xMaxPan, float yMinPan, float yMaxPan)
    {
        xMinNormal = xMinNorm;
        xMaxNormal = xMaxNorm;
        yMinNormal = yMinNorm;
        yMaxNormal = yMaxNorm;

        xMinPanic = xMinPan;
        xMaxPanic = xMaxPan;
        yMinPanic = yMinPan;
        yMaxPanic = yMaxPan;
    }
}
