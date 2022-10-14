namespace LD51
{
    using UnityEngine;
    using UnityEngine.Events;


    public partial class InteractableTrigger : MonoBehaviour, 
                                               IInteractable
    {
        public UnityEvent OnInteract = new UnityEvent();

        public bool ShouldHidePrompt { get; set; }


        public void Interact(PlayerCharacter player)
        {
            this.OnInteract.Invoke();
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class InteractableTrigger
    {
        [CustomEditor(typeof(InteractableTrigger))]
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