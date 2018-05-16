using UnityEngine;
using UnityEngine.UI;

public class SmoothCamera2D : MonoBehaviour
{
    public float dampTime = 0.15f;
    public Transform target;

    Vector3 Velocity = Vector3.zero;
    Vector3 clickLocation;
    float defaultZoom;

    Vector2 xClamp;
    Vector2 yClamp;

    private void Start()
    {
        defaultZoom = Camera.main.orthographicSize;
    }

    private void Update()
    {
        ///Very important!
        #region Define the touch position (CrossPlatform)
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            if (Input.touchCount > 0)
            {
                clickLocation = Input.GetTouch(0).position;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickLocation = Input.mousePosition;
            }
        }
        #endregion
        //If we have something to lockon to...
        if (target)
        {
            target.transform.GetComponent<DragShotMover>().selfSelected = true;
            #region If Button pressed...
#if (!UNITY_STANDALONE)
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
                        target.transform.GetComponent<DragShotMover>().startLocation = Vector3.zero;
                        target.transform.GetComponent<DragShotMover>().releaseLocation = Vector3.zero;
                        //Remove the target.
                        target = null;
                    }
                }
                else
                {
                    //We used to have a target, make sure they are not selected.
                    target.transform.GetComponent<DragShotMover>().selfSelected = false;
                    target.transform.GetComponent<DragShotMover>().startLocation = Vector3.zero;
                    target.transform.GetComponent<DragShotMover>().releaseLocation = Vector3.zero;
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
#if (!UNITY_STANDALONE)
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
                        target.GetComponent<DragShotMover>().startLocation = Vector2.zero;
                        target.GetComponent<DragShotMover>().releaseLocation = Vector2.zero;
                        target.GetComponent<DragShotMover>().mockLocation = Vector2.zero;
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
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, defaultZoom, dampTime);

            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref Velocity, dampTime);

            Vector3 copyVec = transform.position;
            copyVec.x = Mathf.Clamp(copyVec.x, xClamp.x, xClamp.y);
            copyVec.y = Mathf.Clamp(copyVec.y, yClamp.x, yClamp.y);
            transform.position = copyVec;
            #endregion
        }
    }

    public void SetCameraClamp(Vector2 xclamp, Vector2 yclamp)
    {
        xClamp = xclamp;
        yClamp = yclamp;
    }
}