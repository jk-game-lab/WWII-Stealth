#pragma strict

function Start () {

}

function Update () {

}

function OnCollisionEnter( other : Collision ) {
	if (other.gameObject.name == "Bullet(Clone)") {
		Destroy(this.gameObject);
		Destroy(other.gameObject);	
		Scoreboard.score = Scoreboard.score + 10;
	}
}
