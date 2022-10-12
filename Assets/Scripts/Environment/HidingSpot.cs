namespace LD51
{
    using System;
    using UnityEngine;


    [AddComponentMenu("_LD51/Hiding Spot")]
    public partial class HidingSpot : MonoBehaviour
    {
        public PlayerCharacter PlayerCharacter;
        public float MovementWhileHiding = 1f;
        public bool RepositionPlayerWhileHiding = false;
        public Vector2 HidingPositionOffset = new Vector2(0, 0.01f);

        private Collider2D _collider;


        public Collider2D Collider
        {
            get
            {
                if (this._collider == null)
                    this._collider = GetComponent<Collider2D>();
                return this._collider;
            }
        }


        public void OnTriggerStay2D(Collider2D other)
        {
            if (this.PlayerCharacter != null)
                return;
            
            PlayerInput playerInput = other.GetComponent<PlayerInput>();
            if (playerInput == null)
                return;
            
            playerInput.ShowInteractionPrompt();
        }
        
        public void OnTriggerExit2D(Collider2D other)
        {
            PlayerInput playerInput = other.GetComponent<PlayerInput>();
            if (playerInput == null)
                return;
            
            playerInput.HidePrompt();
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