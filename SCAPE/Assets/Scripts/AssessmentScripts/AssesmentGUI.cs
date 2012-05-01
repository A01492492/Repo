using UnityEngine;
using System.Collections;
using System.Text;
using BaxaTech.AssessmentSystem;

public class AssesmentGUI : MonoBehaviour
{
	
	public Texture2D scapeLogo;
	
	#region Internal members

    internal bool AssessmentMode 
    { 
        get { return mAssessmentMode; }
        set { mAssessmentMode = value; }
    }

    #endregion

    #region Private constant

    private const float PASS_MARKS = 8;

    #endregion

    #region Private methods
	
    private void Awake()
    {
        // Make this object persistence across all levels
        DontDestroyOnLoad(gameObject);
		
				
		// Check the presence assessment related objects (from start level)
        if (GameObject.Find("Assessment") == null || 
            GameObject.Find("Assessment").GetComponent<AssessmentManager>() == null)
        {
            Debug.LogError("No assessment object found");
            return;
        }
		
		
        mAssesmentManager = GameObject.Find("Assessment").GetComponent<AssessmentManager>();

        mFeedbackAssessor = gameObject.GetComponent<FeedbackAssessor>();

        mAssesment = new Assesment();
        LoadQuizForLevel();
        
        mWindowHeight = (3 * Screen.height) / 4;
        mWindowWidth = Screen.width / 2;

        mWindowSize = new Rect(0, (Screen.height / 4), mWindowWidth, mWindowHeight);
        mFeedbackWindowSize = new Rect(0, (Screen.height / 4), mWindowWidth, mWindowHeight / 4);
        
    }   
	
	private void Start()
    {
        if(scapeLogo == null)
		{
			Debug.LogError("No logo found. Please add a logo to AssessmentGUI script");
			return;
		}
		
		mBackgroundStyle = new GUIStyle();
		mBackgroundStyle.normal.background = scapeLogo;
	}
	
    private void LoadQuizForLevel()
    {
        if (mLevelIndex < 0)
        {
            Debug.LogError("Error. Unable to add quizes for invalid level");
            return;
        }

        // Reset to first quiz
        mQuizIndex = 0;

        mAssesment.LoadLevel(mLevelIndex);
        mCurrentQuiz = mAssesment.GetQuiz(mQuizIndex);
    }

    private void OnGUI()
    {
        if (AssessmentMode)
        {
            GUI.Label(new Rect((Screen.width) / 8, (Screen.height / 9), (Screen.width) / 4, 100), "", mBackgroundStyle);

            if (mShowAssesmentWindow)
            {
                mWindowSize = GUI.Window(101, mWindowSize, ShowAssesmentWindow, mAssesmentWindowName);
            }

            if (mShowAnswersWindow)
            {
                mWindowSize = GUI.Window(201, mWindowSize, ShowAnswersWindow, mAnswersWindowName);
            }

            if (mShowFeedbackWindow)
            {
                mWindowSize = GUI.Window(301, mWindowSize, ShowFeedbackWindow, mFeedbackWindowName);
            }
        }
    }

    private void ShowAssesmentWindow(int windowID)
    {
        
        GUI.Label(new Rect(10, 40, mWindowSize.width, 200), mCurrentQuiz.Question);		
			
        mCurrentQuiz.SelectedAnswerIndex = GUI.SelectionGrid(new Rect(10, 140, mWindowSize.width - 20, 300),
                                                             mCurrentQuiz.SelectedAnswerIndex, 
                                                             mCurrentQuiz.GetAnswerChoicesArray(),
                                                             1);
		
		
        if (mQuizIndex < 3 && mCurrentQuiz.SelectedAnswerIndex != -1 && GUI.Button(new Rect((mWindowSize.width - 100), 450, 80, 30), "Feedback"))
            ShowFeedback();

        if (mQuizIndex == 3 && mCurrentQuiz.SelectedAnswerIndex != -1 && GUI.Button(new Rect((mWindowSize.width - 100), 450, 80, 30), "Finish"))
            FinishAssesment();

    }

    private void ShowFeedbackWindow(int windowID)
    {
        GUI.Label(new Rect(10, 140, mWindowSize.width, 200), mCurrentQuiz.Feedback);

        if (GUI.Button(new Rect((mWindowSize.width - 100), 450, 80, 30), "Next"))
            GotoNextQuiz();

    }

