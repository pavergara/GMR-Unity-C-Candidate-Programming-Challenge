using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonFileManagerSingleton : MonoBehaviour
{

    public static JsonFileManagerSingleton _Instance { get; private set; }

    public string _filename = "JsonList.json";//"JsonChallenge.json";


    List<string> campos = new List<string>();


    private string jsonString; //String Json loaded from file.
    private JObject JParsed; //Parser Json

    const string JSON_TITLE = "Title";
    const string JSON_HEADERS = "ColumnHeaders";
    const string JSON_DATA = "Data";

    private string _title;
    private string[] _columns;
    private List<List<string>> _data;

    public string Title { get { return (JParsed==null)?null:_title; } }
    public string[] Columns { get { return (JParsed == null) ? null : _columns; } }
    public List<List<string>> Data { get { return (JParsed == null) ? null : _data; } }


    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReadData()
    {

        //erase all data loaded previously

        JParsed = null;

        _title = null;
        _columns = null;
        _data = null;


        if (ValidateFileExist())
        {
            LoadJsonFile();
        }
        else
        {
            Debug.Log("File not Exist: \""+ _filename + "\"");
            
        }
    }


    public bool ValidateFileExist()
    {
        try
        {
            return VerifyExistFile(_filename);
        }
        catch
        {
            
            return false;
        }
    }


    private bool VerifyExistFile(string filename)
    {

        string fullFileName = Path.Combine(Application.streamingAssetsPath, filename);
        print(fullFileName);

        if (File.Exists(fullFileName))
        {
            Debug.Log("Existe JSON Para Leer, " + fullFileName);
            return true;
        }

        throw new Exception("Json file not found", new IOException());


    }

    //read JSON file defined by name, in StreamingAssets path. Result iun global String for parsed after.
    private void JsonBaseRead()
    {
        //Read file
        jsonString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, _filename));

        Debug.Log("antes de..." + jsonString);


    }

    private void LoadJsonFile()
    {
        
        jsonString = "";

        JsonBaseRead(); //Load jsonString

        if (jsonString.Equals(""))
        {
            print("Json read empty");
            return;
        }

        JParsed = null;

        JParsed = JObject.Parse(jsonString);


        //Title
        try
        {
            Debug.Log(" JParsed.Property(" + JSON_TITLE +"):" + JParsed.Property(JSON_TITLE).Value);
            _title = JParsed.Property(JSON_TITLE).Value.ToString();
        }
        catch
        {

            Debug.Log("Error to access property \""+ JSON_TITLE + "\" (maybe empty ?)");
            _title = null;
        }

        //ColumnHeaders
        try
        {
            Debug.Log(" JParsed.Property(" + JSON_HEADERS+"):" + (JArray)JParsed.Property(JSON_HEADERS).Value);

            string[] _out = new string[((JArray)(JParsed.Property("ColumnHeaders").Value)).Count];

            for (int i = 0; i < ((JArray)(JParsed.Property("ColumnHeaders").Value)).Count; i++) //bjr.ColumnHeaders)
            {
                _out[i] = (string)((JArray)(JParsed.Property("ColumnHeaders").Value))[i].Value<string>();
            }
            Debug.Log("return from Json Nsoft");
            _columns = _out;


        }
        catch
        {
            Debug.Log("Error to access property \""+JSON_HEADERS+"\" (maybe empty ?)");
            _columns = null;
        }

        //Data
        try
        {
            Debug.Log(" JParsed.Property(" + JSON_DATA+")[0]:" + ((JArray)JParsed.Property(JSON_DATA).Value)[0]);

            List<List<string>> lista = new List<List<string>>();

            foreach (JObject jo in ((JArray)(JParsed.Property("Data").Value)))
            {
                List<string> values = new List<string>();
                foreach (string s in ((JArray)(JParsed.Property("ColumnHeaders").Value))) //bjr.ColumnHeaders)
                {
                    try
                    {
                        values.Add((string)jo.Property(s).Value);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message + "" + e.StackTrace);
                        values.Add("");
                    } //Dejar columna en blanco si es que no viene en seccion DATA json.
                }

                lista.Add(values);
            }

            _data = lista;

        }
        catch
        {
            Debug.Log("Error to access property \""+ JSON_DATA+"[0]\" (maybe empty ?)");
            _data = null;
        }


    }


}





