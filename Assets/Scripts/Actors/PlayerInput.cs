namespace LD51
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;


    [AddComponentMenu("_LD51/PlayerInput")]
    public partial class PlayerInput : MonoBehaviour
    {
        private static string MovementAxis = "Horizontal";
        private static string PickupButton = "Pickup";
        private static string InteractButton = "Interact";

        public UnityEvent WhileLeftPressed;
        public UnityEvent WhileRightPressed;
        public UnityEvent OnPickupPressed;
        [FormerlySerializedAs("OnEnterDoorPressed")]
        public UnityEvent OnInteractPressed;
        
        
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

        public bool InteractPressed
        {
            get
            {
                return Input.GetButtonDown(InteractButton);
            }
        }

        
        public void Update()
        {
            if (this.LeftHeld)
                this.WhileLeftPressed.Invoke();
            else if (this.RightHeld)
                this.WhileRightPressed.Invoke();
            
            if (this.InteractPressed)
                this.OnInteractPressed.Invoke();
            
            if (this.PickupPressed)
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
            }
        }
    }
}
#endif