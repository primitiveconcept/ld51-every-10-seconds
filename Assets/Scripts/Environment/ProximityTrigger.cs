namespace LD51
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;


    [AddComponentMenu("_LD51/Proximity Trigger")]
    public partial class ProximityTrigger : MonoBehaviour
    {
        public TriggerEvent OnEntered;
        public TriggerEvent OnExited;
        public TriggerEvent WhileInside;


        public void OnDrawGizmos()
        {
            Collider2D collider = this.gameObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                if (collider is BoxCollider2D)
                    Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.extents);
            }
        }


        public void OnValidate()
        {
            Collider2D collider = this.gameObject.GetComponent<Collider2D>();
            if (collider != null
                && collider.isTrigger != true)
            {
                collider.isTrigger = true;
            }
        }


        public void OnTriggerEnter2D(Collider2D other)
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player == null)
                return;
            this.OnEntered.Invoke(other);
        }


        public void OnTriggerExit2D(Collider2D other)
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player == null)
                return;
            this.OnExited.Invoke(other);
        }


        public void OnTriggerStay2D(Collider2D other)
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player == null)
                return;
            this.WhileInside.Invoke(other);
        }


        public void Teleport(Transform targetTransform)
        {
            if (targetTransform == null)
            {
                Debug.LogError("You forgot to assign a target transform for this teleport!");
                return;
            }

            Room room = targetTransform.GetComponentInParent<Room>();
            if (room == null)
            {
                Debug.LogError("You forgot to parent the target transform under a Room object!");
            }

            Vector3 targetPosition = targetTransform.position;
            Game.FindPlayer().transform.position = targetPosition;
            room.RefocusCamera();
        }
    }


    [Serializable]
    public class TriggerEvent : UnityEvent<Collider2D> {}
}

#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class ProximityTrigger
    {
        [CustomEditor(typeof(ProximityTrigger))]
        public class Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
            }
        }
    }
}
#endif