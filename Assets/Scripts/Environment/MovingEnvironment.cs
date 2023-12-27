using UnityEngine;

public class MovingEnvironment : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float xPosLimit;
    [SerializeField] private bool dieInLeftSide = false;

    [SerializeField] private bool openingProp = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!openingProp)
        {
            transform.position = new Vector3(startPosition.x, transform.position.y, startPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), transform.position.y, transform.position.z);

        //Jika mati di Sebelah kiri
        if (dieInLeftSide && transform.position.x <= xPosLimit)
        {
            Destroy(gameObject);
        }

        //Jika mati di sebelah kanan
        if (!dieInLeftSide && transform.position.x >= xPosLimit)
        {
            Destroy(gameObject);
        }
    }
}
