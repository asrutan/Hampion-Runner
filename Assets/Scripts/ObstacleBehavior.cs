using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour {

    GameBehavior game;
    private float m_speed;
    public float speed = 0;

	// Use this for initialization
	void Start () {
        game = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
       m_speed = game.GetSpeed() + speed;
       this.transform.position = new Vector3(this.transform.position.x - m_speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Despawner")
        {
            Destroy(this.gameObject);
        }
    }
}
