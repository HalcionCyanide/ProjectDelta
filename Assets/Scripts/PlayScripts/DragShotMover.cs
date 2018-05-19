using UnityEngine;
using UnityEngine.UI;

public class DragShotMover : MonoBehaviour
{
    public bool selfSelected;
    public bool canDrag;

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
        powerArrow = transform.GetChild(0).gameObject;
        waitText = GameObject.FindGameObjectWithTag("UI_WAIT").GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        selfSelected = Camera.main.GetComponent<CameraHandler>().target == transform ? true : false;

        if (transform.position.x > GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelLoader>().width + 1 ||
            transform.position.x < -1 ||
            transform.position.y > GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelLoader>().height + 1 ||
            transform.position.y < -1)
        {
            transform.position = resetLocation;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if(!selfSelected)
        {
            
        }

        if(selfSelected && GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            canDrag = true;
        }
        else
        {
            canDrag = false;
        }

        //Self is selected, camera is focused on you.
        if (selfSelected)
        {
            //If I am allowed movement...
            if (canDrag)
            {
                RaycastHit2D hit = CheckIfClicked();
                if (hit && hit.collider.gameObject == gameObject)
                {
                    if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        HandleTouch();
                    }
                    else
                    {
                        HandleDrag();
                    }
                }
            }
            else
            {
                powerArrow.GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            powerArrow.GetComponent<Renderer>().enabled = false;
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
            if (touch.phase == TouchPhase.Ended)
            {
                //calculate and release
                releaseLocation = touch.position;
                Feuer();
                powerArrow.GetComponent<Renderer>().enabled = false;
            }
            if (touch.phase == TouchPhase.Moved)
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
        if (Input.GetMouseButtonUp(0))
        {
            //calculate and release
            releaseLocation = Input.mousePosition;
            powerArrow.GetComponent<Renderer>().enabled = false;
            Feuer();
        }
        if (Input.GetMouseButton(0))
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

        float shootPower = Vector2.Distance(releaseLocation,startLocation) * GameManagement.Instance.DragSensitivity;
        shootPower = Mathf.Clamp(shootPower, minimumShootPower, maximumShootPower);

        GetComponent<Rigidbody2D>().AddForce(new Vector2(shootDirection.x, shootDirection.y) * shootPower);
    }

    RaycastHit2D CheckIfClicked()
    {
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

        return hit;
    }

    void ScaleArrow()
    {
        float shootPower = Vector2.Distance(mockLocation, startLocation) * GameManagement.Instance.DragSensitivity;
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
