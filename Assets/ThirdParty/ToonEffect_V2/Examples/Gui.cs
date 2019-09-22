using UnityEngine;
using System.Collections;

public class Gui : MonoBehaviour {
	
	public GameObject[] ssNumbers;
	public GameObject[] ssPunchs;
	public GameObject[] ssWords;
	public GameObject[] sequences;
	private int currentEffectIndex=2;
	private GameObject[] currentEffects;
	
	private string label;
	private string label2;
	
	void OnGUI(){
		
		GUIStyle titleStyle = new  GUIStyle();
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.fontSize = 24;
		titleStyle.normal.textColor = Color.white;
		
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontStyle = FontStyle.Bold;
		
		if (GUI.Button( new Rect( 5,2,50,25), "Prev",buttonStyle)){
			currentEffectIndex--;
			if (currentEffectIndex<0){
				currentEffectIndex = 3;
			}
		}
			
		if (GUI.Button( new Rect( Screen.width-55,2,50,25), "Next",buttonStyle)){
			currentEffectIndex++;
			if (currentEffectIndex>3){
				currentEffectIndex = 0;
			}
		}
		
		
		switch(currentEffectIndex){
			case 0:
				currentEffects = ssNumbers;
				label="Single spriteSheet : NUMBER";
				label2="";
				break;
			case 1:
				currentEffects = ssPunchs;
				label="Single spriteSheet : BACKGROUND";
				label2="";
				break;
			case 2:
				currentEffects = ssWords;
				label="Single spriteSheet : WORD";
				label2="";
				break;
			case 3:
				currentEffects = sequences;
				label="Sequences examples";
				label2="Create your sequences with our C# script";
				break;
			
		}
		
		GUI.color = Color.white;
		GUI.Label(new Rect(Screen.width/2-150,2,420,35),label,titleStyle);
		GUI.Label(new Rect(Screen.width/2-250,Screen.height-50,500,35),label2,titleStyle);
		
		for (int i=0;i<currentEffects.Length/2;i++){
			
			GUI.color = new Color(1f,0.75f,0.5f);
			if (GUI.Button(new Rect( 10,35+i*30,110,20),currentEffects[i].name)){
				Instantiate( currentEffects[i],new Vector3(0,0.5f,0),Quaternion.identity);
			}
		}
			
		int j=0;
		for (int i=currentEffects.Length/2;i<currentEffects.Length;i++){
			GUI.color = new Color(1f,0.75f,0.5f);
			if (GUI.Button(new Rect( Screen.width-120,35+j*30,110,20),currentEffects[i].name)){
				Instantiate( currentEffects[i],new Vector3(0,0.5f,0),Quaternion.identity);
			}
			j++;
		}
		

		
		/*
		GUI.color = new Color(1f,1f,1f);
		GUI.Label( new Rect(Screen.width/2-75,10,200,30),"Spritesheet : " + currentEffects[currentIndex].name);
		//toggle = GUI.Toggle( new Rect(Screen.width/2-50,Screen.height-30,200,30),toggle,"Toogle buttons");
		
		/*
		if (toggle){
			
		}
		else{
		*/
	/*

		
		//}*/
		
		
	}
	

}
