using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _frequency = 4f;
    [SerializeField]
    private float _magnitude = 0.1f;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private GameObject _enemyShield;
    [SerializeField]
    private bool _hasShield = false;
    private UIManager _uiManager;
    [SerializeField]
    private bool _isAgressive = false;
    [SerializeField]
    private float _agressiveDistance = 5.0f;
    private float _fireRate = 1f;
    private float _canFire = -1f;
    private float _awakeTime;
    private float _maxSpeed;
    private bool _isDestroying = false;
    private bool _isWave = false;
    private float _limitDown = -6f;
    private float _limitLx = -9;
    private float _limitRx = 9;
    private float _initY = 7.3f;
    private int _points;
    private AudioSource _audioSource;
    private Animator _animController;
    private Player _player;

    void Start() {
        _enemyShield.SetActive(_hasShield);
        _audioSource = gameObject.GetComponent<AudioSource>();
        _animController = gameObject.GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        initMe();
        if(!_player){
            Debug.LogError("Player script not available");
        }
        if(!_animController){
            Debug.LogError("Animator component not available");
        }
        if(!_audioSource){
            Debug.LogError("AudioSource not available!");
        }
    }

    void initMe(){
        if(_uiManager.ScoreVal > 30 && Random.Range(0,2) == 1){
            _enemyShield.SetActive(true);
        }    
        _canFire = Time.time + Random.Range(0.5f, 1f);
        _isWave = (Random.Range(0.0f, 5.0f) > 3);
        _points = _isWave ? 10 : 5;
        _speed = _isWave ? _speed / 2 : _speed;
        _maxSpeed = _speed;
        Vector3 position = new Vector3(Random.Range(_limitLx, _limitRx), _initY, 0);
        transform.position = position;

        if(!_isWave && !_isAgressive && Time.time > 50f){
            _isAgressive = Random.Range(0,2) == 1;
        }

        if(_isAgressive){
            gameObject.GetComponent<SpriteRenderer>().color =  new Color(255, 0, 255, 255);
        }
    }

    void Fire(){
        if(_isDestroying){
            return;
        }
        print("Fire");
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject enemyLaser = Instantiate(_enemyLaser, pos, Quaternion.identity);
        enemyLaser.transform.parent = this.transform.parent;
    }

     void FirePowerUp(){
        if(_isDestroying){
            return;
        }
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        float min = transform.position.x - 1;
        float max = transform.position.x + 1;
        foreach(GameObject pUp in powerUps){
            if(
                Time.time > _canFire && 
                (
                    pUp.transform.position.x > min 
                    && pUp.transform.position.x < max 
                    && pUp.transform.position.y < transform.position.y + 1f
                )
            ){
                print("FirePowerUp");
                _fireRate = 1f;
                _canFire = Time.time + _fireRate;
               Fire();
            }
        }
    }

    void Update()
    {
        FirePowerUp();
        if(Time.time > _canFire){
            _fireRate = Random.Range(1.0f, 2.5f);
            _canFire = Time.time + _fireRate;
            Fire();
        }

        if(transform.position.y < _limitDown && !_isDestroying){
            initMe();
            return;
        }
        MoveMe();
    }

    void MoveMe(){
        float speed = getMySpeed(_speed, _maxSpeed);
        Vector3 newPos = Vector3.down * speed * Time.deltaTime;
        if(_isWave){
            Vector3 sinPos = Vector3.right * Mathf.Sin(Time.time * _frequency) * _magnitude;
            newPos = new Vector3(sinPos.x, newPos.y, newPos.z);
            transform.Translate(newPos);
            return;
        }
        if(_isAgressive && transform.position.y + 2f > _player.transform.position.y){
            float distance = Vector3.Distance(_player.transform.position, transform.position);
            if(distance <= _agressiveDistance){
                float velocity = _speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, velocity);
                return;
            }
        }
        transform.Translate(newPos);
    }

    private float getMySpeed(float speed, float maxSpeed){
        if(!_isDestroying){
            return speed;
        } 

        float dest = transform.position.y + 1f;
        float speedRatio = (transform.position.y / dest);
        // Debug.Log("speedRatio:"+speedRatio);
        return maxSpeed * speedRatio;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(_isDestroying){
            return;
        }

        if(other.tag == "Laser"){
            if(_enemyShield.activeSelf){
                _enemyShield.SetActive(false);
                Destroy(other.gameObject);
                return;
            }
            _audioSource.Play();
            _isDestroying = true;
            _player.OnEnemyDestroy(_points);
            _enemyShield.SetActive(false);
            _animController.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.3f);
            Destroy(other.gameObject);
        }

        if(other.tag == "Super_Laser"){
            _audioSource.Play();
            _isDestroying = true;
            _enemyShield.SetActive(false);
            _player.OnEnemyDestroy(_points);
            _animController.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.3f);
        }

        if(other.tag == "Player"){
            _audioSource.Play();
            _isDestroying = true;
            _enemyShield.SetActive(false);
            _animController.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.3f);
            if(_player){
                _player.OnHitMe();
            }
        }
    }

}
