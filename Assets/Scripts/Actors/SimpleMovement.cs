namespace LD51
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;


    [AddComponentMenu("_LD51/SimpleMovement")]
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

        [SerializeField]
        private UnityEvent OnStartMovingLeft;
        
        [SerializeField]
        private UnityEvent OnStartMovingRight;

        [SerializeField]
        private UnityEvent OnStopMoving;


        private Vector2 previousVelocity;
        private Rigidbody2D _rigidbody2D;
        

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


        public void FixedUpdate()
        {
            Vector2 totalMovement = this.moveDirection * this.speed; 
            totalMovement += this.externalForce;
            ApplyMovement(totalMovement);
            
            this.moveDirection = Vector2.zero;
        }


        public void Update()
        {
            Vector2 velocity = this.Rigidbody2D.velocity;
            
            if (this.previousVelocity.x == 0)
            {
                if (velocity.x < 0)
                    this.OnStartMovingLeft.Invoke();
                else if (velocity.x > 0)
                    this.OnStartMovingRight.Invoke();
            }
            
            else if (this.previousVelocity.x != 0
                     && velocity.x == 0)
            {
                this.OnStopMoving.Invoke();
            }

            this.previousVelocity = this.Rigidbody2D.velocity;
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