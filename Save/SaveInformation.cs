using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveInformation 
{

	public static void SaveAllInformation()
    {
        PlayerPrefs.SetInt("PLAYERLEVEL", GameInformation.PlayerLevel);
        PlayerPrefs.SetString("PLAYERNAME", GameInformation.PlayerName);
    }
}
