using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 2.5f;
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
    [SerializeField]
    private bool _isSmartEnemy = false;
    private float _escapeDirection;
    [SerializeField]
    private bool _isWave = false;
    private float _fireRate = 1.5f;
    private float _canFire = -1f;
    private float _awakeTime;
    private float _maxSpeed;
    private bool _isDestroying = false;
    private float _limitDown = -6f;
    private float _limitLx = -9;
    private float _limitRx = 9;
    private float _initY = 7.3f;
    private int _points;
    private AudioSource _audioSource;
    private Animator _animController;
    [SerializeField]
    private GameObject _explosionAnimationSmartEnemy;
    [SerializeField]
    private GameObject _smartEnemyShip;
    private Player _player;

    public bool ShieldEnemy
    {
        get { return _enemyShield.activeSelf; }
        set
        {
            _enemyShield.SetActive(value);
        }
    }

    public bool AggressiveEnemy
    {
        get { return _isAgressive; }
        set
        {
            _isAgressive = value;
        }
    }

    public bool WaveEnemy
    {
        get { return _isWave; }
        set
        {
            _isWave = value;
            _points = 10;
        }
    }


    public float Speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
            _maxSpeed = value;
        }
    }

    public bool SmartEnemy
    {
        get { return _isSmartEnemy; }
        set
        {
            _isSmartEnemy = value;
            _points = 10;
        }
    }
    
    
    public float FireRate
    {
        get { return _fireRate; }
        set
        {
            _fireRate = value;
        }
    }
    
    void Start() {
        // _enemyShield.SetActive(_hasShield);
        _audioSource = gameObject.GetComponent<AudioSource>();
        _animController = gameObject.GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if(_explosionAnimationSmartEnemy){
            _explosionAnimationSmartEnemy.SetActive(false);
        }
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
        _canFire = Time.time + Random.Range(0.5f, 1f);
        Vector3 position = new Vector3(Random.Range(_limitLx, _limitRx), _initY, 0);
        transform.position = position;
        if(_isAgressive){
            gameObject.GetComponent<SpriteRenderer>().color =  new Color(255, 0, 255, 255);
        }

        print("isAggressive:"+_isAgressive);
        print("enemyShield:"+_enemyShield.activeSelf);
        print("isWave:"+_isWave);
        print("isSmartEnemy:"+_isSmartEnemy);
        print("Init Enemy Speed:"+_speed);
    }

    void Fire(){
        if(_isDestroying){
            return;
        }
        // print("Fire");
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

    float UpdateEscapeDirection(){
        GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
        bool isRightEscape = false;
        float minX = transform.position.x - 1f;
        float maxX = transform.position.x + 1f;
        GameObject hasCollision = null;
        float offset = 1.5f;
        float minDist = 1f;
        float maxDist = 4.0f;
        foreach(GameObject laser in lasers){
            if( laser.transform.position.x > minX && laser.transform.position.x < maxX){
                float distance = Vector3.Distance(laser.transform.position, transform.position);
                hasCollision = distance > minDist && distance < maxDist ? laser : null;
            }
        }

        if(!hasCollision){
            return 0;
        }

        if(transform.position.x - offset> _limitLx && transform.position.x + offset < _limitRx){
            isRightEscape = (Random.Range(0, 2) == 1);
            return isRightEscape ? offset : -offset; // You can move to right or left
        } 


        if(transform.position.x - offset < _limitLx){
            return offset; // Move to right
        } 


        if(transform.position.x + offset > _limitRx){
            return -offset; // Move to left
        } 

        return 0;
    }

    void MoveMe(){
        float speed = getMySpeed(_speed, _maxSpeed);
        Vector3 newPos = Vector3.down * speed * Time.deltaTime;
        
        if(_isSmartEnemy && !_isWave && _escapeDirection == 0){
            _escapeDirection = UpdateEscapeDirection();
        }

        if(_escapeDirection > 0){
            float newX = 0.1f;
            _escapeDirection = _escapeDirection - newX >= 0 ? _escapeDirection - newX : 0;
            newPos = new Vector3(newX, newPos.y, newPos.z);
        }
        
        if(_escapeDirection < 0){
            float newX = -0.1f;
            _escapeDirection = _escapeDirection + 0.1f <= 0 ? _escapeDirection + 0.1f : 0;
            newPos = new Vector3(newX, newPos.y, newPos.z);
        }

        if(_isWave){
            Vector3 sinPos = Vector3.right * Mathf.Sin(Time.time * _frequency) * _magnitude;
            newPos = new Vector3(sinPos.x, newPos.y, newPos.z);
            transform.Translate(newPos);
            return;
        }

        if(_isAgressive && _player && transform.position.y + 2f > _player.transform.position.y){
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
        print(other.tag);
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

        if(_isDestroying && _explosionAnimationSmartEnemy && _isSmartEnemy){
            if(_smartEnemyShip){
                StartCoroutine("DestroySmartEnemy");
            } else {
                Debug.LogError("Smart Enemy Ship Container not available!");
            }
            
        }
    }

    IEnumerator DestroySmartEnemy(){
        _explosionAnimationSmartEnemy.SetActive(true);
        yield return new WaitForSeconds(1f);
        _smartEnemyShip.SetActive(false);
    }

}
