using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData 
{
   public static int CurrentLevel
   {
      get => PlayerPrefs.GetInt("CurrentLevel", 0);
      set => PlayerPrefs.SetInt("CurrentLevel", value);
   }
}
