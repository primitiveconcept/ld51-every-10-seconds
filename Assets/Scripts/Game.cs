namespace LD51
{
    using DefaultNamespace;
    using UnityEngine;


    public partial class Game : MonoBehaviour
    {
        [SerializeField]
        private GameConfig config;
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


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