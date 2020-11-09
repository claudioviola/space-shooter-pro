﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 12f;
    private float _limitUp = 7.5f;
    private float _limitDown = -7.5f;
    private bool _isEnemyLaser = false;

    public void SetEnemyLaser(){
        _isEnemyLaser = true;
    }

    public bool GetEnemyLaser(){
        return _isEnemyLaser;
    }

    void Update() {
       if(_isEnemyLaser){
           MoveDown();
       } else {
           MoveUp();
       }
    }

    void MoveUp(){
        Vector3 newPos = Vector3.up * _speed * Time.deltaTime;
        transform.Translate(newPos);
        
        if(transform.position.y > _limitUp){
            if(transform.parent.name.Contains("TripleShot")){
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown(){
        Vector3 newPos = Vector3.down * _speed * Time.deltaTime;
        transform.Translate(newPos);
        
        if(transform.position.y < _limitDown){
            if(transform.parent.name.Contains("TripleShot")){
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && _isEnemyLaser){
            Player player = other.GetComponent<Player>();
            player.OnHitMe();
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
