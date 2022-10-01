namespace LD51
{
    using System;
    using UnityEngine;

    [AddComponentMenu("_LD51/SimpleMovement")]
    public partial class SimpleMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10f;

        [SerializeField]
        private Vector2 moveDirection;

        [SerializeField]
        private Vector2 externalForce;

        private Rigidbody2D _rigidbody2D;
        private bool isLocked;

        private bool shouldMove;
        private bool wasMoving;

        private Rigidbody2D Rigidbody2D
        {
            get
            {
                if (this._rigidbody2D == null)
                    this._rigidbody2D = CreateRigidbodyComponent();
                return this._rigidbody2D;
            }
        }


        public void FixedUpdate()
        {
            Vector2 totalMovement = this.moveDirection * this.speed; 
            totalMovement += this.externalForce;
            ApplyMovement(totalMovement);
            
            this.moveDirection = Vector2.zero;
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
            this.moveDirection = Vector2.left;
        }


        public void MoveRight()
        {
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