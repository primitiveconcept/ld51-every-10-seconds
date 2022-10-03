namespace LD51
{
    using UnityEngine;


    public partial class Hiding : MonoBehaviour
    {
        private HidingSpot currentHidingSpot;
        private float originalMovementSpeed;
        private Vector2 originalPlayerPosition;
        
        private PlayerCharacter _player;

        private PlayerCharacter Player
        {
            get
            {
                if (this._player == null)
                    this._player = GetComponent<PlayerCharacter>();
                return this._player;
            }
        }

        private bool IsHiding
        {
            get { return this.currentHidingSpot != null; }
        }
        
        public void TryHide()
        {
            if (this.IsHiding)
                return;
            
            RaycastHit2D[] touchedTriggers = new RaycastHit2D[3];
            this.Player.PlayerCollider.Cast(
                direction: Vector2.zero, 
                results: touchedTriggers, 
                distance: 0, 
                ignoreSiblingColliders: true);
            foreach (RaycastHit2D trigger in touchedTriggers)
            {
                if (trigger.transform == null)
                    continue;
                
                HidingSpot hidingSpot = trigger.transform.GetComponent<HidingSpot>();
                if (hidingSpot != null)
                {
                    Debug.Log($"Hiding behind: {hidingSpot.name}");
                    Hide(hidingSpot);
                    return;
                }
            }
        }

        
        public void Update()
        {
            if (!this.IsHiding)
                return;

            Bounds playerBounds = this.Player.PlayerCollider.bounds;
            Bounds hidingSpotBounds = this.currentHidingSpot.Collider.bounds;

            if (playerBounds.max.x < hidingSpotBounds.min.x
                || playerBounds.min.x > hidingSpotBounds.max.x)
            {
                UnHide();
            }
        }
        
        public void UnHide()
        {
            if (!this.IsHiding)
                return;
            
            if (this.currentHidingSpot.RepositionPlayerWhileHiding)
            {
                this.Player.transform.position = new Vector2(
                    this.Player.transform.position.x,
                    this.originalPlayerPosition.y);    
            }
            
            this.Player.SpriteRenderer.sortingLayerName = "Default";
            this.Player.SpriteRenderer.color = Color.white;
            
            this.Player.Movement.Speed = this.originalMovementSpeed;
            this.currentHidingSpot = null;
        }
        
        public void Hide(HidingSpot hidingSpot)
        {
            // Center player on object
            if (hidingSpot.RepositionPlayerWhileHiding)
            {
                this.originalPlayerPosition = this.Player.transform.position;
                this.Player.transform.position = new Vector2(
                    this.transform.position.x + hidingSpot.HidingPositionOffset.x,
                    this.Player.transform.position.y + hidingSpot.HidingPositionOffset.y);    
            }
            
            this.Player.SpriteRenderer.sortingLayerName = "Hiding";
            this.Player.SpriteRenderer.color = Color.grey;

            this.originalMovementSpeed = this.Player.Movement.Speed;
            this.Player.Movement.Speed = hidingSpot.MovementWhileHiding;

            this.currentHidingSpot = hidingSpot;
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class Hiding
    {
        [CustomEditor(typeof(Hiding))]
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