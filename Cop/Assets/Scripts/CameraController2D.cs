using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Following Settings")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float zoomLimiter = 50f;

    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    private Transform currentTarget;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;

        float distance = GetPlayersDistance();
        float newZoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

    }

    private Vector3 GetCenterPoint()
    {
        var bounds = new Bounds(player1.position, Vector3.zero);
        bounds.Encapsulate(player2.position);
        return bounds.center;
    }

    private float GetPlayersDistance()
    {
        return Vector3.Distance(player1.position, player2.position);
    }

}