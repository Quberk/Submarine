using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapForceFieldController : MonoBehaviour
{
    [SerializeField] private GameObject forceFieldFull;
    [SerializeField] private GameObject trap;
    [SerializeField] private Animator myAnim;
    [SerializeField] private float lifeTime;
    private float lifeCounter = 0f;
    private bool alreadyDead = false;
    private ClawController clawController;

    [SerializeField] private int maxCaptured;
    private List<GameObject> creatureCaptured = new List<GameObject>();
    private bool alreadyFull;


    // Start is called before the first frame update
    void Start()
    {
        clawController = FindObjectOfType<ClawController>();
        forceFieldFull.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        lifeCounter += Time.deltaTime;
        if (lifeCounter >= lifeTime && !alreadyDead)
        {
            myAnim.SetTrigger("Die");
            alreadyDead = true;
        }

        if (creatureCaptured.Count == maxCaptured && !alreadyFull)
        {
            alreadyFull = true;
            forceFieldFull.SetActive(true);
            GetComponent<Renderer>().enabled = false;
        }
    }

    public void EndOfTrap()
    {
        Destroy(trap);
        clawController.ResetIsShooting();

        if (creatureCaptured.Count > 0)
        foreach(GameObject creature in creatureCaptured)
        {
            Destroy(creature);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creature") && creatureCaptured.Count < maxCaptured)
        {
            other.GetComponent<CreatureMovement>().GetCaptured(trap);
            creatureCaptured.Add(other.gameObject);
        }
    }
}
