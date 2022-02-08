using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardClass 
{
    public string imagemCarta;
}
[System.Serializable]
public class Configuracao                
{
    public int tempoTotal;
    public int qtd_Conjuntos;
    public CardClass[] cartas;
}