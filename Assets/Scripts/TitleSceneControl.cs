using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneControl : MonoBehaviour
{
    // Start is called before the first frame update  
    void Start()
    {

    }

    // Update is called once per frame  
    void Update()
    {

    }
    public void GameStart()
    {
        print("GameStart");
        SceneManager.LoadScene("Level-1"); // 使用 SceneManager.LoadScene 替代 Application.LoadLevel  
    }
}
