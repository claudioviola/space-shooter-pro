using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private SpawnManager _spawnManager;
    private bool _isGameOver = false;

    public void OnGameOver(){
        _isGameOver = true;
    }

    public void OnFirstAsteroidDestroyed(){
        _spawnManager.StartMe();
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
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
    }
}
