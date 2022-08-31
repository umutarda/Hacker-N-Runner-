using System.Collections;
using UnityEngine;

public abstract class HackObject : MonoBehaviour
{
    protected abstract void HackGame(out bool gameWin, out bool gameLost, float timePassed);

    public IEnumerator HackStage() 
    {
        float stageStartTime = Time.time;
        bool gameWin = false, gameLost = false;

        while (!gameWin && !gameLost)
        {
            HackGame(out gameWin, out gameLost, Time.time - stageStartTime);
            yield return null;
        }

        yield return 0;
    }

}
