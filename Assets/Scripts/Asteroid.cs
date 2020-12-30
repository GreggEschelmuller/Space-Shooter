using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 0.5f;
    [SerializeField]
    private GameObject _asteroidAnim;
    private SpawnManager _spawnManager;
    [SerializeField]

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, _speed));
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject newAnim = Instantiate(_asteroidAnim, currentPos, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawing();
            Destroy(this.gameObject, 0.25f);
            Destroy(newAnim, 3.0f);
        }
    }
}
