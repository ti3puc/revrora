using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 15f;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
