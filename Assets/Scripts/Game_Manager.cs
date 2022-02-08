using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;
public class Game_Manager : MonoBehaviour
{
   
    #region Variaveis
    [SerializeField] GameObject genericCard;
    [SerializeField] Transform  container;
    [SerializeField] Sprite[] faceUpCards;
    [SerializeField] List<CardData> deck;        //Lista para armazenar os valores das cartas.
    [SerializeField] List<GameObject> cardsGo;   //Lista para armazenar os Game Objects da cena. 
    [SerializeField] JsonReader jsonReader;     //Lista para armazenar os Game Objects da cena. 
    private Configuracao game_Config;
    private bool nextLVL;
    int tempCardId = 0;
    public GameObject areaLayout; 
    public int acertos = 0;

    //Cartas para selecao
    public Card card1;
    public Card card2;
    
    #endregion

    public static Game_Manager instance{get; private set;}

    private void Awake() {
        instance = this;
    }
    void Start()
    {     
        game_Config = jsonReader.config;   
        Setup();
    }

#region Metodos

    //Setup do game.
    private void Setup(){                                                                              
        InitCard();
        Dispense(game_Config.qtd_Conjuntos);
        Control_Area(game_Config.qtd_Conjuntos);
        StartCoroutine(ShowCards(2f));   
    }
    
    //Transforma a lista de cartas encontradas no Json em Sprites. 
    private void InitCard(){                                       
        faceUpCards = new Sprite[game_Config.cartas.Length];

        for(int i = 0; i < game_Config.cartas.Length; i++){  
            
            if(faceUpCards[i] != null){
                Resources.UnloadAsset(faceUpCards[i]);
            }
            faceUpCards[i] = (Sprite)Resources.Load<Sprite>(game_Config.cartas[i].imagemCarta); 
        }
    }

    //Controlar a area que cada carta ocupa.
    private void Control_Area(int qtdConjuntos){                                        
        float newWidth = areaLayout.GetComponent<RectTransform>().rect.width / 245; //Divide a largura da tela pela da largura da carta. 
        float newHeight = areaLayout.GetComponent<RectTransform>().rect.height / 164;//Divide a altura da tela pela altura da carta.
        Vector2 cardSpacing = new Vector2(newWidth * 164, newHeight * 245);

        areaLayout.GetComponent<GridLayoutGroup>().cellSize = cardSpacing * 0.3f;
    }
   
    //Controlar a distribuição das cartas na área.
    private void Dispense(int qtd_conjuntos){ 

        tempCardId = 0;                         //Id de cada par de cartas
        deck = new List<CardData>(); 
        cardsGo = new List<GameObject>();

        for(int i = 0; i < qtd_conjuntos; i++)  // Laco para distribuir as cartas em pares
        {
            for(int j = 0; j < 2; j++){         //Este laço instancia duas cartas (GameObject) com o mesmo id e imagens e aramazena na lista cardsGO.                    
                GameObject card = Instantiate(genericCard, container);
                CardData cardData = new CardData(tempCardId,faceUpCards[i]);
        
                cardsGo.Add(card);
                deck.Add(cardData);   
            }
            tempCardId++;
        }
        deck = Shuffle(deck);                   //randomiza a lista de cartas (Cards) para distribui-las randomicamente

        for(int i = 0; i < deck.Count; i++)     //atribui um card de deck para cada carta
        {    
            cardsGo[i].transform.GetChild(0).GetComponent<Card>().card_id = deck[i].id_Card;
            cardsGo[i].transform.GetChild(0).GetComponent<Card>().cardSprite = deck[i].cardSprite;
        }
    }
     List<CardData> Shuffle(List<CardData> data) 
        {
            for (int i = data.Count - 1; i > 0; i--)
            {
                // Randomiza um número entre 0 e i (para que o intervalo diminua a cada vez)
                int rnd = UnityEngine.Random.Range(0, i);

                // Salva o valor do i atual, caso contrário ele irá sobrescrever quando trocarmos os valores
                CardData temp = data[i];

                // Troca os valores novos e antigos
                data[i] = data[rnd];
                data[rnd] = temp;
            }
            return data;
        }
    
    //Desativa as interações com outras cartas
    private void EnableCardInteraction(bool value){                 
        foreach(GameObject card in cardsGo){
            if(card != null)
                card.GetComponentInChildren<Button>().enabled = value;
        }
    }
    
    //Método para atualizar a lista de cartas
    private void RefreshList(){                                     
        for(int i = 0; i < cardsGo.Count; i++){
            if(cardsGo[i] == null)
                cardsGo.RemoveAt(i);
        }
    }
    
    //Método para Comparar as cartas
    public void  CompareCards(){                                    
        if(card1 != null && card2 != null){         //Compara se existe valores armazenado em card1 e card2.
            if(card1.card_id == card2.card_id){     //Se existir e os valores forem iguais, conta uma tentativa e destrói os componentes
                card1.GetComponent<Card>().DestroySelf(1.0f);
                card2.GetComponent<Card>().DestroySelf(1.0f);
                StartCoroutine(WaitToDestroyCard(1.2f));
                ScoreBoard.instance.Tentativa();
                acertos++;
            }
            else {
                StartCoroutine(WaitToFlipCardDown(0.65f));//Se os valores forem diferente, vira as cartas para baixo.
                ScoreBoard.instance.Tentativa();
            }
        }
        if(acertos == game_Config.qtd_Conjuntos){ //Aumenta a quantidade de conjuntos na tela.
            if(game_Config.qtd_Conjuntos < 4){
                StartCoroutine(NextLevel(1.2f));
                acertos = 0;
            } else {
                StartCoroutine(WinGame(1.0f));
            }
        }
    }   

#endregion

#region Corrotinas
    IEnumerator ShowCards(float time){
        yield return new WaitForSeconds(time);
        areaLayout.GetComponent<Animator>().SetBool("show", false);
    }
    IEnumerator WaitToFlipCardDown(float time){
        EnableCardInteraction(false);
        yield return new WaitForSeconds(time);
        if(card1 != null && card2 != null){
            card1.GetComponentInParent<Animator>().SetBool("flip", false);
            card2.GetComponentInParent<Animator>().SetBool("flip", false);
            card1 = null;
            card2 = null;       
        }
        EnableCardInteraction(true);
    }
    IEnumerator WaitToDestroyCard(float time){
        yield return new WaitForSeconds(time);
        RefreshList();
    }
    IEnumerator NextLevel(float time){
        yield return new WaitForSeconds(time);
        game_Config.qtd_Conjuntos++;
        Setup();
    }
    IEnumerator WinGame(float time){
        yield return new WaitForSeconds(time);
        ScoreBoard.instance.WinGame();
    }
#endregion
}