using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items//そのうちitemMovementとitemGeneratorで分けてもいい
{
    interface INeedPlanet
    {
        void SupplyPlanet(Transform planet);
    }
}