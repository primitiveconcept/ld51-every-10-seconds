namespace LD51
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.U2D;


    [AddComponentMenu("_LD51/Game")]
    public partial class Game : MonoBehaviour
    {
        private static Game _instance;

        [SerializeField]
        private GameConfig config;

        [SerializeField]
        private Transform world;

        [SerializeField]
        private UnityEvent timerActions;

        private Camera _camera;
        private PixelPerfectCamera _pixelPerfectCamera;

        private bool timerShouldRepeat = true;

        public static bool TimerShouldRepeat
        {
            get { return Instance.timerShouldRepeat; }
            set { Instance.timerShouldRepeat = value; }
        }

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

        public static PlayerCharacter PlayerCharacter
        {
            get { return FindObjectOfType<PlayerCharacter>(); }
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


        public void Awake()
        {
            this.timerShouldRepeat = true;
        }


        public void Start()
        {
            StartCoroutine(TimerCoroutine());
        }


        private IEnumerator TimerCoroutine()
        {
            while (this.timerShouldRepeat)
            {
                yield return new WaitForSeconds(this.config.TimerInterval);
                this.timerActions.Invoke();
            }
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;
    using UnityEngine;


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