namespace LD51
{
    using System;
    using System.Collections;
    using UnityEngine;


    public static class Coroutines
    {
        public static IEnumerator InvokeAfterDelay(float duration, Action action)
        {
            yield return new WaitForSeconds(duration);
            action.Invoke();
        }


        public static IEnumerator InvokeRepeatingUntil(float duration, Action repeatingAction)
        {
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                repeatingAction.Invoke();
                yield return 0;
            }
        }
        
        public static IEnumerator ToggleDoorEnteredStatus(ICanEnterDoors doorActivator)
        {
            yield return new WaitForSeconds(0.1f);
            doorActivator.JustEnteredDoor = false;
        }
    }
}