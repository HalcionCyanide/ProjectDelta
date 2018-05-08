using UnityEngine;
using System.Collections;

public class DragShotMover : MonoBehaviour
{
    bool canDrag;

    public bool selfSelected;

    private void Start()
    {
        canDrag = false;
        selfSelected = false;
    }

    private void Update()
    {
        //If not moving.
        if (GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f)
        {
            canDrag = true;
        }
        else
        {
            canDrag = false;
        }
    }

    private void FixedUpdate()
    {
        if(canDrag)
        {

        }
    }
}
