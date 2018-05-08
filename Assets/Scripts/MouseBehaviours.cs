using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviours : MonoBehaviour {

    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    public Vector3 CPgetTouchLocation(int value)
    {
#if (UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
#else
        if (Input.GetMouseButtonDown(value))
        {
            return Input.mousePosition;
        }
        return Vector3.zero;
#endif
    }

    public bool CPgetTouch(int value)
    {
#if (UNITY_IOS || UNITY_ANDROID)
        return (Input.touchCount > 0);
#else
        return (Input.GetMouseButtonDown(value));
#endif
    }

    public void PinchZoom()
    {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (Camera.main.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
            }
        }
    }
}
