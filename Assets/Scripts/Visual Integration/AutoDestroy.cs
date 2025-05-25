using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float _destroyTime = 5f;

    private void Start()
    {
        Invoke(nameof(DestroyObject), _destroyTime);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
