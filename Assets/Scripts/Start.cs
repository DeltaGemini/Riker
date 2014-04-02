using UnityEngine;
using System.Collections;

public class Start : MonoBehaviour {

	void Awake(){
		// You must initialize Dialoguer before using it!
		Dialoguer.Initialize();
	}

	void OnGUI(){
		if(GUI.Button (new Rect(10,10,100,30), "Start!")){

			// The preferred way to start dialogues is with the DialoguerDialogues enum
			Dialoguer.StartDialogue(DialoguerDialogues.Narrator);
			// By default, this enum is automatically updated when you save your dialogues.
			// You can turn this off in the Dialoguer preferences menu.

			// Another way to start a Dialogue is with the index id of the dialogue you wish to start, like so:
			//Dialoguer.StartDialogue(2);
			// But be careful, because if any dialogues are moved, this number must be changed too

		}
	}
}
