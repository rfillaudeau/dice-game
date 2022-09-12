using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _centerText;

    [SerializeField] private Player _player;
    [SerializeField] private Computer _computer;

    private void OnEnable()
    {
        GameManager.onGameIsSetup += DisplayStartingPlayer;
        GameManager.onGameStart += HideCenterText;
        GameManager.onGameOver += DisplayGameOverText;
    }

    private void OnDisable()
    {
        GameManager.onGameIsSetup -= DisplayStartingPlayer;
        GameManager.onGameStart -= HideCenterText;
        GameManager.onGameOver -= DisplayGameOverText;
    }

    private void Start()
    {
        HideCenterText();
    }

    private void DisplayStartingPlayer()
    {
        _centerText.gameObject.SetActive(true);

        if (GameManager.instance.isPlayerTurn)
        {
            _centerText.SetText($"PLAYER STARTS");
        }
        else
        {
            _centerText.SetText($"COMPUTER STARTS");
        }
    }

    private void HideCenterText()
    {
        _centerText.gameObject.SetActive(false);
    }

    private void DisplayGameOverText()
    {
        _centerText.gameObject.SetActive(true);

        if (_player.score > _computer.score)
        {
            _centerText.SetText($"PLAYER WINS {_player.score}-{_computer.score}");
        }
        else if (_player.score == _computer.score)
        {
            _centerText.SetText("DRAW");
        }
        else
        {
            _centerText.SetText($"COMPUTER WINS {_computer.score}-{_player.score}");
        }
    }
}
