namespace LD51
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;


    public partial class Enemy : MonoBehaviour
    {
        public EnemyBehavior Behavior; 

        public bool KillsOnContact;

        public UnityEvent OnKilled;

        [SerializeField]
        private List<string> flags;
        private Collider2D _collider;
        private SimpleMovement _movement;
        private Animator _playerAnimator;
        private SpriteRenderer _spriteRenderer;

        private Vector2 startingPosition;


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
            this.Behavior.Process(this);
        }


        public void OnCollisionEnter2D(Collision2D other)
        {
            if (this.KillsOnContact)
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


        public void Kill()
        {
            this.OnKilled.Invoke();
            this.gameObject.SetActive(false);
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
            }
        }
    }
}
#endif