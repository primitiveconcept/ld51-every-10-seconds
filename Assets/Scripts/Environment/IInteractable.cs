namespace LD51
{
    public interface IInteractable
    {
        string name { get; }
        
        bool ShouldHidePrompt { get; }
        
        void Interact(PlayerCharacter player);
    }
}