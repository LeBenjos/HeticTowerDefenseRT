using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 45 * Time.deltaTime, 0);
    }
}


