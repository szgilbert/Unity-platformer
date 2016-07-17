using UnityEngine;
using System.Collections;

//created by Sam Gilbert
//last modified 12/19/2013
//script for character controller. contains values for movement and health
//also used for any triggers or collisions that affect the player

[RequireComponent(typeof(PlayerPhysics))]
public class Player : MonoBehaviour {
	
	public Transform myTransform;
	//palceholder for zero to use as a reseting value
	private float ZERO_RESET = 0f;
	//spawnpoint
	public Vector3 spawnPoint;
	
	// player speed
	public float BASE_FORWARD_SPEED = 10f;
	public float BASE_SIDEWAYS_SPEED = 5f;
	//player acceleration
	private float acceleration = 20f;
	//speed value trackers
	private float currentForwardSpeed;
	private float currentSidewaysSpeed;
	//max speeds
	private float targetForwardSpeed;
	private float targetSidewaysSpeed;
	
	//all forces are applied in playerPhysics to player
	private PlayerPhysics playerPhysics;
	
	//gravity 
	public float gravity = 1f;
	public float BASE_FALL_SPEED = 80f;
	private float maxFallSpeed;
	public float fallingAcceleration = 30f;
	
	//jumping variables
	private bool jumping;
	public float BASE_JUMP_HEIGHT = 5f;
	public float BASE_JUMP_SPEED = 5f;
	public float JUMP_PAD_SPEED = 40f;
	private float currentJumpSpeed;
	public float decceleration = 30f;
	
	//health related variables
	public float BASE_HEALTH = 100f;
	public static float health;
	private bool dead;
	private float damage = 5f;
	private float lastTick; 
	public float damageRate = 0.1f;

	// Use this for initialization
	void Start () {
		//
		myTransform = transform;
		//sets playerPhysics variable to use methods in that script without initializing an object of it
		playerPhysics = GetComponent<PlayerPhysics>();
		//makes sure jump speed starts at 0 and player is not jumping;
		currentJumpSpeed = ZERO_RESET;
		jumping = false;
		//makes sure you don't start dead and sets initial health value
		health = BASE_HEALTH;
		dead = false;
		//player initial Spawn Point
		spawnPoint = new Vector3(965,22,445);
	}
	void Update(){
		//speeds set in editor
		targetSidewaysSpeed = Input.GetAxisRaw("Horizontal") * BASE_SIDEWAYS_SPEED;
		targetForwardSpeed = Input.GetAxisRaw("Vertical") * BASE_FORWARD_SPEED;
		//current player speeds depend on how long the player has been accelerating, increment towards creates an acceleration curve
		currentForwardSpeed = playerPhysics.IncrementTowards(currentForwardSpeed, targetForwardSpeed, acceleration);
		currentSidewaysSpeed = playerPhysics.IncrementTowards(currentSidewaysSpeed, targetSidewaysSpeed, acceleration);
		//after initial jump command, the players jump velocity decreases as time goes by until it hits zero and gravity takes over
		if(jumping){
		currentJumpSpeed = playerPhysics.decrementTowards(currentJumpSpeed, decceleration);
		}
		//grounded actions
		if(playerPhysics.grounded){
			gravity = ZERO_RESET;
			maxFallSpeed = ZERO_RESET;
			//jumping		
			if(Input.GetButtonDown("Jump")){
				jump();
			}
		}
		//in-air actions
		else{
			if(currentJumpSpeed == ZERO_RESET){
				//apply gravity when not jumping
				fall();
			}
			else{
				playerPhysics.Jumping(Vector3.up * currentJumpSpeed * Time.deltaTime);
			}
		}
		//applying speed to players transform
		playerPhysics.MoveForward(Vector3.forward * currentForwardSpeed * Time.deltaTime);
		playerPhysics.MoveSideways(Vector3.right * currentSidewaysSpeed* Time.deltaTime);
		
		//checks if player is dead and sends them back to the current spawnpoint
		if(dead){
			myTransform.position = spawnPoint;
			currentForwardSpeed = ZERO_RESET;
			currentSidewaysSpeed = ZERO_RESET;
			health = BASE_HEALTH;
			dead = false;
		}
	}
	
		void OnTriggerEnter(Collider col){
		//jumppad trigger to launch player
		if(col.tag == "jumpPad"){
			gravity = ZERO_RESET;
			currentJumpSpeed = JUMP_PAD_SPEED;
			playerPhysics.Jumping(Vector3.up * currentJumpSpeed * Time.deltaTime);
			jumping = true;
		}
		//called if player hits the death floor
		if(col.tag == "death"){
			dead = true;
			gravity = 0;
		}
		//used to make dropping platforms
		if(col.tag == "dropPad"){
			Platform dropPad = col.GetComponent<Platform>();
			dropPad.raise = false;
			dropPad.drop = true;
	
		}
		//spawnpoint trigger resets spawnpoint to current location
		if(col.tag == "spawnPoint"){
			spawnPoint = transform.position;
		}
	}
	//used for any trigger that much be called many times while the player is standing still
	void OnTriggerStay(Collider col){
		//makes character take damage
		if(col.tag == "electric"){
			if(lastTick > damageRate){
				health -= damage;
				lastTick = ZERO_RESET;
			}
			lastTick += Time.deltaTime;
			if(health <= ZERO_RESET){
				dead = true;
			}
		}
		//allows character to stand on moving platforms without falling off
		if(col.tag == "movingBox"){
			transform.parent = col.transform.parent;
		}
		//heals player 
		if(col.tag == "heal"){
			if(lastTick > damageRate && health < BASE_HEALTH){
				health += damage;
				lastTick = ZERO_RESET;
			}
			lastTick += Time.deltaTime;
		}
		//used at the end of the level to exit program
		if(col.tag == "win"){
			Application.Quit();
		}
	}

	void OnTriggerExit(Collider col){
		//disconnects player from moving platforms transform
		if(col.tag == "movingBox"){
			transform.parent = null;
		}
	}
	//used to initiallize jump
	void jump(){
		currentJumpSpeed = BASE_JUMP_SPEED;
		playerPhysics.Jumping(Vector3.up * currentJumpSpeed * Time.deltaTime);
		jumping = true;
	}
	//used to initiallize falling
	void fall(){
		jumping = false;
		maxFallSpeed = BASE_FALL_SPEED;
		gravity = playerPhysics.IncrementTowards(gravity, maxFallSpeed, fallingAcceleration);
		playerPhysics.Falling(Vector3.down * gravity * Time.deltaTime);
	}
}