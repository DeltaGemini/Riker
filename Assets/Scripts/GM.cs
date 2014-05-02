using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GM : MonoBehaviour {	
	
	DialoguerEvents Dialoguer.events;

	public Camera gameCam;
	public GameObject bckImage;

	int sceneNumber;
	
	public AudioSource audioText;
	public AudioSource audioTextEnd;
	public AudioSource audioGood;
	public AudioSource audioBad;
	AudioClip newVoiceOver;
	public AudioSource bgmusic;

	float vWidth;
	float vHeight;

	float bHeight;

	public GUISkin font;

	private bool _showing = false;
	private bool _windowShowing = false;
	private bool _selectionClicked = false;

	//dialoguer text information
	private string _windowText = string.Empty;
	private string[] _choices;
	private Color _guiColor;

	// Occurs when Dialoguer sends a message with the SendMessage node.
	event MessageEventHandler onMessageEvent;
	delegate void MessageEventHandler(string message, string metadata);

	event TextPhaseHandler onTextPhase;
	delegate void TextPhaseHandler(DialoguerTextData data);

	void Awake(){
		// You must initialize Dialoguer before using it!
		Dialoguer.Initialize();

		vWidth = Screen.width;
		vHeight = Screen.height;
		
		bHeight = vHeight * 0.12f;
	}
	
	void Start () {
		addDialoguerEvents();
		
		Dialoguer.StartDialogue(0);
	}


	void OnGUI(){
		if(!_showing) return;
		if(!_windowShowing) return;
		
		GUI.color = _guiColor;
		GUI.depth = 10;
		
		GUI.skin = font;
		
		Rect dialogueBoxRect = new Rect(10, 10, vWidth*0.4f, vHeight-20f);
		Rect dialogueBackBoxRect = new Rect(dialogueBoxRect.x, dialogueBoxRect.y, dialogueBoxRect.width, dialogueBoxRect.height - (bHeight*_choices.Length));
		GUI.Box(dialogueBackBoxRect, string.Empty);
		GUI.color = GUI.contentColor;
		GUI.Label(new Rect(dialogueBackBoxRect.x, dialogueBackBoxRect.y + 10, dialogueBackBoxRect.width, dialogueBackBoxRect.height - 20), _windowText);

		if(_selectionClicked) return;
		
		for(int i = 0; i<_choices.Length; i+=1){
			Rect buttonRect = new Rect(dialogueBoxRect.x, dialogueBoxRect.yMax - (bHeight*(_choices.Length - i)) + 5 , dialogueBoxRect.width, bHeight-5);

			if(GUI.Button(buttonRect, _choices[i])){
				_selectionClicked = true;
				Dialoguer.ContinueDialogue(i);
			}
		}

		GUI.color = GUI.contentColor;
	}

	public void addDialoguerEvents(){
		Dialoguer.events.onStarted += onStartedHandler;
		Dialoguer.events.onEnded += onEndedHandler;
		Dialoguer.events.onInstantlyEnded += onInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += onTextPhaseHandler;
		Dialoguer.events.onWindowClose += onWindowCloseHandler;
		Dialoguer.events.onMessageEvent += SceneChange;
	}

	void Update (){
		/*float ratioWidth = vHeight * bckImage.guiTexture.texture.height / vHeight;
		bckImage.guiTexture.pixelInset = new Rect ((Screen.width - ratioWidth)/2, 0, ratioWidth, Screen.height);
		//bckImage.guiTexture.pixelInset = new Rect ((Screen.width - ratioWidth)/2, 0, ratioWidth, vHeight);
		*/
	}

	void SceneChange(string message, string metadata) {
		//Debug.Log (message + ", " + metadata);
		//bckImage.texture = Resources.Load(message) as Texture;
		bckImage.transform.GetComponent<UITexture>().mainTexture=Resources.Load(message) as Texture;
		sceneNumber = System.Convert.ToInt32 (message);
		Dialoguer.StartDialogue (sceneNumber);
	}

	private void onStartedHandler(){
		_showing = true;
	}
	
	private void onEndedHandler(){
		_showing = false;
	}
	
	private void onInstantlyEndedHandler(){
		_showing = true;
		_windowShowing = false;
		_selectionClicked = false;
	}
	
	private void onTextPhaseHandler(DialoguerTextData data){
		_guiColor = GUI.contentColor;
		_windowText = data.text;
		
		if(data.windowType == DialoguerTextPhaseType.Text){
			_choices = new string[1] {"Weiter"};
			// wenn es eine der letzten 4 szenen ist, schreibe: "Ende" statt "Weiter"
			Debug.Log (sceneNumber);
			if (sceneNumber > 6) {
				_choices = new string[1] {"Nochmal spielen?"};
				//bgmusic = gameCam.
				//bgmusic.CrossFade();
			}
			else if (sceneNumber == 0)
				_choices = new string[1] {"Los geht's!"};
		}else{
			_choices = data.choices;
		}
		
		// Set the color
		switch(data.theme){
		case "bad":
			_guiColor = Color.red;
			break;
		case "good":
			_guiColor = Color.green;
			break;
		default:
			_guiColor = GUI.contentColor;
			break;
		}

		audioText.Stop();

		newVoiceOver = Resources.Load (data.audio) as AudioClip;
		//voiceOvers[sceneNumber]
		audioText.PlayOneShot (newVoiceOver);
		
		_windowShowing = true;
		_selectionClicked = false;
	}
	
	private void onWindowCloseHandler(){
		_windowShowing = false;
		_selectionClicked = false;
	}
}
