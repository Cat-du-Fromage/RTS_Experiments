using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class JSON_UnitsList : MonoBehaviour
{
    //public UnitsList UnitsList = new UnitsList();
    public TextAsset textJSON;
    public List<object> Unitslists = new List<object>();
    // Start is called before the first frame update
    void Start()
    {
        textJSON = Resources.Load("UnitsList") as TextAsset;
        if (textJSON != null)
        {
            Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<string, object>>(textJSON.text);
            JArray jsonUnits = (JArray)json["Units"];
            foreach (JObject a in jsonUnits)
            {
                Unitslists.Add(new Units(a["name"].ToString(), (int)a["CombatMelee"], (int)a["CombatRange"], (int)a["LifePoint"], (int)a["SizeRegiment"]));
            }
        }
        else
        {
            print("PAS de JSON");
        }
    }
}


