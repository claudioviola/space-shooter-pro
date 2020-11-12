using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _shieldIndicator;
    
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
    private Slider _thrusterSlider;

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

    public void OnShieldHit(int shieldLeft){
        Transform currentShield = _shieldIndicator.transform.GetChild(shieldLeft);
        currentShield.gameObject.GetComponent<Image>().color =  new Color(0.5f, 0.5f, 0.5f, 1f);
        if(shieldLeft < 1){
            _shieldIndicator.SetActive(false);
        }
    }

    public void OnPlayerCollectHealthPowerUp(int livesLeft){
        _livesDisplayer.sprite = _livesSprite[livesLeft];
    }

    public void OnPlayerCollectShieldPowerUp(int shieldLeft){
        foreach(Transform shield in _shieldIndicator.transform){
            shield.GetComponent<Image>().color =  new Color(1f, 1f, 1f, 1f);
        }
        _shieldIndicator.SetActive(true);
    }

    public void OnThrusterChargeUpdate(float val){
        _thrusterSlider.value = val;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _livesDisplayer = GameObject.Find("Lives_Displayer").GetComponent<Image>();
        _thrusterSlider = GameObject.Find("Thruster_Bar").GetComponent<Slider>();
        _gameOverContainer.SetActive(false);
        _shieldIndicator.SetActive(false);
        UpdateScore(0);
        if(!_gameManager){
            Debug.LogError("Game_Manager not found!");
        }
        if(!_thrusterSlider){
            Debug.LogError("Thruster_Bar not found!");
        }
        if(!_livesDisplayer){
            Debug.LogError("Lives_Displayer not found!");
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
