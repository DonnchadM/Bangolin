using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.025f;
    public Vector3 offset;
    public float lookAheadDistance = 5.5f;

    private Vector3 velocity = Vector3.zero;
    private float fixedZPosition;

    void Start()
    {
        fixedZPosition = transform.position.z;
        Screen.SetResolution(640, 480, true);
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        float direction = target.localScale.x >= 0 ? 1 : -1;
        Vector3 desiredPosition = target.position + new Vector3(offset.x * direction, offset.y, 0);
        desiredPosition += new Vector3(lookAheadDistance * direction, 0, 0);
        Vector3 smoothedPosition = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(desiredPosition.x, desiredPosition.y, 0), ref velocity, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, fixedZPosition);
    }
}
