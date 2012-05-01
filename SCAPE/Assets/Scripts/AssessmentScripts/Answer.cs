using UnityEngine;
using System.Collections;

public class Answer
{
    public Answer(string answerDescription, string feedback, float score) 
    {
        this.mAnswerDescription = answerDescription;
        this.mFeedback = feedback;
        this.mScore = score;
    }


    #region Internal properties

    internal string AnswerDescription 
    { 
        get { return mAnswerDescription; } 
    }

    internal string Feedback 
    { 
        get { return mFeedback; } 
    }

    internal float Score 
    { 
        get { return mScore; } 
    }

    #endregion

    #region Private members

    private string mAnswerDescription;
    private string mFeedback;
    private float mScore;

    #endregion
}
