using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private float speed = 0.05f;
    private Renderer ren;

    private void Start()
    {
        ren = GetComponent<Renderer>();
    }

    void Update () {
        Vector2 offset = new Vector2(Time.time * speed,0f);
        ren.material.SetTextureOffset("_MainTex",offset);
	}
}
