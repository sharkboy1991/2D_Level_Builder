using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TileManager : MonoBehaviour
{
    public Camera _cam;

    [Header("Tiles selection grid")]
    public Image[] _tileBtns;
    public Color[] tileBtnsCol;
    int tileSelected = 0;

    [Header("Tiles colors")]
    public ColorPicker colPickA;
    public ColorPicker colPickB;
    Color[] colorPicked = new Color[2];

    public Image[] colorBtns = new Image[2];

    [Header("Tiles prefabs")]
    public GameObject tileTrigger;
    public GameObject[] tileSet;

    [Header("Grid variables")]
    public int xTile = 100;//maximum x trigger count
    public int yTile = 50;//maximum y trigger count

    public Transform triggerParent;
    public Transform tileParent;

    [Header("Trigger Highlight")]
    public GameObject highlightAdd;
    public GameObject highlightRemove;
    public Image[] actionBtnsImage;
    public LayerMask rayLayerMask;
    GameObject tileClone;
    GameObject highlightClone;
    bool addTile = true;//check if can add or remove tiles
    bool highlighted = false;
    bool canRaycast = true;
    string highlightedName = "";

    [Header("Save level created")]
    public GameObject saveObject;
    public int levelID = 0;

    void Start()
    {
        ActionButtonUpdate(0);//select action button

        GetColors(); //get palette color on start

        TileButtonUpdate(0);//update grid color on start

        CreateTriggerGrid();//create trigger grid
    }

    public void ActionButtonUpdate(int btn)
    {
        actionBtnsImage[0].color = tileBtnsCol[0];
        actionBtnsImage[1].color = tileBtnsCol[0];

        if (btn == 0)
        {
            addTile = true;
            actionBtnsImage[0].color = tileBtnsCol[1];
        }
        else if (btn == 1)
        {
            addTile = false;
            actionBtnsImage[1].color = tileBtnsCol[1];
        }
    }

    public void ActionRaycastUpdate(bool b)
    {
        if (b)
        {
            canRaycast = false;
        }
        else
        {
            canRaycast = true;
        }
    }

    private void Update()
    {
        //check trigger grig
        if (canRaycast)
        {
            CastRay();
        }
        else
        {
            if (highlighted)
            {
                highlighted = false;

                if (highlightClone != null)
                    Destroy(highlightClone);
            }
        }

        //add tile prefab to selected trigger
        if (canRaycast && Input.GetMouseButtonDown(0))
        {
            AddTile();
        }
    }

    void CastRay()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, rayLayerMask);
        if (hit)
        {
            if (hit.collider.tag == "GridTile")
            {
                if (hit.collider.name != highlightedName)
                {
                    highlightedName = hit.collider.name;

                    highlighted = true;

                    //delete old highlight clone
                    if (highlightClone != null)
                        Destroy(highlightClone);

                    if (addTile)
                    {
                        highlightClone = Instantiate(highlightAdd);//instantiate add highlight prefab
                    }
                    else
                    {
                        highlightClone = Instantiate(highlightRemove);//instantiate remove highlight prefab
                    }

                    //change clone local position
                    highlightClone.transform.localPosition = new Vector3(hit.collider.transform.localPosition.x,
                        hit.collider.transform.localPosition.y, 0);
                }
            }
        }
        else
        {
            if (highlighted)
            {
                highlighted = false;

                if(highlightClone != null)
                    Destroy(highlightClone);
            }
        }
    }

    void AddTile()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, rayLayerMask);
        if (hit)
        {
            if (hit.collider.tag == "GridTile")
            {
                if (addTile)
                {
                    //delete trigger childs
                    if (hit.collider.transform.childCount > 0)
                    {
                        Destroy(hit.collider.transform.GetChild(0).gameObject);
                    }

                    //add tile
                    tileClone = Instantiate(tileSet[tileSelected]);//instantiate highlight prefab

                    //change tile color
                    tileClone.GetComponent<SpriteRenderer>().color = colorPicked[1];

                    //change child tile color
                    if (tileClone.transform.childCount > 0)
                        tileClone.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = colorPicked[0];

                    //change clone local position
                    tileClone.transform.localPosition = new Vector3(hit.collider.transform.localPosition.x,
                            hit.collider.transform.localPosition.y, 0);

                    //change clone parent
                    tileClone.transform.SetParent(hit.collider.transform);
                }
                else
                {
                    //delete trigger childs
                    if (hit.collider.transform.childCount > 0)
                    {
                        Destroy(hit.collider.transform.GetChild(0).gameObject);
                    }
                }
            }
        }
    }

    public void TileButtonUpdate(int btn)
    {
        //reset grid color
        for (int i = 0; i < _tileBtns.Length; i++)
        {
            _tileBtns[i].color = tileBtnsCol[0];
        }

        tileSelected = btn;
        _tileBtns[tileSelected].color = tileBtnsCol[1];//change image color for selected tile
    }

    void CreateTriggerGrid()
    {
        //calculate triggers x start position
        float xMaxPos = (1.28f * xTile) - 1.28f;
        float xStarPos = -(xMaxPos / 2f);

        //calculate triggers y start position
        float yMaxPos = (1.28f * yTile) - 1.28f;
        float yStarPos = -(yMaxPos / 2f);

        for (int y = 0; y < yTile; y++)
        {
            for (int x = 0; x < xTile; x++)
            {
                GameObject cloneTrigger = Instantiate(tileTrigger);//instantiate trigger prefab
                cloneTrigger.transform.localPosition = new Vector3(xStarPos + (1.28f * x), yStarPos +(1.28f * y), 0);//trigger new position
                cloneTrigger.transform.SetParent(triggerParent.transform);//change trigger parent
                cloneTrigger.transform.name = "GridTile_" + y + "_" + x;//change clone name
            }
        }
    }
 
    public void GetColors()
    {
        colPickA.gameObject.SetActive(false);//hide color palette a
        colorBtns[0].color = colPickA.GetColor();//change image color
        colorPicked[0] = colorBtns[0].color;

        colPickB.gameObject.SetActive(false);//hide color palette b
        colorBtns[1].color = colPickB.GetColor();//change image color
        colorPicked[1] = colorBtns[1].color;
    }

    //set color from palette a
    public void SetColorA(Color a)
    {
        colorPicked[0] = a;
        colorBtns[0].color = a;//change image color

        Invoke("EnableRaycast", 0.1f);
    }

    //set color from palette b
    public void SetColorB(Color b)
    {
        colorPicked[1] = b;
        colorBtns[1].color = b;//change image color

        Invoke("EnableRaycast", 0.1f);
    }

    void EnableRaycast()
    {
        canRaycast = true;
    }

    public void SaveLevel()
    {
        //get trigger grid count
        int childCount = triggerParent.transform.childCount;

        //change tiles parent
        for (int i=0; i<childCount; i++)
        {
            if (triggerParent.transform.GetChild(i).childCount > 0)
            {
                triggerParent.transform.GetChild(i).transform.GetChild(0).transform.SetParent(tileParent);
            }
        }

        Destroy(triggerParent.gameObject);//destroy trigger grid
        Destroy(gameObject.transform.GetChild(0).transform.GetChild(0).gameObject);//destroy canvas ui

        //save create level as prefab
        string localPath = "Assets/LevelCreated/" + saveObject.name + levelID + ".prefab";
        Object prefab = PrefabUtility.SaveAsPrefabAsset(saveObject, localPath);
    }
}
