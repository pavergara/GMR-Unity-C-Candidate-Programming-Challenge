using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LoadUIController : MonoBehaviour
{


    public Text TextfieldRef;

    public Text UITextTitle;

    public Transform HeaderTransform, DataTransform;

    public JsonFileManagerSingleton RefJsonFileLoader;



    //private string title;
    //private string[] HeadersColumns;
    //private List<List<string>> data;

    private int HeadersColumnsCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        LoadUIDataFronJson();
    }


    public void LoadUIDataFronJson()
    {
        if (!RefJsonFileLoader)
        {
            print("Dont have any Json Reference");
            return;
        }

        //Leee Json y almacena en la clase. Sin este llamado, por defeto no se carga el Json.
        RefJsonFileLoader.ReadData();

        //Post read data, load on UI

        LoadTitle(RefJsonFileLoader.Title ?? "No data read from Title");

        LoadHeaders(RefJsonFileLoader.Columns ?? (new string[] { "No data read from Columns " }));
        //fix to length of headers
        //HeadersColumns = RefJsonFileLoader.Columns;
        HeadersColumnsCount = (RefJsonFileLoader.Columns==null)?0: RefJsonFileLoader.Columns.Length;


        LoadData(RefJsonFileLoader.Data ?? new List<List<string>> { new List<string> { "No data read from Data" } } );
    }




    void LoadTitle(string titleText)
    {

        //Text UITextTitle = transform.GetChild(0).gameObject.GetComponent<Text>();

        UITextTitle.text = titleText;
    }


    void LoadHeaders(string[] titles)
    {
        //Transform HeaderTransform = transform.GetChild(1).GetChild(0).transform;

        //string[] titles = jr.GetColumnTitles();

        //ColumnHeaders

        Text CellTemp;

        foreach (string s in titles)
        {
            CellTemp = Instantiate<Text>(TextfieldRef, HeaderTransform);
            CellTemp.GetComponent<Text>().text = s;
            CellTemp.fontStyle = FontStyle.Bold;
            CellTemp.gameObject.SetActive(true);
        }
        //
        HeaderTransform.gameObject.GetComponent<GridLayoutGroup>().constraintCount = titles.Length;
        HeaderTransform.gameObject.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
        
    }

    void LoadData(List<List<string>> values)
    {
        

        Text CellTemp;

        //Data
        foreach (List<string> ls in values)
        {
            foreach (string s in ls)
            {
                CellTemp = Instantiate<Text>(TextfieldRef, DataTransform);
                CellTemp.GetComponent<Text>().text = s;
                CellTemp.fontStyle = FontStyle.Normal;
                CellTemp.gameObject.SetActive(true);

            }
        }
        //
        DataTransform.gameObject.GetComponent<GridLayoutGroup>().constraintCount = HeadersColumnsCount;
        DataTransform.gameObject.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
        
    }


    public void CleanContent()
    {
        //Title
        UITextTitle.text = "New Text";

        //Headers
        foreach(Transform t in HeaderTransform)
        {
            Destroy(t.gameObject);
        }

        //Data
        foreach (Transform t in DataTransform)
        {
            Destroy(t.gameObject);
        }


    }

    public void ReloadDataoFromJson()
    {
        CleanContent();
        LoadUIDataFronJson();
    }
}
