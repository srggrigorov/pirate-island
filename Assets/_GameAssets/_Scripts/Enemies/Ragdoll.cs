using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField]
    private List<Collider> _colliders;
    
    //We need to store rigidbodies due to bug with collider.attachedRigidbody being null;
    [SerializeField]
    private List<Rigidbody> _rigidbodies;

    public void SetActive(bool value)
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            _colliders[i].enabled = value;
            _rigidbodies[i].isKinematic = !value;
        }
    }
}
