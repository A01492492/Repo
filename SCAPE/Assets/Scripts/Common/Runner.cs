using UnityEngine;
using System.Collections.Generic;
using BaxaTech.ReviewSystem;

/// <summary>
///		Provides Vtr-like controls functionality to govern a set of ReviewGroup instances
///		in the ReviewSystem
/// </summary>
public class Runner : MonoBehaviour 
{
		
	/// <summary>
	///		Set of possible states the vcr may be in
	/// </summary>
	public enum Mode
	{
		Stopped,
		Rewinding,
		FastForwarding,
		Recording,
		Playing,
		Paused,
	}

	/// <summary>
	///		Set of names of ReviewGroups whose state is to be managed by this instance
	/// </summary>
	public List<string> GroupNames = new List<string>();

	/// <summary> State of the Vcr </summary>
	public Mode State = Mode.Stopped;

	/// <summary>
	///		Current playback/recording time
	/// </summary>
	
	public float CurrentTime = 0;

	/// <summary>
	/// Total length of recorded data
	/// </summary>
	
	public float Length = 0;
	
	internal void JumpTo(float time)
	{
		foreach (ReviewGroup group in m_groups)
		{
			group.Replay();
			group.MoveReplayTo(time);
			group.Pause();
		}
		
		CurrentTime = time;
		State = Mode.Paused;
	}	
	
	internal void Start()
	{
		foreach (ReviewGroup group in m_groups)
		{
			group.Replay();
			group.MoveReplayToStart();
		}
		CurrentTime = 0;
		State = Mode.Stopped;
	}
	
	internal void Play()
	{
		foreach (ReviewGroup group in m_groups)
			group.Replay();
		State = Mode.Playing;
	}
	
	internal void Pause()
	{
		foreach (ReviewGroup group in m_groups)
			group.Pause();
		State = Mode.Paused;
	}
	
	internal void FFwd()
	{
		foreach (ReviewGroup group in m_groups)
		{
			group.Replay();
			group.ReplayRate = 3.0f;
		}
		State = Mode.FastForwarding;
	}
	
	internal void Stop()
	{
		if (State != Mode.Recording)
			Start();
		else
		{
			foreach (ReviewGroup group in m_groups)
				group.UpdateAction = UpdateAction.None;
		}

		State = Mode.Stopped;
	}
	
	internal void Record()
	{
		foreach (ReviewGroup group in m_groups)
			group.Record();
		
		State = Mode.Recording;
	}
	
	/// <summary>
	///		Remove all data recorded for all groups being managed by the instance 
	/// </summary>
	internal void Clear()
	{		
		foreach (ReviewGroup group in m_groups)
		{
			group.Buffer.Clear();
            group.Buffer = null;
            group.Buffer = new FrameBuffer();
		}
		CurrentTime = Length = 0;
	}

    internal void RefreshBuffer() 
    {
        Stop();
        Clear();
    }

	internal void RefreshGroups()
	{
		m_groups = new List<ReviewGroup>();
		foreach (string name in GroupNames)
		{
			ReviewGroup cur = ReviewSystem.Instance.GetGroup(name);
			if (cur != null)
				m_groups.Add(cur);
		}
	}

	
	private void End()
	{
		foreach (ReviewGroup group in m_groups)
		{
			group.Replay();
			group.MoveReplayToEnd();
		}
		CurrentTime = Length;
		State = Mode.Stopped;
	}
	
	private void Rewind()
	{
		foreach (ReviewGroup group in m_groups)
		{
			group.Replay();
			group.ReplayRate = -3.0f;
		}
		State = Mode.Rewinding;
	}

	
	/// <summary>
	///		Update method for the callback
	/// </summary>	
	private void Update()
	{
		switch(State)
		{
			case Mode.Rewinding:
				CurrentTime -= 3*Time.deltaTime;
				if (CurrentTime <= 0)
					Start();
			break;

			case Mode.Playing:
				CurrentTime += Time.deltaTime;
				if (CurrentTime >= Length)
					End();
			break;

			case Mode.FastForwarding:
				CurrentTime += 3*Time.deltaTime;
				if (CurrentTime >= Length)
					End();
			break;

			case Mode.Recording:
				Length += Time.deltaTime;
				CurrentTime += Time.deltaTime;
			break;
		}
	}
	
	private List<ReviewGroup> m_groups = new List<ReviewGroup>();
	
}
