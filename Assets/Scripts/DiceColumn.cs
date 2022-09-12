using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DiceColumn : MonoBehaviour
{
    public static event Action<int> onSelected;
    public static event Action<int, int> onDiceAdded;
    public static event Action onDicesRemoved;

    public int score { get; private set; }
    public MovingDice[] dices { get; private set; }
    public bool isSelectable { get; private set; }

    [NonSerialized] public int index;

    [SerializeField] private Transform[] _slots;
    [SerializeField] private TextMeshPro _scoreText;
    [SerializeField] private ColumnSelector _selector;

    public void AddDice(MovingDice dice)
    {
        StartCoroutine(AddDiceCoroutine(dice));
    }

    public void RemoveDicesWithNumber(int number)
    {
        StartCoroutine(RemoveDicesWithNumberCoroutine(number));
    }

    public bool IsFull()
    {
        foreach (MovingDice dice in dices)
        {
            if (dice == null)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsEmpty()
    {
        foreach (MovingDice dice in dices)
        {
            if (dice != null)
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
        this.isSelectable = isSelectable;
        _selector.SetIsSelectable(isSelectable);
    }

    private void Start()
    {
        dices = new MovingDice[_slots.Length];

        SetIsSelectable(false);

        UpdateScore();
    }

    private void OnMouseDown()
    {
        if (!isSelectable || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Select();
    }

    private void UpdateScore()
    {
        score = 0;

        for (int i = 0; i < dices.Length; i++)
        {
            if (dices[i] == null)
            {
                continue;
            }

            int multiplier = Array.FindAll(dices, d => d != null && d.dice.number == dices[i].dice.number).Length;
            if (multiplier == 0)
            {
                multiplier = 1;
            }

            score += dices[i].dice.number * multiplier;

            dices[i].dice.SetMultiplier(multiplier);
        }

        _scoreText.SetText(score.ToString());
    }

    private IEnumerator AddDiceCoroutine(MovingDice dice)
    {
        for (int i = 0; i < dices.Length; i++)
        {
            if (dices[i] == null)
            {
                dices[i] = dice;

                dice.MoveTo(_slots[i].position);

                break;
            }
        }

        while (dice.isMoving)
        {
            yield return null;
        }

        UpdateScore();

        onDiceAdded?.Invoke(dice.dice.number, index);
    }

    private IEnumerator RemoveDicesWithNumberCoroutine(int number)
    {
        float waitTimeBeforeReorder = 0f;
        for (int i = 0; i < dices.Length; i++)
        {
            if (dices[i] == null)
            {
                continue;
            }

            if (dices[i].dice.number == number)
            {
                dices[i].dice.Remove();

                waitTimeBeforeReorder = dices[i].dice.removeTime;

                dices[i] = null;
            }
        }

        yield return new WaitForSeconds(waitTimeBeforeReorder);

        for (int i = 0; i < dices.Length; i++)
        {
            if (dices[i] != null)
            {
                continue;
            }

            for (int j = i + 1; j < dices.Length; j++)
            {
                if (dices[j] != null)
                {
                    dices[j].MoveTo(_slots[i].position);

                    dices[i] = dices[j];
                    dices[j] = null;
                    break;
                }
            }
        }

        bool hasMovingDice = true;
        while (hasMovingDice)
        {
            hasMovingDice = false;
            foreach (MovingDice dice in dices)
            {
                if (dice != null && dice.isMoving)
                {
                    hasMovingDice = true;
                    break;
                }
            }

            yield return null;
        }

        UpdateScore();

        onDicesRemoved?.Invoke();
    }
}
