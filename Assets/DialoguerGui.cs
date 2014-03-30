using UnityEngine;
using System.Collections;

public class DialoguerGui : MonoBehaviour {

	private bool _showing;

	private string _text;
	private string[] _choices;

	// Use this for initialization
	void Start (){
		Dialoguer.events.onStarted += onStarted;
		Dialoguer.events.onEnded += onEnded;
		Dialoguer.events.onTextPhase += onTextPhase;
	}

	void OnGUI (){
		if (!_showing)
			return;

		GUI.Box (new Rect(10,10,200,150), _text);

		if (_choices == null && _choices.Length <= 0) {
			if (GUI.Button (new Rect (10, 220, 200, 30), "continue")) {
					Dialoguer.ContinueDialogue ();
			}
		} else {
			for(int i=0; i<_choices.Length; i++){
				if(GUI.Button(new Rect(10,220 + (40*i),200, 30), _choices[i])){
					Dialoguer.ContinueDialogue(i);
				}
			}
		}
	}

	private void onStarted(){
		_showing = true;
	}

	private void onEnded(){
		_showing = false;
	}

	private void onTextPhase(DialoguerTextData data){
		_text = data.text;
		_choices = data.choices;
	}
}
