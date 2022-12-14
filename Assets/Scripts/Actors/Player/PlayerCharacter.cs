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
        [Tooltip("Names of items needed to progress")]
        [SerializeField]
        private List<string> keyItems;

        [Tooltip("Miscellaneous data to be stored on the player during runtime.")]
        [SerializeField]
        private List<string> flags;

        [Tooltip("Specifically, the player's avatar sprite (do not confuse with button prompt sprite")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private CharacterAnimation _characterAnimation;
        private Collider2D _collider;
        private PlayerInput _input;
        private SimpleMovement _movement;
        private Animator _playerAnimator;

        public bool IsSpotted { get; set; }
        public bool IsCrawling { get; set; }

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

        public SpriteRenderer SpriteRenderer
        {
            get { return this.spriteRenderer; }
        }

        public PlayerInput Input
        {
            get
            {
                if (this._input == null)
                    this._input = GetComponent<PlayerInput>();
                return this._input;
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
            CheckForInteractions();
        }


        public bool JustEnteredDoor { get; set; }

        public bool WillEnterDoors
        {
            get { return true; }
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


        public void TryInteract()
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
                
                IInteractable interactable = trigger.transform.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    Debug.Log($"Interacting with: {interactable.name}");
                    interactable.Interact(this);
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


        private void CheckForInteractions()
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
                
                IPickup pickup = trigger.transform.GetComponent<IPickup>();
                if (pickup != null)
                {
                    this.Input.ShowPickupPrompt();
                    return;
                }
                
                IInteractable interactable = trigger.transform.GetComponent<IInteractable>();
                if (interactable != null
                    && !interactable.ShouldHidePrompt)
                {
                    this.Input.ShowInteractionPrompt();
                    return;
                }
            }
            
            this.Input.HidePrompt();
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

                EditorGUILayout.Toggle(nameof(IsCrawling), playerCharacter.IsCrawling);
            }
        }
    }
}
#endif