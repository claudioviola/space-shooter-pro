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
        initMe();
        _audioSource = gameObject.GetComponent<AudioSource>();
        _animController = gameObject.GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
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
        _canFire = Time.time + Random.Range(0.5f, 1f);
        _isWave = Random.Range(0.0f, 5.0f) > 2 ? true : false;
        _points = _isWave ? 10 : 5;
        _speed = _isWave ? _speed / 2 : _speed;
        _maxSpeed = _speed;
        Vector3 position = new Vector3(Random.Range(_limitLx, _limitRx), _initY, 0);
        transform.position = position;
    }

    void Fire(){
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject enemyLaser = Instantiate(_enemyLaser, pos, Quaternion.identity);
        enemyLaser.transform.parent = this.transform.parent;
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        foreach(Laser l in lasers){
            l.SetEnemyLaser();
        }
    }

    void Update()
    {
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
            // Debug.Log("_frequency:"+_frequency);
            // Debug.Log("_magnitude:"+_magnitude);
            Vector3 sinPos = Vector3.right * Mathf.Sin(Time.time * _frequency) * _magnitude;
            newPos = new Vector3(sinPos.x, newPos.y, newPos.z);
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
            if(other.GetComponent<Laser>().GetEnemyLaser()){
                return;
            }
            _audioSource.Play();
            _isDestroying = true;
            _player.OnEnemyDestroy(_points);
            _animController.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.3f);
            Destroy(other.gameObject);
        }

        if(other.tag == "Super_Laser"){
            _audioSource.Play();
            _isDestroying = true;
            _player.OnEnemyDestroy(_points);
            _animController.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.3f);
        }

        if(other.tag == "Player"){
            _audioSource.Play();
            _isDestroying = true;
            _animController.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.3f);
            if(_player){
                _player.OnHitMe();
            }
        }
    }

}
