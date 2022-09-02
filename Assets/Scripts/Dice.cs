using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Dice : MonoBehaviour
{
    public int number { get { return _number; } }

    [SerializeField] private Sprite[] _sprites;

    private SpriteRenderer _spriteRenderer;

    private int _number;

    public void SetNumber(int number, int multiplier = 1)
    {
        _number = number;

        if (number > 0)
        {
            gameObject.SetActive(true);

            _spriteRenderer.sprite = _sprites[_number - 1];
            SetMultiplier(multiplier);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void SetMultiplier(int multiplier)
    {
        switch (multiplier)
        {
            case 2:
                _spriteRenderer.color = new Color(1f, 0.8352941f, 0.5254902f, 1f);
                break;

            case 3:
                _spriteRenderer.color = new Color(0.4588235f, 0.7529412f, 1f, 1f);
                break;

            default:
                _spriteRenderer.color = Color.white;
                break;
        }
    }
}
