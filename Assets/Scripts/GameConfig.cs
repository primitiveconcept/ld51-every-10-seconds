namespace LD51
{
    using UnityEngine;


    [CreateAssetMenu()]
    public partial class GameConfig : ScriptableObject
    {
        [Header("Global Game Settings")]
        public float TimerInterval = 10f;
        
        [Header("Map Editing")]
        public int MaxRoomsHorizontal = 6;
        public float RoomPadding = 2; 
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;
    using UnityEngine;


    partial class GameConfig
    {
        [CustomEditor(typeof(GameConfig))]
        public class Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                
                GUILayout.Space(30);
                if (GUILayout.Button("Edit Control Bindings"))
                {
                    UnityEditor.SettingsService.OpenProjectSettings("Project/Input Manager");
                }
            }
        }
    }
}
#endif