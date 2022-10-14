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


        public void RefocusCamera()
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


        public static Room GetClosest(Vector2 position)
        {
            Room[] allRooms = FindObjectsOfType<Room>();
            Room closestRoom = null;
            float leastDistance = Mathf.Infinity;
            foreach (Room room in allRooms)
            {
                float distance = Vector2.Distance(room.transform.position, position);
                if (distance < leastDistance)
                {
                    leastDistance = distance;
                    closestRoom = room;
                }
            }

            return closestRoom;
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;
    using UnityEditor.Events;
    using UnityEngine;
    using UnityEngine.Events;


    partial class Room
    {
        [CustomEditor(typeof(Room))]
        public class Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                
                GUILayout.Space(30f);

                if (GUILayout.Button("Create Hiding Spot"))
                {
                    CreateHidingSpot();
                }
                
                if (GUILayout.Button("Create Key Item Pickup"))
                {
                    CreateKeyItemPickup();
                }

                if (GUILayout.Button("Create Interactable Item"))
                {
                    CreateInteractable();
                }
                
                if (GUILayout.Button("Create Door Pair"))
                {
                    MultiButtonPopup.Show(
                        "What type of door?",
                        ("To New Room", CreateDoorToNewRoom),
                        ("To Existing Room", CreateDoorToExistingRoom)
                    );
                }
            }


            private void CreateInteractable()
            {
                TextInputPopup.Show(
                    "Enter item name",
                    name =>
                        {
                            Room room = this.target as Room;
                            GameObject newItem = new GameObject(name);
                            newItem.transform.SetParent(room.transform);
                            newItem.transform.localPosition = Vector3.zero;
                            SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();
                            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/_Debug/debug-item.png");
                            spriteRenderer.sprite = sprite;
                            BoxCollider2D collider = newItem.AddComponent<BoxCollider2D>();
                            collider.isTrigger = true;
                            InteractableTrigger interactableTrigger =
                                newItem.AddComponent<InteractableTrigger>();
                            UnityEventTools.AddPersistentListener(interactableTrigger.OnInteract);
                            Selection.activeGameObject = newItem;
                        });
            }


            private void CreateHidingSpot()
            {
                Room room = this.target as Room;
                GameObject newItem = new GameObject("Hiding Spot");
                newItem.transform.SetParent(room.transform);
                newItem.transform.localPosition = Vector3.zero;
                
                SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/_Debug/debug-hiding-spot.png");
                spriteRenderer.sprite = sprite;
                BoxCollider2D collider = newItem.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
                HidingSpot hidingSpot = newItem.AddComponent<HidingSpot>();
                Selection.activeGameObject = newItem;
                        
            }


            private void CreateKeyItemPickup()
            {
                TextInputPopup.Show(
                    "Enter key item name",
                    name =>
                        {
                            Room room = this.target as Room;
                            GameObject newItem = new GameObject(name);
                            newItem.transform.SetParent(room.transform);
                            newItem.transform.localPosition = Vector3.zero;
                            SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();
                            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/_Debug/debug-item.png");
                            spriteRenderer.sprite = sprite;
                            BoxCollider2D collider = newItem.AddComponent<BoxCollider2D>();
                            collider.isTrigger = true;
                            KeyItemPickup pickup = newItem.AddComponent<KeyItemPickup>();
                            Selection.activeGameObject = newItem;
                        });
            }


            private void CreateDoorToNewRoom()
            {
                GameWorld gameWorld = FindObjectOfType<GameWorld>();
                TextInputPopup.Show("Enter new room name",
                    roomName =>
                        {
                            GameObject newRoom = GameWorld.Inspector.CreateRoom(gameWorld, roomName);
                            
                            Door firstDoor = CreateDoor($"Door to {newRoom.name}");
                            Door secondDoor = CreateDoor($"Door to {this.target.name}");

                            firstDoor.TargetObject = secondDoor.transform;
                            firstDoor.ActivateOnContact = true;
                            
                            secondDoor.TargetObject = firstDoor.transform;
                            secondDoor.ActivateOnContact = true;
                            secondDoor.transform.SetParent(newRoom.transform);
                            secondDoor.transform.localPosition = Vector3.zero;
                            
                            Selection.activeGameObject = secondDoor.gameObject;
                        });
            }


            private void CreateDoorToExistingRoom()
            {
                
                TextInputPopup.Show("Enter existing room name",
                    roomName =>
                        {
                            GameObject existingRoom = GameObject.Find(roomName);
                            if (existingRoom == null)
                            {
                                Debug.LogError($"Could not find room with name {existingRoom}!");
                                return;
                            }
                            
                            Door firstDoor = CreateDoor($"Door to {roomName}");
                            Door secondDoor = CreateDoor($"Door to {this.target.name}");

                            firstDoor.TargetObject = secondDoor.transform;

                            secondDoor.TargetObject = firstDoor.transform;
                            secondDoor.transform.SetParent(existingRoom.transform);
                            secondDoor.transform.localPosition = Vector3.zero;
                            
                            Selection.activeGameObject = secondDoor.gameObject;
                        });
            }


            private Door CreateDoor(string name)
            {
                Room room = this.target as Room;
                GameObject newDoorObject = new GameObject(name);
                newDoorObject.transform.SetParent(room.transform);
                newDoorObject.transform.localPosition = new Vector2(0, -2.5f);
                
                
                SpriteRenderer spriteRenderer = newDoorObject.AddComponent<SpriteRenderer>();
                Sprite roomSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/_Debug/debug-door.png");
                spriteRenderer.sprite = roomSprite;
                spriteRenderer.sortingLayerName = "BackWall";
                spriteRenderer.color = Color.clear;

                BoxCollider2D collider = newDoorObject.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
                    
                return newDoorObject.AddComponent<Door>();
            }
        }
    }
}
#endif