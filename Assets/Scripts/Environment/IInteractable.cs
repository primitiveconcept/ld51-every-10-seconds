namespace LD51
{
    public interface IInteractable
    {
        string name { get; }
        
        void Interact(PlayerCharacter player);
    }
}