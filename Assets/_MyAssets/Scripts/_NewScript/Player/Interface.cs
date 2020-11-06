using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    interface IRail
    {
        Transform GoalPlanet { get; }
        void MoveAlongPath(Transform tr);//本当は軌道を渡して、移動自体はplayerがやるのが望ましい
    }

    interface IItem
    {
        void Get();
    }
}