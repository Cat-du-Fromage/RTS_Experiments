using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

[System.Serializable]
public class Units
{
    public string Name;
    public int CombatMelee;
    public int CombatRange;
    public int LifePoint;
    public int SizeRegiment;

    public Units(string Name, int CombatMelee, int CombatRange, int LifePoint, int SizeRegiment)
    {
        this.Name = Name;
        this.CombatMelee = CombatMelee;
        this.CombatRange = CombatRange;
        this.LifePoint = LifePoint;
        this.SizeRegiment = SizeRegiment;
    }
    /*
    public static Units fromDictionnary(Dictionary<string, object> json)
    {
        return Units(json["Name"], json["CombatMelee"], json["CombatRange"], json["LifePoint"], json["SizeRegiment"]);
    }
    */
}
