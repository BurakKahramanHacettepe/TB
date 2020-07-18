using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public GameObject GameOverPanel;
    public TextMeshProUGUI scoreText;
    private Transform player;
    private float maxHigh = 0;
    private int highScore = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        highScore = PlayerPrefs.GetInt("highscore");
        //Debug.Log(highScore);
    }

    void Update()
    {
        if (Mathf.RoundToInt(player.position.y) > maxHigh) //to avoid lose score when rotating
        {
            maxHigh = Mathf.RoundToInt(player.position.y);
        }
        scoreText.SetText(maxHigh.ToString());
        if(Mathf.RoundToInt(maxHigh) > highScore) //save highscore
        {
            highScore = Mathf.RoundToInt(maxHigh);
            PlayerPrefs.SetInt("highscore", highScore);
        }
    }
    public void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void Retry()
    {
        LeanTween.scale(GameOverPanel.gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(LoadGameScene);
        
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

}
