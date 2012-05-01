using UnityEngine;
using System.Collections;

public class EventDelegater : MonoBehaviour
{
	
	// This variable will hold the function to be called
	// In this way we can reuse the EventDelegator script
	public string methodToInvoke;
	public MonoBehaviour scriptContainer;
	
	private void Start()
	{
		
		if(scriptContainer == null)
		{
			Debug.LogError("No script container found. Please add script which contains the method");
			return;
		}
		
		if(methodToInvoke == null)
		{
			Debug.LogError("No method found. Please add the method to invoke");
			return;
		}
		
	}
	
	private void OnMouseEnter()
	{
		mMouseEnter = true;
	}
	
	private void OnMouseDown()
	{
		if(mMouseEnter && scriptContainer != null)
		{
			Debug.Log("Clicked");
			scriptContainer.Invoke(methodToInvoke, 0f);
		}
	}
	
	private void OnMouseExit()
	{
		mMouseEnter = false;
	}
	
	private bool mMouseEnter = false;
	 
}
