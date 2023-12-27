using UnityEngine;

public class PhysicsIgnoreLayer : MonoBehaviour
{
    // Start is called before the first frame update

    private void Start()
    {
        Physics.IgnoreLayerCollision(12, 13, true);
        Physics.IgnoreLayerCollision(12, 15, true);
    }
}
