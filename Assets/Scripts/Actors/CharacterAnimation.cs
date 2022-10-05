namespace LD51
{
    using UnityEngine;


    [AddComponentMenu("_LD51/Animation Parameters")]
    public partial class CharacterAnimation : MonoBehaviour
    {
        // Cached keys
        private static readonly int IsMovingIndex = Animator.StringToHash("IsMoving");
        private static readonly int IsHidingIndex = Animator.StringToHash("IsHiding");
        private static readonly int IsCrawlingIndex = Animator.StringToHash("IsCrawling");
        private static readonly int IsUsingFlashlightIndex = Animator.StringToHash("IsUsingFlashlight");
        private static readonly int SpeedIndex = Animator.StringToHash("Speed");

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

        public bool IsHiding
        {
            get { return this.Animator.GetBool(IsHidingIndex); }
            set { this.Animator.SetBool(IsHidingIndex, value); }
        }

        public bool IsCrawling
        {
            get { return this.Animator.GetBool(IsCrawlingIndex); }
            set { this.Animator.SetBool(IsCrawlingIndex, value); }
        }

        public float Speed
        {
            get { return this.Animator.GetFloat(SpeedIndex); }
            set { this.Animator.SetFloat(SpeedIndex, value); }
        }

        public bool FlipX
        {
            get { return this.SpriteRenderer.flipX; }
            set { this.SpriteRenderer.flipX = value; }
        }

        public bool IsUsingFlashlight
        {
            get { return this.Animator.GetBool(IsUsingFlashlightIndex); }
            set { this.Animator.SetBool(IsUsingFlashlightIndex, value); }
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;
    using UnityEngine;


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
                
                EditorGUILayout.Toggle(nameof(CharacterAnimation.FlipX), parameters.FlipX);
                EditorGUILayout.Toggle(nameof(CharacterAnimation.IsMoving), parameters.IsMoving);
                
                parameters.IsUsingFlashlight = EditorGUILayout.Toggle(nameof(CharacterAnimation.IsUsingFlashlight), parameters.IsUsingFlashlight);
                parameters.IsHiding = EditorGUILayout.Toggle(nameof(CharacterAnimation.IsHiding), parameters.IsHiding);
                parameters.IsCrawling = EditorGUILayout.Toggle(nameof(CharacterAnimation.IsCrawling), parameters.IsCrawling);
                
                GUILayout.Space(30f);

                if (GUILayout.Button("Create Parameters"))
                {
                    // TODO
                }
            }
        }
    }
}
#endif