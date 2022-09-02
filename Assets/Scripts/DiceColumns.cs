using UnityEngine;

public class DiceColumns : MonoBehaviour
{
    public DiceColumn[] columns { get { return _columns; } }

    [SerializeField] protected DiceColumn[] _columns;

    public void AddDiceInColumn(int diceNumber, int columnIndex)
    {
        _columns[columnIndex].AddDice(diceNumber);
    }

    public void RemoveDiceInColumn(int diceNumber, int columnIndex)
    {
        _columns[columnIndex].RemoveDice(diceNumber);
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
