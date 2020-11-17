using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    protected float _speed = 1.5f;

    [SerializeField]
    protected GameObject _explosion;
    protected bool _isDestroing = false;

    protected AudioSource _audioSource;
    protected GameManager _gameManager;
    protected Renderer _renderer;
    protected int _points = 3;
    protected Player _player;
    private float _limitDown = -6f;
    private float _limitLx = -9;
    private float _limitRx = 9;
    private float _initY = 7.3f;
    private GameObject _asteroidChild;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        if(!_gameManager){
            Debug.LogError("Game_Manager not available!");
        }
        if(!_audioSource){
            Debug.LogError("AudioSource not available!");
        }
        if(!_player){
            Debug.LogError("Player not available!");
        }
        InitMe();
    }

    protected virtual void InitMe(){
        _asteroidChild = transform.GetChild(0).gameObject;
         if(!_asteroidChild){
            Debug.LogError("Asteroid Child not available in Asteroid");
        }
        Vector3 position = new Vector3(Random.Range(_limitLx, _limitRx), _initY, 0);
        transform.position = position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        float speed = _speed;
        Vector3 newPos = Vector3.down * speed * Time.deltaTime;
        transform.Translate(newPos);
        _asteroidChild.transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Laser" && !_isDestroing){
            Destroy(other.gameObject);
            _player.OnEnemyDestroy(_points);
            StartCoroutine("DestroyMe");
        }

         if(other.tag == "Player" && !_isDestroing){
            other.GetComponent<Player>().OnHitMe();
            StartCoroutine("DestroyMe");
        }

         if(other.tag == "Super_Laser"){
            Destroy(other.gameObject);
            print(_player);
            _player.OnEnemyDestroy(_points);
            StartCoroutine("DestroyMe");
        }
    }

    protected virtual IEnumerator DestroyMe(){
        _isDestroing = true;
        _audioSource.Play();
        GameObject explosion = Instantiate(_explosion, this.gameObject.transform);
        yield return new WaitForSeconds(0.5f);
        _asteroidChild.SetActive(false);
        yield return new WaitForSeconds(2.1f);
        Destroy(this.gameObject);
    }

}
