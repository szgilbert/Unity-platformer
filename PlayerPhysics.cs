using UnityEngine;
using System.Collections;

//created by Sam Gilbert
//last modified 12/19/2013
//script used in conjuntion with Player class
// applies vectors to player's transform and contains the increment/decrement methods

public class PlayerPhysics : MonoBehaviour {
	public bool grounded;

	
	//player (gameobject) aka transform to move when i press the wasd keys
	public void MoveForward(Vector3 moveAmountF) {
		//player to move forward back
		transform.Translate(moveAmountF);
	}
	
	public void MoveSideways(Vector3 moveAmountS){
		//player to move left and right
		transform.Translate(moveAmountS);
	}
	
	public void Falling(Vector3 gravity){
		//apply gravity to player
		transform.Translate(gravity);
	}
	
	public void Jumping(Vector3 jumpForce){
		//apply jump force
		
		transform.Translate(jumpForce);
		
	}
	//collisions
	void OnCollisionEnter(Collision col){
		//stops gravity from affecting the player when in contact with ground
		if(col.collider.tag == "Platform"){
			grounded = true;
		}
	}
	//reapplys gravity to player when not jumping
	void OnCollisionExit(Collision col){
		if(col.collider.tag == "Platform")
		{
			grounded = false;
		}
	}
	//used to create curved increments instead of linear ones
	public float IncrementTowards(float n, float target, float a){
		if(n == target){
		return n;	
		}
		else{
			float dir = Mathf.Sign(target - n);
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target - n))? n: target;// if n has passed target then return target, otherwise return n
		}
	}
	//reverse of increment
	public float decrementTowards(float n, float d){
		if(n<=0){
			return 0;
		}
		else{
			n -= d * Time.deltaTime;
			return n;
		}
	}
}
