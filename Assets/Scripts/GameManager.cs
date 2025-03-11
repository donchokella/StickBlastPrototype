using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int level = 1;

    public Text scoreText;
    public Text levelText;
    public Text comboText;

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
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        UpdateUI();
    }

    public void NextLevel()
    {
        level++;
        GridManager.Instance.ResetGrid();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Puan: " + score;
        if (levelText != null) levelText.text = "Seviye: " + level;
        if (comboText != null)
        {
            if (currentCombo > 1)
                comboText.text = "Combo x" + currentCombo;
            else
                comboText.text = "";
        }
    }
}
