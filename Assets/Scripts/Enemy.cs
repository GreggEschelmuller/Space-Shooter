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
    private GameObject _laserPrefab;

    [SerializeField]
    private AudioSource _explosion;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

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
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.3)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7, 0);
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
            Destroy(this.gameObject, 2.0f);
 
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

            Destroy(this.gameObject, 2.0f);
        }
    }
}
