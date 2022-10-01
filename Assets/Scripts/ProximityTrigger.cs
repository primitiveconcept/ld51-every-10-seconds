namespace LD51
{
    using UnityEngine;
    using UnityEngine.Events;


    [AddComponentMenu("_LD51/ProximityTrigger")]
    public partial class ProximityTrigger : MonoBehaviour
    {
        public TriggerEvent OnEntered;
        public TriggerEvent OnExited;
        public TriggerEvent WhileInside;


        public void OnTriggerEnter2D(Collider2D other)
        {
            this.OnEntered.Invoke(other);
        }


        public void OnTriggerExit2D(Collider2D other)
        {
            this.OnExited.Invoke(other);
        }


        public void OnTriggerStay2D(Collider2D other)
        {
            this.WhileInside.Invoke(other);
        }
    }


    public class TriggerEvent : UnityEvent<Collider2D> {}
}

#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class ProximityTrigger
    {
        [CustomEditor(typeof(ProximityTrigger))]
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