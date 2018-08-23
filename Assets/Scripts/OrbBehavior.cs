using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehavior : MonoBehaviour {

    GameBehavior game;
    private float m_speed;
    public float speed = 0;

    private Collider2D fragTarget = null;

    // Use this for initialization
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        m_speed = game.GetSpeed() + speed *-1;
        this.transform.position = new Vector3(this.transform.position.x - m_speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
    }

    public void TeleFrag()
    {
        if(fragTarget != null)
        {
            fragTarget.gameObject.GetComponent<EnemyBehavior>().Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            fragTarget = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        fragTarget = null;
    }
}
