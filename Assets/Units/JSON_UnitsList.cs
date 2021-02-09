using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;
using System;

public class JSON_UnitsList : MonoBehaviour
{
    //public UnitsList UnitsList = new UnitsList();
    public TextAsset textJSON;
    public List<Unit> Unitslist;
    public List<GameObject> UnitPrefabList = new List<GameObject>();
    private Unit _unit;

    // Start is called before the first frame update
    void Start()
    {
        /*
        Unit unit = new Unit("name", 1, 1, 1, Unit.Grades.foot);
        switch(unit.grade)
        {
            case Unit.Grades.artillerie:
                Debug.Log("arti");
                break;
            case Unit.Grades.foot:
                Debug.Log("foot");
                break;
            case Unit.Grades.horse:
                Debug.Log("horse");
                break;
        }
        */
        //Stats Units List
        string jsonUnitsList = File.ReadAllText("Assets/Resources/UnitsList.json");
        JObject jsonUnitObject = JObject.Parse(jsonUnitsList);
        //[JsonProperty("fld_descr")]
        Unitslist = jsonUnitObject["Units"].ToObject<List<Unit>>();
        
        Debug.Log(Unitslist[0].Name);
        Debug.Log(Unitslist[1].Name);
        Debug.Log(Unitslist[2].Name);

        Console.WriteLine("\nExists: Unirs with name=PrussianSoldier: {0}",
            Unitslist.Exists(item => item.Name == "PrussianSoldier"));

        //Load all Prefabs and add them to a list
        foreach (GameObject unitModel in Resources.LoadAll("Prefabs/Units", typeof(GameObject)))
        {
            UnitPrefabList.Add(unitModel);
        }
        //Attribut the conrresponding prefab to each unit of THE UNITSLIST
        foreach (Unit unit in Unitslist)
        {
            unit.UnitPrefab = UnitPrefabList.Where(x => x.name.Contains(unit.Name)).SingleOrDefault();
        }

        //TEST
        //COMMENT TROUVER LE PREFAB PAR RAPPORT A UN AUTRE ATTRIBUT
        Instantiate(Unitslist.Find(x => x.Name.Contains("British")).UnitPrefab, new Vector3(-4f,3f,-6f), Quaternion.identity);
    }
}


