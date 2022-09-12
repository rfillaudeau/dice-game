using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Dice), typeof(BoxCollider2D))]
public class MovingDice : MonoBehaviour
{
    public static event System.Action onRollEnded;

    public Dice dice { get; private set; }
    public bool isMoving { get; private set; }

    [SerializeField] private float _rollForce = 10f;
    [SerializeField] private float _rollTime = 2f;
    [SerializeField] private float _randomizeNumberRate = 1f;

    [SerializeField] private float _movingSpeed = 1f;
    [SerializeField] private float _stopRadius = 0.1f;
    [SerializeField] private float _movingElevation = 1f;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

    private Vector3? _targetPosition;

    public void Roll()
    {
        if (isMoving)
        {
            return;
        }

        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        _rigidbody.AddForce(direction.normalized * _rollForce, ForceMode2D.Impulse);

        StartCoroutine(RollingCoroutine());
    }

    public void MoveTo(Vector3 position)
    {
        Vector3 targetPosition = position;
        targetPosition.z -= _movingElevation;

        _targetPosition = targetPosition;
        _collider.enabled = false;

        isMoving = true;
    }

    private void Awake()
    {
        dice = GetComponent<Dice>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        isMoving = false;
    }

    private void Update()
    {
        if (_targetPosition != null)
        {
            Vector3 direction = (_targetPosition.Value - transform.position).normalized;

            transform.Translate(direction * _movingSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPosition.Value) <= _stopRadius)
            {
                Vector3 targetPosition = _targetPosition.Value;
                targetPosition.z += _movingElevation;

                transform.position = targetPosition;
                _targetPosition = null;

                isMoving = false;
            }
        }
    }

    private IEnumerator RollingCoroutine()
    {
        isMoving = true;

        float elapsedTime = 0f;
        while (elapsedTime < _rollTime)
        {
            dice.SetRandomNumber();

            yield return new WaitForSeconds(_randomizeNumberRate);

            elapsedTime += _randomizeNumberRate;
        }

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        isMoving = false;

        onRollEnded?.Invoke();
    }
}
