using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MovePlatform : MonoBehaviour
{
    [SerializeField] Vector3 movePosition;
    [SerializeField] float moveSpeed;
    [SerializeField] [Range(0,1)]float moveProgress;//ползунок 0-не двигается
    Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

   
    void Update()
    {
        moveProgress = Mathf.PingPong(Time.time*moveSpeed, 1);
        Vector3 offset = movePosition * moveProgress;
        transform.position = startPosition + offset;
    }
}
