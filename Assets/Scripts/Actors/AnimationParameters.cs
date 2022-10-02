namespace LD51
{
    using System.Runtime.CompilerServices;
    using UnityEngine;


    public class AnimationParameters : MonoBehaviour
    {
        public const string IsMovingKey = "IsMoving";

        public Animator Animator;

        public bool IsMoving
        {
            get { return this.Animator.GetBool(IsMovingKey); }
            set { this.Animator.SetBool(IsMovingKey, value); }
        }
    }
}