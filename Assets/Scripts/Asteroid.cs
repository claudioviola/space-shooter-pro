using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private GameObject _explosion;
    
    private AudioSource _audioSource;
    private GameManager _gameManager;
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        if(!_gameManager){
            Debug.LogError("Game_Manager not available!");
        }
        if(!_audioSource){
            Debug.LogError("AudioSource not available!");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Laser"){
            Destroy(other.gameObject);
            StartCoroutine("DestroyMe");
        }

         if(other.tag == "Player"){
            other.GetComponent<Player>().OnHitMe();
            StartCoroutine("DestroyMe");
        }
    }

    IEnumerator DestroyMe(){
        _audioSource.Play();
        GameObject explosion = Instantiate(_explosion, this.gameObject.transform);
        yield return new WaitForSeconds(0.5f);
        _renderer.enabled = false;
        yield return new WaitForSeconds(2.1f);
        _gameManager.OnFirstAsteroidDestroyed();
        Destroy(this.gameObject);
    }

}
