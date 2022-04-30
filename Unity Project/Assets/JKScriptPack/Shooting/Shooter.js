#pragma strict

public var bullet : GameObject;
public var speed : float = 5.0f;

function Start () {

}

function Update () {

	if ( Input.GetButtonDown("Fire1") ) {
	
		var projectile : GameObject;
		projectile = Instantiate(bullet, this.transform.position, this.transform.rotation);

		projectile.GetComponent.<Rigidbody>().velocity = this.transform.forward * speed;
			
		Destroy(projectile, 2.0f);
		
	}

}


