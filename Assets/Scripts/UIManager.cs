using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _centerText;

    private void OnEnable()
    {
        GameManager.onGameOver += DisplayGameOverText;
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= DisplayGameOverText;
    }

    private void Start()
    {
        _centerText.gameObject.SetActive(false);
    }

    private void DisplayGameOverText()
    {
        _centerText.gameObject.SetActive(true);

        int playerScore = GameManager.instance.playerScore;
        int computerScore = GameManager.instance.computerScore;

        if (playerScore > computerScore)
        {
            _centerText.SetText($"PLAYER WINS {playerScore}-{computerScore}");
        }
        else if (playerScore == computerScore)
        {
            _centerText.SetText("DRAW");
        }
        else
        {
            _centerText.SetText($"COMPUTER WINS {computerScore}-{playerScore}");
        }
    }
}
