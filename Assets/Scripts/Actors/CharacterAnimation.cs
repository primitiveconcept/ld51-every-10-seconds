namespace LD51
{
    using UnityEngine;


    [AddComponentMenu("_LD51/Animation Parameters")]
    public partial class CharacterAnimation : MonoBehaviour
    {
        // Cached keys
        private static readonly int IsMovingIndex = Animator.StringToHash("IsMoving");

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        public Animator Animator
        {
            get
            {
                if (this._animator == null)
                    this._animator = GetComponent<Animator>();
                return this._animator;
            }
        }

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (this._spriteRenderer == null)
                    this._spriteRenderer = GetComponent<SpriteRenderer>();
                return this._spriteRenderer;
            }
        }

        public bool IsMoving
        {
            get { return this.Animator.GetBool(IsMovingIndex); }
            set { this.Animator.SetBool(IsMovingIndex, value); }
        }


        public bool FlipX
        {
            get { return this.SpriteRenderer.flipX; }
            set { this.SpriteRenderer.flipX = value; }
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;


    partial class CharacterAnimation
    {
        [CustomEditor(typeof(CharacterAnimation))]
        public class Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                
                CharacterAnimation parameters = this.target as CharacterAnimation;

                if (parameters.Animator == null
                    || parameters.Animator.runtimeAnimatorController == null)
                {
                    return;
                }
                    

                EditorGUILayout.Toggle(nameof(CharacterAnimation.IsMoving), parameters.IsMoving);
                EditorGUILayout.Toggle(nameof(CharacterAnimation.FlipX), parameters.FlipX);
            }
        }
    }
}
#endif