namespace LD51
{
    using UnityEngine;


    [AddComponentMenu("_LD51/Items/Key Item")]
    public partial class KeyItemPickup : MonoBehaviour, IPickup
    {
        public void Pickup(PlayerCharacter player)
        {
            Debug.Log($"Picked up key item: {this.name}");
            player.KeyItems.Add(this.name);
            Destroy(this.gameObject);
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class KeyItemPickup
    {
        [CustomEditor(typeof(KeyItemPickup))]
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