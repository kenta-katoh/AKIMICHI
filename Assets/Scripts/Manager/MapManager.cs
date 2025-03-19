using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akimichi.Game
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager instance = null;

        [SerializeField]
        private GameObject mapSpacesRoot = null;
        private List<MapSpaceViewBase> mapSpaceList = new List<MapSpaceViewBase>();
        private List<MapSpaceViewBase> playerStartMapSpaceList = new List<MapSpaceViewBase>();

        private void Awake()
        {
            
        }

        private void Start()
        {
            // どこか管理されている箇所で行いたい
            CashMapSpaces();
            CashPlayerStartMapSpaces();
        }

        public static MapManager Instance()
        {
            if (instance == null) instance = new MapManager();
            return instance;
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
    }
}
