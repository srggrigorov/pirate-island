using UnityEngine;

[DisallowMultipleComponent]
public class TransformRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}