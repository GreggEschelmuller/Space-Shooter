using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Animator _deathAnimation;

    [SerializeField]
    private AudioSource _explosion;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosion = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _deathAnimation = GetComponent<Animator>();

        if(_deathAnimation == null)
        {
            Debug.LogError("Animator is NULL");
        }
        if (_explosion == null)
        {
            Debug.LogError("Audio source is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.3)
        {
            transform.position = new Vector3(Random.Range(-9f,9f), 7, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
    
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _deathAnimation.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosion.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
 
        }
        else if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _deathAnimation.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosion.Play();

            Destroy(this.gameObject, 2.8f);
        }
    }
}
