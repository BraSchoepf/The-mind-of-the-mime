using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IResettable
{
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    public void ResetObject()
    {
        transform.position = _initialPosition;
        // Restablecer otros estados necesarios
    }
}
