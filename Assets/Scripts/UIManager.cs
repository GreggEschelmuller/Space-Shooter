using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelText;
    private GameManager _gameManager;
    [SerializeField]
    private Text _shieldsText;
    [SerializeField]
    private Text _ammoText;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _shieldsText.text = "Shields Health: " + 0;
        _ammoText.text = "Ammo: " + 15;
        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
          
    }

    public void  UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateShields(int shieldHealth)
    {
        _shieldsText.text = "Shields Health: " + shieldHealth.ToString();
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();

        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
