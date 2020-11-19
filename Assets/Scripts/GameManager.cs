using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private bool _isGameOver = false;
    private bool _isStarted = false;
    private float _score = 0;
    [SerializeField]
    private float _nextLevelScore = 20;
    private int _gameLevel = 0;
    private float _startTime = 0f;

    int _level1_min = 30;
    int _level1_max = 40;
    int _level2_min = 40;
    int _level2_max = 70;
    int _level3_min = 70;
    int _level3_max = 110;
    int _level4_min = 110;
    int _level4_max = 130;
    int _level5_min = 130;
    
    // int _level1_min = 10;
    // int _level1_max = 20;
    // int _level2_min = 20;
    // int _level2_max = 30;
    // int _level3_min = 30;
    // int _level3_max = 40;
    // int _level4_min = 40;
    // int _level4_max = 60;
    // int _level5_min = 60;

    public void OnGameOver(){
        _isGameOver = true;
    }

    public void OnFirstAsteroidDestroyed(){
        _spawnManager.StartMe();
        _startTime = Time.time;
        _isStarted = true;
    }

    public void OnScoreUpdate(float score){
        _score = score;
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if(!_spawnManager){
            Debug.LogError("Spawn_Manager Script Component not available");
        }
        if(!_uiManager){
            Debug.LogError("UI_Manager not available");
        }
        _uiManager.SetGameLevel(_gameLevel);
    }

    // Update is called once per frame
     void Update(){
        if(_isGameOver && Input.GetKeyDown(KeyCode.R)){
            // SceneManager.LoadScene("Game");
            // Load scene by index is quicker then using String
            SceneManager.LoadScene(1); // Check > Build Settings screen Game
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }


        if(_isGameOver || !_isStarted){
            return;
        }

        float gameTime = Time.time - _startTime;

        if(gameTime > _level1_min && gameTime < _level1_max && _gameLevel == 0) {
            // start asteroid
            _gameLevel++;
            _uiManager.SetGameLevel(_gameLevel);
            _spawnManager.PlayLevel(_gameLevel);
        } else if(gameTime >= _level2_min && gameTime < _level2_max && _gameLevel == 1) {
            // start shield enemies and agressive enemy
            _gameLevel++;
            _uiManager.SetGameLevel(_gameLevel);
            _spawnManager.PlayLevel(_gameLevel);
        } else if(gameTime >= _level3_min && gameTime < _level3_max && _gameLevel == 2) {
            // start wave enemies
            _gameLevel++;
            _uiManager.SetGameLevel(_gameLevel);
            _spawnManager.PlayLevel(_gameLevel);
        } else if(gameTime >= _level4_min && gameTime < _level4_max && _gameLevel == 3) {
            // Increase speed enemy
            _gameLevel++;
            _uiManager.SetGameLevel(_gameLevel);
            _spawnManager.PlayLevel(_gameLevel);
        } else if(gameTime >= _level5_min && _gameLevel == 4) {
            // Play the boss
            _gameLevel++;
            _uiManager.SetGameLevel(_gameLevel);
            _spawnManager.PlayLevel(_gameLevel);
        }
    }
}
