using UnityEngine;
using BaxaTech.ReviewSystem;

/// <summary>
///		Workaround class for the PlatePressure demo, sets up the necessary ReviewGroups
/// </summary>
public class GroupSetup : MonoBehaviour
{
	/// <summary>
	///	Callback triggered to update the component. Used as a delayed Start() method,
	///	waits until the ReviewSystem has been initialized then creates the groups
	/// </summary>
	void Update()
	{
		if (enabled && ReviewSystem.Created)
		{
			ReviewSystem.Instance.CreateGroup("Test");
			enabled = false; // only create the groups once
		}
	}
}