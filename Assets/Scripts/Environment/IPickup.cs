namespace LD51
{
    using UnityEngine;


    public interface IPickup
    {
        void Pickup(PlayerCharacter player);
    }


    public static class PickupExtensions
    {
        public static void TryShowPrompt(this IPickup pickup, Collider2D other)
        {
            PlayerInput playerInput = other.GetComponent<PlayerInput>();
            if (playerInput == null)
                return;
            
            playerInput.ShowPickupPrompt();
        }
        
        public static void TryHidePrompt(this IPickup pickup, Collider2D other)
        {
            PlayerInput playerInput = other.GetComponent<PlayerInput>();
            if (playerInput == null)
                return;
            
            playerInput.HidePrompt();
        }
    }
}