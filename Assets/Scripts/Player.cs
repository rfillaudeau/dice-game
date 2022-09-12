using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int score { get; private set; }
    public DiceGrid diceGrid;
    public Transform diceBox;

    [SerializeField] private TextMeshPro _scoreText;

    public void UpdateScore()
    {
        score = diceGrid.GetScore();

        _scoreText.SetText(score.ToString());
    }

    public void SetCanSelectColumn(bool canSelect)
    {
        foreach (DiceColumn column in diceGrid.columns)
        {
            column.SetIsSelectable(canSelect);
        }
    }
}
