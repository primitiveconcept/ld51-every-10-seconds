namespace LD51
{
    using UnityEngine;


    [AddComponentMenu("_LD51/Room")]
    public partial class Room : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        public Bounds Bounds
        {
            get
            {
                return this.SpriteRenderer.bounds;
            }
        }

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (this._spriteRenderer == null)
                    this._spriteRenderer = GetComponent<SpriteRenderer>();
                return this._spriteRenderer;
            }
        }


        public void RefocusCamera(Vector2 targetPosition)
        {
            Transform cameraTransform = Game.Camera.transform;
            Vector3 roomPosition = this.transform.position;
            cameraTransform.position = new Vector3(
                roomPosition.x,
                roomPosition.y,
                cameraTransform.position.z);
            
            /* Will need to get this working to allow for scrolling.
            Transform cameraTransform = Game.Camera.transform;
            
            float verticalExtent = Game.Camera.orthographicSize;
            float horizontalExtent = verticalExtent * Screen.width / Screen.height;
            Debug.Log($"Vertical: {verticalExtent}, Horizontal: {horizontalExtent}");

            float leftBound = this.Bounds.min.x + horizontalExtent;
            float rightBound = this.Bounds.max.x - horizontalExtent;
            float bottomBound = this.Bounds.min.y + verticalExtent;
            float topBound = this.Bounds.max.y - verticalExtent;

            Vector3 finalCameraPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, leftBound, rightBound),
                Mathf.Clamp(targetPosition.y, bottomBound, topBound),
                cameraTransform.position.z);

            cameraTransform.position = finalCameraPosition;
            */
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;
    using UnityEngine;


    partial class Room
    {
        [CustomEditor(typeof(Room))]
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