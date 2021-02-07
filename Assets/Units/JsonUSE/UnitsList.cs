using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class UnitsList
{
    public List<Units> Units = new List<Units>();
    public TextAsset textJSON;
    /*
    public static Units fromDictionary(Dictionary<string, object> json)
    {
        return Units(json["name"], json["CombatMelee"], json["CombatRange"], json["LifePoint"], json["SizeRegiment"]);
    }
        */
    public UnitsList()
    {


    }
}
