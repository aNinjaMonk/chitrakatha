﻿using UnityEngine;
using System.Collections;

public class Game1 : MonoBehaviour {
	public Texture[] imgs;
	public GUISkin mygui;
	public MovieTexture[] PlayersGrowl;
	
	int state = 0;
	float startTime;
	float endTime;
	float startoffset = 3;
	float scale = 1;
	public GameObject[] players;	
	public GameObject dest;
	GetWinner playerwon;
	bool Gameover = false;	
	bool inc = true;
	CollisionFlags collisionFlags; 
	bool Player2start = false;
	
	public float speed = 500.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 500.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool grounded = false;
	
	float lookSpeed = 15.0f;
	float moveSpeed = 5.0f;
	 
	float rotationX = 0.0f;
	float rotationY = 0.0f;
	
	// Use this for initialization
	void Start () {
		playerwon = (GetWinner)dest.GetComponent("GetWinner");
		//AnimatorTimeline.Play("Take1");
		//playerwon.winState = 1;
	}
	void Awake () {
	    players[0].rigidbody.freezeRotation = true;
	    players[0].rigidbody.useGravity = false;
	}
	float OsciallateValues(float _scale,float _lower,float _upper,float _factor)
	{
			if(inc == true)
			{	if(_scale <= 1+_upper)	_scale += _factor;
				else		inc = false;
			}
			else
			{	if(_scale >= 1 - _lower) _scale -= _factor;
				else		inc = true;
			}
			return _scale;		
	}
	void OnGUI(){
		GUI.skin = mygui;
		if(state == 0)
		{
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),imgs[0]);
			if(GUI.Button(new Rect(Screen.width/2 - 180,3*Screen.height/4 - 50,300,300),imgs[1]) || Input.GetKeyDown(KeyCode.X))
			{
				state = 1;
				startTime = Time.time;
			}
			GUI.DrawTexture(new Rect(Screen.width/2 - 170,3*Screen.height/4 - 50,300 * scale,300 * scale),imgs[1],ScaleMode.StretchToFill);
		}
		else if(state == 2)
		{
			if((endTime-startTime >= startoffset) && (endTime-startTime <= startoffset + 3))
			{
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),PlayersGrowl[0]);
				PlayersGrowl[0].Play();
				GUI.DrawTexture(new Rect(0,Screen.height-200,300,200),imgs[2]);
			}
			else if((endTime-startTime >= startoffset + 3) && (endTime-startTime <= startoffset + 6))
			{
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),PlayersGrowl[1]);
				PlayersGrowl[1].Play();
				GUI.DrawTexture(new Rect(0,Screen.height-200,300,200),imgs[3]);
			}
			else if((endTime-startTime >= startoffset + 6) && (endTime-startTime <= startoffset + 7))
			{ 
				GUI.DrawTexture(new Rect(Screen.width/2 - imgs[4].width/4,Screen.height/2 - 100,300,200),imgs[4]);
			}
			else if(endTime-startTime >= startoffset + 7)
			{
				state = 4;
				SmoothFollow scr = (SmoothFollow)Camera.main.GetComponent("SmoothFollow");
				scr.isEnabled = true;
			}
		}
		else if(state == 3)
		{
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),imgs[8]);
			/*
			
			if(GUI.Button(new Rect(80,Screen.height/3,200,200),imgs[7]))
			{
				state = 0;				
			}*/			
			
		}
		else if(Gameover == false && playerwon.winState != 0)
		{	
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),imgs[8]);
			if(playerwon.winState == 1){
				GUI.DrawTexture(new Rect(Screen.width/2 - 100,20,imgs[5].width,2*imgs[5].height/3-50),imgs[5]);
			}
			else if(playerwon.winState == 2){
				GUI.DrawTexture(new Rect(Screen.width/2 - 100,20,imgs[6].width,2*imgs[6].height/3-50),imgs[6]);
			}
			if(GUI.Button(new Rect(80,Screen.height/3,200,200),imgs[7]) || Input.GetKeyDown(KeyCode.X))
			{
				state = 0;				
			}			
			GUI.DrawTexture(new Rect(40,Screen.height/3-30,300 * scale,300 * scale),imgs[7],ScaleMode.StretchToFill);
			//Gameover = true;
			//playerwon.winState = 0;
		}
	}
	// Update is called once per frame 
	void FixedUpdate () {
		endTime = Time.time;
		if(state == 0 || (playerwon.winState != 0 && Gameover == false))
		{
			if(inc == true)
			{	if(scale <= 1.05f)	scale += 0.005f;
				else		inc = false;
			}
			else
			{	if(scale >= 0.95f) scale -= 0.005f;
				else		inc = true;
			}
		}
		if(state == 1)
		{
			AnimatorTimeline.Play("Take1");
			state = 2;
		}
		else if(state == 4)
		{
			if(Player2start == false)
			{
				AnimatorTimeline.Play("Take2");
				//players[1].animation.Play();
				Player2start = true;
			}			
			
			/*rotationY += Input.GetAxis("Mouse Y")*lookSpeed;
			rotationY = Mathf.Clamp (rotationY, -90, 90);*/
			//players[0].transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
			//players[0].transform.position += players[0].transform.right*moveSpeed*Input.GetAxis("Horizontal");			
			
		 	rotationX += Input.GetAxis("Mouse X")*lookSpeed;
			players[0].transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
			players[0].transform.position += players[0].transform.forward*moveSpeed*Input.GetAxis("Vertical");
			
			/*
			if (grounded) {
	        // Calculate how fast we should be moving
		        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		        targetVelocity = transform.TransformDirection(targetVelocity);
		        targetVelocity *= speed;
	 
		        // Apply a force that attempts to reach our target velocity
		        Vector3 velocity = players[0].rigidbody.velocity;
		        Vector3 velocityChange = (targetVelocity - velocity);
		        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		        velocityChange.y = 0;
		        players[0].rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	 			Debug.Log("Jump");
		        // Jump
		        if (canJump && Input.GetButton("Jump")) {
		            players[0].rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
		        }
		    }
	 
		    // We apply gravity manually for more tuning control
		    players[0].rigidbody.AddForce(new Vector3 (0, -gravity * players[0].rigidbody.mass, 0));
	 
		    grounded = false;
			*/
			
			/*var v = Input.GetAxisRaw("Vertical");
			var h = Input.GetAxisRaw("Horizontal");
			
			Transform cameraTransform = Camera.main.transform;
			
			Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
			
			Vector3 right = new Vector3(forward.z, 0, -forward.x);
			
			forward.y = 0;
			forward = forward.normalized;
			
			Vector3 targetDir = forward * v + right * h;
			
			//1st Player Controls
			if(Input.GetKeyDown(KeyCode.W)){
				//players[1].transform.position += forward * 1;
				players[0].rigidbody.AddForce(targetDir.normalized * (500)); 
				//players[0].rigidbody.
			}
			else if(Input.GetKey(KeyCode.A)){
				players[0].rigidbody.AddTorque(targetDir.normalized * (500));
				//players[0].animation.Stop();				
			}
			else if(Input.GetKey(KeyCode.D)){
				
			}*/
		}
	}
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
}
