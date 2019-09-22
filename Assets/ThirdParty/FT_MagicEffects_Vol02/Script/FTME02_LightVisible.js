#pragma strict
var myLight : Light;

function Start ()
{
    myLight = GetComponent(Light);
}

function Update () {

	if (Input.GetKeyDown(KeyCode.UpArrow)){
		if(myLight.intensity == 0)
		myLight.intensity = 0.4;
		else
		myLight.intensity = 0;
		}
}