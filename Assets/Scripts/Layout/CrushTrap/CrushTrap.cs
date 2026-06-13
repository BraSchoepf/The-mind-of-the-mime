using UnityEngine;

public class CrushTrap : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float _attackDistance = 3f;
    [SerializeField] private float _attackSpeed = 8f;
    [SerializeField] private float _returnSpeed = 2f;
    [SerializeField] private float _holdTime = 0.2f;
    [SerializeField] private float _pauseBetweenCycles = 1f;
    [SerializeField] private bool _moveHorizontal = false;


    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private enum TrapState { Idle, Expanding, Holding, Contracting, Pausing }
    private TrapState _state = TrapState.Idle;
    private float _holdCounter;
    private float _pauseCounter;
    private bool _playerInRange = false; // ← nuevo

    private void Start()
    {
        _startPosition = transform.position;
        _targetPosition = _moveHorizontal
            ? _startPosition + Vector3.right * _attackDistance
            : _startPosition + Vector3.down * _attackDistance;
    }

    private void Update()
    {
        switch (_state)
        {
            case TrapState.Idle:
                if (_playerInRange)
                    _state = TrapState.Expanding;
                break;

            case TrapState.Expanding:
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _attackSpeed * Time.deltaTime);
                if (transform.position == _targetPosition)
                {
                    _holdCounter = _holdTime;
                    _state = TrapState.Holding;
                }
                break;

            case TrapState.Holding:
                _holdCounter -= Time.deltaTime;
                if (_holdCounter <= 0f)
                    _state = TrapState.Contracting;
                break;

            case TrapState.Contracting:
                transform.position = Vector3.MoveTowards(transform.position, _startPosition, _returnSpeed * Time.deltaTime);
                if (transform.position == _startPosition)
                {
                    if (_playerInRange)
                    {
                        _pauseCounter = _pauseBetweenCycles;
                        _state = TrapState.Pausing; // ← pausa antes de volver a atacar
                    }
                    else
                    {
                        _state = TrapState.Idle;
                    }
                }
                break;

            case TrapState.Pausing:
                _pauseCounter -= Time.deltaTime;
                if (_pauseCounter <= 0f)
                    _state = _playerInRange ? TrapState.Expanding : TrapState.Idle;
                break;
        }
    }

    public void PlayerDetected()
    {
        _playerInRange = true;
        if (_state == TrapState.Idle)
            _state = TrapState.Expanding;
    }

    public void PlayerLeft()
    {
        _playerInRange = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && _state == TrapState.Expanding)
            CheckPointSystem.instance.TeleportToCheckpoint();
    }

    public void OnPlayerHit()
    {
        if (_state == TrapState.Expanding)
            CheckPointSystem.instance.TeleportToCheckpoint();
    }

    private void OnDrawGizmos()
    {
        Vector3 target = Application.isPlaying ? _targetPosition :
            (_moveHorizontal
                ? transform.position + Vector3.right * _attackDistance
                : transform.position + Vector3.down * _attackDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(target, transform.localScale);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target);
    }
}