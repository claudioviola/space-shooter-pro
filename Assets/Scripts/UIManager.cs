using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Text _scoreTxt;
    [SerializeField]
    private GameObject _gameOverContainer;
    [SerializeField]
    private GameObject _gameOverTxt;
    private Image _livesDisplayer;
    private int _scoreVal;
    private Coroutine _gameOverRoutine;
    private GameManager _gameManager;

    public void OnEnemyDestroied(int points){
        _scoreVal += points;
        UpdateScore(_scoreVal);
    }

    public void OnPlayerHit(int livesLeft){
        livesLeft = livesLeft > 0 ? livesLeft : 0;
        _livesDisplayer.sprite = _livesSprite[livesLeft];
        if(livesLeft < 1){
            _gameOverContainer.SetActive(true);
            _gameOverRoutine = StartCoroutine(GameOverRoutine());
            _gameManager.OnGameOver();
        }
    }

    public void OnPlayerCollectHealthPowerUp(int livesLeft){
        _livesDisplayer.sprite = _livesSprite[livesLeft];
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _livesDisplayer = GameObject.Find("Lives_Displayer").GetComponent<Image>();
        _gameOverContainer.SetActive(false);
        UpdateScore(0);
        if(!_gameManager){
            Debug.LogError("Game_Manager not found!");
        }
    }

    IEnumerator GameOverRoutine(){
        while(true){
            yield return new WaitForSeconds(0.5f);
            _gameOverTxt.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            _gameOverTxt.SetActive(true);
        }
    }

    void UpdateScore(int score){
        _scoreTxt.text = "Score:" + score;
    }
}
