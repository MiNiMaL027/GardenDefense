using Controllers;
using Godot;

namespace Interfaces
{
    public interface IPressable
    {
        public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController);
        public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController);
        public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController);
    }
}
