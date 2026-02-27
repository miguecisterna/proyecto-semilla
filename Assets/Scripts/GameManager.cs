using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;

    public ScoreEntry(string name, int scoreValue)
    {
        playerName = name;
        score = scoreValue;
    }
}

[System.Serializable]
public class ScoreList
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scoring")]
    [SerializeField] private int pointsPerSecond = 5;
    [SerializeField] private int pointsPerKill = 10;

    private float survivalTime;
    private int killCount;
    private bool isGameOver;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private const string HighScoreKey = "HighScore";

    public int FinalScore { get; private set; }

    private const string RankingKey = "Top10Ranking";

    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI rankingText;

    private ScoreList scoreList = new ScoreList();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadRanking();
    }

    private void Update()
    {
        if (isGameOver) return;

        survivalTime += Time.deltaTime;
    }

    public void RegisterKill()
    {
        killCount++;
    }

    public void GameOver()
    {
        isGameOver = true;

        FinalScore = Mathf.RoundToInt(survivalTime * pointsPerSecond)
                     + (killCount * pointsPerKill);

        gameOverPanel.SetActive(true);

        timeText.text = "Time: " + survivalTime.ToString("F1");
        killText.text = "Kills: " + killCount;
        scoreText.text = "Total Score: " + FinalScore;

        Time.timeScale = 0f;

        int savedHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (IsHighScore(FinalScore))
        {
            nameInputPanel.SetActive(true);
        }
        else
        {
            ShowRanking();
        }

        Debug.Log("HighScore guardado: " + GetHighScore());
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void SaveRanking()
    {
        string json = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString(RankingKey, json);
        PlayerPrefs.Save();
    }

    private void LoadRanking()
    {
        if (PlayerPrefs.HasKey(RankingKey))
        {
            string json = PlayerPrefs.GetString(RankingKey);
            scoreList = JsonUtility.FromJson<ScoreList>(json);
        }
        else
        {
            scoreList = new ScoreList();
        }
    }

    private bool IsHighScore(int newScore)
    {
        if (scoreList.scores.Count < 10)
            return true;

        return newScore > scoreList.scores[scoreList.scores.Count - 1].score;
    }

    public void SubmitName()
    {
        string playerName = nameInputField.text.ToUpper();

        if (playerName.Length > 3)
            playerName = playerName.Substring(0, 3);

        if (string.IsNullOrEmpty(playerName))
            playerName = "AAA";

        scoreList.scores.Add(new ScoreEntry(playerName, FinalScore));

        scoreList.scores.Sort((a, b) => b.score.CompareTo(a.score));

        if (scoreList.scores.Count > 10)
            scoreList.scores.RemoveAt(10);

        SaveRanking();

        nameInputPanel.SetActive(false);

        ShowRanking();
    }

    private void ShowRanking()
    {
        rankingText.text = "TOP 10\n\n";

        for (int i = 0; i < scoreList.scores.Count; i++)
        {
            rankingText.text += (i + 1) + ". "
                + scoreList.scores[i].playerName
                + " - "
                + scoreList.scores[i].score
                + "\n";
        }
    }
}