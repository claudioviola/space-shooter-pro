using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _smartEnemy;
    [SerializeField]
    private GameObject _asteroid;
    [SerializeField]
    private GameObject _superLaserPowerUp;
    [SerializeField]
    private GameObject _enemiesContainer;
    [SerializeField]
    private GameObject _powerUpContainer;
    [SerializeField]
    private GameObject[] _advancedPowerUps;
    [SerializeField]
    private GameObject[] _basicPowerUps;
    [SerializeField]
    private float _superLaserPowerUpTime = 30f;
    [SerializeField]
    private float _spawnEnemyTime = 4f;
    [SerializeField]
    private float _asteroidEnemyTime = 20f;
    [SerializeField]
    private float _spawnAdvancedPowerUpTime = 30f;
    [SerializeField]
    private float _spawnBasicPowerUpTime = 2f;
    [SerializeField]
    private float _minPowerUpTime = 3f;
    [SerializeField]
    private float _maxPowerUpTime = 7f;
    [SerializeField]
    private bool isSpawningEnemy = true;
    [SerializeField]
    private bool isSpawningAsteroid = true;
    [SerializeField]
    private bool isSpawningPowerUp = true;
    private bool shouldSpawnAmmo = false;
    private int _gameLevel = 0;
    private GameObject _boss;

    public void NeedAmmo(){
        print("NeedAmmo");
        shouldSpawnAmmo = true;
    }
    
    public void OnPlayerDeath(){
        StopCoroutine("SmartEnemyRoutine");
        StopCoroutine("EnemyRoutine");
        StopCoroutine("BasicPowerUp");
        StopCoroutine("AdvancePowerUp");
        
    }

    public void initMe(){
        // StartCoroutine("SmartEnemyRoutine");
        StartCoroutine("EnemyRoutine");
        StartCoroutine("BasicPowerUp");
        StartCoroutine("AdvancePowerUp");
    }

    public void PlayLevel(int level){
        switch(level){
            case 1: //Asteroid + Shield + Aggressive
                _gameLevel = 1;
                StartCoroutine("AsteroidCoroutine");
            break;
            case 2: 
                _gameLevel = 2;
            break;     
            case 3: // Wave + Shield + Aggressive
                _gameLevel = 3;
            break;   
            case 4: // SmartEnemyRoutine + Increase Enemy Speed + Wave + Shield + Aggressive
                _gameLevel = 4;
                _spawnEnemyTime = 5f;
                StartCoroutine("SmartEnemyRoutine");
            break;   
            case 5:
                // play the boss
                Debug.Log("play the boss");
                _gameLevel = 5;
                StopCoroutine("EnemyRoutine");
                StopCoroutine("AdvancePowerUp");
                StopCoroutine("AsteroidCoroutine");
                StopCoroutine("SmartEnemyRoutine");
                StartCoroutine("BossRoutine");
            break;               
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _boss = GameObject.Find("Boss");
        _boss.SetActive(false);
        
        if(!_boss){
            Debug.LogError("Boss not available");
        }
    }

    //Called by GameManager
    public void StartMe(){
        initMe();
    }

    private IEnumerator BossRoutine(){
        yield return new WaitForSeconds(5f);
        _boss.SetActive(false);
        _boss.GetComponent<Boss>().PlayMe();
    }

    private IEnumerator EnemyRoutine(){
        yield return new WaitForSeconds(1f);
        while(isSpawningEnemy){
            GameObject newEnemy = Instantiate(_enemy);
            Enemy enemy = newEnemy.GetComponent<Enemy>();
            newEnemy.transform.parent = _enemiesContainer.transform;
            enemy.Speed = 1.8f;
            enemy.FireRate = 3f;
            if(_gameLevel >= 1 && _gameLevel <= 2) { // Shield + Aggressive
                enemy.Speed = 2.2f;
                enemy.FireRate = 2.5f;
                enemy.ShieldEnemy = Random.Range(0,2) == 1;
                enemy.AggressiveEnemy = !enemy.ShieldEnemy ? Random.Range(0,2) == 1 : false;
                print("EnemyRoutine>_gameLevel:"+_gameLevel);
            } else if (_gameLevel == 3) { // Wave + Shield + Aggressive
                enemy.Speed = 2.5f;
                enemy.FireRate = 2f;
                enemy.WaveEnemy = Random.Range(0.0f, 5.0f) > 3;
                enemy.ShieldEnemy = !enemy.WaveEnemy ? Random.Range(0,2) == 1 : false;
                enemy.AggressiveEnemy = !enemy.WaveEnemy ? Random.Range(0,2) == 1 : false;
            } else if (_gameLevel == 4) { // Increase Enemy Speed + Wave + Shield + Aggressive
                enemy.FireRate = 1.9f;
                enemy.WaveEnemy = Random.Range(0.0f, 5.0f) > 3;
                enemy.Speed = enemy.WaveEnemy ? 1f : 3f;
                enemy.ShieldEnemy = !enemy.WaveEnemy ? Random.Range(0,2) == 1 : false;
                enemy.AggressiveEnemy = !enemy.WaveEnemy ? Random.Range(0,2) == 1 : false;
            }
            
            yield return new WaitForSeconds(_spawnEnemyTime);
        }
    }

    private IEnumerator SmartEnemyRoutine(){
        while(isSpawningEnemy){
            GameObject newEnemy = Instantiate(_smartEnemy);
            Enemy enemy = newEnemy.GetComponent<Enemy>();
            newEnemy.transform.parent = _enemiesContainer.transform;
            Debug.Log("new Smart Enemy");
            enemy.WaveEnemy = false;
            enemy.ShieldEnemy = false;
            enemy.AggressiveEnemy = false;
            enemy.SmartEnemy = true;
            enemy.FireRate = 2.3f;
            enemy.Speed = 1.8f;
            yield return new WaitForSeconds(_spawnEnemyTime);
        }
    }


    private IEnumerator AsteroidCoroutine(){
        yield return new WaitForSeconds(1f);
        while(isSpawningAsteroid){
            GameObject newAsteroid = Instantiate(_asteroid);
            newAsteroid.transform.parent = _enemiesContainer.transform;
            yield return new WaitForSeconds(_asteroidEnemyTime);
        }
    }

    private IEnumerator AdvancePowerUp(){ // Health + Super Laser
        yield return new WaitForSeconds(_spawnAdvancedPowerUpTime);
        while(isSpawningPowerUp){
            int powerUpId = Random.Range(0, _advancedPowerUps.Length);
            GameObject newPowerUp = _advancedPowerUps[powerUpId];
            GameObject current = Instantiate(newPowerUp, _powerUpContainer.transform);
            yield return new WaitForSeconds(_spawnAdvancedPowerUpTime);
        }
    }
    
    private IEnumerator BasicPowerUp(){ // Ammo + Triple + Shield + Speed
        yield return new WaitForSeconds(_spawnBasicPowerUpTime);
        while(isSpawningPowerUp){
            int powerUpId = shouldSpawnAmmo ? 1 : Random.Range(0, _basicPowerUps.Length);
            GameObject newPowerUp = _basicPowerUps[powerUpId];
            GameObject current = Instantiate(newPowerUp, _powerUpContainer.transform);
            yield return new WaitForSeconds(_spawnBasicPowerUpTime);

            if(shouldSpawnAmmo && powerUpId == 1){
                shouldSpawnAmmo = false;
            }
        }
    }
}
