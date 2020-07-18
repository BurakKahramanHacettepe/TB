using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratePlanets : MonoBehaviour
{

    public GameObject planet;
    // Start is called before the first frame update
    private int Y;
    void Start()
    {
        for (int i = 10; i < 31; i+=4)
        {
            float y_offset = Random.Range(-1f,1f);
            Instantiate(planet, new Vector2(Random.Range(-2f, 2f),i+y_offset), Quaternion.identity);
        }
        Y = 34;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void PoolMe(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);


        float y_offset = Random.Range(-1f, 1f);
        gameObject.transform.position = new Vector3(Random.Range(-2f, 2f),Y+y_offset);
        Y += 4;
    }
}
