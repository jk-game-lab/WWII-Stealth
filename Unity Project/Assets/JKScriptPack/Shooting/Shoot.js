#pragma strict

public var bullet : GameObject;
public var speed : float = 5;

function Start () {

}

function Update () {

	if (Input.GetButtonDown("Fire1")) {
		Fire();
	}

}

function Fire () {
	
	var projectile : GameObject;
	projectile = Instantiate(bullet, this.transform.position, this.transform.rotation );
	projectile.GetComponent.<Rigidbody>().velocity = speed * this.transform.forward;

	Destroy(projectile, 2.0f);
		
}