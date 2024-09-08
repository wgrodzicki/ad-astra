public class FinalScoreDisplay : ScoreDisplay
{
    private void Awake()
    {
        DisplayScore();
    }

    public override void DisplayScore()
    {
        displayText.text = "Your final score: " + GameManager.score.ToString();
    }
}
