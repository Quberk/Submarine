using UnityEngine;

public class MovingEnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prop;
    [SerializeField] private float yMin, yMax;
    [SerializeField] private float spawnTime;
    private float spawnCounter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter += Time.deltaTime;

        if (spawnCounter >= spawnTime)
        {
            float rand = Random.Range(0, prop.Length * 100f);
            float yPos = Random.Range(yMin, yMax);

            for (int i = 0; i < prop.Length; i++)
            {
                if (rand <= i * 100f)
                {
                    Vector3 positionSpawn = new Vector3(transform.position.x, yPos, transform.position.z);
                    Instantiate(prop[i], positionSpawn, Quaternion.identity);
                    break;
                }
            }
            spawnCounter = 0f;
        }
    }
}
