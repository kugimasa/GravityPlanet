using ItemSystem;
using RailSystem;
using System.Collections.Generic;
using UnityEngine;

namespace StageGeneration
{
    public abstract class ItemGenerator : MonoBehaviour
    {
        public abstract IEnumerable<IItem> Create(IEnumerable<Transform> planets, IEnumerable<IRail> rails);
    }
}