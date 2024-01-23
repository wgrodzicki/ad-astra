using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the display of the final score at the end of the last level
/// </summary>
public class FinalScoreDisplay : ScoreDisplay
{
    void Awake()
    {
        DisplayScore();
    }

    public override void DisplayScore()
    {
        displayText.text = "Your final score: " + GameManager.score.ToString();
    }
}
