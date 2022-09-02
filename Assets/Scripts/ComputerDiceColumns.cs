using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerDiceColumns : DiceColumns
{
    [SerializeField] private ComputerDifficulty _difficulty = ComputerDifficulty.Easy;

    [SerializeField] private PlayerDiceColumns _playerColumns;

    [SerializeField] private float _timeBetweenSteps = 1f;

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
        _columns[GetRandomAvailableIndex()].Select();
    }

    private IEnumerator Normal(int diceNumber)
    {
        yield return new WaitForSeconds(_timeBetweenSteps);

        int? sameNumberInPlayerIndex = GetIndexForSameNumberInPlayerColumns(diceNumber);
        if (sameNumberInPlayerIndex != null)
        {
            _columns[sameNumberInPlayerIndex.Value].Select();

            yield break;
        }

        yield return new WaitForSeconds(_timeBetweenSteps);

        int? sameNumberInIndex = GetIndexForSameNumber(diceNumber);
        if (sameNumberInIndex != null)
        {
            _columns[sameNumberInIndex.Value].Select();

            yield break;
        }

        yield return new WaitForSeconds(_timeBetweenSteps);

        int? emptyColumnIndex = GetIndexForEmptyColumn();
        if (emptyColumnIndex != null)
        {
            _columns[emptyColumnIndex.Value].Select();

            yield break;
        }

        yield return new WaitForSeconds(_timeBetweenSteps);

        _columns[GetRandomAvailableIndex()].Select();
    }

    private int? GetIndexForSameNumber(int diceNumber)
    {
        foreach (DiceColumn column in _columns)
        {
            if (column.IsFull())
            {
                continue;
            }

            foreach (int number in column.numbers)
            {
                if (number == diceNumber)
                {
                    return column.index;
                }
            }
        }

        return null;
    }

    private int? GetIndexForSameNumberInPlayerColumns(int diceNumber)
    {
        foreach (DiceColumn column in _playerColumns.columns)
        {
            if (_columns[column.index].IsFull())
            {
                continue;
            }

            foreach (int number in column.numbers)
            {
                if (number == diceNumber)
                {
                    return column.index;
                }
            }
        }

        return null;
    }

    private int? GetIndexForEmptyColumn()
    {
        foreach (DiceColumn column in _columns)
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
            int randomIndex = Random.Range(0, _columns.Length);

            if (_columns[randomIndex].IsFull())
            {
                continue;
            }

            return randomIndex;
        }
    }
}
