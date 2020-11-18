using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemy;
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
    public void NeedAmmo(){
        print("NeedAmmo");
        shouldSpawnAmmo = true;
    }
    
    public void OnPlayerDeath(){
        StopCoroutine("EnemyRoutine");
        StopCoroutine("BasicPowerUp");
        StopCoroutine("AdvancePowerUp");
        StopCoroutine("SuperLaserPowerUpRoutine");
        
    }

    public void initMe(){
        StartCoroutine("EnemyRoutine");
        StartCoroutine("BasicPowerUp");
        StartCoroutine("AdvancePowerUp");
    }

    public void PlayLevel(int level){
        switch(level){
            case 1:
                _gameLevel = 1;
                StartCoroutine("AsteroidCoroutine");
            break;
            case 2:
                // start shield enemies and agressive enemy
                _gameLevel = 2;
            break;     
            case 3:
                // start wave enemy
                _gameLevel = 3;
            break;   
            case 4:
                // increase enemy speed
                _gameLevel = 4;
            break;   
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Called by GameManager
    public void StartMe(){
        initMe();
    }

    private IEnumerator EnemyRoutine(){
        yield return new WaitForSeconds(1f);
        while(isSpawningEnemy){
            GameObject newEnemy = Instantiate(_enemy);
            Enemy enemy = newEnemy.GetComponent<Enemy>();
            newEnemy.transform.parent = _enemiesContainer.transform;
            print("_gameLevel"+_gameLevel);
            if(_gameLevel == 2){
                enemy.ShieldEnemy = true;
                enemy.AggressiveEnemy = true;
            } else if (_gameLevel == 3) {
                enemy.ShieldEnemy = true;
                enemy.AggressiveEnemy = true;
                enemy.WaveEnemy = true;
            } else if (_gameLevel == 4) {
                enemy.ShieldEnemy = true;
                enemy.AggressiveEnemy = true;
                enemy.WaveEnemy = true;
                enemy.Speed = enemy.Speed + 2;
            }
            
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
            int powerUpId = Random.Range(0, _advancedPowerUps.Length-1);
            GameObject newPowerUp = _advancedPowerUps[powerUpId];
            GameObject current = Instantiate(newPowerUp, _powerUpContainer.transform);
            yield return new WaitForSeconds(_spawnAdvancedPowerUpTime);
        }
    }
    
    private IEnumerator BasicPowerUp(){ // Ammo + Triple + Shield + Speed
        yield return new WaitForSeconds(_spawnBasicPowerUpTime);
        while(isSpawningPowerUp){
            int powerUpId = shouldSpawnAmmo ? 1 : Random.Range(0, _basicPowerUps.Length-1);
            GameObject newPowerUp = _basicPowerUps[powerUpId];
            GameObject current = Instantiate(newPowerUp, _powerUpContainer.transform);
            yield return new WaitForSeconds(_spawnBasicPowerUpTime);

            if(shouldSpawnAmmo && powerUpId == 1){
                shouldSpawnAmmo = false;
            }
        }
    }
}
