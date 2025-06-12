using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class MapManager : ManagerBase<MapManager>
    {
        private GameObject mapSpacesRoot = null;
        private List<MapSpaceLogicBase> mapSpaceList = new List<MapSpaceLogicBase>();
        private List<MapSpaceLogicBase> playerStartMapSpaceList = new List<MapSpaceLogicBase>();
        private System.Random rand = new System.Random();
        private int mapSpaceLastIndex = 0;

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
                    this.mapSpaceList.Add((MapSpaceLogicBase)map.Logic);
                    this.mapSpaceLastIndex = i;
                }
            }
        }

        // スタート位置取得
        private void CashPlayerStartMapSpaces()
        {
            this.playerStartMapSpaceList.Clear();
            foreach (MapSpaceLogicBase mapSpace in this.mapSpaceList)
            {
                if (mapSpace.IsStartingPosition())
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
                    MapSpaceLogicBase space = this.playerStartMapSpaceList[item];
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
        public MapSpaceLogicBase GetStartMapSpace(int index)
        {
            return this.playerStartMapSpaceList[index];
        }

        /// <summary>
        /// インデックスからマスの取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MapSpaceLogicBase GetMapSpace(int index)
        {
            MapSpaceLogicBase result = null;
            foreach (MapSpaceLogicBase mapSpace in this.mapSpaceList)
            {
                if(mapSpace.Index == index)
                {
                    result = mapSpace;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 時計回りにつぎのマスの取得
        /// </summary>
        /// <param name="mapSpace"></param>
        /// <returns></returns>
        public MapSpaceLogicBase NextMapSpace(MapSpaceLogicBase mapSpace)
        {
            MapSpaceLogicBase result = null;
            
            int index = mapSpace.Index;
            index++;
            if(index > this.mapSpaceLastIndex)
            {
                index = 0;
            }
            result = GetMapSpace(index);

            return result;
        }

        /// <summary>
        /// 時計回りにひとつ前のマスの取得
        /// </summary>
        /// <param name="mapSpace"></param>
        /// <returns></returns>
        public MapSpaceLogicBase PreviousMapSpace(MapSpaceLogicBase mapSpace)
        {
            MapSpaceLogicBase result = null;

            int index = mapSpace.Index;
            index--;
            if (index < 0)
            {
                index = this.mapSpaceLastIndex;
            }
            result = GetMapSpace(index);

            return result;
        }

        /// <summary>
        /// マス目数取得
        /// </summary>
        /// <returns></returns>
        public int GetMapSpaces()
        {
            return this.mapSpaceList.Count;
        }

        /// <summary>
        /// マスに所属を送信
        /// </summary>
        /// <param name="index"></param>
        /// <param name="playerIndex"></param>
        public void SendAffiliation(int index)
        {
            ClearSendData();
            this.datas[0] = index;
            this.datas[1] = (int)PlayerManager.Instance().PlayerIndex;
            NetworkManager.Instance().SendEvent(EventConst.Event.AffiliationMapSpace, this.datas);
        }
    }
}
