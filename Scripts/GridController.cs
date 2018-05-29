using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour {

    bool gridIsActive = false;
    public int iCount = 40;
    public int jCount = 40;
    public float terrainWidth = 40;
    public float terrainHeight = 40;
    float cell_00_x = 0.5f;
    float cell_00_z = 0;
    float cellSize = 1;


    public GameObject cellPrefab;
    public GameObject []decorObjectPrefabs;
    public Sprite emptyCellSprite;
    public Sprite fullCellSprite;
    public Sprite canBeSetCellSprite;


    // Use this for initialization
    void Start () {
        cell_00_z = terrainHeight - 0.5f;
        CreateGrid();
        DrawAllGrid();
        SetDecorativeObjects();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateGrid()
    {
        

        for (int i = 0; i < iCount; i++)
        {
            for (int j = 0; j < jCount; j++)
            {
                GameObject newCell = Instantiate<GameObject>(cellPrefab);
                newCell.transform.SetParent(transform);
                newCell.transform.localPosition = new Vector3(cell_00_x + cellSize * j, 0.01f, cell_00_z - cellSize * i);
                GridCell cell = newCell.GetComponent<GridCell>();
                if(cell!=null)
                {
                    cell.i = i;
                    cell.j = j;
                }
            }

        }


    }


    //Устанавливаем объекты по краям карты
    void SetDecorativeObjects()
    {
        GameObject newDecObject = null;
        for (int i = 0; i < iCount; i++)
        {
            for (int j = 0; j < jCount; j++)
            {
                if (i == 0 || i == iCount-1 || j == 0 || j == jCount-1)
                {
                    //Находимся у края границы
                    int prefabNum = Random.Range(0, decorObjectPrefabs.Length);
                    if(newDecObject == null) newDecObject = Instantiate<GameObject>(decorObjectPrefabs[prefabNum]);
                    DecorativeObject dObj = newDecObject.GetComponent<DecorativeObject>();
                    if (HaveAnEmptyCells(i, j, dObj.objectSize))
                    {
                        newDecObject.transform.position = GetMiddlePos(i, j, dObj.objectSize);
                        SetCellsFlag(i, j, dObj.objectSize, false);//Устанавливаем флаг (Ячейка занята)
                        newDecObject = null;
                    }
                    //Debug.Log("Находимся в ячейке i = " + i + " j = " + j);

                }
                else continue;

            }

        }

        if (newDecObject != null) Destroy(newDecObject);


    }

    public bool HaveAnEmptyCells(int _i,int _j,int cellsCount)
    {
        bool haveAnEmptycells = true;
        int iFrom = _i;
        int jFrom = _j;

        //Проверка на выход за границы массива
        if (_i + cellsCount > iCount) iFrom = iFrom - (_i + cellsCount - iCount );
        if (_j + cellsCount > jCount) jFrom = jFrom - (_j + cellsCount - jCount);


        for(int i = iFrom;i<iFrom+cellsCount; i++)
        {
            for (int j = jFrom; j < jFrom + cellsCount; j++)
            {
                GridCell cell =  transform.GetChild(i * jCount + j).GetComponent<GridCell>();
                if(cell!=null)
                {
                    if (!cell.isEmpty) haveAnEmptycells = false;
                }

            }

        }

        return haveAnEmptycells;
    }

    public void SetCellsFlag(int _i, int _j, int cellsCount,bool isEmpty)
    {
        int iFrom = _i;
        int jFrom = _j;

        //Проверка на выход за границы массива
        if (_i + cellsCount > iCount) iFrom = iFrom - (_i + cellsCount - iCount);
        if (_j + cellsCount > jCount) jFrom = jFrom - (_j + cellsCount - jCount);


        for (int i = iFrom; i < iFrom + cellsCount; i++)
        {
            for (int j = jFrom; j < jFrom + cellsCount; j++)
            {
                GridCell cell = transform.GetChild(i * jCount + j).GetComponent<GridCell>();
                if (cell != null)
                {
                    cell.isEmpty = isEmpty;
                }

            }

        }


    }

    public Vector3 GetMiddlePos(int _i, int _j, int cellsCount)
    {
        int iFrom = _i;
        int jFrom = _j;        

        //Проверка на выход за границы массива
        if (_i + cellsCount > iCount) iFrom = iFrom - (_i + cellsCount - iCount);
        if (_j + cellsCount > jCount) jFrom = jFrom - (_j + cellsCount - jCount);


        Vector3 firstCellPos = transform.GetChild(iFrom * jCount + jFrom).position;
        Vector3 minPos = new Vector3(firstCellPos.x, firstCellPos.y, firstCellPos.z);
        Vector3 maxPos = new Vector3(firstCellPos.x, firstCellPos.y, firstCellPos.z);
        

        for (int i = iFrom; i < iFrom + cellsCount; i++)
        {
            for (int j = jFrom; j < jFrom + cellsCount; j++)
            {
                Vector3 currentPos = transform.GetChild(i * jCount + j).position;
                if (currentPos.x < minPos.x) minPos.x = currentPos.x;
                if (currentPos.z < minPos.z) minPos.z = currentPos.z;
                if (currentPos.x > maxPos.x) maxPos.x = currentPos.x;
                if (currentPos.z > maxPos.z) maxPos.z = currentPos.z;

            }

        }

        Vector3 middlePos = minPos + (maxPos - minPos) / 2;
        //Debug.Log("minPos = " + minPos);
        //Debug.Log("maxPos = "  + maxPos);
        //Debug.Log("middlePos = " + middlePos);

        return middlePos;

    }

    //Активация/дизактивация сетки
    public void ActiveGridButtonClick()
    {
        gridIsActive = !gridIsActive;
        DrawAllGrid();
        
    }

    public void DrawAllGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentCellT = transform.GetChild(i);
            GridCell currentCell = currentCellT.GetComponent<GridCell>();
            if (gridIsActive)
            {
                if (currentCell != null)
                {
                    if (currentCell.isEmpty) currentCellT.GetComponent<SpriteRenderer>().sprite = emptyCellSprite;
                    else currentCellT.GetComponent<SpriteRenderer>().sprite = fullCellSprite;
                }

            }
            else currentCellT.GetComponent<SpriteRenderer>().sprite = null;

        }

    }

    public void DrawDecObjectBordersOnGrid(int _i, int _j, int cellsCount)
    {
        if (!gridIsActive) return;
        
        //Отрисовка участка сетки на который занимает декоративный объект
        bool objectCanBeSet = HaveAnEmptyCells(_i, _j, cellsCount);


        int iFrom = _i;
        int jFrom = _j;

        //Проверка на выход за границы массива
        if (_i + cellsCount > iCount) iFrom = iFrom - (_i + cellsCount - iCount);
        if (_j + cellsCount > jCount) jFrom = jFrom - (_j + cellsCount - jCount);        

        for (int i = iFrom; i < iFrom + cellsCount; i++)
        {
            for (int j = jFrom; j < jFrom + cellsCount; j++)
            {
                GridCell cell = transform.GetChild(i * jCount + j).GetComponent<GridCell>();
                if (cell != null)
                {
                    if (objectCanBeSet) cell.GetComponent<SpriteRenderer>().sprite = canBeSetCellSprite;
                    else cell.GetComponent<SpriteRenderer>().sprite = fullCellSprite;

                }
            }
        }

    }

    public void ReDrawGridRect(int _i, int _j, int cellsCount)
    {
        if (!gridIsActive) return;

        int iFrom = _i;
        int jFrom = _j;

        //Проверка на выход за границы массива
        if (_i + cellsCount > iCount) iFrom = iFrom - (_i + cellsCount - iCount);
        if (_j + cellsCount > jCount) jFrom = jFrom - (_j + cellsCount - jCount);

        for (int i = iFrom; i < iFrom + cellsCount; i++)
        {
            for (int j = jFrom; j < jFrom + cellsCount; j++)
            {
                GridCell cell = transform.GetChild(i * jCount + j).GetComponent<GridCell>();
                if (cell != null)
                {
                    if (cell.isEmpty) cell.GetComponent<SpriteRenderer>().sprite = emptyCellSprite;
                    else cell.GetComponent<SpriteRenderer>().sprite = fullCellSprite;

                }
            }
        }

    }


}
