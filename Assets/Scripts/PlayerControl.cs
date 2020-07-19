using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    private Camera cam;

    private Rigidbody2D rb_player;
    public static bool isConnected = false;
    private GameObject[] obstacles;
    public GameObject leftBorder, rightBorder, lowerBorder;
    private Collider2D leftCollider, rightCollider, lowerCollider;
    private SpriteRenderer leftRenderer, rightRenderer, lowerRenderer;
    private float borderLength;
    private Color half_clear;
    private GameControllerScript gamecontroller;

    public bool lost;
    void Start()
    {
        isConnected = false;
        borderLength = leftBorder.GetComponent<BoxCollider2D>().bounds.size.y;
        rb_player = GetComponent<Rigidbody2D>();
        rb_player.velocity = new Vector2(0, 8f);

        obstacles = GameObject.FindGameObjectsWithTag("ObstacleTag");
        leftCollider = leftBorder.GetComponent<BoxCollider2D>();
        rightCollider = rightBorder.GetComponent<BoxCollider2D>();
        lowerCollider = lowerBorder.GetComponent<BoxCollider2D>();

        leftRenderer = leftBorder.GetComponent<SpriteRenderer>();
        rightRenderer = rightBorder.GetComponent<SpriteRenderer>();
        lowerRenderer = lowerBorder.GetComponent<SpriteRenderer>();

        half_clear = new Color(1, 1, 1, 0.25f);

        gamecontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


    }

    void Update()
    {
        //obstacles = GameObject.FindGameObjectsWithTag("ObstacleTag");

        if (isConnected)
        {
            leftCollider.enabled = false;
            leftRenderer.color = half_clear;

            rightCollider.enabled = false;
            rightRenderer.color = half_clear;

            lowerCollider.enabled = false;
            lowerRenderer.color = half_clear;

        }
        else if (!isConnected)
        {
            leftCollider.enabled = true;
            leftRenderer.color = Color.white;

            rightCollider.enabled = true;
            rightRenderer.color = Color.white;

            lowerCollider.enabled = true;
            lowerRenderer.color = Color.white;

        }

        if (rb_player.velocity.magnitude < 8) rb_player.velocity = rb_player.velocity.normalized * 8; //keep velocity same

        if (Input.GetMouseButtonDown(0)) //clicked
        {
            GetComponent<DistanceJoint2D>().enabled = true;
            GetComponent<DistanceJoint2D>().connectedBody = GetClosestObstacle().GetComponent<Rigidbody2D>();
            isConnected = true;
        }
        if (Input.GetMouseButtonUp(0)) //released
        {
            GetComponent<DistanceJoint2D>().enabled = false;
            GetComponent<DistanceJoint2D>().connectedBody = null;
            isConnected = false;
            if(transform.position.x <= leftBorder.transform.position.x || transform.position.x >= rightBorder.transform.position.x)
            {
                GameOver(transform.position.x > 0 ? rightBorder : leftBorder);
            }
        }
    }

    private GameObject GetClosestObstacle() //this function returns the closest obstacle to connect
    {
        float minDis = -1;
        GameObject res = null;
        foreach (GameObject o in obstacles)
        {
            if(minDis == -1)
            {
                minDis = Vector2.Distance(this.transform.position, o.transform.position);
                res = o;
            }
            else
            {
                if(Vector2.Distance(this.transform.position, o.transform.position) < minDis)
                {
                    minDis = Vector2.Distance(this.transform.position, o.transform.position);
                    res = o;
                }
            }
        }

        return res;
    }

    private void GameOver(GameObject obs)
    {
        

        gamecontroller.GameOver(obs);


    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("SlowZone"))
        {
            Slow();
        }
        else
        {
            GameOver(collision.gameObject);
        }
    }

    private void Slow()
    {
        SlowTime();
        zoomIn();
    }
    private void Normalize()
    {
        if (!lost)
        {
            FastenTime();
            zoomOut();
        }
        
    }

    public void SlowTime()
    {
        LeanTween.value(gameObject, 1f, 0.05f, 0.5f).setOnUpdate((float flt) => {
            Time.timeScale = flt;
        }).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutExpo).setOnComplete(Normalize);
    }
    public void FastenTime()
    {
        LeanTween.value(gameObject, 0.05f, 1, 0.5f).setOnUpdate((float flt) => {
            Time.timeScale = flt;
        }).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInExpo);
    }
    public void zoomIn()
    {
        LeanTween.value(cam.gameObject, cam.orthographicSize, 2f, 0.4f).setOnUpdate((float flt) => {
            cam.orthographicSize = flt;
        }).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocalY(cam.gameObject, 0, 0.4f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutExpo);
    }
    public void zoomOut()
    {
        LeanTween.value(cam.gameObject, cam.orthographicSize, 5f, 0.4f).setOnUpdate((float flt) => {
            cam.orthographicSize = flt;
        }).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInExpo);
        LeanTween.moveLocalY(cam.gameObject, 3, 0.4f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInExpo);
    }
}
