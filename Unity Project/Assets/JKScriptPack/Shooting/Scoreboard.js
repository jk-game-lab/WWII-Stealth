#pragma strict

public static var score : int;
public var display : UI.Text;

function Start () {
	score = 0;
}

function Update () {
	display.text = score.ToString();
}