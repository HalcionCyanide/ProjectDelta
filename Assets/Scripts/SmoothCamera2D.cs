using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    Vector3 clickLocation;

    private void Update()
    {
    #region CROSS-DEVICE STUFF
#if (UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount > 0)
        {
            clickLocation = Input.GetTouch(0).position;
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            clickLocation = Input.mousePosition;
        }
#endif
    #endregion
        if (target)
        {
            target.transform.GetComponent<DragShotMover>().selfSelected = true;

            #region IF BUTTON PRESS
#if (UNITY_IOS || UNITY_ANDROID)
            if (Input.touchCount > 0)
#else
            if (Input.GetMouseButtonDown(0))
#endif
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
        else
        {
            #region IF BUTTON PRESS
#if (UNITY_IOS || UNITY_ANDROID)
            if (Input.touchCount > 0)
#else
            if (Input.GetMouseButtonDown(0))
#endif
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
            #region FOCUS CAM
            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            #endregion
        }
        else
        {
            #region FREE CAM
            //Do FreeCam here.
            //Do PinchToZoom here.
            #endregion
        }
    }
}