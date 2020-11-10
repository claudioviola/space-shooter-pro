using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _laserContainer;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _boostSpeed = 2f;
    [SerializeField]
    private float _normalSpeed = 6f;
    [SerializeField]
    private float _fireRate = 0.1f;
    [SerializeField]
    private int _lives = 3;
    private int _shieldStrength = 3;
    [SerializeField]
    private float _laserOffest = -0.8f;
    [SerializeField]
    private bool _isTripleShotEnabled = false;
    [SerializeField]
    private float _powerUpSpeedVal = 5f;
    private bool _isPowerUpSpeedEnabled = false;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _laserAudioSource;
    [SerializeField]
    private GameObject _damageLeft;
    [SerializeField]
    private GameObject _damageRight;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private MainCamera _mainCamera;
    private float _canFire = 0f;
    private float _initYPos = -3.5f;
    private float _limitRx = 11f;
    private float _limitLx = -11f;
    private float _limitTop = 6f;
    private float _limitDown = -4f;

    public void OnCollect(string type){
        switch(type){
            case "TRIPLE":
                StartCoroutine("EnableTriplePowerUp");
                break;
            case "SPEED":
                StartCoroutine("EnableSpeedPowerUp");
                break;
            case "SHIELD":
                // StartCoroutine("EnableShieldPowerUp");
                _shieldStrength = 3;
                _shield.SetActive(true);
                _uiManager.OnPlayerCollectShieldPowerUp(_shieldStrength);
                break;
            case "HEALTH":
                AddLive();
                break;                
            default:
                Debug.Log("On Collect Default");
                break;
        }
        
    }

    public void OnEnemyDestroy(int points){
        _uiManager.OnEnemyDestroied(points);
    }

    public void OnHitMe(){
        if(_shield.activeSelf){
            UpdateShield();
            return;
        }
        _mainCamera.ShakeMe();
        _lives--;
        _uiManager.OnPlayerHit(_lives);
        if(_lives == 2){
            _damageLeft.SetActive(true);
        }
        if(_lives == 1){
            _damageRight.SetActive(true);
        }
        if(_lives < 1){
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
        }
    }

    private void UpdateShield(){
        _shieldStrength--;
        _uiManager.OnShieldHit(_shieldStrength);
        if(_shieldStrength < 1){
            _shield.SetActive(false);
        }
    }

    private void AddLive(){
        if(_lives < 3){
            _lives++;
            _uiManager.OnPlayerCollectHealthPowerUp(_lives);
        }
    }

    IEnumerator EnableTriplePowerUp(){
        _isTripleShotEnabled = true;
        yield return new WaitForSeconds(5f);
        _isTripleShotEnabled = false;
    }

    IEnumerator EnableSpeedPowerUp(){
        _isPowerUpSpeedEnabled = true;
        yield return new WaitForSeconds(5f);
        _speed = _normalSpeed;
        _isPowerUpSpeedEnabled = false;
    }

    void UpdateSpeed(){
        if(_isPowerUpSpeedEnabled){
            _speed = _normalSpeed + _powerUpSpeedVal;
            return;
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            _speed = _normalSpeed + _boostSpeed;
        } else {
            _speed = _normalSpeed;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _damageLeft.SetActive(false);
        _damageRight.SetActive(false);
        transform.position = new Vector3(0, _initYPos, 0);
        _laserAudioSource = gameObject.GetComponent<AudioSource>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        _speed = _normalSpeed;
        if(!_spawnManager){
            Debug.LogError("SpawnManager not found!");
        }
        if(!_uiManager){
            Debug.LogError("UI_Manager not found!");
        }
        if(!_mainCamera){
            Debug.LogError("Main Camera not found!");
        }
        if(!_laserAudioSource){
            Debug.LogError("AudioSource component not found in Player!");
        } else {
            _laserAudioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        MoveMe();
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire){
            Fire();
        }
    }

    void Fire(){
        _laserAudioSource.Play();
        _canFire = Time.time + _fireRate;
        float initY = _isTripleShotEnabled ? transform.position.y : transform.position.y + _laserOffest;
        Vector3 pos = new Vector3(transform.position.x, initY, 0);
        GameObject newShot = _isTripleShotEnabled ? _tripleShot : _laser;
        GameObject newLaser = Instantiate(newShot, pos, Quaternion.identity, _laserContainer.transform);
        newLaser.transform.parent = _laserContainer.transform;
    }

    void MoveMe(){
        float hVal = Input.GetAxis("Horizontal");
        float vVal = Input.GetAxis("Vertical");

        Vector3 newPos = new Vector3(hVal, vVal, 0);
        transform.Translate(newPos * _speed * Time.deltaTime);
        
        if(transform.position.x > _limitRx) {
            transform.position = new Vector3(_limitLx, transform.position.y, 0);
        } else if (transform.position.x < _limitLx) {
            transform.position = new Vector3(_limitRx, transform.position.y, 0);
        }

        float yClamped = Mathf.Clamp(transform.position.y, _limitDown, _limitTop);
        transform.position = new Vector3(transform.position.x, yClamped, transform.position.z);
    }
}
