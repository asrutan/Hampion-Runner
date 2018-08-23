using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour {

    private float m_scrollSpeed = 5;

    private Vector3 m_startSpot;
    private Vector3 m_endSpot;

    private GameObject[] sprites;

    public GameObject prefabFloor;

    //private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        /*
        sprites = new GameObject[3];
	    for(int i = 0; i < 3; i++)
        {
            sprites[i] = this.transform.GetChild(i).gameObject;
        }     

        m_startSpot = sprites[0].transform.localPosition;
        m_endSpot = sprites[2].transform.localPosition;
        //m_endSpot = new Vector3(m_endSpot.x, m_endSpot.y, m_endSpot.z);
        */
    }
	

	void FixedUpdate () {
        //Scroll();
	}

    public void Scroll()
    {
        for(int i = 0; i < 3; i++)
        {
            sprites[i].transform.position = new Vector3(sprites[i].transform.position.x - m_scrollSpeed * Time.deltaTime, sprites[i].transform.position.y, sprites[i].transform.position.z);
            if(sprites[i].transform.localPosition.x <= m_endSpot.x)
            {
                sprites[i].transform.localPosition = new Vector3(m_startSpot.x, sprites[i].transform.localPosition.y, sprites[i].transform.localPosition.z);
            }
        }
    }

    public void SetSpeed(float speed)
    {
        m_scrollSpeed = speed;
    }
}
