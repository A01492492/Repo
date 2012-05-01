using UnityEngine;
using System.Collections;
using Shell;

/**
* @class PlayerDemoRunner
*
* @brief Main class for the PlayerDemo test. Manages replaying/recording of a 
*		1st-Person Controller's actions
*/
public class PlayerDemoRunner : MonoBehaviour 
{
	///Name by which the group used in the test is identified
	public const string GroupName = "Test";

	///Maximum length a recording may be, in seconds
	public const float MaxRecordLength = 30;

	/**
	* Called when the user clicks the "Record" button
	*/
	public void OnRecordClicked()
	{
		Debug.Log("PlayerDemoRunner.OnRecordClicked()");

		// toggle the recording
		if(m_reviewState == ReviewState.Recording)
			SetReviewState(ReviewState.None);
		else
			SetReviewState(ReviewState.Recording);
	}

	/**
	* Called when the user clicks the "Play" button
	*/
	public void OnPlayClicked()
	{
		// turn on playback
		SetReviewState(ReviewState.Replaying);
	}

	/**
	* Called when the user clicks the "Play" button
	*/
	public void OnPauseClicked()
	{
		// pause playback
		SetReviewState(ReviewState.ReplayPaused);
	}

	/**
	* @enum ReviewState
	*
	* @brief Enumerates the different states the user may be in, in reference to the review system
	*/ 
	private enum ReviewState
	{
		///User is in 'free roam' mode, not currently using the review system
		None,

		///User is recording new actions with the review system
		Recording,

		///User is replaying actions with the review system
		Replaying,

		///User has paused the replay
		ReplayPaused
	}

	/**
	* Called each time the physics scene is updated
	*/
	private void FixedUpdate()
	{
		// update the progress bars
		switch(m_reviewState)
		{
			case ReviewState.Recording:
				if(m_group.frameBuffer.currentTime >= MaxRecordLength)
				{
					SetReviewState(ReviewState.None);
					m_recordTimeline.Value = 1;
				}
				else
					m_recordTimeline.Value = m_group.frameBuffer.currentTime/MaxRecordLength;
			break;

			case ReviewState.Replaying:
				m_replayTimeline.Value = m_group.frameBuffer.currentTime/MaxRecordLength;
			break;
		}
	}

	/**
	* Called to render the UnityGUI elements used in the test
	*/
	private void OnGUI()
	{
		if(m_reviewState != ReviewState.None)
		{
			// save the current font color so it can be restored.
			Color cache = GUI.contentColor;
			GUI.contentColor = new Color(1, 0, 0);

			if(m_reviewState == ReviewState.ReplayPaused)
				GUILayout.Label("Playback Paused.");
			else
			{
				// calculate how many dots to display
				m_textTimer += Time.deltaTime;
				while(m_textTimer > 3)
					m_textTimer -= 3;
				
				switch((int)(m_textTimer))
				{
					case 0:
						GUILayout.Label(m_reviewState.ToString() + ".");
					break;

					case 1:
						GUILayout.Label(m_reviewState.ToString() + "..");
					break;

					case 2:
						GUILayout.Label(m_reviewState.ToString() + "...");
					break;
				}
			}

			// restore font color
			GUI.contentColor = cache;
		}
	}

	/**
	* Called when the replay has reached its end
	*/
	public void OnReplayFinished()
	{
		SetReviewState(ReviewState.None);
	}

	/**
	* Assign whether or not the player is enabled
	*/
	private void SetPlayerEnabled(bool enabled)
	{
		if(m_playerEnabled != enabled)
		{
			if(enabled)
			{
				m_player.GetComponent<CharacterMotor>().enabled = true;
				m_player.GetComponent<MouseLook>().enabled = true;
				m_player.GetComponent<FPSInputController>().enabled = true;

				m_camera.GetComponent<MouseLook>().enabled = true;

				Screen.lockCursor = true;
			}
			/*else
			{
				m_player.GetComponent<CharacterMotor>().enabled = false;
				m_player.GetComponent<MouseLook>().enabled = false;
				m_player.GetComponent<FPSInputController>().enabled = false;

				m_camera.GetComponent<MouseLook>().enabled = false;

				Screen.lockCursor = false;
			}*/

			// update the enabled flag
			m_playerEnabled = enabled;
		}
	}

