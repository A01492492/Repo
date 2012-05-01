using UnityEngine;
using System.Collections;

public class CommingSoon : MonoBehaviour
{
	public Texture2D image;
	
	private void OnGUI()
	{
		GUI.Label(new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 50, 400, 300), image);
	}

	// Use this for initialization
	void Start () 
	{
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private GUIStyle style = new GUIStyle();
}
