using UnityEngine;
using System.Collections;

public class DragShotMover : MonoBehaviour
{
    public bool canDrag;
    public bool selfSelected;

    [HideInInspector]
    public Vector3 startLocation;
    [HideInInspector]
    public Vector3 releaseLocation;
    Vector3 clickLocation;

    private void Start()
    {
        //Initialize all variables.
        canDrag = false;
        selfSelected = false;
    }

    private void Update()
    {
        #region Find out if the player is allowed to move.
        //usually use extremely small numbers, like 0 or < 0.1
        if (selfSelected && GetComponent<Rigidbody2D>().velocity.magnitude == 0) //< 0.1f)
        {
            canDrag = true;
        }
        else
        {
            canDrag = false;
            //startLocation = Vector3.zero;
            //releaseLocation = Vector3.zero;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        //Self is selected, camera is focused on you.
        if(selfSelected)
        {
            //If I am allowed movement...
            if(canDrag)
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

                RaycastHit2D hit = Physics2D.Raycast(
                    new Vector2(
                        Camera.main.ScreenToWorldPoint(clickLocation).x,
                        Camera.main.ScreenToWorldPoint(clickLocation).y),
                    Vector2.zero,
                    0);
                if(hit)
                {
                    #region Perform all drag operations here.
                    if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        HandleTouch();
                    }
                    else
                    {
                        HandleDrag();
                    }
                    #endregion
                }
            }
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                //store the touch location
                startLocation = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                //calculate and release
                releaseLocation = touch.position;
            }
        }
    }

    void HandleDrag()
    {
        if(Input.GetMouseButtonUp(0))
        {
            //calculate and release
            releaseLocation = Input.mousePosition;
        }
        else if(Input.GetMouseButtonDown(0))
        {
            //store the touch location
            startLocation = Input.mousePosition;
        }
    }
}
