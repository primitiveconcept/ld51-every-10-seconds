namespace LD51.Player
{
    using System.Collections.Generic;
    using UnityEngine;


    public partial class Flashlight : MonoBehaviour
    {
        public float Range;
        
        private CharacterAnimation _characterAnimation;
        private PlayerCharacter _player;
        private List<Enemy> hitEnemies = new List<Enemy>();
        
        public bool IsUsingFlashlight { get; private set; }

        private CharacterAnimation CharacterAnimation
        {
            get
            {
#if UNITY_EDITOR
                // Lazy instantiation for Editor tools only -- otherwise, handled in Awake
                if (this._characterAnimation == null)
                    this._characterAnimation = GetComponentInChildren<CharacterAnimation>(includeInactive: true);
#endif
                return this._characterAnimation;
            }
        }
        
        private PlayerCharacter Player
        {
            get
            {
                if (this._player == null)
                    this._player = GetComponent<PlayerCharacter>();
                return this._player;
            }
        }


        private void CastLight()
        {
            const int maxHits = 5;

            
            RaycastHit2D[] results = new RaycastHit2D[maxHits];
            Vector2 direction = this.Player.Movement.FacingDirection < 0
                                    ? Vector2.left
                                    : Vector2.right;
            
            // Jesus H Christ, Unity, why did you make raycasts this cumbersome?!
            int numberOfHits = Physics2D.Raycast(
                origin: this.transform.position, 
                direction: direction, 
                contactFilter: new ContactFilter2D().NoFilter(), 
                results: results, 
                distance: this.Range);

            for (int i = 0; i < numberOfHits; i++)
            {
                RaycastHit2D hit = results[i];

                // Use on-contact doors to limit range to within a room.
                Door door = hit.transform.GetComponent<Door>();
                if (door != null
                    && door.ActivateOnContact)
                {
                    break;
                }

                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    bool enemyCanSee = this.Player.Movement.FacingDirection != enemy.Movement.FacingDirection;
                    if (enemyCanSee 
                        && !enemy.IsLit)
                    {
                        this.hitEnemies.Add(enemy);
                        enemy.GetLit();
                    }
                        
                    break;
                }
            }
        }

        public void Awake()
        {
            this._characterAnimation = GetComponentInChildren<CharacterAnimation>(includeInactive: true);
        }
        
        public void Update()
        {
            UpdateAnimator();
            
            if (this.IsUsingFlashlight)
                CastLight();
        }


        public void TurnOn()
        {
            this.IsUsingFlashlight = true;
            this.Player.Movement.Locked = true;
        }


        public void TurnOff()
        {
            this.IsUsingFlashlight = false;
            this.Player.Movement.Locked = false;
            foreach (Enemy enemy in this.hitEnemies)
            {
                enemy.GetUnlit();
            }
            this.hitEnemies.Clear();
        }
        
        private void UpdateAnimator()
        {
            if (this.CharacterAnimation.Animator == null
                || this.CharacterAnimation.Animator.runtimeAnimatorController == null)
            {
                return;
            }

            this.CharacterAnimation.IsUsingFlashlight = this.IsUsingFlashlight;
        }
    }
}


#if UNITY_EDITOR
namespace LD51.Player
{
    using UnityEditor;


    partial class Flashlight
    {
        [CustomEditor(typeof(Flashlight))]
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