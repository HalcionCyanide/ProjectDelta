using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform target;
    public float dampTime = 0.15f;
    float defaultZoom;

    public float PanSpeed = 20f;
    public float ZoomSpeedTouch = 0.1f;
    public float ZoomSpeedMouse = 0.5f;

    [HideInInspector]
    public Vector2 BoundsX;
    [HideInInspector]
    public Vector2 BoundsY;
    public Vector2 ZoomBounds = new Vector2(1f, 15f);

    Vector3 Velocity = Vector3.zero;
    Vector3 lastPanPosition;
    int panFingerId; // Touch mode only

    bool wasZoomingLastFrame; // Touch mode only
    Vector2[] lastZoomPositions; // Touch mode only

    void Start()
    {
        defaultZoom = Camera.main.orthographicSize;
    }

    void Update()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }
    void FixedUpdate()
    {
        if(target)
        {
            #region Lockon to target
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, defaultZoom, dampTime);

            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref Velocity, dampTime);

            Vector3 copyVec = transform.position;
            copyVec.x = Mathf.Clamp(copyVec.x, BoundsX.x, BoundsX.y);
            copyVec.y = Mathf.Clamp(copyVec.y, BoundsY.x, BoundsY.y);
            transform.position = copyVec;
            #endregion
        }
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the Camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the Camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        if(!target)
        {
            // Determine how much to move the Camera
            Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);
            Vector3 move = new Vector3(offset.x * PanSpeed, offset.y * PanSpeed, 0);

            // Perform the movement
            transform.Translate(move, Space.World);

            // Ensure the Camera remains within bounds.
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
            pos.y = Mathf.Clamp(transform.position.y, BoundsY[0], BoundsY[1]);
            transform.position = pos;

            // Cache the position
            lastPanPosition = newPanPosition;
        }
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0 || target)
        {
            return;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }

    public void SetBounds(Vector2 boundX, Vector2 boundY)
    {
        BoundsX = boundX;
        BoundsY = boundY;
    }
}