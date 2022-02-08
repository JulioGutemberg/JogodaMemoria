using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimControl : MonoBehaviour
{   
    #region Variaveis
    private Animator anCard;
    public  Sprite   cardSprite;
    public  Sprite   backFace;
    public  Image    cardFace;

    #endregion

    private void Start() {
        anCard = GetComponent<Animator>();
        cardSprite = transform.GetChild(0).GetComponent<Card>().cardSprite;
        cardFace = transform.GetChild(0).GetComponent<Image>();
    }
    #region Metodos
    public void OnFlipCard(){
        cardFace.sprite = cardSprite;
    }
    public void OnFlipCardDown(){
        cardFace.sprite = backFace;
    }
    
    public void FlipWhenSelected(){
        anCard.SetBool("flip",true);
    }
    #endregion
}
