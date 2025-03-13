using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int level = 1;

    public int levelScoreLimit = 250;

    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text comboText;

    private int currentCombo = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int points)
    {
        score += points;
        currentCombo++;
        UpdateUI();

        if (score >= levelScoreLimit)
        {
            NextLevel();
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        UpdateUI();
    }

   
    public void NextLevel()
    {
        level++;
        levelScoreLimit = Mathf.RoundToInt(levelScoreLimit * 1.2f);

        score = 0;
        ResetCombo();

        GridManager.Instance.ResetGrid();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Puan: " + score;
        if (levelText != null) levelText.text = "Seviye: " + level;
        if (comboText != null)
            comboText.text = currentCombo > 1 ? "Combo x" + currentCombo : "";
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
