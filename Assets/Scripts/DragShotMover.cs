using UnityEngine;
using System.Collections;

public class DragShotMover : MonoBehaviour
{
    public bool canDrag;
    public bool selfSelected;

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
                #region Perform all drag operations here.

                //Drag to launch code here.

                #endregion
            }
        }
    }
}
