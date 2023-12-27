using UnityEngine;

public class ClawAnimation : MonoBehaviour
{

    [SerializeField] private ClawController clawController;
    [SerializeField] private ClawCapture clawCapture;
    [SerializeField] private GameObject fishCatchUi;

    // Start is called before the first frame update
    void Start()
    {
        fishCatchUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationDone()
    {
        if (clawCapture.GetCapturedCreature() != null)
            fishCatchUi.SetActive(true);

        clawController.ResetIsShooting();
        clawCapture.DestroyCreature();
    }
}
