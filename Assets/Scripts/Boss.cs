using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _frequency = 4f;
    [SerializeField]
    private float _magnitude = 0.4f;
    [SerializeField]
    GameObject _explosion;
    [SerializeField]
    GameObject _damageRight;
    [SerializeField]
    GameObject _damageLeft;
    int _lives = 3;
    bool _isDestroying = false;
    bool _isIntroAnimationEnd = false;
    bool _isFireAnimationEnd = false;
    GameObject _spaceShip;
    [SerializeField]
    GameObject _bossFire;
    PlayableDirector _playable;


    public void PlayMe(){
        gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {   
        // gameObject.SetActive(false);
        _explosion.SetActive(false);
        _damageLeft.SetActive(false);
        _damageRight.SetActive(false);
        _spaceShip = transform.Find("Space_Ship").gameObject;
        _playable = transform.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isFireAnimationEnd && !_isDestroying){
            MoveMe();
        }
    }

    void MoveMe(){
        Vector3 newPos = Vector3.right * Mathf.Sin(Time.time * _frequency) * _magnitude;
        transform.Translate(newPos);
    }

    private void OnAnimationEnd(){
        _isIntroAnimationEnd = true;
        _bossFire.GetComponent<BossFire>().Fire();
    }

    public void OnFireAnimationEnd(){
        _isFireAnimationEnd = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Laser"){
            _lives--;
            if(_lives == 2){
                _damageLeft.SetActive(true);
            }
            if(_lives == 1){
                _damageRight.SetActive(true);
            }
            if(_lives < 1){
                _isDestroying = true;
                StartCoroutine("DestroyRoutine");
            }
            print("laser");
            Destroy(other.gameObject);
        }

        if(other.tag == "Player"){
            Player player = other.GetComponent<Player>();
            player.OnHitMe();
        }
    }


    IEnumerator DestroyRoutine(){
        _explosion.SetActive(true);
        yield return new WaitForSeconds(1f);
        _damageLeft.SetActive(false);
        _damageRight.SetActive(false);
        _spaceShip.SetActive(false);
        Destroy(gameObject, 2.3f);
    }
}
