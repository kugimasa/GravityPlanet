using ItemSystem;
using RailSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StageGeneration
{
    public class ItemFactory : MonoBehaviour
    {
        [SerializeField] private ItemGenerator[] generators = default;

        public IItem[] Create(in Transform[] planets, in IRail[] rails)
        {
            return CreateWithAllGenerators(planets, rails).ToArray();
        }

        private IEnumerable<IItem> CreateWithAllGenerators(Transform[] planets, IRail[] rails)
        {
            foreach (var generator in generators)
            {
                foreach(var item in generator.Create(planets, rails))
                {
                    yield return item;
                }
            }
        }
    }
}