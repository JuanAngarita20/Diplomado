using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action ReduceBattery;
    public static void triggerReduceBattery(){
        ReduceBattery?.Invoke();
    }

    public static event Action TurnOnVideo;
    public static void VideoStarted(){
        TurnOnVideo?.Invoke();
    }

    public static event Action TurnOffVideo;
    public static void VideoEnded(){
        TurnOffVideo?.Invoke();
    }

   

    


}
