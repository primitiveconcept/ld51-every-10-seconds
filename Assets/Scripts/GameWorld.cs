namespace LD51
{
    using UnityEngine;


    [AddComponentMenu("_LD51/GameWorld")]
    public partial class GameWorld : MonoBehaviour
    {
        
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;
    using UnityEngine;


    partial class GameWorld
    {
        [CustomEditor(typeof(GameWorld))]
        public class Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                GUILayout.Space(30f);
                
                if (GUILayout.Button("Add Room"))
                {
                    TextInputPopup.Show("Enter new room name",
                        roomName =>
                            {
                                GameWorld gameWorld = target as GameWorld;
                                GameObject newRoom = CreateRoom(gameWorld, roomName);
                                Selection.activeGameObject = newRoom;
                                EditorApplication.ExecuteMenuItem("Edit/Frame Selected"); 
                            });
                }
            }
            
            public static GameObject CreateRoom(GameWorld gameWorld, string name)
            {
                if (string.IsNullOrEmpty(name))
                    name = "New Room";
                GameObject newRoom = new GameObject(name);

                // Add essential components
                newRoom.AddComponent<Room>();
                SpriteRenderer spriteRenderer = newRoom.AddComponent<SpriteRenderer>();
                Sprite roomSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/_Debug/debug-room.png");
                spriteRenderer.sprite = roomSprite;
                spriteRenderer.size = new Vector2(
                    Game.PixelPerfectCamera.refResolutionX / 100f, 
                    Game.PixelPerfectCamera.refResolutionY / 100f);
                spriteRenderer.sortingLayerName = "Room";
                
                // Add walls
                CreateWall("LeftWall", newRoom.transform, -7.15f);
                CreateWall("RightWall", newRoom.transform, 7.15f);
                
                // Automatically position the room inside the scene
                RepositionRoom(newRoom, gameWorld);

                return newRoom;
            }


            private static void RepositionRoom(GameObject newRoom, GameWorld gameWorld)
            {
                Bounds sceneBounds = GetSceneBounds();
                //float newX = sceneBounds.max.x + (Game.PixelPerfectCamera.refResolutionX / 100f) + Game.Config.RoomPadding;
                float newX = 0;
                float newY = sceneBounds.min.y - (Game.PixelPerfectCamera.refResolutionY / 100f);
                newY -= (Game.PixelPerfectCamera.refResolutionY / 100f) + Game.Config.RoomPadding;
                /* TODO: Figure out a max column count system.
                if (newX >= Game.Config.MaxRoomsHorizontal * (Game.PixelPerfectCamera.refResolutionX / 100f))
                {
                    newX = 0f;
                    newY -= (Game.PixelPerfectCamera.refResolutionY / 100f) + Game.Config.RoomPadding;
                }
                */

                newRoom.transform.position = new Vector3(newX, newY, 0f);
                newRoom.transform.SetParent(gameWorld.transform);
            }


            private static GameObject CreateWall(
                string name,
                Transform room, 
                float xPosition)
            {
                GameObject newWall = new GameObject(name);
                newWall.transform.SetParent(room.transform);
                newWall.transform.localPosition = new Vector3(xPosition, 0f, 0f);
                newWall.transform.localScale = new Vector3(1f, 10f, 1f);
                newWall.AddComponent<BoxCollider2D>();
                
                return newWall;
            }


            private static Bounds GetSceneBounds()
            {
                Bounds sceneBounds = new Bounds();
                foreach (Renderer r in GameObject.FindObjectsOfType<Renderer>())
                {
                    sceneBounds.Encapsulate(r.bounds);
                }
                return sceneBounds;
            }
        }
    }
}
#endif