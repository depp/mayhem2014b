﻿using UnityEngine;
using System.Collections;

public class Monster : FourDirectionMob {
	public float health;
	public float damage;
	public float attackSpeed;
	
	public float curMomentum;
	float maxMomentum = 50f;
	float slapStrength = 2.0f;
	float enticeStrength = 3.0f;
	
	float wanderThreshold = 5.0f;
	float wanderCooldownTime = 1.5f;
	float wanderCooldown;
	
	public GameObject prefabHuman;

	// Use this for initialization
	new public void Start () {
		RandomizeFacing();
		wanderCooldown = wanderCooldownTime;
	}
	
	// Update is called once per frame
	new public void Update () {
		//handle input
		if (Input.GetKeyDown (KeyCode.Space)) {
			Slap();
		}
		
		if (Input.GetMouseButtonDown(0)) {
			//Yeah this is a really silly way to do it but meh
			var pos = Camera.main.WorldToScreenPoint(transform.position);
			var dir = Input.mousePosition - pos;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			angle = (angle + 315) % 360;
			FourDirectionMob.Direction enticeDirection = (FourDirectionMob.Direction)Mathf.Floor(angle / 90);
			
			//Entice(enticeDirection);
			
			var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = 5;
		}
		
		if (Input.GetMouseButtonDown(1)) {
			var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = 5;
			
			GameObject.Instantiate(prefabHuman, worldPos, Quaternion.identity);
		}
		
		//TODO: check for things to respond to
		
		//wander if momentum is critically low
		if (curMomentum < wanderThreshold && wanderCooldown <= 0) {
			RandomizeFacing();
			wanderCooldown = wanderCooldownTime;
		}
		
		//if nothing interesting, just move forward
		float moveSpeed = runSpeed * (curMomentum / maxMomentum);
		Move (curDirection, moveSpeed);
		
		//decrement timer(s)
		if (wanderCooldown > 0)
			wanderCooldown -= Time.deltaTime;
	}
	
	//punish the monster, slowing it down and hurting its feelings
	public void Slap() {
		//TODO: play slap animation
		curMomentum = Mathf.Max(curMomentum - slapStrength, 0);
	}
	
	//encourage the monster in a certain direction. if the direction differs from
	//the current direction, will reduce momentum
	public void Entice(Direction dir) {
		//TODO: play entice animation
		if (dir == curDirection) {
			curMomentum = Mathf.Min(curMomentum + enticeStrength, maxMomentum);
		} else if (curMomentum < wanderThreshold) {
			SetFacing(dir);
		}
	}
}
