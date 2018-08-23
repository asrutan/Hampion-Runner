using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehavior : MonoBehaviour {

    private float m_width = 5;
    private float m_height = 5;

    private BoxCollider2D topCollider;
    private BoxCollider2D sideCollider;

    private SpriteRenderer spriteRenderer;

    GameBehavior game;
    private float m_speed;

    // Use this for initialization
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameBehavior>();
    }

    void Update()
    {
        m_speed = game.GetSpeed();
        this.transform.position = new Vector3(this.transform.position.x - m_speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
    }

    public void Init(float width, float height)
    {
        m_width = width;
        m_height = height;

        InitializeSprite();
        InitializeColliders();
    }

    void InitializeSprite()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(m_width, m_height);
        //spriteRenderer.size.Equals;
    }

    void InitializeColliders()
    {
        topCollider = this.transform.GetChild(0).GetComponent<BoxCollider2D>();
        sideCollider = this.transform.GetChild(1).GetComponent<BoxCollider2D>();

        topCollider.size = new Vector2(m_width, m_height);
        sideCollider.size = new Vector2(m_width, m_height - 1);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Despawner")
        {
            game.DecFloor();
            Destroy(this.gameObject);
        }
    }
}
