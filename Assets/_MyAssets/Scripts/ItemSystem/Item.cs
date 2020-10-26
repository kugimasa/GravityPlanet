using System;
using UnityEngine;

namespace ItemSystem
{
    public class Item : MonoBehaviour, IItem
    {
        [SerializeField] string m_itemID ="Item";
        public string ItemID => m_itemID;

        public event EventHandler<string> OnGet;

        public void Get()
        {
            OnGet?.Invoke(this, ItemID);
            Destroy(gameObject);
        }
    }
}
