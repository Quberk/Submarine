using UnityEngine;

public class CreatureSpesification : MonoBehaviour
{
    [SerializeField] private string creatureName;
    [Range(0, 100f)][SerializeField] private int fearCuriousLevel;
    [SerializeField] private GameObject fishPhoto;
    [SerializeField] private GameObject baitReplace;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetBait()
    {
        return baitReplace;
    }

    public string GetFishName()
    {
        return creatureName;
    }

    public int GetFearCuriousLevel()
    {
        return fearCuriousLevel;
    }

    public GameObject GetFishPhoto()
    {
        return fishPhoto;
    }
}
