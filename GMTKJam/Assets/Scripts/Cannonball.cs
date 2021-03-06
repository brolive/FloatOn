﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour {

    Vector3 moveDirection;
    public float speed = 1.0f;

    public bool ready = false;
    public float elapsed = 0.0f;

    public GameObject owner;

	// Use this for initialization
	void Start () {

        // Rotate to player

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        float distance = Vector2.Distance(player.position, transform.position);

        float angleRad = Mathf.Atan2(player.position.y - transform.position.y,
                                     player.position.x - transform.position.x);
        float angle = (180 / Mathf.PI) * angleRad;

        transform.rotation = Quaternion.Euler(0, 0, angle + 270);

        moveDirection = transform.up * 4;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        elapsed += Time.deltaTime;
        if (elapsed > 0.75)
            ready = true;

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        RaycastHit2D hit = Physics2D.Raycast(transform.position,
                                           moveDirection,
                                           .5f);
        if (hit.collider != null)
        {
            //Debug.Log("ball hit");
            if (hit.collider.tag == "Tile")
            {
                //Debug.Log("TILE");
                Explode();
            }

            if (hit.collider.tag == "Player")
            {
                //Debug.Log("PLAYER");
                HitPlayer();
                Explode();

            }
        }

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < 1)
            {
                HitPlayer();
                Explode();
            }
        }

        GameObject belch = GameObject.FindGameObjectWithTag("Belch");
        if (belch != null)
        {
            float bDistance = Vector2.Distance(transform.position, belch.transform.position);
            if (bDistance < 2)
            {
                Debug.Log("REFLECT");
                //moveDirection *= -1;
                moveDirection = player.transform.up * 5;
                Destroy(belch);
            }
        }
    }

    public void Explode()
    {
        GameObject e = Instantiate(GameManager.instance.explosion, transform.position, Quaternion.Euler(0,0,0));
        e.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        Destroy(gameObject);
    }

    void HitPlayer()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().moveDirection = moveDirection * 2;
        GameObject.Find("Player").GetComponent<PlayerController>().gasAmount = 0;
        GameObject.Find("Player").GetComponent<PlayerController>().state = PlayerController.PlayerStates.Fall;

    }
}
