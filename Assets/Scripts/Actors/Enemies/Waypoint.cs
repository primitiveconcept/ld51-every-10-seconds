namespace LD51
{
    using System;
    using UnityEngine;


    [Serializable]
    public class Waypoint
    {
        [Tooltip("Coordinates are relative to starting position (local space)")]
        public Vector2 Destination;
        
        [Tooltip("How many seconds to wait/idle before moving to next waypoint")]
        public float WaitDuration;
    }
}
