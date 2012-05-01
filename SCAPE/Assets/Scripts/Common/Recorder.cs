using UnityEngine;
using System.Collections;

public class Recorder : MonoBehaviour 
{
	
	#region internal Methods
	
	internal void StartRecording()
	{
		Debug.Log("STARTING RECORDING");
		runner.Record();
	}
	
	internal void StopRecording()
	{
		Debug.Log("STOPPING RECORDING");
		runner.Stop();
	}
	
	#endregion
	
	#region Private methods

	// Use this for initialization
	private void Start() 
	{
		if(GameObject.Find("Runner") == null)
		{
			Debug.LogError("No Runner found");
			return;
		}
		
		runner = GameObject.Find("Runner").GetComponent<Runner>();
		
		//StartRecording();
	}
	
	
	#endregion
	
	private Runner runner;
}
