using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    // [SerializeField]
    // private Button _btnPlayGame;

    // Start is called before the first frame update
    void Start()
    {
        // _btnPlayGame.onClick.AddListener(PlayGame);
    }

    // OnClick has been set in Unity
    public void PlayGame(){
        // SceneManager.LoadScene("Game");
        // Load scene by index is quicker then using String
        SceneManager.LoadScene(1); // Check > Build Settings screen Game
    }

    public void QuitGame(){
        Application.Quit();
    }

    
}
