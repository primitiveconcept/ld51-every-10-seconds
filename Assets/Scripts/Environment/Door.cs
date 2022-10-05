namespace LD51
{
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

            ICanEnterDoors activator = col.GetComponent<ICanEnterDoors>();
            if (activator != null
                && !activator.JustEnteredDoor)
            {
                Activate(activator);
            }
        }


        public void Activate(ICanEnterDoors activator)
        {
            if (activator is PlayerCharacter playerCharacter)
            {
                if (!string.IsNullOrEmpty(this.RequiredKey)
                    && !playerCharacter.KeyItems.Contains(this.RequiredKey))
                {
                    DenyEntry();
                    return;
                }    
            }
            
            this.OnActivated.Invoke();

            Enter(activator);
        }


        private void Enter(ICanEnterDoors activator)
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

            float feetOffset = activator.Collider.bounds.extents.y;
            Vector3 targetPosition = GetTargetPosition();
            activator.transform.position = new Vector2(
                targetPosition.x,
                targetPosition.y + feetOffset);
            activator.JustEnteredDoor = true;
            activator.StartCoroutine(Coroutines.ToggleDoorEnteredStatus(activator));
            
            if (activator is PlayerCharacter)
                room.RefocusCamera();
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