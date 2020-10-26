using UnityEngine;

namespace PlayerMovement
{
    public interface IPlayerInput
    {
        /// <summary>移動入力. 大きさ1以下, xが横方向, yが縦方向</summary>
        Vector2 MoveVector();
    }
}
