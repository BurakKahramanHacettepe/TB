using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public GameObject GameOverPanel;
    public TextMeshProUGUI panelScoreText;
    public TextMeshProUGUI scoreText;

    public ParticleSystem trail;
    public ParticleSystem burst;

    public GameObject force;
    public GameObject shockwave;

    public GameObject orbit;

    private GameObject player;
    private Transform player_t;

    private float maxHigh = 0;
    private int highScore = 0;

    private CameraShake camshake;
    private Camera cam;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player_t = player.transform;
        highScore = PlayerPrefs.GetInt("highscore");
        camshake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

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
        orbit.SetActive(false);
        player.GetComponent<PlayerControl>().enabled = false;
        Slow();
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
        Invoke("OpenPanel", 0.15f);

    }
    private void Slow()
    {
        SlowTime();
        zoomIn();
    }
    public void SlowTime()
    {
        LeanTween.value(gameObject, 1f, 0.05f, 0.5f).setOnUpdate((float flt) => {
            Time.timeScale = flt;
        }).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutQuad);
    }


    public void zoomIn()
    {
        float time = 1f;
        LeanTween.value(cam.gameObject, cam.orthographicSize, 2f, time).setOnUpdate((float flt) => {
            cam.orthographicSize = flt;
        }).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocalY(cam.gameObject, 0, time).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutQuad);
    }
    public void Explosion()
    {
        Material mat = shockwave.GetComponent<Renderer>().material; 

        LeanTween.value(shockwave, 0f, 1.5f, 10f).setOnUpdate((float flt) => {
            mat.SetFloat("Vector1_46709344", flt);//Color1
        }).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutQuad); ;
    }
    void OpenPanel()
    {
        panelScoreText.SetText("High score: " + highScore);
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

    public void Orbit(GameObject res,float distance)
    {
        orbit.transform.position = res.transform.position;
        orbit.transform.localScale = new Vector2(distance/5f,distance/5f);

    }
}