	/**
	* Assign the review state to sync gui/input states to
	*/
	private void SetReviewState(ReviewState state)
	{
		switch(state)
		{
			case ReviewState.None:
				m_recordButton.controlIsEnabled = true;
				m_pauseButton.controlIsEnabled = false;
				if(m_group.frameBuffer != null && m_group.frameBuffer.frameCount > 0)
				{
					m_playButton.controlIsEnabled = true;
				}
				else
					m_playButton.controlIsEnabled = false;

				m_group.updateAction = ReviewUpdateAction.None;
			break;

			case ReviewState.Recording:
				m_recordButton.controlIsEnabled = true;
				m_playButton.controlIsEnabled = false;
				m_pauseButton.controlIsEnabled = false;

				if(m_reviewState == ReviewState.ReplayPaused)
				{
					// regenerate
					ReviewFrameBuffer regenBuffer = new ReviewFrameBuffer();
					m_group.frameBuffer.CopyFramesTo(ref regenBuffer);
					m_group.frameBuffer = regenBuffer;

					// make sure the replay timeline is cleared
					m_replayTimeline.Value = 0;
				}
				else
				{
					// create a new buffer
					m_group.frameBuffer = new ReviewFrameBuffer();
				}

				m_group.updateAction = ReviewUpdateAction.Record;
			break;

			case ReviewState.Replaying:
				m_recordButton.controlIsEnabled = false;
				m_playButton.controlIsEnabled = false;
				m_pauseButton.controlIsEnabled = true;

				// reset the read/write position in the frame buffer if we're initiating a replay
				if(m_reviewState == ReviewState.None)
					m_group.frameBuffer.SeekToFrame(0);

				m_group.updateAction = ReviewUpdateAction.Replay;
			break;

			case ReviewState.ReplayPaused:
				m_recordButton.controlIsEnabled = true;
				m_playButton.controlIsEnabled = true;
				m_pauseButton.controlIsEnabled = false;

				m_group.updateAction = ReviewUpdateAction.None;
			break;
		}

		m_reviewState = state;
	}

	/**
	* Called before the script's update methods are called for the first time,
	*	should be used to initialize the behavior.
	*/
	private void Start() 
	{
		// retrieve gui controls
		m_recordTimeline = GameObject.Find("RecordTimeline").GetComponent<UIProgressBar>();
		m_replayTimeline = GameObject.Find("ReplayTimeline").GetComponent<UIProgressBar>();

		m_recordButton = GameObject.Find("RecordButton").GetComponent<UIActionBtn>();
		m_playButton = GameObject.Find("PlayButton").GetComponent<UIActionBtn>();
		m_pauseButton = GameObject.Find("PauseButton").GetComponent<UIActionBtn>();

		// set up review group
		m_group = ReviewSystem.instance.GetGroup(GroupName);
		m_group.updateTrigger = ReviewUpdateTrigger.OnUpdate;
		m_group.playbackEvent += new PlaybackFinished(this.OnReplayFinished);

		// retrieve recorded objects
		m_player = GameObject.Find("Player");
		m_camera = GameObject.Find("Camera");

		// initiallly set the player to a disabled state
		SetPlayerEnabled(false);

		// set initial state
		SetReviewState(ReviewState.None);
	}
	
	/**
	* Update method triggered each time a frame is rendered in the scene.
	*/
	private void Update()
	{
		// enable/disable the player controls based on whether or not the user is 
		//	pressing the left mouse button down
		switch(m_reviewState)
		{
			case ReviewState.None:
			case ReviewState.Recording:
				if(Input.GetMouseButtonDown(1))
					SetPlayerEnabled(true);
				else if(Input.GetMouseButtonUp(1))
					SetPlayerEnabled(false);
			break;
		}
	}

	///Main camera object
	private GameObject m_camera = null;

	///Review group used to record/replay the test objects
	private Shell.ReviewGroup m_group = null;

	///pause button
	private UIActionBtn m_pauseButton = null;

	///Play button
	private UIActionBtn m_playButton = null;

	///Player object
	private GameObject m_player = null;

	///Whether or not the player object is currently enabled.
	private bool m_playerEnabled = true;

	///GUI record button
	private UIActionBtn m_recordButton = null;

	///GUI Progress bar that shows how much data has been recorded
	private UIProgressBar m_recordTimeline = null;

	///GUI Progress bar that shows how much data has been replayed
	private UIProgressBar m_replayTimeline = null;

	///Review state
	private ReviewState m_reviewState = ReviewState.None;

	///timer used to update the display text during recording/playback
	private float m_textTimer = 0;
}
