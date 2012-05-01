using UnityEngine;
using System.Collections;
using BaxaTech.ReviewSystem;

public class GreenButtonTracker : ButtonTracker 
{
	#region Public member
	
	public MoleculeInstantiator moleculeInstantiator;
	public HotSpotBucket hotSpotBucket;
	public ToggleMonitor redButton;
	public Light greenButtonIndicator;
	
	#endregion
	
	#region protected methods
	
	protected override void ChangeState()
	{
		mState = GetComponent<ToggleMonitor>().ToggleState;
		greenButtonIndicator.enabled = mState;
		Debug.Log("Setting green button state to " + mState);
		
		moleculeInstantiator.Invoke("CleanSpace", 0f);
		
		if(mState && hotSpotBucket != null)
		{
			redButton.ToggleState = false;
			hotSpotBucket.Invoke("ChangeWaterToIce", 0f);
		}
		else
		{
			hotSpotBucket.Invoke("ChangeIceToWater", 0f);
		}
	}
	
	#endregion
	
	#region Private members
	
	private bool mState = false;
	
	#endregion
}
