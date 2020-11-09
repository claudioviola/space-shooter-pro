using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{


    
    [SerializeField]
    private AudioClip _powerUpAudioClip;
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private string _type = "triple";
    private float _initY = 7.3f;
    private float _limitLx = -9;
    private float _limitRx = 9;

    // Start is called before the first frame update
    void Start()
    {
        float initX = Random.Range(_limitLx, _limitRx);
        transform.position = new Vector3(initX, _initY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6.5f){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            Player player = other.GetComponent<Player>();
            if(player){
                player.OnCollect(_type);
            }
            AudioSource.PlayClipAtPoint(_powerUpAudioClip, transform.position);
            Destroy(this.gameObject);
        }
    }
}
