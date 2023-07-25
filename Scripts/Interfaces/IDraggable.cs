using Controllers;

namespace Interfaces
{
    public interface IDraggable
    {
        public void StartDrag(PlayerController playerController);
        public void CancelDrag(PlayerController playerController);
        public void CompleteDrag(PlayerController playerController);

    }
}
