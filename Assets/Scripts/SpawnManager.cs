using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _superLaserPowerUp;
    [SerializeField]
    private GameObject _enemiesContainer;
    [SerializeField]
    private GameObject _powerUpContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private float _superLaserPowerUpTime = 30f;
    [SerializeField]
    private float _spawnEnemyTime = 2f;
    [SerializeField]
    private float _spawnPowerUpTime = 2f;
    [SerializeField]
    private float _minPowerUpTime = 3f;
    [SerializeField]
    private float _maxPowerUpTime = 7f;
    [SerializeField]
    private bool isSpawningEnemy = true;
    [SerializeField]
    private bool isSpawningPowerUp = true;
    private bool shouldSpawnAmmo = false;

    public void NeedAmmo(){
        print("NeedAmmo");
        shouldSpawnAmmo = true;
    }
    
    public void OnPlayerDeath(){
        StopCoroutine("EnemyRoutine");
        StopCoroutine("PowerUpRoutine");
        StopCoroutine("SuperLaserPowerUpRoutine");
    }

    public void initMe(){
        StartCoroutine("EnemyRoutine");
        StartCoroutine("PowerUpRoutine");
        StartCoroutine("SuperLaserPowerUpRoutine");
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
            newEnemy.transform.parent = _enemiesContainer.transform;
            yield return new WaitForSeconds(_spawnEnemyTime);
        }
    }

    private IEnumerator SuperLaserPowerUpRoutine(){
        while(true){
            yield return new WaitForSeconds(_superLaserPowerUpTime);
            Instantiate(_superLaserPowerUp, _powerUpContainer.transform);
        }
    }
    private IEnumerator PowerUpRoutine(){
        yield return new WaitForSeconds(_spawnPowerUpTime);
        while(isSpawningPowerUp){
            int powerUpId = shouldSpawnAmmo ? 4 : Random.Range(0, _powerUps.Length-1);

            GameObject newPowerUp = _powerUps[powerUpId];
            // Instantiate utilizzo con il passaggio del container come secondo parametro 
            // https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
            // si evita il current.transform.parent = _powerUpContainer.transform;
            GameObject current = Instantiate(newPowerUp, _powerUpContainer.transform);
            yield return new WaitForSeconds(Random.Range(_minPowerUpTime, _maxPowerUpTime));

            if(shouldSpawnAmmo && powerUpId == 4){
                shouldSpawnAmmo = false;
            }
        }
    }
}
