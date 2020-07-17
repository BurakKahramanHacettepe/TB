using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralPlanets : MonoBehaviour
{
    public int width_height = 64;


    private Color Color1,Color2;
    private float scale;
    private float radius;
    private float offsetX = 0f, offsetY = 0f;
    private Vector2 centre;

    private int layers = 5;

    private Material AtmosphereMat;
    private new SpriteRenderer renderer;
    // Start is called before the first frame update
    void OnEnable()
    {
        transform.localScale = RandomScale();
        Color1 = RandomColor();
        Color2 = RandomColor();

        AtmosphereMat = GetComponentInChildren<Renderer>().material;
        print(AtmosphereMat.GetColor("_EmissionColor"));

        AtmosphereMat.SetColor("_EmissionColor", Color1);

        centre = new Vector2(width_height / 2, width_height / 2);
        radius = width_height / 2;
        offsetX = Random.value * 1000f;
        offsetY = Random.value * 1000f;

        scale = Random.value * 5f + 2f;

        layers = Mathf.RoundToInt(Random.value * 3f + 2f);
        print(layers);
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = GenerateSprite();

    }

    private Vector2 RandomScale()
    {
        float s = Random.value + 0.5f;
        return new Vector2(s, s);

    }

    private void Update()
    {
        //renderer.sprite = GenerateSprite();

    }


    Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    private Sprite GenerateSprite()
    {
        Texture2D texture = new Texture2D(width_height, width_height);
        Rect rect = new Rect(0, 0, width_height, width_height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);


        for (int x = 0; x < width_height; x++)
        {
            for (int y = 0; y < width_height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }

        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        Sprite sprite = Sprite.Create(texture,rect,pivot);
        return sprite;
    }

    private Color CalculateColor(int x, int y)
    {
        Vector2 point = new Vector2(x, y);
        if (Vector2.Distance(centre,point) > radius)
        {
            return Color.clear;

        }
       
        float xCor = (float)x / width_height * scale + offsetX;
        float yCor = (float)y / width_height * scale + offsetY;

        float c = Mathf.PerlinNoise(xCor, yCor);
        c = Mathf.Round(c* layers) / layers;
        return Color.Lerp(Color1,Color2,c);

    }

 
}
