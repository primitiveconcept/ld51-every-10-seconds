namespace DefaultNamespace
{
    using UnityEngine;


    [CreateAssetMenu()]
    public partial class GameConfig : ScriptableObject
    {
        
    }
}


#if UNITY_EDITOR
namespace DefaultNamespace
{
    using UnityEditor;


    partial class GameConfig
    {
        [CustomEditor(typeof(GameConfig))]
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