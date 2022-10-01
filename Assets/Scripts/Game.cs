namespace LD51
{
    using UnityEngine;
    using UnityEngine.U2D;


    [AddComponentMenu("_LD51/Game")]
    public partial class Game : MonoBehaviour
    {
        private static Game _instance;

        [SerializeField]
        private GameConfig config;

        private Camera _camera;
        private PixelPerfectCamera _pixelPerfectCamera;

        public static GameConfig Config
        {
            get { return Instance.config; }
        }

        public static PixelPerfectCamera PixelPerfectCamera
        {
            get
            {
                if (Instance._pixelPerfectCamera == null)
                    Instance._pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();
                return Instance._pixelPerfectCamera;
            }
        }

        public static Camera Camera
        {
            get
            {
                if (Instance._camera == null)
                    Instance._camera = Camera.main;
                return Instance._camera;
            }
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