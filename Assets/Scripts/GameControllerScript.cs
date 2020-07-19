using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public GameObject GameOverPanel;
    public TextMeshProUGUI scoreText;

    public ParticleSystem trail;
    public ParticleSystem burst;

    public GameObject force;

    private GameObject player;
    private Transform player_t;
    private float maxHigh = 0;
    private int highScore = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player_t = player.transform;
        highScore = PlayerPrefs.GetInt("highscore");
        //Debug.Log(highScore);
    }

    void Update()
    {
        if (Mathf.RoundToInt(player_t.position.y) > maxHigh) //to avoid lose score when rotating
        {
            maxHigh = Mathf.RoundToInt(player_t.position.y);
        }
        scoreText.SetText(maxHigh.ToString());
        if(Mathf.RoundToInt(maxHigh) > highScore) //save highscore
        {
            highScore = Mathf.RoundToInt(maxHigh);
            PlayerPrefs.SetInt("highscore", highScore);
        }
    }
    public void GameOver(GameObject obs)
    {
        player.GetComponent<PlayerControl>().lost = true;

        force.transform.position = obs.transform.position;
        Particles();
        player.GetComponent<Rigidbody2D>().simulated = false;
        Invoke("OpenPanel", 0.2f);

    }
    void OpenPanel()
    {
        GameOverPanel.SetActive(true);


    }
    void Particles()
    {
        
        force.SetActive(true);
        trail.Pause();
        trail.Clear();
        burst.gameObject.SetActive(true);
    }
   

    public void Retry()
    {
        Time.timeScale = 1f;
        LeanTween.scale(GameOverPanel.gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(LoadGameScene);
        
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

}
