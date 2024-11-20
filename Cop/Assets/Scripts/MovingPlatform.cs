using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float xSpeed = 1f;
    [SerializeField] private float ySpeed = 1f;
    [SerializeField] private float travelTime = 0.5f;
    [SerializeField] private float rSpeed = 0f;

    private float timer = 0f;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0);
            transform.Rotate(0, 0, rSpeed * Time.deltaTime);
            timer += Time.deltaTime;

            if (timer >= travelTime)
            {
                xSpeed = -xSpeed;
                ySpeed = -ySpeed;
                timer = 0;
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }
}