using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Dice : MonoBehaviour
{
    public event System.Action<int> onMultiplierChanged;
    public event System.Action onRemoved;

    public int number { get; private set; }
    public int multiplier { get; private set; }
    public float removeTime { get { return _removeTime; } }

    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _removeTime = 1f;

    private SpriteRenderer _spriteRenderer;

    public void SetNumber(int number)
    {
        this.number = number;

        _spriteRenderer.sprite = _sprites[number - 1];
    }

    public void SetRandomNumber()
    {
        SetNumber(Random.Range(1, 7));
    }

    public void SetMultiplier(int multiplier)
    {
        if (this.multiplier != multiplier)
        {
            onMultiplierChanged?.Invoke(multiplier);
        }

        this.multiplier = multiplier;
    }

    public void Remove()
    {
        onRemoved?.Invoke();

        Destroy(gameObject, _removeTime);
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetRandomNumber();
    }
}
