using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb_player;
    private bool isConnected = false;
    private GameObject[] obstacles;
    public GameObject leftBorder, rightBorder, lowerBorder;
    private float borderLength;
    void Start()
    {
        borderLength = leftBorder.GetComponent<BoxCollider2D>().bounds.size.y;
        rb_player = GetComponent<Rigidbody2D>();
        rb_player.velocity = new Vector2(0, 3f);

        //obstacles = GameObject.FindGameObjectsWithTag("ObstacleTag");
    }

    void Update()
    {
        obstacles = GameObject.FindGameObjectsWithTag("ObstacleTag");
        InfiniteBorders();

        if (isConnected)
        {
            leftBorder.GetComponent<BoxCollider2D>().enabled = false;
            rightBorder.GetComponent<BoxCollider2D>().enabled = false;
            lowerBorder.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (!isConnected)
        {
            leftBorder.GetComponent<BoxCollider2D>().enabled = true;
            rightBorder.GetComponent<BoxCollider2D>().enabled = true;
            lowerBorder.GetComponent<BoxCollider2D>().enabled = true;
        }

        if (rb_player.velocity.magnitude < 3) rb_player.velocity = rb_player.velocity.normalized * 3; //keep velocity same

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
                GameOver();
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

    private void InfiniteBorders()
    {
        //make left and right borders infinite
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ObstacleTag")
        {
            GameOver();
        }
        else if(collision.gameObject.tag == "Finish")
        {
            GameOver();
        }
    }
}
