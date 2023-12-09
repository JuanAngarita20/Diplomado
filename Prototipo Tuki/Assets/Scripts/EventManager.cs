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


    public static event Action<int> AccionInterruptor; 
    public static void InterruptorTrigger(int id){
        AccionInterruptor?.Invoke(id);
    }


    public static event Action DamageObject; 
    public static void GnawObject(){
        DamageObject?.Invoke();
    }

    public static event Action StopMovForAnim; 
    public static void ActionStopMovement(){
       StopMovForAnim?.Invoke();
    }


    public static event Action RestartMovAfterAnim; 
    public static void RestartMovement(){
       RestartMovAfterAnim?.Invoke();
    }

    


}
