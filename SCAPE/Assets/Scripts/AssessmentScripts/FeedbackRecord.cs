using UnityEngine;
using System.Collections;
using BaxaTech.AssessmentSystem;

public class FeedbackRecord : AssessmentRecord
{
    public FeedbackRecord(IAssessor accessor, float timestamp)
        : base(accessor, timestamp) 
    {
    }

    public Quiz Quiz
    {
        get { return m_quiz; }
        set { m_quiz = value; }
    }

    private Quiz m_quiz;
	
}
