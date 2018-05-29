using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MagazineWind : MonoBehaviour {

    [SerializeField] GridController grid;
    [SerializeField] GameObject buttonPrefab;    

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);

        //Создаем кнопки по количеству префабов
        for (int i = 0; i < grid.decorObjectPrefabs.Length; i++)
        {
            DecorativeObject decObj = grid.decorObjectPrefabs[i].GetComponent<DecorativeObject>();
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(transform.GetChild(0));
            newButton.transform.GetChild(0).GetComponent<Image>().sprite = decObj.objectSprite;
            newButton.transform.GetChild(1).GetComponent<Text>().text = decObj.decObjectName;
            newButton.transform.GetChild(2).GetComponent<Text>().text = decObj.objectSize + "x" + decObj.objectSize;
            newButton.GetComponent<UI_MagButton>().decObjPrefab = grid.decorObjectPrefabs[i];
            

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MagazineButtonClick()
    {
        if (Managers.creationController.CreationModeOn) return;

        if (!gameObject.activeSelf) ShowWindow();
        else HideWindow();
    }

    public void ShowWindow()
    {
        MoveCamera cameraController = Camera.main.GetComponent<MoveCamera>();
        if (cameraController != null) cameraController.blockMovement = true;
        gameObject.SetActive(true);        
    }

    public void HideWindow()
    {
        MoveCamera cameraController = Camera.main.GetComponent<MoveCamera>();
        if (cameraController != null)
        {
            if(!Managers.creationController.CreationModeOn) cameraController.blockMovement = false;
        }
        gameObject.SetActive(false);       

    }
}
