using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    private Rigidbody2D thisRigidbody;
    public float jumpForce = 10000;

    private GameBehavior game;

    public GameObject orbPrefab;
    public GameObject pigPrefab;

    private GameObject orb;
    private OrbBehavior orbScript;

    private bool jumping = true;
    private bool jButtonHeld = false;

    public float defaultSpeed = 20;
    private float m_speed = 20;

    private float m_orbLifeTimer = 4f;
    private float orbCoolDown = 10;

    private float speedIncrease = 15;
    private float speedDecrease = 10;

    private float m_maxSpeed = 100;
    private float m_minSpeed = 20;

    public bool abilityActive = false;
    public bool abilityCooldown = false;

    public Sprite[] sprites;

    private SpriteRenderer sr;
    private int srIndex = 0;
    // Use this for initialization
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameBehavior>();
        thisRigidbody = this.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        orb = Instantiate(orbPrefab);
        orbScript = orb.GetComponent<OrbBehavior>();
        orb.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!abilityActive)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jButtonHeld = false;
            }
            else
            {
                ApplySlowdown();
            }
        }
    }

    void Update()
    {
        if (!abilityActive)
        {
            Animate();
        }
    }

    public void Jump()
    {
        if (!jumping)
        {
            jButtonHeld = true;
            jumping = true;
            thisRigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode2D.Impulse);
        }
    }

    void ApplySlowdown()
    {
        if (m_speed > m_minSpeed && !jumping)
        {
            m_speed -= speedDecrease;
        }
    }

    void Animate()
    {
        if (jumping)
        {
            if (srIndex != 1)
            {
                srIndex = 1;
                sr.sprite = sprites[srIndex];
            }
        }
        else
        {
            if (srIndex != 0)
            {
                srIndex = 0;
                sr.sprite = sprites[srIndex];
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!abilityActive)
        {
            if (other.gameObject.tag == "Floor")
            {
                jumping = false;
                ApplySlowdown();
                if (jButtonHeld && m_speed < m_maxSpeed)
                {
                    m_speed += speedIncrease;
                }
            }
            else if (other.gameObject.tag == "Hurt")
            {
                m_speed = 0;
                game.EndGame();
            }
        }
    }

    public float GetSpeed()
    {
        return m_speed;
    }

    public void CastOrb()
    {
        if (!abilityActive && !abilityCooldown)
        {
            abilityActive = true;
            sr.enabled = false;
            Instantiate(pigPrefab).transform.position = this.transform.position;
            orb.SetActive(true);
            orb.transform.position = this.transform.position;
            m_speed = orbScript.speed;
        }
        else if(abilityActive)
        {
            orbScript.TeleFrag();

            game.AbilityOnCoolDown();
            abilityActive = false;
            this.transform.position = orb.transform.position;

            orb.SetActive(false);
            sr.enabled = true;
            m_speed = defaultSpeed;
        }
    }
}
