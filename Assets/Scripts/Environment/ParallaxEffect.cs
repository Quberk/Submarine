using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float startpos;
    [SerializeField] private GameObject movingObject;
    [SerializeField] private float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (movingObject.transform.position.x * (1 - parallaxEffect));
        float distance = (movingObject.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

    }
}
