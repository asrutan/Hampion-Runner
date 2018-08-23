using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public float fireInterval = 2;
    private float m_timeStart;
    private float m_speed;
    public float speed = 0;
    public GameObject rocketPrefab;
    private GameBehavior game;

	// Use this for initialization
	void Start () {
        game = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameBehavior>();
        m_timeStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if(!game.gameOver)
        {
            m_speed = game.GetSpeed() + speed;
            this.transform.position = new Vector3(this.transform.position.x - m_speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            if (Time.time - m_timeStart >= fireInterval)
            {
                Fire();
                m_timeStart = Time.time;
            }
        }
	}

    void Fire()
    {
        Instantiate(rocketPrefab).transform.position = this.transform.position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Despawner")
        {
            Debug.Log(other.gameObject);
            Die();
        }
    }

    public void Die()
    {
        game.SpawnEnemy();
        Destroy(this.gameObject);
    }
}
