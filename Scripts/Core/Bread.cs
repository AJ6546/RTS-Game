using UnityEngine;

public class Bread : MonoBehaviour
{
    private void Start()
    {
        // destroys after 1 sec
        Destroy(gameObject, 1f);
    }
}
