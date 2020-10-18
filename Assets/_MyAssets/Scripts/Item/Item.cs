using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] string m_itemType="sampleItem";
        public void Get()
        {
            GetItemHolder.GetItem(m_itemType);
            Destroy(this.gameObject);
        }
    }
}
