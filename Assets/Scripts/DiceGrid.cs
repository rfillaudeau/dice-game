using UnityEngine;

public class DiceGrid : MonoBehaviour
{
    public DiceColumn[] columns { get { return _columns; } }

    [SerializeField] protected DiceColumn[] _columns;

    public void AddDiceInColumn(MovingDice dice, int columnIndex)
    {
        _columns[columnIndex].AddDice(dice);
    }

    public void RemoveDiceInColumn(int diceNumber, int columnIndex)
    {
        _columns[columnIndex].RemoveDicesWithNumber(diceNumber);
    }

    public int GetScore()
    {
        int score = 0;

        foreach (DiceColumn column in _columns)
        {
            score += column.score;
        }

        return score;
    }

    public bool IsFull()
    {
        foreach (DiceColumn column in _columns)
        {
            if (!column.IsFull())
            {
                return false;
            }
        }

        return true;
    }

    protected virtual void Start()
    {
        for (int i = 0; i < _columns.Length; i++)
        {
            _columns[i].index = i;
        }
    }
}
