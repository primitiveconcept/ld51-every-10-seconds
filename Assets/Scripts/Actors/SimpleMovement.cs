namespace LD51
{
    using UnityEngine;
    using UnityEngine.Events;
    

    [AddComponentMenu("_LD51/Simple Movement")]
    public partial class SimpleMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10f;

        [SerializeField]
        private bool locked;

        [SerializeField]
        private Vector2 moveDirection;

        [SerializeField]
        private Vector2 externalForce;
        private Rigidbody2D _rigidbody2D;
        private CharacterAnimation _characterAnimation;

        private Vector2 facingDirection;

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

        private Rigidbody2D Rigidbody2D
        {
            get
            {
                if (this._rigidbody2D == null)
                    this._rigidbody2D = CreateRigidbodyComponent();
                return this._rigidbody2D;
            }
        }

        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        public bool Locked
        {
            get { return this.locked; }
            set { this.locked = value; }
        }


        public void Awake()
        {
            this._characterAnimation = GetComponentInChildren<CharacterAnimation>(includeInactive: true);
        }


        public void Update()
        {
            if (this.Rigidbody2D.velocity.x != 0)
                this.facingDirection = this.Rigidbody2D.velocity.normalized;
            
            if (this.CharacterAnimation != null)
                UpdateAnimator();
        }


        public void FixedUpdate()
        {
            Vector2 totalMovement = this.moveDirection * this.speed; 
            totalMovement += this.externalForce;
            ApplyMovement(totalMovement);
            
            this.moveDirection = Vector2.zero;
        }


        private void UpdateAnimator()
        {
            if (this.CharacterAnimation.Animator == null
                || this.CharacterAnimation.Animator.runtimeAnimatorController == null)
            {
                return;
            }
            
            Vector2 velocity = this.Rigidbody2D.velocity;
            this.CharacterAnimation.IsMoving = velocity.x != 0;
            this.CharacterAnimation.Speed = this.speed;
            this.CharacterAnimation.FlipX = this.facingDirection.x < 0;
        }


        private Rigidbody2D CreateRigidbodyComponent()
        {
            Rigidbody2D rigidbody2D = this.gameObject.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0;
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            
            return rigidbody2D;
        }


        public void MoveLeft()
        {
            if (this.locked)
                return;
            
            this.moveDirection = Vector2.left;
        }


        public void MoveRight()
        {
            if (this.locked)
                return;
            this.moveDirection = Vector2.right;
        }


        private void ApplyMovement(Vector2 velocity)
        {
            this.Rigidbody2D.velocity = velocity;
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class SimpleMovement
    {
        [CustomEditor(typeof(SimpleMovement))]
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