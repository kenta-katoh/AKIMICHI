using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchingPlayerContents : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName = null;

    public void SetPlayerData(string name)
    {
        this.playerName.text = name;
    }

    public void ClearData()
    {
        this.playerName.text = string.Empty;
    }
}
