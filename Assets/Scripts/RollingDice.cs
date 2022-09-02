using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Dice))]
public class RollingDice : MonoBehaviour
{
    public event System.Action onRollEnded;

    public Dice dice { get { return _dice; } }

    [SerializeField] private float _throwForce = 10f;
    [SerializeField] private float _throwTorque = 10f;
    [SerializeField] private float _rollForSeconds = 2f;
    [SerializeField] private float _randomizeNumberRate = 1f;

    private Rigidbody2D _rigidbody;
    private Dice _dice;

    private bool _isRolling = false;

    public void Roll()
    {
        if (_isRolling)
        {
            return;
        }

        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        _rigidbody.AddForce(direction.normalized * _throwForce, ForceMode2D.Impulse);
        _rigidbody.AddTorque(_throwTorque);

        _isRolling = true;

        StartCoroutine(RandomizeNumber());
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _dice = GetComponent<Dice>();
    }

    private void Start()
    {
        _isRolling = false;
        _dice.SetNumber(1);
    }

    private IEnumerator RandomizeNumber()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _rollForSeconds)
        {
            _dice.SetNumber(Random.Range(1, 7));

            yield return new WaitForSeconds(_randomizeNumberRate);

            elapsedTime += _randomizeNumberRate;
        }

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _isRolling = false;

        onRollEnded?.Invoke();
    }
}
