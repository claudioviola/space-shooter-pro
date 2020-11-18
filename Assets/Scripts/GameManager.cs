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

        if(gameTime >  30 && gameTime < 50 && _gameLevel == 0) {
            // start asteroid
            _gameLevel++;
            _spawnManager.PlayLevel(1);
        } else if(gameTime >= 50 && gameTime < 90 && _gameLevel == 1) {
            // start shield enemies and agressive enemy
            _gameLevel++;
            _spawnManager.PlayLevel(2);
        } else if(gameTime >= 90 && gameTime < 120 && _gameLevel == 2) {
            // start wave enemies
            _gameLevel++;
            _spawnManager.PlayLevel(3);
        } else if(gameTime >= 120 && _gameLevel == 3) {
            // Increase speed enemy
            _gameLevel++;
            _spawnManager.PlayLevel(4);
        }
    }
}
