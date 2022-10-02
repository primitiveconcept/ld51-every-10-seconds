namespace LD51
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public partial class PlayerCharacter : MonoBehaviour
    {
        [SerializeField]
        private List<string> keyItems;

        public bool JustEnteredDoor { get; set; }
        
        private CapsuleCollider2D _playerCollider;

        public List<string> KeyItems
        {
            get { return this.keyItems; }
        }

        public CapsuleCollider2D PlayerCollider
        {
            get
            {
                if (this._playerCollider == null)
                    this._playerCollider = GetComponent<CapsuleCollider2D>();
                return this._playerCollider;
            }
        }


        public IEnumerator ToggleDoorEnteredStatus()
        {
            yield return new WaitForSeconds(0.1f);
            this.JustEnteredDoor = false;
        }
        

        public void TryDoor()
        {
            RaycastHit2D[] touchedTriggers = new RaycastHit2D[3];
            this.PlayerCollider.Cast(
                direction: Vector2.zero, 
                results: touchedTriggers, 
                distance: 0, 
                ignoreSiblingColliders: true);
            foreach (RaycastHit2D trigger in touchedTriggers)
            {
                if (trigger.transform == null)
                    continue;
                
                Door door = trigger.transform.GetComponent<Door>();
                if (door != null)
                {
                    Debug.Log($"Activating door: {door.name}");
                    door.Activate(this);
                    return;
                }
            }

            Debug.Log("No door found");
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class PlayerCharacter
    {
        [CustomEditor(typeof(PlayerCharacter))]
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