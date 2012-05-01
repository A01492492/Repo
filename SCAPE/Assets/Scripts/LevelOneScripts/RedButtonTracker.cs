using UnityEngine;
using System.Collections;
using BaxaTech.ReviewSystem;

public class RedButtonTracker : ButtonTracker 
{
	#region Public member
	
	public MoleculeInstantiator moleculeInstantiator;
	public ToggleMonitor greenButton;
	public Light redButtonIndicator;
	#endregion
	
	#region protected methods
	
	protected override void ChangeState()
	{
		mState = GetComponent<ToggleMonitor>().ToggleState;
		redButtonIndicator.enabled = mState;
		
		moleculeInstantiator.Invoke("CleanSpace", 0f);
		
		if(mState && greenButton != null)
		{
			greenButton.ToggleState = false;
		}
		else
		{
			
		}
	}
	
	#endregion
	
	#region Private members
	
	private bool mState = false;
	
	#endregion
}