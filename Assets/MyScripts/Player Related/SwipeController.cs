using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour
{
    public bool tap, longTap;
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;
    private Vector2 endTouch;
    public float startTouchTime;
    public float endTouchTime;
    public float touchTime;
    public bool canUpdateLongTouch;
    private void Update()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = longTap = false;

        #region Standolone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            canUpdateLongTouch = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if (canUpdateLongTouch)
            {
                touchTime += Time.unscaledDeltaTime;
                endTouch = Input.mousePosition;
                if (touchTime > 0.5f && Vector2.Distance(endTouch, startTouch) < 100)
                {
                    if (!LevelManager.Instance.uIManager.buttonRun.isPressed)
                    {
                        canUpdateLongTouch = false;
                        touchTime = 0;
                        longTap = true;
                    }
                }
            }
        }


        else if (Input.GetMouseButtonUp(0))
        {
            touchTime = 0;
            isDraging = false;
            endTouchTime = Time.time;
            var difference = endTouchTime - startTouchTime;
            endTouch = Input.mousePosition;
            if (difference < 0.25f && Vector2.Distance(endTouch, startTouch) < 100)
                tap = true;
            Reset();
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length > 0)
        {
            if (canUpdateLongTouch == true)
            {
                touchTime += Time.unscaledDeltaTime;
            }
            endTouch = Input.mousePosition;

            if (touchTime > 0.5f && Vector2.Distance(endTouch, startTouch) < 100)
            {
                    if (!LevelManager.Instance.uIManager.buttonRun.isPressed)
                    {
                        touchTime = 0;
                        longTap = true;
                        canUpdateLongTouch = false;
                    }
                

            }


            if (Input.touches[0].phase == TouchPhase.Began)
            {
                canUpdateLongTouch = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
                startTouchTime = Time.time;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                touchTime = 0;
                endTouchTime = Time.time;
                var difference = endTouchTime - startTouchTime;
                endTouch = Input.touches[0].position;
                if (difference < 0.25f && Vector2.Distance(endTouch, startTouch) < 100)
                    tap = true;


                isDraging = false;
                Reset();
            }
        }
        #endregion

        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }



        if (swipeDelta.magnitude > 125)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x < 0)
                    swipeLeft = true;
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                if (y < 0)
                    swipeDown = true;
                else
                {
                    swipeUp = true;
                }
            }
            Reset();
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }



    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    public bool IsDragging { get { return isDraging; } }
    public bool Tap { get { return tap; } }

    public bool LongTap { get { return tap; } set { tap = value; } }
}
