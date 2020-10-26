using UnityEngine;

namespace PlayerMovement
{
    public class PlayerInputAxis : IPlayerInput
    {
        public Vector2 MoveVector()
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        }
    }
}