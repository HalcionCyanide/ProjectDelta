using UnityEngine;
using UnityEngine.UI;

public class DragShotMover : MonoBehaviour
{
    public bool canDrag;
    public bool selfSelected;
    public float maximumShootPower = 1000f;
    public float minimumShootPower = 100f;

    GameObject powerArrow;
    Text waitText;

    [HideInInspector]
    public Vector2 startLocation;
    [HideInInspector]
    public Vector2 releaseLocation;
    [HideInInspector]
    public Vector2 mockLocation;

    Vector2 clickLocation;
    Vector2 resetLocation;

    private void Start()
    {
        //Initialize all variables.
        canDrag = false;
        selfSelected = true;
        powerArrow = transform.GetChild(0).gameObject;
        waitText = GameObject.FindGameObjectWithTag("UI_WAIT").GetComponent<Text>();
    }

    private void OnBecameInvisible()
    {
        transform.position = resetLocation;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void Update()
    {
        #region Find out if the player is allowed to move.
        //usually use extremely small numbers, like 0 or < 0.1
        if (Camera.main.GetComponent<CameraHandler>().target)
        {
            if (selfSelected && GetComponent<Rigidbody2D>().velocity.magnitude == 0) //< 0.1f)
            {
                canDrag = true;
                waitText.enabled = false;
            }
            else
            {
                canDrag = false;
                powerArrow.GetComponent<Renderer>().enabled = false;
                waitText.enabled = true;
            }
        }
        else
            waitText.enabled = false;
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
                if(hit && hit.collider.CompareTag("Player"))
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
                    //Drag Code here.
                    
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
                powerArrow.GetComponent<Renderer>().enabled = false;
                Feuer();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                //scale arrow
                powerArrow.GetComponent<Renderer>().enabled = true;
                mockLocation = touch.position;
                ScaleArrow();
            }
        }
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //store the touch location
            startLocation = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //calculate and release
            releaseLocation = Input.mousePosition;
            powerArrow.GetComponent<Renderer>().enabled = false;
            Feuer();
        }
        else if (Input.GetMouseButton(0))
        {
            //scale arrow
            powerArrow.GetComponent<Renderer>().enabled = true;
            mockLocation = Input.mousePosition;
            ScaleArrow();
        }
    }

    void Feuer()
    {
        resetLocation = transform.position;
        Vector2 shootDirection = -(releaseLocation - startLocation).normalized;

        float shootPower = Vector2.Distance(releaseLocation,startLocation);
        shootPower = Mathf.Clamp(shootPower, minimumShootPower, maximumShootPower);

        Debug.Log("Shot with power " + shootPower.ToString() + " in direction " + shootDirection);

        GetComponent<Rigidbody2D>().AddForce(new Vector2(shootDirection.x, shootDirection.y) * shootPower);
    }

    void ScaleArrow()
    {
        float shootPower = Vector2.Distance(mockLocation, startLocation);
        shootPower = Mathf.Clamp(shootPower, minimumShootPower, maximumShootPower);

        //scale factor is power represented as a percentage of power range, 
        //multiplied by the scaling range of the arrow (0, 1)
        float FactorToScaleBy = shootPower / maximumShootPower;

        powerArrow.transform.localScale = new Vector3(FactorToScaleBy, FactorToScaleBy, 1);
        Vector3 copyVec = powerArrow.transform.localScale;
        copyVec.x = Mathf.Clamp(copyVec.x, minimumShootPower / maximumShootPower, 1);
        copyVec.y = Mathf.Clamp(copyVec.y, minimumShootPower / maximumShootPower, 1);
        powerArrow.transform.localScale = copyVec;

        //calculate shooting direction
        Vector2 shootDirection = -(mockLocation - startLocation).normalized;
        float ang = Vector2.Angle(shootDirection, Vector2.right);
        Vector3 Angle = Vector3.Cross(shootDirection, Vector2.right);
        if (Angle.z > 0)
            ang = 360 - ang;
            
        powerArrow.transform.eulerAngles = new Vector3(0, 0, 180 + ang);
    }
}
