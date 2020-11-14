using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperLaser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 12.0f;
    private bool _isAnimationEnd = false;
    private float _canFire;
    [SerializeField]
    private float _fireRate = 1f;
    private GameObject _player;

    public bool GetEnemyLaser(){
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(_isAnimationEnd && Time.time > _canFire){
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if(transform.position.y > 7.41f){
                Destroy(this.gameObject);
            }
        } else {
            transform.position = _player.transform.position;
        }
    }

    public void PlayMe(){
        print("PlayMe");
        _canFire = Time.time + _fireRate;
        _isAnimationEnd = true;
    }
}
