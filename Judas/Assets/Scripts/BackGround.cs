using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] GameObject grass;
    [SerializeField] int grassQuantity = 10;

    private void Start() {
        GenerateGrass();
    }

    private void GenerateGrass()
    {
        for(int i = 0; i < grassQuantity; i++)
        {
            float rdX = UnityEngine.Random.Range(transform.localScale.x / 2, -transform.localScale.x / 2);
            float rdY = UnityEngine.Random.Range(transform.localScale.y / 2, -transform.localScale.y / 2);
            GameObject newGrass = Instantiate(grass, new Vector3(transform.position.x + rdX, transform.position.y + rdY, 0), Quaternion.identity);

            newGrass.transform.localScale *= UnityEngine.Random.Range(0.4f, 1.2f);
        }
    }


    //-------
    //TEST pour générer des fonds avec du perlin noise, flm pour l'instant à voir plus tard
    //-------
    // private Texture GenerateTexture()
    // {
    //     Texture2D text = new Texture2D(pixelWidth, pixelHeight);

    //     for(int x = 0; x < pixelWidth; x++) 
    //     {
    //         for(int y = 0; y < pixelHeight; y++) 
    //         {
    //             Color col = PerlinColor(x, y);
    //             text.SetPixel(x, y, col);
    //         }
    //     }

    //     text.Apply();
    //     return text;
    // }

    // private Color PerlinColor(int x, int y)
    // {
    //     float xCoord = (float)x / pixelWidth * scale;
    //     float yCoord = (float)y / pixelHeight * scale;

    //     float res = Mathf.PerlinNoise(xCoord, yCoord);

    //     return new Color(color1.r * res + color2.r, color1.g * res + color2.g, color1.b * res + color2.b);
    // }
}
