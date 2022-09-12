using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static System.Action onGameIsSetup;
    public static System.Action onGameStart;
    public static System.Action onGameOver;

    public static GameManager instance { get; private set; }

    public bool isPlayerTurn { get; private set; }

    [SerializeField] private Player _player;
    [SerializeField] private Computer _computer;

    [SerializeField] private MovingDice _dicePrefab;

    [SerializeField] private float _timeBeforeGameStart = 2f;

    private MovingDice _currentDice;

    private bool _canRollDice;
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
        MovingDice.onRollEnded += DiceRollEnded;

        DiceColumn.onSelected += ColumnSelected;
        DiceColumn.onDiceAdded += DiceAddedInColumn;
        DiceColumn.onDicesRemoved += DicesRemovedInColumn;
    }

    private void OnDisable()
    {
        MovingDice.onRollEnded -= DiceRollEnded;

        DiceColumn.onSelected -= ColumnSelected;
        DiceColumn.onDiceAdded -= DiceAddedInColumn;
        DiceColumn.onDicesRemoved -= DicesRemovedInColumn;
    }

    private void Start()
    {
        _isGameOver = false;
        isPlayerTurn = Random.Range(0, 2) == 1;

        _canRollDice = false;

        _player.SetCanSelectColumn(false);

        UpdateScores();

        onGameIsSetup?.Invoke();

        StartCoroutine(StartGame());
    }

    private void Update()
    {
        RollDice();
    }

    private void RollDice()
    {
        if (!_canRollDice || _isGameOver)
        {
            return;
        }

        Vector3 position;

        if (isPlayerTurn)
        {
            position = _player.diceBox.position;
        }
        else
        {
            position = _computer.diceBox.position;
        }

        _currentDice = Instantiate(_dicePrefab, position, Quaternion.identity);
        _currentDice.Roll();

        _canRollDice = false;
    }

    private void DiceRollEnded()
    {
        if (isPlayerTurn)
        {
            _player.SetCanSelectColumn(true);
        }
        else
        {
            _computer.SelectColumn(_currentDice.dice.number);
        }
    }

    private void ColumnSelected(int columnIndex)
    {
        if (isPlayerTurn)
        {
            _player.SetCanSelectColumn(false);

            _player.diceGrid.AddDiceInColumn(_currentDice, columnIndex);
        }
        else
        {
            _computer.diceGrid.AddDiceInColumn(_currentDice, columnIndex);
        }
    }

    private void DiceAddedInColumn(int diceNumber, int columnIndex)
    {
        if (isPlayerTurn)
        {
            _computer.diceGrid.RemoveDiceInColumn(_currentDice.dice.number, columnIndex);
        }
        else
        {
            _player.diceGrid.RemoveDiceInColumn(_currentDice.dice.number, columnIndex);
        }

        _currentDice = null;
    }

    private void DicesRemovedInColumn()
    {
        UpdateScores();

        isPlayerTurn = !isPlayerTurn;

        if (_player.diceGrid.IsFull() || _computer.diceGrid.IsFull())
        {
            _isGameOver = true;

            onGameOver?.Invoke();

            return;
        }

        _canRollDice = true;
    }

    private void UpdateScores()
    {
        _player.UpdateScore();
        _computer.UpdateScore();
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(_timeBeforeGameStart);

        _canRollDice = true;

        onGameStart?.Invoke();
    }
}
