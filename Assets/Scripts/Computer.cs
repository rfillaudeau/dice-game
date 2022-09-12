using System.Collections;
using UnityEngine;
using TMPro;

public class Computer : MonoBehaviour
{
    public int score { get; private set; }
    public DiceGrid diceGrid;
    public Transform diceBox;

    [SerializeField] private TextMeshPro _scoreText;

    [SerializeField] private ComputerDifficulty _difficulty = ComputerDifficulty.Easy;

    [SerializeField] private Player _player;

    [SerializeField] private float _timeBeforeSelection = 1f;

    public void UpdateScore()
    {
        score = diceGrid.GetScore();

        _scoreText.SetText(score.ToString());
    }

    public void SelectColumn(int diceNumber)
    {
        if (_difficulty == ComputerDifficulty.Easy)
        {
            Easy(diceNumber);
            return;
        }

        StartCoroutine(Normal(diceNumber));
    }

    private void Easy(int diceNumber)
    {
        diceGrid.columns[GetRandomAvailableIndex()].Select();
    }

    private IEnumerator Normal(int diceNumber)
    {
        yield return new WaitForSeconds(_timeBeforeSelection);

        int? sameNumberInPlayerIndex = GetIndexForSameNumberInPlayerColumns(diceNumber);
        if (sameNumberInPlayerIndex != null)
        {
            diceGrid.columns[sameNumberInPlayerIndex.Value].Select();

            yield break;
        }

        yield return null;

        int? sameNumberInIndex = GetIndexForSameNumber(diceNumber);
        if (sameNumberInIndex != null)
        {
            diceGrid.columns[sameNumberInIndex.Value].Select();

            yield break;
        }

        yield return null;

        int? emptyColumnIndex = GetIndexForEmptyColumn();
        if (emptyColumnIndex != null)
        {
            diceGrid.columns[emptyColumnIndex.Value].Select();

            yield break;
        }

        yield return null;

        diceGrid.columns[GetRandomAvailableIndex()].Select();
    }

    private int? GetIndexForSameNumber(int diceNumber)
    {
        foreach (DiceColumn column in diceGrid.columns)
        {
            if (column.IsFull())
            {
                continue;
            }

            foreach (MovingDice dice in column.dices)
            {
                if (dice != null && dice.dice.number == diceNumber)
                {
                    return column.index;
                }
            }
        }

        return null;
    }

    private int? GetIndexForSameNumberInPlayerColumns(int diceNumber)
    {
        foreach (DiceColumn column in _player.diceGrid.columns)
        {
            if (diceGrid.columns[column.index].IsFull())
            {
                continue;
            }

            foreach (MovingDice dice in column.dices)
            {
                if (dice != null && dice.dice.number == diceNumber)
                {
                    return column.index;
                }
            }
        }

        return null;
    }

    private int? GetIndexForEmptyColumn()
    {
        foreach (DiceColumn column in diceGrid.columns)
        {
            if (column.IsEmpty())
            {
                return column.index;
            }
        }

        return null;
    }

    private int GetRandomAvailableIndex()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, diceGrid.columns.Length);

            if (diceGrid.columns[randomIndex].IsFull())
            {
                continue;
            }

            return randomIndex;
        }
    }
}
