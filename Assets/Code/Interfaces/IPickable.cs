namespace Interfaces
{
    public interface IPickable : IMonoBehaviour, ISelectable
    {
        bool IsPicked { get; set; }
    }
}
