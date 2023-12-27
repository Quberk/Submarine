using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineDetector : MonoBehaviour
{
    [SerializeField] private CreatureMovement creatureMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Submarine"))
        {
            creatureMovement.DetectingSubmarine(other.gameObject);
        }
    }
}
