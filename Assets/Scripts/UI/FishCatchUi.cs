using UnityEngine;

public class FishCatchUi : MonoBehaviour
{
    [SerializeField] private ClawCapture clawCapture;
    [SerializeField] private GameObject fishCatchPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BaitButton()
    {
        clawCapture.UseAsBait();
        fishCatchPanel.SetActive(false);
    }

    public void SaveButton()
    {
        clawCapture.SaveToTank();
        fishCatchPanel.SetActive(false);
    }
}
