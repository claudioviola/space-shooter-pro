using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAsteroid : Asteroid
{

    override protected void InitMe(){
        // print("ciao");
        _renderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    override protected void Update()
    {
        gameObject.transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    override protected IEnumerator DestroyMe(){
        _isDestroing = true;
        _audioSource.Play();
        GameObject explosion = Instantiate(_explosion, this.gameObject.transform);
        yield return new WaitForSeconds(0.5f);
        _renderer.enabled = false;
        yield return new WaitForSeconds(2.1f);
        _gameManager.OnFirstAsteroidDestroyed();
        Destroy(this.gameObject);
    }

}
