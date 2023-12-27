using UnityEngine;

public class DebugController : MonoBehaviour
{
    public static DebugController Instance;

    private void Awake()
    {
        #region Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion
    }

    public void DebugLog(string debugNote)
    {
        Debug.Log(debugNote);
    }
}
