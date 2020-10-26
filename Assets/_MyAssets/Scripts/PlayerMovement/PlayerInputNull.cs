using UnityEngine;

namespace PlayerMovement
{
    public class PlayerInputNull : IPlayerInput
    {
        /// <summary>常に0</summary>
        public Vector2 MoveVector() => Vector2.zero;
    }
}