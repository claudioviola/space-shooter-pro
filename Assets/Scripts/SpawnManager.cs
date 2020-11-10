using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemiesContainer;
    [SerializeField]
    private GameObject _powerUpContainer;
    [SerializeField]
    private GameObject[] _powerUps;
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
    
    public void OnPlayerDeath(){
        StopCoroutine("EnemyRoutine");
        StopCoroutine("PowerUpRoutine");
    }

    public void initMe(){
        StartCoroutine("EnemyRoutine");
        StartCoroutine("PowerUpRoutine");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    private IEnumerator PowerUpRoutine(){
        yield return new WaitForSeconds(_spawnPowerUpTime);
        while(isSpawningPowerUp){
            int powerUpId = Random.Range(0, _powerUps.Length-1);
            GameObject newPowerUp = _powerUps[powerUpId];
            // Instantiate utilizzo con il passaggio del container come secondo parametro 
            // https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
            // si evita il current.transform.parent = _powerUpContainer.transform;
            GameObject current = Instantiate(newPowerUp, _powerUpContainer.transform);
            yield return new WaitForSeconds(Random.Range(_minPowerUpTime, _maxPowerUpTime));
        }
    }
}
