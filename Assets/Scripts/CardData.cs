using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardData //nao possui monobehaviour pois nao precisa existir na cena como objeto,
{                     //apenas precisamos de seus valores
    public Sprite cardSprite;
    public int id_Card;
    public CardData(int id_Card, Sprite cardSprite){
        this.id_Card = id_Card;
        this.cardSprite = cardSprite;
    }
}
