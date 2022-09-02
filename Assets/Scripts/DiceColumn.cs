using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DiceColumn : MonoBehaviour
{
    public static event Action<int> onSelected;

    public int score { get { return _score; } }
    public int[] numbers { get { return _numbers; } }
    public bool isSelectable { get { return _isSelectable; } }
    [NonSerialized] public int index;

    [SerializeField] private Dice[] _dices;
    [SerializeField] private TextMeshPro _scoreText;
    [SerializeField] private ColumnSelector _selector;

    private int[] _numbers;
    private int _score;
    private bool _isSelectable;

    public void AddDice(int diceNumber)
    {
        for (int i = 0; i < _numbers.Length; i++)
        {
            if (_numbers[i] == 0)
            {
                _numbers[i] = diceNumber;
                break;
            }
        }

        UpdateColumn();
    }

    public void RemoveDice(int diceNumber)
    {
        for (int i = 0; i < _numbers.Length; i++)
        {
            if (_numbers[i] == diceNumber)
            {
                _numbers[i] = 0;
            }
        }

        // Reorder column
        for (int i = 0; i < _numbers.Length; i++)
        {
            if (_numbers[i] > 0)
            {
                continue;
            }

            for (int j = i + 1; j < _numbers.Length; j++)
            {
                if (_numbers[j] > 0)
                {
                    _numbers[i] = _numbers[j];
                    _numbers[j] = 0;
                    break;
                }
            }
        }

        UpdateColumn();
    }

    public bool IsFull()
    {
        foreach (int number in _numbers)
        {
            if (number == 0)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsEmpty()
    {
        foreach (int number in _numbers)
        {
            if (number > 0)
            {
                return false;
            }
        }

        return true;
    }

    public void Select()
    {
        if (IsFull())
        {
            return;
        }

        SetIsSelectable(false);

        onSelected?.Invoke(index);
    }

    public void SetIsSelectable(bool isSelectable)
    {
        _isSelectable = isSelectable;
        _selector.SetIsSelectable(isSelectable);
    }

    private void Start()
    {
        _numbers = new int[3];

        SetIsSelectable(false);

        UpdateColumn();
    }

    private void OnMouseDown()
    {
        if (!isSelectable || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Select();
    }

    private void UpdateColumn()
    {
        UpdateScore();
        UpdateScoreText();
        UpdateDices();
    }

    private void UpdateScoreText()
    {
        _scoreText.SetText(_score.ToString());
    }

    private void UpdateScore()
    {
        _score = 0;

        for (int i = 0; i < _numbers.Length; i++)
        {
            int multiplier = Array.FindAll(_numbers, x => x == _numbers[i]).Length;
            if (multiplier == 0)
            {
                multiplier = 1;
            }

            _score += _numbers[i] * multiplier;
        }
    }

    private void UpdateDices()
    {
        for (int i = 0; i < _numbers.Length; i++)
        {
            if (_numbers[i] == 0)
            {
                _dices[i].SetNumber(0);
                continue;
            }

            int multiplier = Array.FindAll(_numbers, x => x == _numbers[i]).Length;
            if (multiplier == 0)
            {
                multiplier = 1;
            }

            _dices[i].SetNumber(_numbers[i], multiplier);
        }
    }
}
