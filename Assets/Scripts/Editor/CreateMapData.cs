using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateScriptableObject/CreateMapData")]
public class CreateMapData : ScriptableObject
{
    public List<SpaceData> list = new List<SpaceData>();

    public enum SpaceType
    {
        Plus,
        Minus,
        Event
    }

    [System.Serializable]
    public class SpaceData
    {
        public bool IsCorner;
        public SpaceType Type;
        public int EventID;
    }
}
