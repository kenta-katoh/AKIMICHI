using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akimichi.Game
{
    public class MapManager : ManagerBase<MapManager>
    {
        private GameObject mapSpacesRoot = null;
        private List<MapSpaceViewBase> mapSpaceList = new List<MapSpaceViewBase>();
        private List<MapSpaceViewBase> playerStartMapSpaceList = new List<MapSpaceViewBase>();
        private System.Random rand = new System.Random();

        public override void DataTransfer(ManagerData data)
        {
            base.DataTransfer(data);
            this.mapSpacesRoot = ((MapManagerData)data).MapSpacesRoot;
        }

        public override void Initialize()
        {
            base.Initialize();
            CashMapSpaces();
            CashPlayerStartMapSpaces();
        }

        // マス目情報取得
        private void CashMapSpaces()
        {
            this.mapSpaceList.Clear();
            int childCount = this.mapSpacesRoot.transform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                MapSpaceViewBase map = this.mapSpacesRoot.transform.GetChild(i).GetComponent<MapSpaceViewBase>();
                if (map != null)
                {
                    map.SetIndex(i);
                    this.mapSpaceList.Add(map);
                }
            }
        }

        // スタート位置取得
        private void CashPlayerStartMapSpaces()
        {
            this.playerStartMapSpaceList.Clear();
            foreach (MapSpaceViewBase mapSpace in this.mapSpaceList)
            {
                if (mapSpace.IsStartingPosition)
                {
                    this.playerStartMapSpaceList.Add(mapSpace);
                }
            }
        }

        /// <summary>
        /// スタート位置のランダムシード
        /// </summary>
        public List<int> StartPositionSetting()
        {
            List<int> result = new List<int>();
            int count = this.rand.Next(1, 6);
            for (int i = 0; i < count; ++i)
            {
                result.Add(rand.Next(0, this.playerStartMapSpaceList.Count));
            }
            return result;
        }

        /// <summary>
        /// スタート位置のシャッフル
        /// </summary>
        /// <param name="list"></param>
        public void StartPositionShuffle(List<int> list)
        {
            foreach (int item in list)
            {
                if(item < this.playerStartMapSpaceList.Count)
                {
                    MapSpaceViewBase space = this.playerStartMapSpaceList[item];
                    this.playerStartMapSpaceList.RemoveAt(item);
                    this.playerStartMapSpaceList.Add(space);
                }
            }
        }

        /// <summary>
        /// スタート位置取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MapSpaceViewBase GetStartMapSpace(int index)
        {
            return this.playerStartMapSpaceList[index];
        }
    }
}
