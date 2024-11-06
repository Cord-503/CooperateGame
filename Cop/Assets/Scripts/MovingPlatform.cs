using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float xSpeed = 1f;
    [SerializeField] private float travelTime = 0.5f;
    [SerializeField] private float rSpeed = 0f;


    private float timer = 0f;

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(xSpeed * Time.deltaTime, 0, 0);

        transform.Rotate(0, 0, rSpeed * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= travelTime)
        {
            xSpeed = -xSpeed;
            timer = 0;
        }
    }
}
