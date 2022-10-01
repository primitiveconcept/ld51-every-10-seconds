namespace LD51
{
    using UnityEngine;


    [CreateAssetMenu()]
    public partial class GameConfig : ScriptableObject
    {
        
    }
}


#if UNITY_EDITOR
namespace LD51
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