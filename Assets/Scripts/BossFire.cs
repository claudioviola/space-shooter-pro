using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossFire : MonoBehaviour
{

    PlayableDirector _fireDirector;
    SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
        // gameObject.SetActive(false);
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _fireDirector = gameObject.GetComponent<PlayableDirector>();
        _fireDirector.Play();
        _fireDirector.Stop();
        _sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(){
        _fireDirector.Play();
        _sprite.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){ //hit twice the player
            print("OnTriggerEnter2D");
            Player player = other.GetComponent<Player>();
            player.OnHitMe();
            player.OnHitMe();
        }
    }

    private void OnAnimationEnd(){
        print("OnAnimationEnd");
        _fireDirector.enabled = false;
        gameObject.SetActive(false);
        Boss boss = transform.parent.GetComponent<Boss>();
        boss.OnFireAnimationEnd();
    }
}
