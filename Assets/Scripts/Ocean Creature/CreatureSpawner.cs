using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] creatures;
    [SerializeField] private float creaturesLimit;
    [SerializeField] private GameObject xMinimumSpawn, xMaximumSpawn, yMinimumSpawn, yMaximumSpawn;
    [SerializeField] private GameObject xMinPanic, xMaxPanic, yMinPanic, yMaxPanic;

    // Start is called before the first frame update
    void Start()
    {
        LoopInstantiatingCreature(0);
    }

    // Update is called once per frame
    void Update()
    {
        int currentCreaturesExist = GameObject.FindGameObjectsWithTag("Creature").Length;

        if (currentCreaturesExist < creaturesLimit)
        {
            LoopInstantiatingCreature(currentCreaturesExist);
        }
    }

    void LoopInstantiatingCreature(int startingNum)
    {
        for (int i = startingNum; i < creaturesLimit; i++)
        {
            float random = Random.Range(0, creatures.Length * 100f);

            for (int j = 0; j < creatures.Length; j++)
            {
                if (random <= (j + 1) * 100f)
                {
                    Vector3 spawnPos = new Vector3(Random.Range(xMinimumSpawn.transform.position.x, xMaximumSpawn.transform.position.x),
                                                    Random.Range(yMinimumSpawn.transform.position.y, yMaximumSpawn.transform.position.x), 9f);

                    GameObject fish = Instantiate(creatures[j], spawnPos, Quaternion.Euler(-90f, 0f, 0f));
                    fish.GetComponent<CreatureMovement>().SetTheBoundariesPosition(xMinimumSpawn.transform.position.x, xMaximumSpawn.transform.position.x, 
                                                                                    yMinimumSpawn.transform.position.y, yMaximumSpawn.transform.position.y,
                                                                                    xMinPanic.transform.position.x, xMaxPanic.transform.position.x, 
                                                                                    yMinPanic.transform.position.y, yMaxPanic.transform.position.y);
                    break;
                }
            }
        }
    }
}
