using UnityEngine;
using UnityEngine.UI;

public class ClawCapture : MonoBehaviour
{
    [SerializeField] private ClawController clawController;
    [SerializeField] private GameObject capturedPos;
    private GameObject capturedCreature;

    private GameObject bait;

    [Header("UI Catch")]
    [SerializeField] private Text fishNameText;
    [SerializeField] private Slider fearCuriousLevel;
    [SerializeField] private GameObject fishPhotoPanel;
    [SerializeField] private Vector3 fishPhotoLocalSize;

    private GameObject fishPhotoUi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (capturedCreature != null)
        {
            capturedCreature.transform.position = capturedPos.transform.position;
        }

        if (bait != null)
        {
            bait.transform.position = capturedPos.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creature"))
        {
            clawController.ResetIsShooting();
            Destroy(fishPhotoUi);

            other.GetComponent<CreatureMovement>().enabled = false;
            other.GetComponent<BoxCollider>().enabled = false;
            bait = Instantiate(other.GetComponent<CreatureSpesification>().GetBait(), transform.position, Quaternion.identity);

            fishNameText.text = other.GetComponent<CreatureSpesification>().GetFishName();
            fearCuriousLevel.value = (float)other.GetComponent<CreatureSpesification>().GetFearCuriousLevel();
            GameObject fishPhoto = Instantiate(other.GetComponent<CreatureSpesification>().GetFishPhoto(), transform.position, Quaternion.identity);
            fishPhotoUi = fishPhoto;
            fishPhoto.transform.SetParent(fishPhotoPanel.transform);
            fishPhoto.GetComponent<RectTransform>().localPosition = Vector3.zero;
            fishPhoto.GetComponent<RectTransform>().localScale = fishPhotoLocalSize;

            bait.GetComponent<MeshRenderer>().enabled = false;

            clawController.DonePiercing();
            
            capturedCreature = other.gameObject;
        }
    }

    public GameObject GetCapturedCreature()
    {
        return capturedCreature;
    }

    public void DestroyCreature()
    {
        Destroy(capturedCreature);
        capturedCreature = null;
    }

    public void DestroyBait()
    {
        Destroy(bait);
        bait = null;
    }

    public void UseAsBait()
    {
        bait.GetComponent<MeshRenderer>().enabled = true;
    }

    public void SaveToTank()
    {
        DestroyBait();
    }
}
