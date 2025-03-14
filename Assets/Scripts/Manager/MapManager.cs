using Akimichi.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager instance = null;

    [SerializeField]
    private GameObject mapSpacesRoot = null;
    private static List<MapSpaceViewBase> mapSpaceList = new List<MapSpaceViewBase>();
    private static List<MapSpaceViewBase> playerStartMapSpaceList = new List<MapSpaceViewBase>();

    private void Awake()
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
        mapSpaceList.Clear();
        int childCount = this.mapSpacesRoot.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            MapSpaceViewBase map = this.mapSpacesRoot.transform.GetChild(i).GetComponent<MapSpaceViewBase>();
            if (map != null)
            {
                map.SetIndex(i);
                mapSpaceList.Add(map);
            }
        }
    }

    // スタート位置取得
    private void CashPlayerStartMapSpaces()
    {
        playerStartMapSpaceList.Clear();
        foreach (MapSpaceViewBase mapSpace in mapSpaceList)
        {
            if(mapSpace.IsStartingPosition)
            {
                playerStartMapSpaceList.Add(mapSpace);
            }
        }
    }
}
