using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{
    [Tooltip("Target To Follow")]
    public Transform target;

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    Vector3 clickLocation;

    float defaultZoom;

    //FreeCam
    Vector3 touchOrigin;
    public float panSpeed = 4.0f;

    private void Start()
    {
        defaultZoom = Camera.main.orthographicSize;
    }

    private void Update()
    {
        ///Very important!
        #region Define the touch position (CrossPlatform)
        clickLocation = GetComponent<MouseBehaviours>().CPgetTouchLocation(0);
        #endregion
        //If we have something to lockon to...
        if (target)
        {
            target.transform.GetComponent<DragShotMover>().selfSelected = true;

            #region If Button pressed...
            if (GetComponent<MouseBehaviours>().CPgetTouch(0))
            #endregion
            {
                //Fire a ray in 2D space to check for player.
                RaycastHit2D hit = Physics2D.Raycast(
                    new Vector2(
                        Camera.main.ScreenToWorldPoint(clickLocation).x, 
                        Camera.main.ScreenToWorldPoint(clickLocation).y), 
                    Vector2.zero, 
                    0);
                if (hit)
                {
                    //If we hit something that isnt the player, freecam
                    if (!hit.collider.CompareTag("Player"))
                    {
                        //We used to have a target, make sure they are not selected.
                        target.transform.GetComponent<DragShotMover>().selfSelected = false;
                        //Remove the target.
                        target = null;
                    }
                }
                else
                {
                    //We used to have a target, make sure they are not selected.
                    target.transform.GetComponent<DragShotMover>().selfSelected = false;
                    //If we hit nothing
                    //Remove the target.
                    target = null;
                }
            }
        }
        //We don't have anything to Lockon to, find one!
        else
        {
            #region If Button pressed...
            if (GetComponent<MouseBehaviours>().CPgetTouch(0))
            #endregion
            {
                //Fire a ray in 2D space to check for player.
                RaycastHit2D hit = Physics2D.Raycast(
                    new Vector2(
                        Camera.main.ScreenToWorldPoint(clickLocation).x,
                        Camera.main.ScreenToWorldPoint(clickLocation).y),
                    Vector2.zero,
                    0);
                if (hit)
                {
                    //We clicked on a player, lock on to them.
                    if (hit.collider.CompareTag("Player"))
                    {
                        //We will deal with the dragging on the player end.
                        hit.transform.GetComponent<DragShotMover>().selfSelected = true;
                        target = hit.transform;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            #region Lockon to target
            Camera.main.orthographicSize = Mathf.LerpUnclamped(Camera.main.orthographicSize, defaultZoom, dampTime);

            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            #endregion
        }
        else
        {
            //Do FreeCam here.
            #region Allow Freecam
            if (Input.GetMouseButtonDown(1))
            {
                touchOrigin = GetComponent<MouseBehaviours>().CPgetTouchLocation(1);
            }
            if (Input.GetMouseButton(1))
            {
                
            }

            #endregion

            //Do Zoom here.
            #region Zoom

#if (UNITY_IOS || UNITY_ANDROID)
    //Run this for mobile.
    GetComponent<MouseBehaviours>().PinchZoom();
#else

            Camera.main.orthographicSize += Input.mouseScrollDelta.y;
    //Clamp Orthographic CameraSize
    Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 1f);
#endif
    #endregion
        }
    }
}