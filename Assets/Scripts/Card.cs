using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    #region Variaveis
    [SerializeField] bool selected = false;
    private Animator animParent;
    private Image    cardFront;
    private Image    cardBack;
    public  Sprite   cardSprite;
    public  int      card_id;

    #endregion
    private void Awake(){

        cardFront  = GetComponent<Image>();
        animParent = GetComponentInParent<Animator>();

    }
    private void Start() {
        cardFront.sprite = cardSprite;
    }
    public void SelectThisCard(){                               //Método para selecionar as cartas
        selected = !selected;
        if(Game_Manager.instance.card1 == null){                //Seleciona e adiciona o valor em carta 1.
            Game_Manager.instance.card1 = this;
        }
    
        else if(Game_Manager.instance.card1 != null){           //Seleciona e adiciona o valor em carta 2.
            Game_Manager.instance.card2 = this;
        }
    
        if(Game_Manager.instance.card1 != null && Game_Manager.instance.card2 != null){
            Game_Manager.instance.CompareCards();               //Pega os dois valores armazenados e compara.
        } 
    }
    public void DestroySelf(float time){                        
        Destroy(gameObject.transform.parent.gameObject, time);
    }      
}
