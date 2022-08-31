using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : HackObject
{
    [SerializeField] private float angleErrorMargin;
    [SerializeField] string[] moves = {"right","left","up","down"};
    [SerializeField] int[] angles = { 0, 180, 90, -90};
    [SerializeField] int count;
    [SerializeField] static bool computerControls = true;
    Vector2 startTouchPos = -Vector2.one;
    int lastMove = -1;
    int counter;
    protected override void HackGame(out bool gameWin, out bool gameLost, float timePassed) 
    {
        gameWin = false;
        gameLost = false;

        if (lastMove == -1) 
        {
            if (count == counter) 
            {
                gameWin = true;
                return;
            }
                
            lastMove = Random.Range(0, 4);
            counter++;
            Debug.Log("Move is: " + moves[lastMove]);
        }

        if (computerControls) 
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                startTouchPos = Input.mousePosition;
            }
                
            else if (Input.GetMouseButtonUp(0)) 
            {
                if (startTouchPos.x != -1)
                {
                    if (!Evaluate(Input.mousePosition))
                        gameLost = true;
                }
            }
        }

        else 
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                    startTouchPos = touch.position;
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (startTouchPos.x != -1)
                    {
                        if (!Evaluate(touch.position))
                            gameLost = true;
                    }


                }
            }
        }
        
        
    }

    private bool Evaluate(Vector2 endPointerPosition)
    {
        float delta = angles[lastMove] - (lastMove != 1 ? Vector2.SignedAngle(Vector2.right, endPointerPosition - startTouchPos)
                        : Vector2.Angle(Vector2.right, endPointerPosition - startTouchPos));

        lastMove = -1;
        startTouchPos = -Vector2.one;

        if (Mathf.Abs(delta) <= angleErrorMargin)
        {
            Debug.Log("Success with " + delta);
            
            return true;
        }

        else
        {
            Debug.Log("Fail with " + delta);
            counter = 0;
            return false;
        }


        
    }

}

