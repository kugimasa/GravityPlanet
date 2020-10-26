using UnityEngine;

namespace GravitySystem
{
    public interface ICalcVelocity
    {
        /// <param name="direction">速さ0でも方向が欲しい場合もあるので、outで対応</param>
        Vector3 GetVelocity(out Vector3 direction);
    }
}