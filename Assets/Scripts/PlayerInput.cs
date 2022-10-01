namespace LD51
{
    using UnityEngine;
    using UnityEngine.Events;


    [AddComponentMenu("_LD51/PlayerInput")]
    public partial class PlayerInput : MonoBehaviour
    {
        private static string MovementAxis = "Horizontal";
        private static string PickupButton = "Pickup";
        private static string EnterDoorButton = "EnterDoor";

        public UnityEvent WhileLeftPressed;
        public UnityEvent WhileRightPressed;
        public UnityEvent OnPickupPressed;
        public UnityEvent OnEnterDoorPressed;
        
        
        public bool LeftHeld
        {
            get { return Input.GetAxisRaw(MovementAxis) < 0; }
        }

        public bool RightHeld
        {
            get { return Input.GetAxisRaw(MovementAxis) > 0; }
        }

        public bool Idle
        {
            get { return Input.GetAxisRaw(MovementAxis) == 0; }
        }

        public bool PickupPressed
        {
            get
            {
                return Input.GetButtonDown(PickupButton);
            }
        }

        public  bool EnterDoorPressed
        {
            get
            {
                return Input.GetButtonDown(EnterDoorButton);
            }
        }


        public void Update()
        {
            if (LeftHeld)
                this.WhileLeftPressed.Invoke();
            else if (RightHeld)
                this.WhileRightPressed.Invoke();
            
            if (EnterDoorPressed)
                this.OnEnterDoorPressed.Invoke();
            
            if (PickupPressed)
                this.OnPickupPressed.Invoke();
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