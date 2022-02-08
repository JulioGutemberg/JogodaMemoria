using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreBoard : MonoBehaviour
{

    #region Variaveis
    [SerializeField] GameObject Win;
    [SerializeField] GameObject game_Over;
    [SerializeField] GameObject cardContainer;
    [SerializeField] GameObject timePlacar;
    [SerializeField] GameObject pontuacaoPlacar;
    [SerializeField] int tentativasTotal = 0;
    private Configuracao gameConfig; 
    public float currentTimer;
    public bool isrunning = true;
    public TMP_Text tempo;
    public TMP_Text pontuacao;
    public TMP_Text winTempo;
    public TMP_Text winPontuacao;
    
    #endregion

    public static ScoreBoard instance{get; private set;}

private void Awake() {
    instance = this;
}
void Start ()
{   
    gameConfig = JsonReader.instance.config;
    currentTimer = gameConfig.tempoTotal;
}

// Update is called once per frame
void Update(){

    if(isrunning){
        currentTimer -= Time.deltaTime;
        tempo.text = "0:" + Mathf.RoundToInt(currentTimer).ToString();

        if (currentTimer < 0){
            GameOver();
            isrunning = false;           
        }
    }
}

    #region Metodos
    public void Tentativa(){

        tentativasTotal++;
        pontuacao.text = tentativasTotal.ToString();

    }
        private void GameOver(){
        cardContainer.SetActive(false);
        timePlacar.SetActive(false);
        pontuacaoPlacar.SetActive(false);
        game_Over.SetActive(true);

    }
    public void WinGame(){
        timePlacar.SetActive(false);
        pontuacaoPlacar.SetActive(false);
        winTempo.text = tempo.text;
        winPontuacao.text = pontuacao.text;
        Win.SetActive(true);
        isrunning = false;
        
    }
    public void StartGame(){
        SceneManager.LoadScene(0);        
    }
    #endregion
}