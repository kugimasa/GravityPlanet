using UnityEngine;

namespace RailSystem
{
    public interface IRailSetter
    {
        /// <summary>惑星fromからtoへのパスを設定</summary>
        /// <remarks>スタート地点がローカル座標の原点となるように実装</remarks>
        void SetWayPoints(Vector3[] points, Transform goalPlanet);
    }
}