using UnityEngine;

public class FogChangeRegion : MonoBehaviour
{
    [SerializeField] private string colorString;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private Light mainLight;

    private bool transtiioning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!transtiioning)
            return;

        Color newCol;
        bool bConverted = ColorUtility.TryParseHtmlString(colorString, out newCol);
        //Did it successfully parse the Hex?
        if (bConverted)
        {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, newCol, transitionSpeed * Time.deltaTime);
            mainLight.color = Color.Lerp(mainLight.color, newCol, transitionSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Submarine"))
        {
            transtiioning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Submarine"))
        {
            transtiioning = false;
        }
    }
}
