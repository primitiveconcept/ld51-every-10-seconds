namespace LD51
{
    using UnityEngine;


    public partial class HidingSpot : MonoBehaviour
    {
        public bool InUse;

        public Vector2 HidingPositionOffset = new Vector2(0, 0.01f);

        private Vector2 originalPlayerPosition;


        public void Hide(PlayerCharacter playerCharacter)
        {
            // Center player on object
            this.originalPlayerPosition = playerCharacter.transform.position;
            playerCharacter.transform.position = new Vector2(
                this.transform.position.x + this.HidingPositionOffset.x,
                playerCharacter.transform.position.y + this.HidingPositionOffset.y);
            
            playerCharacter.SpriteRenderer.sortingLayerName = "Hiding";
            playerCharacter.SpriteRenderer.color = Color.grey;

            playerCharacter.Movement.Locked = true;
            
            this.InUse = true;
        }


        public void UnHide(PlayerCharacter playerCharacter)
        {
            playerCharacter.transform.position = new Vector2(
                playerCharacter.transform.position.x,
                this.originalPlayerPosition.y);
            
            playerCharacter.SpriteRenderer.sortingLayerName = "Default";
            playerCharacter.SpriteRenderer.color = Color.white;
            
            playerCharacter.Movement.Locked = false;
            
            this.InUse = false;
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