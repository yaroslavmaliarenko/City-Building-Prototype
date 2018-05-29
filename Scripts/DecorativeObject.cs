using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorativeObject : MonoBehaviour {

    public int objectSize = 1;
    public string decObjectName;
    public Sprite objectSprite;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        if (!Managers.creationController.CreationModeOn)
        {
            Debug.Log("Object name - " + decObjectName);
            Debug.Log("Object size - " + objectSize + " x " + objectSize);

        }
        
    }
}
