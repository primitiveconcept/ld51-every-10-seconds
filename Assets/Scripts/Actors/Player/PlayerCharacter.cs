namespace LD51
{
    using System.Collections;
    using System.Collections.Generic;
    using LD51;
    using UnityEngine;


    [AddComponentMenu("_LD51/Player Character")]
    public partial class PlayerCharacter : MonoBehaviour,
                                           ICanEnterDoors
    {
        [SerializeField]
        private List<string> keyItems;

        [SerializeField]
        private List<string> flags;
        
        private CharacterAnimation _characterAnimation;
        private Collider2D _collider;
        private SimpleMovement _movement;
        private Animator _playerAnimator;
        private SpriteRenderer _spriteRenderer;

        public bool IsSpotted { get; set; }
        public bool IsCrawling { get; set; }
        
        public bool JustEnteredDoor { get; set; }

        public List<string> KeyItems
        {
            get { return this.keyItems; }
        }
        
        public bool WillEnterDoors
        {
            get { return true; }
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

        public Collider2D Collider
        {
            get
            {
                if (this._collider == null)
                    this._collider = GetComponent<Collider2D>();
                return this._collider;
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
        
        private CharacterAnimation CharacterAnimation
        {
            get
            {
#if UNITY_EDITOR
                // Lazy instantiation for Editor tools only -- otherwise, handled in Awake
                if (this._characterAnimation == null)
                    this._characterAnimation = GetComponentInChildren<CharacterAnimation>(includeInactive: true);
#endif
                return this._characterAnimation;
            }
        }


        public void Awake()
        {
            this._characterAnimation = GetComponentInChildren<CharacterAnimation>(includeInactive: true);
        }


        public void Update()
        {
            UpdateAnimator();
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
            this.Collider.Cast(
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
            this.Collider.Cast(
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


        private void UpdateAnimator()
        {
            if (this.CharacterAnimation.Animator == null
                || this.CharacterAnimation.Animator.runtimeAnimatorController == null)
            {
                return;
            }

            this.CharacterAnimation.IsCrawling = this.IsCrawling;
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
                
                PlayerCharacter playerCharacter = this.target as PlayerCharacter;

                EditorGUILayout.Toggle(nameof(PlayerCharacter.IsCrawling), playerCharacter.IsCrawling);
            }
        }
    }
}
#endif