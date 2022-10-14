namespace LD51
{
    using System;
    using UnityEngine;


    [AddComponentMenu("_LD51/Hiding Spot")]
    public partial class HidingSpot : MonoBehaviour, 
                                      IInteractable
    {
        public PlayerCharacter PlayerCharacter;
        public float MovementWhileHiding = 1f;
        public bool RepositionPlayerWhileHiding = false;
        public Vector2 HidingPositionOffset = new Vector2(0, 0.01f);

        private Collider2D _collider;
        private IInteractable interactableImplementation;

        public Collider2D Collider
        {
            get
            {
                if (this._collider == null)
                    this._collider = GetComponent<Collider2D>();
                return this._collider;
            }
        }

        public bool ShouldHidePrompt
        {
            get { return this.PlayerCharacter != null; }
        }
        
        public void Interact(PlayerCharacter player)
        {
            player.GetComponent<Hiding>().TryHide();
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