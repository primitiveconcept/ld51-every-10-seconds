namespace LD51
{
    using System.Collections;
    using UnityEngine;


    public interface ICanEnterDoors
    {
        bool WillEnterDoors { get; }
        bool JustEnteredDoor { get; set; }
        Collider2D Collider { get; }
        Transform transform { get; }
        Coroutine StartCoroutine(IEnumerator toggleDoorEnteredStatus);
    }
}