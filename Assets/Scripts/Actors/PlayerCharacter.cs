namespace LD51
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public partial class PlayerCharacter : MonoBehaviour
    {
        [SerializeField]
        private List<string> keyItems;

        [SerializeField]
        private List<string> flags;

        private SimpleMovement _movement;
        private Animator _playerAnimator;

        private CapsuleCollider2D _playerCollider;
        private SpriteRenderer _spriteRenderer;

        public bool JustEnteredDoor { get; set; }

        public List<string> KeyItems
        {
            get { return this.keyItems; }
        }

        public SimpleMovement Movement
        {
            get
            {
                if (this._movement == null)
                    this._movement = GetComponent<SimpleMovement>();
                return this._movement;
            }
        }

        public CapsuleCollider2D PlayerCollider
        {
            get
            {
                if (this._playerCollider == null)
                    this._playerCollider = GetComponent<CapsuleCollider2D>();
                return this._playerCollider;
            }
        }

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (this._spriteRenderer == null)
                    this._spriteRenderer = GetComponentInChildren<SpriteRenderer>(includeInactive: true);
                return this._spriteRenderer;
            }
        }


        public IEnumerator ToggleDoorEnteredStatus()
        {
            yield return new WaitForSeconds(0.1f);
            this.JustEnteredDoor = false;
        }


        public void AddFlag(string flag)
        {
            if (this.flags.Contains(flag))
            {
                Debug.LogWarning($"Player already has flag: {flag}");
                return;
            }

            this.flags.Add(flag);
        }


        public void RemoveFlag(string flag)
        {
            if (!this.flags.Contains("flag"))
            {
                Debug.LogWarning($"Tried to remove flag player doesn't have: {flag}");
                return;
            }

            this.flags.Remove(flag);
        }


        public bool HasFlag(string flag)
        {
            return this.flags.Contains(flag);
        }


        public void TryDoor()
        {
            RaycastHit2D[] touchedTriggers = new RaycastHit2D[3];
            this.PlayerCollider.Cast(
                direction: Vector2.zero, 
                results: touchedTriggers, 
                distance: 0, 
                ignoreSiblingColliders: true);
            foreach (RaycastHit2D trigger in touchedTriggers)
            {
                if (trigger.transform == null)
                    continue;
                
                Door door = trigger.transform.GetComponent<Door>();
                if (door != null)
                {
                    Debug.Log($"Activating door: {door.name}");
                    door.Activate(this);
                    return;
                }
            }
        }

        public void TryPickUpItem()
        {
            RaycastHit2D[] touchedTriggers = new RaycastHit2D[3];
            this.PlayerCollider.Cast(
                direction: Vector2.zero, 
                results: touchedTriggers, 
                distance: 0, 
                ignoreSiblingColliders: true);
            foreach (RaycastHit2D trigger in touchedTriggers)
            {
                if (trigger.transform == null)
                    continue;
                
                IPickup item = trigger.transform.GetComponent<IPickup>();
                if (item != null)
                {
                    item.Pickup(this);
                    return;
                }
            }

            Debug.Log("No pickup found");
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class PlayerCharacter
    {
        [CustomEditor(typeof(PlayerCharacter))]
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