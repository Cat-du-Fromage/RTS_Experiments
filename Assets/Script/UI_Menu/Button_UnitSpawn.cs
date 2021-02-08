using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Button_UnitSpawn : MonoBehaviour
{
    /*
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
