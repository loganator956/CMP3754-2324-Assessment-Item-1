using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourRandomiser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
