using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Items
{
    public class ItemGenerator
    {
        /// <summary>
        /// itemを惑星に立つようにする
        /// </summary>
        /// <param name="item">アイテム</param>
        /// <param name="planet">惑星</param>
        public static void DirectToPlanet(Transform item,Transform planet)
        {
            item.up = (item.position - planet.position).normalized;
        }

        /// <summary>
        /// pointParentの子の場所にアイテムを配置する
        /// </summary>
        /// <param name="itemPrefab">作成するアイテム</param>
        /// <param name="pointParent">作成する位置の親</param>
        /// <returns></returns>
        public static List<GameObject> GenerateItem_point(GameObject itemPrefab,Transform pointParent)
        {
            //作成地点の作成
            var pointList = new List<Vector3>();
            foreach (Transform child in pointParent)
            {
                //タグで管理したほうがいいかも？
                pointList.Add(child.position);
            }
            //作成
            List<GameObject> resultList = new List<GameObject>();
            foreach (var point in pointList)
            {
                var obj = GameObject.Instantiate(itemPrefab, point, Quaternion.identity);
                resultList.Add(obj);
            }
            return resultList;
        }
        /// <summary>
        /// 惑星上にランダムにアイテムを配置する
        /// </summary>
        /// <param name="itemPrefab">作成するアイテム</param>
        /// <param name="targetPlanet">対象の惑星</param>
        /// <param name="generateCount">作成する数</param>
        /// <param name="distanceFromGround">地面からの距離</param>
        /// <returns></returns>
        public static List<GameObject> GenerateItem_onPlanet_random(GameObject itemPrefab, SphereCollider targetPlanet,int generateCount, float distanceFromGround = 1)
        {
            List<GameObject> resultList = new List<GameObject>();
            for(int i = 0; i < generateCount; i++)
            {
                var pos=GenerateItemPos_onPlanet_random(targetPlanet, distanceFromGround);
                var obj = GameObject.Instantiate(itemPrefab, pos, Quaternion.identity);
                resultList.Add(obj);
            }
            return resultList;
        }
        /// <summary>
        /// CinemachineSmoothPath上にアイテムを配置する
        /// </summary>
        /// <param name="itemPrefab">作成するアイテム</param>
        /// <param name="path">対象のpath</param>
        /// <param name="generateCount">作成する数</param>
        /// <param name="startpos">作成を開始する場所(0~1)</param>
        /// <param name="endpos">作成を終了するする場所(0~1)</param>
        /// <returns></returns>
        public static List<GameObject> GenerateItem_withpath(GameObject itemPrefab,CinemachineSmoothPath path,int generateCount,float startpos=0,float endpos=1)
        {
            //値の修正
            endpos = Mathf.Clamp01(endpos);
            startpos = Mathf.Clamp01(startpos);
            if (endpos <= startpos)
            {
                var temp = endpos;
                endpos = startpos;
                startpos = temp;
            }
            //経路の作成
            var posList = new List<Vector3>();
            var distance = (endpos - startpos) / generateCount;
            for (int i = 0; i < generateCount; i++)
            {
                posList.Add(path.EvaluatePositionAtUnit(startpos + distance * i, CinemachinePathBase.PositionUnits.Normalized));
            }

            //アイテムの生成
            List<GameObject> resultList = new List<GameObject>();
            foreach (var pos in posList)
            {
                var obj = GameObject.Instantiate(itemPrefab, pos, Quaternion.identity);
                resultList.Add(obj);
            }
            return resultList;
        }
        #region local
        static Vector3 GenerateItemPos_onPlanet_random(SphereCollider targetPlanet, float distanceFromGround = 1)
        {
            var planetTransform = targetPlanet.transform;
            var center = planetTransform.position;
            var posx = Random.Range(-1.0f, 1.0f);
            var posy = Random.Range(-1.0f, 1.0f);
            var posz = Random.Range(-1.0f, 1.0f);
            var randPos = new Vector3(posx, posy, posz).normalized;
            var gravityDirection = (randPos - center).normalized;
            //完全な球体のみを想定
            var pos = center + gravityDirection * (planetTransform.localScale.x * targetPlanet.radius + distanceFromGround);
            return pos;
        }
        #endregion
    }
}