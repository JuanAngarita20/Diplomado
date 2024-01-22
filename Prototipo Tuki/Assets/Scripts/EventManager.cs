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


    public static event Action<float> DamageObject; 
    public static void GnawObject(float id){
        DamageObject?.Invoke(id);
    }

    //Eventos animaciones
    public static event Action AnimGnaw; 
    public static void StartGnawAnim(){
        AnimGnaw?.Invoke();
    }

    public static event Action StopMovForAnim; 
    public static void ActionStopMovement(){
       StopMovForAnim?.Invoke();
    }


    public static event Action RestartMovAfterAnim; 
    public static void RestartMovement(){
       RestartMovAfterAnim?.Invoke();
    }


    public static event Action StopInteractionForPushing; 
    public static void CurrentlyPushing(){
      StopInteractionForPushing?.Invoke();
    }

    public static event Action RestartInteractionForPushing; 
    public static void NolongerPushing(){
      RestartInteractionForPushing?.Invoke();
    }



    //Eventos Bateria
    public static event Action StartLosingControl; 
    public static void BatteryOver80(){
        StartLosingControl?.Invoke();
    }

    public static event Action RegainControl; 
    public static void BatteryUnder80(){
        RegainControl?.Invoke();
    }

    public static event Action GameOver; 
    public static void BatteryReach100(){
        GameOver?.Invoke();
    }

    public static event Action ZoneGameOver; 
    public static void TriggerZoneGameOver(){
        ZoneGameOver?.Invoke();
    }


    public static event Action FinisehdLevel1; 
    public static void TriggerFinisehdLevel1(){
        FinisehdLevel1?.Invoke();
    }

    

    


}