    private void ShowAnswersWindow(int windowID)
    {
		string result = GetResult();
        
        GUI.Box(new Rect(mWindowSize.width / 4, 20, mWindowSize.width / 2, 30), result);        

        mScrollPosition = GUI.BeginScrollView(new Rect(20, 50, mWindowSize.width - 20, (mWindowSize.height / 2) - 20),
                                                 mScrollPosition,
                                                 new Rect(0, 0, mWindowSize.width, mWindowSize.height));
		
        GUI.TextArea(new Rect(0, 0, mWindowSize.width, (mWindowHeight / 2)), mAssesmentManager.GetResult);

        GUI.EndScrollView();

        if (GUI.Button(new Rect((mWindowSize.width - 100), 450, 80, 30), "Continue"))
        {
            mShowAnswersWindow = false;
            FateDecider();            
        }
 
    }

    private string GetResult()
    {
        StringBuilder result = new StringBuilder();

        result.Append("Your score is ");
        result.Append(mAssesmentManager.TotalScore);
        result.Append(" out of 10.");

        if (mAssesmentManager.TotalScore >= 8)
            result.Append(" Go To Next Level");
        else
            result.Append(" Go Back");

        return result.ToString();
    }

    private void ShowFeedback()
    {
        mShowAssesmentWindow = false;
        mShowAnswersWindow = false;
        mShowFeedbackWindow = true;        
    }

    private void FinishAssesment()
    {
        PerformAssesment(mCurrentQuiz);		
		mAssesmentManager.CalculateResult();
		
        mShowAssesmentWindow = false;
        mShowFeedbackWindow = false;
        mShowAnswersWindow = true;
    }

    private void GotoPreviousQuiz()
    {
        --mQuizIndex;
        mCurrentQuiz = mAssesment.GetQuiz(mQuizIndex);
    }

    private void GotoNextQuiz()
    {
        PerformAssesment(mCurrentQuiz);

        mShowFeedbackWindow = false;
        mShowAnswersWindow = false;
        mShowAssesmentWindow = true;

        ++mQuizIndex;
        mCurrentQuiz = mAssesment.GetQuiz(mQuizIndex);           
        
    }

    private void PerformAssesment(Quiz quiz)
    {
        Debug.Log("Saving " + quiz.Question);
        // Save data to accessor here
        mFeedbackAssessor.Quiz = mCurrentQuiz;
        mFeedbackAssessor.PerformAssessment(Time.time);
    }

    private void FateDecider()
    {
		GameObject runner = GameObject.Find("Runner");
		runner.GetComponent<ReviewSystemVtr>().showVcr = false;
        
        mAssesmentManager.ScapeMode();
		
        if (mAssesmentManager.TotalScore >= PASS_MARKS)
        {
            mLevelIndex++;
            Application.LoadLevel(mLevelIndex);
        }
		else
		{
			ResetAssessment();			
			//mAssesmentManager.ScapeMode();
			GameObject.Find("HotSpotForWaterContainer").GetComponent<HotSpotBucket>().Reset();
            runner.GetComponent<ReviewSystemVtr>().recording = false;
		}      
			
    }
	
	private void ResetAssessment()
	{
		LoadQuizForLevel();
		
		mShowFeedbackWindow = false;
        mShowAnswersWindow = false;
        mShowAssesmentWindow = true;
	}
	
    #endregion
    

    #region Private members
        
    private string mAssesmentWindowName = "Assesment";
    private string mAnswersWindowName = "Answers";
    private string mFeedbackWindowName = "Feedback";

    private bool mShowAssesmentWindow = true;
    private bool mShowAnswersWindow = false;
    private bool mShowFeedbackWindow = false;
    private bool mAssessmentMode = false;
	
    
    private Assesment mAssesment;
    private AssessmentManager mAssesmentManager;
    private FeedbackAssessor mFeedbackAssessor;
    private Quiz mCurrentQuiz;

    private Rect mWindowSize;
    private Rect mFeedbackWindowSize;
    
	private GUIStyle mBackgroundStyle;

    private float mWindowHeight;
    private float mWindowWidth;
    
    private int mQuizIndex = 0;
    private int mLevelIndex = 1;

    private Vector2 mScrollPosition = Vector2.zero;

    #endregion
}