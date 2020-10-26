using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class ItemGetter : MonoBehaviour
    {
        private Dictionary<string, int> m_itemCount = new Dictionary<string, int>();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IItem item)) return;

            if (!m_itemCount.ContainsKey(item.ItemID))
            {
                m_itemCount[item.ItemID] = 0;
            }

            m_itemCount[item.ItemID]++;
            item.Get();
        }
    }
}