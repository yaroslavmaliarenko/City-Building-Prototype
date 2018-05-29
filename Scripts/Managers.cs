using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour {

    public static DecObjectsCreation creationController;

	// Use this for initialization
	void Start () {

        creationController = GetComponent<DecObjectsCreation>();


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
