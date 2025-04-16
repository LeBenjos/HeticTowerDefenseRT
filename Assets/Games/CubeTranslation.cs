using UnityEngine;

public class CubeTranslation : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 1f;
    private Vector3 _startPosition;
    private Vector3 _position;

    void Start()
    {
        _startPosition = transform.position;
        _position = _startPosition;
    }

    void Update()
    {
        _position.y = _startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        _position.z = _startPosition.z + Mathf.Cos(Time.time * frequency) * amplitude;
        transform.position = _position;
    }
}