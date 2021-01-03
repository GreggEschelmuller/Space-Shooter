using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // _ before variable indicates private
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    private float _boost = 1.5f; //left shift boost
    private int _ammo = 15;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private bool _isMultiShotActive = false;
    private int _shieldHealth = 3;

    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _multiShot;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;


    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;

    [SerializeField]
    private AudioSource _laserSound;
 


// Start is called before the first frame update
void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        
        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_laserSound == null)
        {
            Debug.LogError("Laser audio on player is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_isSpeedBoostActive == true)
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift)) //while left shift is held time - movement is multiplied by boost
            {
                transform.Translate(direction * _speed * _boost * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction * _speed * Time.deltaTime);
            }
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        if (_ammo > 0)
        {
            _canFire = Time.time + _fireRate;
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShot, transform.position + new Vector3(-2.38f, 0.59f, 6.95f), Quaternion.identity);
                _ammo -= 1;
                _uiManager.UpdateAmmo(_ammo);
            }
            else if (_isMultiShotActive == true)
            {
                Instantiate(_multiShot, transform.position + new Vector3(-2.4f, 1.0f, -0.36f), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                _ammo -= 1;
                _uiManager.UpdateAmmo(_ammo);
            }
            _laserSound.Play();
        }
    }

    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            if (_shieldHealth > 1)
            {
                _shieldHealth -= 1;
                _uiManager.UpdateShields(_shieldHealth);
            }
            else
            {
                _uiManager.UpdateShields(0);
                _isShieldsActive = false;
                _shieldVisualizer.SetActive(false);
            }
            return;
        }
        else
        {
            _lives -= 1;
            if(_lives == 2)
            {
                _rightEngine.SetActive(true);
            }
            else if(_lives == 1)
            {
                _leftEngine.SetActive(true);
            }
            _uiManager.UpdateLives(_lives);
            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
    }


    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDown());
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void MultiShotActive()
    {
        _isMultiShotActive = true;
        StartCoroutine(MultiShotPowerDown());
    }
    IEnumerator MultiShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isMultiShotActive = false;
    }

    public void ShieldActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldHealth = 3;
        _uiManager.UpdateShields(_shieldHealth);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void UpdateAmmo()
    {
        _ammo += 10;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void UpdateHealth()
    {
        if (_lives < 3)
        {
            if (_lives == 1)
            {
                _lives += 1;
                _uiManager.UpdateLives(_lives);
                _leftEngine.SetActive(false);
            }
            else if(_lives == 2)
            {
                _lives += 1;
                _uiManager.UpdateLives(_lives);
                _rightEngine.SetActive(false);
            }
        }
    }
}
