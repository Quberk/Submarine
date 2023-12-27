using UnityEngine;

public class BacgroundController : MonoBehaviour
{
    [SerializeField] private GameObject myCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(myCam.transform.position.x, myCam.transform.position.y, transform.position.z);
        transform.position = target;
    }
}
