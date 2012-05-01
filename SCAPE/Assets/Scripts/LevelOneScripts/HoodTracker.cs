using UnityEngine;
using System.Collections;
using BaxaTech.ReviewSystem;

public class HoodTracker : ButtonTracker 
{
	#region Public members
	
	public HotSpotBucket hotSpotBucket;
	public ToggleMonitor redButton;
	public MoleculeInstantiator moleculeInstantiator;
	
	#endregion
	
	protected override void ChangeState()
	{
		mState = GetComponent<ToggleMonitor>().ToggleState;
		
		if(redButton != null && redButton.ToggleState)
		{
			moleculeInstantiator.CleanSpace();
			
			if(mState)
			{
				moleculeInstantiator.ShowMolecule(true);
			}	
		}		
	}
	
	#region Private members
	
	private bool mState = false;
	
	#endregion
}