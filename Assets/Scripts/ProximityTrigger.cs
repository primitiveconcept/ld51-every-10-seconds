namespace LD51
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;


    [AddComponentMenu("_LD51/ProximityTrigger")]
    public partial class ProximityTrigger : MonoBehaviour
    {
        public TriggerEvent OnEntered;
        public TriggerEvent OnExited;
        public TriggerEvent WhileInside;

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
            this.OnEntered.Invoke(other);
        }


        public void OnTriggerExit2D(Collider2D other)
        {
            this.OnExited.Invoke(other);
        }


        public void OnTriggerStay2D(Collider2D other)
        {
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
            Game.PlayerCharacter.transform.position = targetPosition;
            room.RefocusCamera(targetPosition);
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