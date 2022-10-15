namespace LD51
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;


    [AddComponentMenu("_LD51/Enemy")]
    public partial class Enemy : MonoBehaviour,
                                 ICanEnterDoors
    {
        [Tooltip("AI for the enemy")]
        public EnemyBehavior Behavior;
        
        [Tooltip("Whether player should die by merely touching monster")]
        public bool KillsPlayerOnContact;
        
        [Tooltip("Miscellaneous data to be stored on the monster during runtime.")]
        public List<string> Flags;
        
        [Tooltip("Actions to execute upon being hit by light")]
        public UnityEvent OnLit;
        
        [Tooltip("Actions to execute upon light no longer hitting monster")]
        public UnityEvent OnBecameUnlit;

        [Tooltip("Whether the monster may move through on-contact doors")]
        [SerializeField]
        private bool willEnterDoors;

        private Collider2D _collider;
        private SimpleMovement _movement;
        private Animator _playerAnimator;
        private SpriteRenderer _spriteRenderer;

        private Vector2 startingPosition;
        private bool suspendBehavior;

        public bool IsLit { get; private set; }

        public bool WillEnterDoors
        {
            get { return this.willEnterDoors; }
            set { this.willEnterDoors = value; }
        }

        public Vector2 StartingPosition
        {
            get
            {
                if (!Application.isPlaying)
                    return this.transform.position;
                    
                return this.startingPosition;
            }
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


        public void Awake()
        {
            this.startingPosition = this.transform.position;
            
            this.Behavior.Initialize(this);
        }


        public void Update()
        {
            if (!this.suspendBehavior)
                this.Behavior.Process(this);
        }


        public void OnCollisionEnter2D(Collision2D other)
        {
            if (this.KillsPlayerOnContact)
            {
                PlayerCharacter playerCharacter = other.gameObject.GetComponent<PlayerCharacter>();
                if (playerCharacter == null)
                    return;

                Debug.Log("DEATH TO PLAYER!");
            }
        }


        public void OnDrawGizmos()
        {
            if (this.Behavior != null)
                this.Behavior.OnDrawEnemyGizmos(this);
        }


        public void OnDrawGizmosSelected()
        {
            if (this.Behavior != null)
                this.Behavior.OnDrawEnemyGizmosSelected(this);
        }


        public bool JustEnteredDoor { get; set; }


        internal void GetLit()
        {
            this.IsLit = true;
            this.OnLit.Invoke();
        }


        internal void GetUnlit()
        {
            this.IsLit = false;
            this.OnBecameUnlit.Invoke();
        }


        public void KillAfter(float duration)
        {
            StartCoroutine(Coroutines.InvokeAfterDelay(
                duration: duration, 
                action: () => this.gameObject.SetActive(false)));
        }


        public void FleeFromPlayer(float duration)
        {
            this.suspendBehavior = true;
            Transform player = Game.FindPlayer().transform;

            StartCoroutine(
                Coroutines.InvokeAfterDelay(
                    duration: duration,
                    action: () => this.suspendBehavior = false));
            StartCoroutine(
                Coroutines.InvokeRepeatingUntil(
                    duration: duration,
                    repeatingAction: () =>
                        {
                            if (player.position.x < this.transform.position.x)
                                this.Movement.MoveRight();
                            else
                                this.Movement.MoveLeft();
                        }));
        }


        public void Stun(float duration)
        {
            
        }


        public void AddFlag(string flag)
        {
            if (this.Flags.Contains(flag))
            {
                Debug.LogWarning($"Player already has flag: {flag}");
                return;
            }

            this.Flags.Add(flag);
        }


        public void RemoveFlag(string flag)
        {
            if (!this.Flags.Contains("flag"))
            {
                Debug.LogWarning($"Tried to remove flag player doesn't have: {flag}");
                return;
            }

            this.Flags.Remove(flag);
        }


        public bool HasFlag(string flag)
        {
            return this.Flags.Contains(flag);
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class Enemy
    {
        [CustomEditor(typeof(Enemy))]
        public class Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                Enemy enemy = this.target as Enemy;
                enemy.IsLit = EditorGUILayout.Toggle(nameof(Enemy.IsLit), enemy.IsLit);
                enemy.suspendBehavior = EditorGUILayout.Toggle(nameof(Enemy.suspendBehavior), enemy.suspendBehavior);
            }
        }
    }
}
#endif