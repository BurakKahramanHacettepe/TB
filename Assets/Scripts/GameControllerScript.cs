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
    public GameObject shockwave;


    private GameObject player;
    private Transform player_t;
    private float maxHigh = 0;
    private int highScore = 0;

    private CameraShake camshake;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player_t = player.transform;
        highScore = PlayerPrefs.GetInt("highscore");
        camshake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
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
        Vector3 offset = obs.transform.position - shockwave.transform.position;

        shockwave.transform.rotation = Quaternion.LookRotation(
                               Vector3.forward, // Keep z+ pointing straight into the screen.
                               offset           // Point y+ toward the target.
                             );
        camshake.shakeDuration = 0.21f;
        Explosion();
        player.GetComponent<PlayerControl>().lost = true;

        force.transform.position = obs.transform.position;
        Particles();
        player.GetComponent<Rigidbody2D>().simulated = false;
        Invoke("OpenPanel", 0.2f);

    }
    public void Explosion()
    {
        Material mat = shockwave.GetComponent<Renderer>().material; 

        LeanTween.value(shockwave, 0f, 1f, 3f).setOnUpdate((float flt) => {
            mat.SetFloat("Vector1_46709344", flt);//Color1
        }).setIgnoreTimeScale(true);
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
