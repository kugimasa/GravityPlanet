using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRules
{
    public interface ITimeController
    {
        void InitializeTime();
        float CurrentTime { get; }

        void UpdateTime();
    }

    public interface IPlayer
    {
        bool CanMove { get; set; }
        Transform transform { get; }
    }
}
