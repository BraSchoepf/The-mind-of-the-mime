using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] _movementPoint;
    [SerializeField] private float speed = 2f;

    private int _nextPoint = 1;
    private bool _movementSequence = true;

    private void Start()
    {
        if (_movementPoint.Length < 2)
        {
            Debug.LogError("There should be at least two points for the platform to move between.");
            enabled = false; 
            return;
        }
    }

    private void Update()
    {
        
        if (_movementSequence && _nextPoint >= _movementPoint.Length - 1)
        {
            _movementSequence = false;
        }
       
        else if (!_movementSequence && _nextPoint <= 0)
        {
            _movementSequence = true;
        }

        if (_nextPoint >= 0 && _nextPoint < _movementPoint.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position, _movementPoint[_nextPoint].position,
                speed * Time.deltaTime);
        
            if (Vector2.Distance(transform.position, _movementPoint[_nextPoint].position) < 0.1f)
            {
                _nextPoint = _movementSequence ? _nextPoint + 1 : _nextPoint - 1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }

    }
}

