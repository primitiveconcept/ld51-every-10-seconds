namespace LD51
{
    using UnityEngine;


    public partial class PlayerInput : MonoBehaviour
    {
        private static string MovementAxis = "Horizontal";
        private static string InteractionsAxis = "Vertical";

        public static bool LeftHeld
        {
            get { return Input.GetAxisRaw(MovementAxis) < 0; }
        }

        public static bool RightHeld
        {
            get { return Input.GetAxisRaw(MovementAxis) > 0; }
        }

        public static bool Idle
        {
            get { return Input.GetAxisRaw(MovementAxis) == 0; }
        }

        public static bool PickupHeld
        {
            get { return Input.GetAxisRaw(InteractionsAxis) < 0; }
        }

        public static bool EnterDoorHeld
        {
            get { return Input.GetAxisRaw(InteractionsAxis) > 0; }
        }


        public void Update()
        {
            // Uncomment to check button inputs in the console.
            //DebugInputMapping();
        }


        private static void DebugInputMapping()
        {
            if (LeftHeld)
                Debug.Log("Left");
            if (RightHeld)
                Debug.Log("Right");
            if (PickupHeld)
                Debug.Log("Pick up");
            if (EnterDoorHeld)
                Debug.Log("Enter door");
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;
    using UnityEngine;


    partial class PlayerInput
    {
        [CustomEditor(typeof(PlayerInput))]
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