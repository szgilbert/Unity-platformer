using UnityEngine;
using System.Collections;

//created by Sam Gilbert
//last modified 12/19/2013
//script for use on platforms to control movement

public class Platform : MonoBehaviour {
	
	//values used in platform movement
	private float moveSpeed;
	public Vector3 startPos;
	private Vector3 currentPos;
	public Vector3 endPos;
	public float moveRate = 30;
	public bool isMovingPlat;
	//falling platform variables
	public float maxDropSpeed;
	public float currentDropSpeed;
	public float dropAcceleration;
	public bool drop;
	public bool raise;

	
	void Start () {
		currentPos = startPos;
		moveSpeed = 0;
		drop = false;
		raise = false;

	}
	void Update()
	{
		//moves platforms between two points by a set of increments that is reset after each frame
		if(isMovingPlat){
    		transform.position = Vector3.Lerp(currentPos,endPos, moveSpeed);
    		moveSpeed += Time.deltaTime/moveRate;
			if(moveSpeed > .1f){
				moveSpeed =1;
			}
			if(transform.position == endPos){
	    	SetTargetPosition(startPos);
		}
		currentPos = transform.position;
		}
		//when a player steps on a dropping platform it falls until it reaches its vertical endpoint
		if(drop){
			currentDropSpeed = IncrementTowards(currentDropSpeed, maxDropSpeed, dropAcceleration);
			transform.Translate(Vector3.down * currentDropSpeed * Time.deltaTime);
			if(transform.position.y < endPos.y){
				drop = false;
				raise = true;
			}
		}
		//after a falling platform reaches its vertical endpoint it resets
		if(raise){
			if(transform.position != startPos){
				currentPos = transform.position;
				transform.position = Vector3.Lerp(currentPos, startPos, moveSpeed);
				moveSpeed += Time.deltaTime/moveRate;
				currentDropSpeed = 0;
			}
			else{
				moveSpeed = 0;
			}
		}
		
	}
 	//when a platform reaches its endpoint SetTargetPosition makes its startpoint its new endpoint
	public void SetTargetPosition(Vector3 newTargetPosition){
   		startPos = transform.position;
    	endPos = newTargetPosition;
    	moveSpeed = 0;
	}
	
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
}
