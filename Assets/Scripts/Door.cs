namespace LD51
{
    using UnityEngine;
    using UnityEngine.Events;


    [AddComponentMenu("_L51/Door")]
    public partial class Door : MonoBehaviour
    {
        public string RequiredKey;

        public Transform TargetObject;

        public UnityEvent OnActivated;


        public void Activate(PlayerCharacter player)
        {
            if (!string.IsNullOrEmpty(this.RequiredKey)
                && !player.KeyItems.Contains(this.RequiredKey))
            {
                DenyEntry();
            }
            
            // If has an OnActivated event assigned, do that instead of entering.
            if (this.OnActivated.GetPersistentEventCount() > 0)
            {
                this.OnActivated.Invoke();
                return;
            }
            
            
            Enter(player);
        }


        private void Enter(PlayerCharacter player)
        {
            if (this.TargetObject == null)
            {
                Debug.LogError("You forgot to assign TargetObject for this door!");
                return;
            }

            Room room = this.TargetObject.transform.GetComponentInParent<Room>();
            if (room == null)
            {
                Debug.LogError("You forgot to parent the TargetObject under a Room object!");
            }

            Vector3 targetPosition = this.TargetObject.transform.position;
            player.transform.position = targetPosition;
            room.RefocusCamera(targetPosition);

        }


        private void DenyEntry()
        {
            Debug.Log($"Player lacks key item: {this.RequiredKey}");
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class Door
    {
        [CustomEditor(typeof(Door))]
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