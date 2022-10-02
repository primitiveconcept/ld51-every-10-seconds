namespace LD51
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;


    [AddComponentMenu("_LD51/Door")]
    public partial class Door : MonoBehaviour
    {
        public string RequiredKey;
        public string[] RequiredKeys;
        public Transform TargetObject;
        public bool ActivateOnContact;
        public UnityEvent OnActivated;


        public Vector2 GetTargetPosition()
        {
            Collider2D collider = this.TargetObject.GetComponent<Collider2D>();
            if (collider == null)
                return this.TargetObject.transform.position;

            return new Vector2(
                collider.bounds.center.x,
                collider.bounds.min.y);
        }
        
        
        public void OnDrawGizmos()
        {
            Vector2 endpoint = GetTargetPosition();
            
            Color previousColor = Gizmos.color;
            
            if (!string.IsNullOrEmpty(this.RequiredKey))
                Gizmos.color = Color.red;
            else if (this.ActivateOnContact)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.yellow;
            
            if (this.TargetObject != null)
            {
                Debug.DrawLine(this.transform.position, endpoint, Color.green);
                Gizmos.DrawWireSphere(endpoint, 0.25f);
                
            }
            
            Collider2D collider = this.gameObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                if (collider is BoxCollider2D)
                {
                    Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
                }
            }
                
            Gizmos.color = previousColor;
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


        public void OnTriggerEnter2D(Collider2D col)
        {
            if (!this.ActivateOnContact)
                return;

            PlayerCharacter player = col.GetComponent<PlayerCharacter>();
            if (player != null
                && !player.JustEnteredDoor)
            {
                Activate(player);
            }
        }


        public void Activate(PlayerCharacter player)
        {
            if (!string.IsNullOrEmpty(this.RequiredKey)
                && !player.KeyItems.Contains(this.RequiredKey))
            {
                DenyEntry();
                return;
            }
            
            this.OnActivated.Invoke();

            Enter(player);
        }


        private void Enter(PlayerCharacter player)
        {
            
            
            if (this.TargetObject == null)
            {
                Debug.LogError("You forgot to assign TargetObject for this door!");
                return;
            }

            Room room = this.TargetObject.transform.GetComponentInParent<Room>();
            if (room == null)
            {
                Debug.LogError("You forgot to parent the TargetObject under a Room object!");
            }

            float playerFeetOffset = player.PlayerCollider.bounds.extents.y;
            Vector3 targetPosition = GetTargetPosition();
            player.transform.position = new Vector2(
                targetPosition.x,
                targetPosition.y + playerFeetOffset);
            player.JustEnteredDoor = true;
            player.StartCoroutine(player.ToggleDoorEnteredStatus());
            room.RefocusCamera(targetPosition);
        }


        private void DenyEntry()
        {
            Debug.Log($"Player lacks key item: {this.RequiredKey}");
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class Door
    {
        [CustomEditor(typeof(Door))]
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