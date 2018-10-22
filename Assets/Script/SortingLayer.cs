using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour {
    public int sortL;
    private Renderer rend;
	
	void Start () {
        rend = GetComponent<Renderer>();
        rend.sortingOrder = sortL;
	}
	
}
