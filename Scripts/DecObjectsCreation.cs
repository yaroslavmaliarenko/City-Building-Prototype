using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DecObjectsCreation : MonoBehaviour {

    bool creationModeOn = false;
    [SerializeField] GridController grid;
    DecorativeObject currentObject;
    GridCell currentCell;
    GridCell prevCell;

    public bool CreationModeOn
    {
        get {return creationModeOn;}        
    }

    // Use this for initialization
    void Start () {

        currentObject = null;
        prevCell = null;
        currentCell = null;
    }
	
	// Update is called once per frame
	void Update () {

        //if (creationModeOn && Input.touchCount > 0)
        if (creationModeOn )
        {
            RaycastHit hit;
            int layerMask = 1 << 9;
            layerMask = ~layerMask;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, Mathf.Infinity, layerMask))
            {
                //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;//Исправление пробивания рейкаста через UI
                //if (Input.GetTouch(0).phase == TouchPhase.Ended) return;
                
                //Расчет координат для позиционирования текущего располагаемого объекта
                currentCell = hit.collider.GetComponent<GridCell>();                
                if (currentCell!=null)
                {                    
                    currentObject.transform.position = grid.GetMiddlePos(currentCell.i, currentCell.j, currentObject.objectSize);
                    if (currentCell != prevCell && prevCell!= null) grid.ReDrawGridRect(prevCell.i, prevCell.j, currentObject.objectSize);//Перерисовать участо сетки при переходе на новую ячейку
                    grid.DrawDecObjectBordersOnGrid(currentCell.i, currentCell.j, currentObject.objectSize);//Рисуем участок сетки на которой расположен наш объект с учетом возможно его размещения
                    prevCell = currentCell;

                        ////Установка нового объекта
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (grid.HaveAnEmptyCells(currentCell.i, currentCell.j, currentObject.objectSize))
                        {
                            grid.SetCellsFlag(currentCell.i, currentCell.j, currentObject.objectSize, false);
                            grid.DrawAllGrid();//Перерисовать с учетом нового объекта
                            creationModeOn = false;
                            currentObject = null;

                            //
                            MoveCamera cameraController = Camera.main.GetComponent<MoveCamera>();
                            if (cameraController != null) cameraController.blockMovement = false;
                        }
                    }

                }






            }


        }
		
	}

    public void BeginObjectCreation(GameObject decObjPrefab)
    {
        creationModeOn = true;
        GameObject newDecObject = Instantiate<GameObject>(decObjPrefab);
        currentObject = newDecObject.GetComponent<DecorativeObject>();
        currentCell = null;
        //newDecObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);    
        //MoveCamera cameraController = Camera.main.GetComponent<MoveCamera>();
        //if (cameraController != null) cameraController.blockMovement = true;
    }

    public void SetObjectButtonClick()
    {
        if (creationModeOn && currentCell!=null)
        {
            
            if (grid.HaveAnEmptyCells(currentCell.i, currentCell.j, currentObject.objectSize))
            {
                grid.SetCellsFlag(currentCell.i, currentCell.j, currentObject.objectSize, false);
                grid.DrawAllGrid();//Перерисовать с учетом нового объекта
                creationModeOn = false;
                currentObject = null;

                MoveCamera cameraController = Camera.main.GetComponent<MoveCamera>();
                if (cameraController != null) cameraController.blockMovement = false;

            }

        }
    }

}
