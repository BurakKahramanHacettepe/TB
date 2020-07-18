using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GameOverPanel;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void Retry()
    {

        SceneManager.LoadScene("GameScene",LoadSceneMode.Single);
    }

}
