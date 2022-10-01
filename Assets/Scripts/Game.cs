namespace LD51
{
    using LD51;
    using UnityEngine;


    public partial class Game : MonoBehaviour
    {
        private static Game _instance;

        [SerializeField]
        private GameConfig config;

        public static GameConfig Config
        {
            get { return _instance.config; }
        }

        private static Game Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<Game>();
                return _instance;
            }
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class Game
    {
        [CustomEditor(typeof(Game))]
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