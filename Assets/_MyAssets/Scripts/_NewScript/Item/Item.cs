using Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour,IItem
    {
        [SerializeField] string m_itemType="sampleItem";

        public void Get()
        {
            GetItemHolder.GetItem(m_itemType);
            GetAction();
        }

        //継承してアイテム個別の処理をする
        //わざわざ継承するのも微妙な気がする
        protected virtual void GetAction() {
            Destroy(this.gameObject);
        }
    }
}
