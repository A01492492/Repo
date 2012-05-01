using UnityEngine;
using System.Collections;
using BaxaTech.ReviewSystem;

public abstract class ButtonTracker : MonoBehaviour
{
	#region Protected abstract methods
	
	protected abstract void ChangeState();
	
	#endregion
	
	void Start()
	{
		GetComponent<ToggleMonitor>().OnToggleChanged += ChangeState;
		ChangeState();
	}
	
	private void OnMouseDown()
	{
		if(mouseOver)
		{
			GetComponent<ToggleMonitor>().Toggle();
		}
	}	
	
	private void OnMouseEnter()
	{
		mouseOver = true;
	}
	
	private void OnMouseExit()
	{
		mouseOver = false;
	}
	
	// flag to keep track if mouse is over object or not
	private bool mouseOver = false;
	
}
