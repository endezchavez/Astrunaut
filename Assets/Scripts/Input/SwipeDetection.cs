using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwipeDetection : MonoBehaviour
{
    public enum SwipeDir { UP, DOWN, NONE};

    #region Events
    public delegate void SwipeUp();
    public event SwipeUp OnSwipeUp;
    public delegate void SwipeDown();
    public event SwipeDown OnSwipeDown;
    #endregion



    [SerializeField]
    private float minDistance = 0.1f;
    [SerializeField]
    private float maxTime = 1f;
    [SerializeField, Range(0, 1)]
    private float directionThreshold = 0.9f;

    private Vector2 startPos;
    private float startTime;

    private Vector2 endPos;
    private float endTime;

    private void OnEnable()
    {
        InputManager.Instance.OnStartTouch += SwipeStart;
        InputManager.Instance.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnStartTouch -= SwipeStart;
        InputManager.Instance.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 pos, float time)
    {
        startPos = pos;
        startTime = time;
    }

    private void SwipeEnd(Vector2 pos, float time)
    {
        endPos = pos;
        endTime = time;
        DetectSwipe();
    }

    public void DetectSwipe()
    {
        if(Vector3.Distance(startPos, endPos) >= minDistance && (endTime -startTime) <= maxTime)
        {
            Vector3 dir = endPos - startPos;
            Vector2 dir2D = new Vector2(dir.x, dir.y).normalized;
            SwipeDirection(dir2D);
        }

    }

    private void SwipeDirection(Vector2 dir)
    {
        if(Vector2.Dot(Vector2.up, dir) > directionThreshold)
        {
            if(OnSwipeUp != null)
            {
                OnSwipeUp();
            }
        }
        else if (Vector2.Dot(Vector2.down, dir) > directionThreshold)
        {
            if (OnSwipeDown != null)
            {
                OnSwipeDown();
            }
        }

    }
}
