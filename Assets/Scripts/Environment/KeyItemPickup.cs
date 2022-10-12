namespace LD51
{
    using System;
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


        public void OnTriggerStay2D(Collider2D other)
        {
            this.TryShowPrompt(other);
        }


        public void OnTriggerExit2D(Collider2D other)
        {
            this.TryHidePrompt(other);
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