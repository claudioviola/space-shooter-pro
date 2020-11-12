using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{


    [SerializeField]
    private float _slotCharge = 2f;
    [SerializeField]
    private float _maxThrusterCharge = 100f;
    [SerializeField]
    private float _thrusterChargeTick = 0.05f;
    private float _currentThrusterCharge = 100f;
    private Vector3 _lastPosition;
    private bool _chargeCoroutineIsRunning;
    private bool _throttlingCoroutineIsRunning;
    [SerializeField]
    private float _thrusterFlickeringIndicator = 10.0f;
    private float _timeFlickering = 0.05f;
    private float _canFlickering = 0f;
    private SpriteRenderer _spriteRender;

    public float getCharge(){
        return _currentThrusterCharge;
    }
    
    
    public float getLeftPercantage(){
        return _currentThrusterCharge / _maxThrusterCharge;
    }

    public void UpdateChargeThruster(){
        if(_lastPosition != transform.position){
            _currentThrusterCharge = 
                _currentThrusterCharge - _thrusterChargeTick > 0 
                ? _currentThrusterCharge - _thrusterChargeTick 
                : 0;
            _lastPosition = transform.position;
            if(_chargeCoroutineIsRunning){
                StopCoroutine("RechargeThursterRoutine");
                _chargeCoroutineIsRunning = false;
            }
        } else {
            if(!_chargeCoroutineIsRunning){
                StartCoroutine("RechargeThursterRoutine");
            }
        }
    }

    IEnumerator RechargeThursterRoutine(){
        _chargeCoroutineIsRunning = true;
        while(_currentThrusterCharge < _maxThrusterCharge){
            // print(_lastPosition == transform.position);
            _currentThrusterCharge = _currentThrusterCharge + _slotCharge < 100f 
                ? _currentThrusterCharge + _slotCharge 
                : 100f;
            print("_currentThrusterCharge"+_currentThrusterCharge);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Start()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _currentThrusterCharge = _maxThrusterCharge;
        _lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChargeThruster();
        print("_currentThrusterCharge"+_currentThrusterCharge);
        if(_currentThrusterCharge < _thrusterFlickeringIndicator){
            if(Time.time > _canFlickering && _spriteRender.enabled){
                _canFlickering = Time.time + _timeFlickering;
                _spriteRender.enabled = false;
            }

            if(Time.time > _canFlickering && !_spriteRender.enabled){
                _canFlickering = Time.time + _timeFlickering;
                _spriteRender.enabled = true;
            }
        } else {
            _spriteRender.enabled = true;
        }
    }
}
