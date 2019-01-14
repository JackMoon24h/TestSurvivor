using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadInformation 
{

    public static void LoadAllInformation()
    {
        GameInformation.PlayerName = PlayerPrefs.GetString("PLAYERNAME");
        GameInformation.PlayerLevel = PlayerPrefs.GetInt("PLAYERLEVEL");
    }
}
