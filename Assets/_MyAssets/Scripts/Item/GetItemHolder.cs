using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public static class GetItemHolder
    {
        static Dictionary<string, int> m_itemGetCount=new Dictionary<string, int>();
        // Start is called before the first frame update
        
        static void Reset()
        {
            m_itemGetCount = new Dictionary<string, int>();
        }

        public static void GetItem(string itemType)
        {
            if(!m_itemGetCount.ContainsKey(itemType)) m_itemGetCount[itemType]=0;
            m_itemGetCount[itemType]++;
#if UNITY_EDITOR
            //OutLog();
#endif
        }

        static void OutLog()
        {
            string log = "";
            foreach(var data in m_itemGetCount)
            {
                log += $"key:{data.Key},count:{data.Value}\n";
            }
            Debug.Log(log);
        }
    }
}
