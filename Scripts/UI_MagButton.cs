using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MagButton : MonoBehaviour,IPointerClickHandler {

    public GameObject decObjPrefab;

    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        //transform.parent.parent.gameObject.SetActive(false);//Сделать окно невидимым (Временное решение)
        UI_MagazineWind mainWnd = GetComponentInParent<UI_MagazineWind>();
        //Переходим  в режим размещения/создания объекта на игровом поле       

        Managers.creationController.BeginObjectCreation(decObjPrefab);        
        if (mainWnd != null) mainWnd.HideWindow();

    }


}
