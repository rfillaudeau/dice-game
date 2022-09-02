using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static System.Action onGameOver;

    public static GameManager instance { get; private set; }

    public int playerScore { get; private set; }
    public int computerScore { get; private set; }

    [SerializeField] private TextMeshPro _playerScoreText;
    [SerializeField] private TextMeshPro _computerScoreText;

    [SerializeField] private RollingDice _playerDice;
    [SerializeField] private RollingDice _computerDice;

    [SerializeField] private PlayerDiceColumns _playerColumns;
    [SerializeField] private ComputerDiceColumns _computerColumns;

    private TurnState _state;

    private bool _isPlayerTurn;
    private bool _isGameOver;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void OnEnable()
    {
        _playerDice.onRollEnded += DiceRollEnded;
        _computerDice.onRollEnded += DiceRollEnded;

        DiceColumn.onSelected += ColumnSelected;
    }

    private void OnDisable()
    {
        _playerDice.onRollEnded -= DiceRollEnded;
        _computerDice.onRollEnded -= DiceRollEnded;

        DiceColumn.onSelected -= ColumnSelected;
    }

    private void Start()
    {
        _isGameOver = false;
        _isPlayerTurn = Random.Range(0, 2) == 1;

        _state = TurnState.RollDice;

        _playerDice.gameObject.SetActive(false);
        _computerDice.gameObject.SetActive(false);

        _playerColumns.SetCanSelectColumn(false);

        UpdateScores();
        UpdateScoreTexts();
    }

    private void Update()
    {
        HandleTurn();
    }

    private void HandleTurn()
    {
        if (_isGameOver)
        {
            return;
        }

        if (_state == TurnState.RollDice)
        {
            if (_isPlayerTurn)
            {
                _playerDice.gameObject.SetActive(true);
                _playerDice.Roll();
            }
            else
            {
                _computerDice.gameObject.SetActive(true);
                _computerDice.Roll();
            }

            _state = TurnState.WaitForDice;
        }
    }

    private void DiceRollEnded()
    {
        _state = TurnState.PlaceDice;

        if (_isPlayerTurn)
        {
            _playerColumns.SetCanSelectColumn(true);
        }
        else
        {
            _computerColumns.SelectColumn(_computerDice.dice.number);
        }
    }

    private void ColumnSelected(int columnIndex)
    {
        if (_isPlayerTurn)
        {
            _playerDice.gameObject.SetActive(false);
            _playerColumns.SetCanSelectColumn(false);

            _playerColumns.AddDiceInColumn(_playerDice.dice.number, columnIndex);
            _computerColumns.RemoveDiceInColumn(_playerDice.dice.number, columnIndex);
        }
        else
        {
            _computerDice.gameObject.SetActive(false);

            _computerColumns.AddDiceInColumn(_computerDice.dice.number, columnIndex);
            _playerColumns.RemoveDiceInColumn(_computerDice.dice.number, columnIndex);
        }

        UpdateScores();
        UpdateScoreTexts();

        _isPlayerTurn = !_isPlayerTurn;

        if (_playerColumns.IsFull() || _computerColumns.IsFull())
        {
            _isGameOver = true;

            onGameOver?.Invoke();

            return;
        }

        _state = TurnState.RollDice;
    }

    private void UpdateScores()
    {
        playerScore = _playerColumns.GetScore();
        computerScore = _computerColumns.GetScore();
    }

    private void UpdateScoreTexts()
    {
        _playerScoreText.SetText(playerScore.ToString());
        _computerScoreText.SetText(computerScore.ToString());
    }
}
