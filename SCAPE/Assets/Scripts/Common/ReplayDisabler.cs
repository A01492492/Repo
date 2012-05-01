using UnityEngine;
using System.Collections.Generic;
using BaxaTech.ReviewSystem;


/// <summary>
///		Class that enables/disables a collection of sibling components based on the update action
///		of a ReviewSystem.ReviewGroup
/// </summary>
public class ReplayDisabler : MonoBehaviour
{
	/// <summary>Whether or not the state of registered behaviors should be enabled when 
	/// the group's update action is UpdateAction.None</summary>
	public bool EnableOnNone;

	/// <summary> Name of the review group whose state changes should trigger enable/disable </summary>
	public string GroupName;

	/// <summary> Collection of Component instances to enable/disable. Actual type 
	/// must be derived from either Behaviour or a Collider</summary>
	public List<Component> Registry;

	/// <summary> Called before the component is destroyed </summary>
	private void OnDestroy()
	{
		if (m_registered && ReviewSystem.Created)
		{
			ReviewGroup grp = ReviewSystem.Instance.GetGroup(GroupName);
			if (grp != null)
				grp.UpdateActionChanged -= OnGroupUpdateActionChanged;
		}
	}

	/// <summary>
	/// Event handler triggered whenever the update action of the review group changes
	/// </summary>
	/// <param name="group">
	///		Group whose update action changed.
	/// </param>
	private void OnGroupUpdateActionChanged(ReviewGroup group)
	{
		bool enabled = !(group.UpdateAction == UpdateAction.Replay || 
			(group.UpdateAction == UpdateAction.None && !EnableOnNone));
		foreach (Component c in Registry)
		{
			if (c.GetType().IsSubclassOf(typeof(Collider)))
				;//((Collider)c).enabled = enabled;
			else if (c.GetType().IsSubclassOf(typeof(Behaviour)))
				((Behaviour)c).enabled = enabled;
		}
	}

	/// <summary> Called to update the component </summary>
	private void Update()
	{
		if (m_check && ReviewSystem.Created)
		{
			ReviewGroup grp = ReviewSystem.Instance.GetGroup(GroupName);
			if (grp != null && Registry != null && Registry.Count > 0)
			{
				grp.UpdateActionChanged += OnGroupUpdateActionChanged;
				OnGroupUpdateActionChanged(grp);
				m_registered = true;
			}
			m_check = false;
		}
	}

	/// <summary>
	/// Since this component's initialization depends on whether or not the 
	/// ReviewSystem has been initialized, this flag determines whether or
	/// not to check the registration status of the instance.
	/// </summary>
	private bool m_check = true;

	/// <summary> Called to update the component </summary>
	private bool m_registered = false;
}