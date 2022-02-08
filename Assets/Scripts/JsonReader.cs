using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class JsonReader : MonoBehaviour         // JsonReader e onde Desserializamos os dados do Json.
{  

#region Variaveis                                       
    [SerializeField]private string json;

    private StreamReader reader;
    
    public Configuracao config;

    #endregion
    public static JsonReader instance {get; private set;}

    private void Awake()
    {
        reader = new StreamReader(Application.dataPath + "/Json/configuracoes.json");
        json = reader.ReadToEnd();
        config = JsonUtility.FromJson<Configuracao>(json);
        instance = this;
    }
}
