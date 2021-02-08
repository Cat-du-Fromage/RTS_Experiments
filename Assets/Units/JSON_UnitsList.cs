using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class JSON_UnitsList : MonoBehaviour
{
    public UnitsList UnitsList = new UnitsList();
    public TextAsset textJSON;
    public List<object> Unitslists = new List<object>();
    // Start is called before the first frame update
    void Start()
    {
        textJSON = Resources.Load("UnitsList") as TextAsset;
        if (textJSON != null)
        {
            //Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<string, object>>(textJSON.text);
            Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<string, object>>(textJSON.text);
            JArray jsonUnits = (JArray)json["Units"];
            foreach (JObject a in jsonUnits)
            {
                Unitslists.Add(jsonUnits);
                print(a["name"]);
                print(a["LifePoint"]);
                print(a["SizeRegiment"]);
            }

            Debug.Log(Unitslists);
            Debug.Log(json);
        }
        else
        {
            print("PAS de JSON");
        }

        /*
        textJSON = Resources.Load("UnitsList") as TextAsset;
        if(textJSON != null)
        {
            UnitsList = JsonUtility.FromJson<UnitsList>(textJSON.text);
            foreach(Units unit in UnitsList.Units)
            {
                print(unit.Name);
                print(unit.LifePoint);
                print(unit.SizeRegiment);
            }
        }
        else
        {
            print("PAS de JSON");
        }
        */
    }

    // Update is called once per frame
    void Update()
    {

    }
}


