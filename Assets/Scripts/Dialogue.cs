namespace LD51
{
    using UnityEngine;
    using UnityEngine.UI;


    public partial class Dialogue : MonoBehaviour
    {
        private static Dialogue _instance;

        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private Text text;

        private PlayerInput _playerInput;

        private static Dialogue Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<Dialogue>();
                return _instance;
            }
        }


        public void Start()
        {
            Hide();
        }


        public void Update()
        {
            if (!this.panel.activeInHierarchy)
                return;

            if (Input.GetMouseButton(0)
                || Input.GetButtonUp(PlayerInput.FlashlightButton)
                || Input.GetButtonUp(PlayerInput.SubmitButton)
                || Input.GetButtonUp(PlayerInput.CancelButton))
            {
                Hide();
            }
        }


        public static void Show(string text)
        {
            Instance.text.text = text;
            Instance.panel.SetActive(true);
        }


        public static void Hide()
        {
            Instance.text.text = string.Empty;
            Instance.panel.SetActive(false);
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class Dialogue
    {
        [CustomEditor(typeof(Dialogue))]
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