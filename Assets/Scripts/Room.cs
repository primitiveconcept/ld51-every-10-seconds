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
                
                GUILayout.Space(30f);
                
                if (GUILayout.Button("Create Door Pair"))
                {
                    Door firstDoor = CreateDoor("Door to Next Room", -1f);
                    Door secondDoor = CreateDoor($"Door Back to {this.target.name}", 1f);

                    firstDoor.TargetObject = secondDoor.transform;
                    secondDoor.TargetObject = firstDoor.transform;
                    Selection.activeGameObject = firstDoor.gameObject;
                }
            }


            private Door CreateDoor(string name, float xPosition)
            {
                Room room = this.target as Room;
                GameObject newDoorObject = new GameObject(name);
                newDoorObject.transform.SetParent(room.transform);
                newDoorObject.transform.localPosition = new Vector2(xPosition, -2.5f);
                
                
                SpriteRenderer spriteRenderer = newDoorObject.AddComponent<SpriteRenderer>();
                Sprite roomSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/_Debug/debug-door.png");
                spriteRenderer.sprite = roomSprite;
                /*
                spriteRenderer.size = new Vector2(
                    Game.PixelPerfectCamera.refResolutionX / 100f, 
                    Game.PixelPerfectCamera.refResolutionY / 100f);
                    */
                spriteRenderer.sortingLayerName = "BackWall";

                BoxCollider2D collider = newDoorObject.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
                    
                return newDoorObject.AddComponent<Door>();
            }
        }
    }
}
#endif