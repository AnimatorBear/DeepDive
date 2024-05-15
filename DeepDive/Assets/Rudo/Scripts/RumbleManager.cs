using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    //Allows you to call the manager without having to find its object
    public static RumbleManager instance;

    //current Gamepad
    private Gamepad pad;

    private Coroutine stopRumbleCouratine;

    public float currentDuration = 0;

    void Start()
    {
        //Sets the static instance to be this object
        if(instance == null)
        {
            instance = this;
        }  
    }
 
    /// <summary>
    /// Max Freq = 1;
    /// </summary>
    /// <param name="lowFreq"></param>
    /// <param name="highFreq"></param>
    /// <param name="duration"></param>
    public void RumblePulse(float lowFreq,float highFreq,float duration)
    {
        //Sets the gamepad
        pad = Gamepad.current;

        if(pad != null)
        {
            //Makes it shake
            pad.SetMotorSpeeds(lowFreq,highFreq);

            //Calls the stop shake
            stopRumbleCouratine = StartCoroutine(StopRumble(duration,pad));
        }

    }

    
    private IEnumerator StopRumble(float duration,Gamepad pad)
    {
        //Timer (I dont know why the tutorial guy didnt use the normal IEnumerator thingy but hey)
        float elapsedTime = 0;
        if(currentDuration < 0)
        {
            currentDuration = duration;
        }
        else
        {
            currentDuration += duration;
        }
        while(elapsedTime < currentDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Stops the shake
        pad.SetMotorSpeeds(0, 0);
    }
}
