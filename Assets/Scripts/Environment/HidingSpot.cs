namespace LD51
{
    using UnityEngine;


    public partial class HidingSpot : MonoBehaviour
    {
        public PlayerCharacter PlayerCharacter;
        public float MovementWhileHiding = 1f;
        public bool RepositionPlayerWhileHiding = false;
        public Vector2 HidingPositionOffset = new Vector2(0, 0.01f);

        private Collider2D _collider;
        private float originalMovementSpeed;

        private Vector2 originalPlayerPosition;

        private Collider2D Collider
        {
            get
            {
                if (this._collider == null)
                    this._collider = GetComponent<Collider2D>();
                return this._collider;
            }
        }

        public bool InUse
        {
            get { return this.PlayerCharacter != null; }
        }


        public void Update()
        {
            if (!this.InUse)
                return;

            Bounds playerBounds = this.PlayerCharacter.PlayerCollider.bounds;
            Bounds hidingSpotBounds = this.Collider.bounds;

            if (playerBounds.max.x < hidingSpotBounds.min.x
                || playerBounds.min.x > hidingSpotBounds.max.x)
            {
                UnHide(this.PlayerCharacter);
            }
        }


        public void Hide(PlayerCharacter playerCharacter)
        {
            // Center player on object
            if (this.RepositionPlayerWhileHiding)
            {
                this.originalPlayerPosition = playerCharacter.transform.position;
                playerCharacter.transform.position = new Vector2(
                    this.transform.position.x + this.HidingPositionOffset.x,
                    playerCharacter.transform.position.y + this.HidingPositionOffset.y);    
            }
            
            playerCharacter.SpriteRenderer.sortingLayerName = "Hiding";
            playerCharacter.SpriteRenderer.color = Color.grey;

            this.originalMovementSpeed = playerCharacter.Movement.Speed;
            playerCharacter.Movement.Speed = this.MovementWhileHiding;

            this.PlayerCharacter = playerCharacter;
        }


        public void UnHide(PlayerCharacter playerCharacter)
        {
            if (this.RepositionPlayerWhileHiding)
            {
                playerCharacter.transform.position = new Vector2(
                    playerCharacter.transform.position.x,
                    this.originalPlayerPosition.y);    
            }
            
            playerCharacter.SpriteRenderer.sortingLayerName = "Default";
            playerCharacter.SpriteRenderer.color = Color.white;
            
            playerCharacter.Movement.Speed = this.originalMovementSpeed;

            this.PlayerCharacter = null;
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class HidingSpot
    {
        [CustomEditor(typeof(HidingSpot))]
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