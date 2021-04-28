using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/**
 * Replay System
 * How does it work?
 *      The system starts a never ending coroutine at the start of each level, it records each (refreshRateTime) a frame into a Queue
 *      The data recorded each frame is the position of the player and the velocity it had
 *      Then, when player loses a life, the system dumps the recorded data into another Queue that will be used to replay the playthrough using the ghost player
 *      While, at the same time, records the current playthrough
 *      More info down below the code
 */
public class ReplaySystem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRb;                     // Rigidbody of the object we want to record
    [SerializeField] private Transform _ghostPlayer;                    // The game object used as ghost for the replay
    [SerializeField] private Transform _respawnPoint;                   // The point where the ghost respawns, should be the same as the player's
    [SerializeField] private float refreshRateTime = .1f;               // RefreshRateTime, how often the system captures, lower values = smoother transitions but slower
                                                                        // Runtime experience, higher values = faster experience but whacky movement
    [SerializeField] private float playerDeathAnimationTime = 1.4f;     // How long is the death animation?

    private Queue<Vector3> recordingPositions;                          // The Queue that's used for recording - positions
    private Queue<Vector3> recodringVelocities;                         // The Queue that's used for recording - velocities
    private Queue<Vector3> replayPositions;                             // The Queue that's used for replaying - positions
    private Queue<Vector3> replayVelocities;                            // The Queue that's used for replaying - velocities

    private Animator _animator;
    private WaitForSeconds refreshRate;
    private WaitForSeconds playerDeathAnimation;

    private Vector3 _direction;
    private Vector3 _nextPosition;
    private Vector3 _velocity;
    private Vector3 _playerLocalScale;

    private PlayerMovement playerMovement;

    private void Start()
    {
        _ghostPlayer.gameObject.SetActive(false);
        PlayerHealthSystem.OnPlayerDeathEvent += StopRecording;
        playerMovement = _playerRb.GetComponent<PlayerMovement>();

        recordingPositions = new Queue<Vector3>();
        replayPositions = new Queue<Vector3>();
        replayVelocities = new Queue<Vector3>();
        recodringVelocities = new Queue<Vector3>();

        _animator = _ghostPlayer.GetComponentInChildren<Animator>();
        _playerLocalScale = _ghostPlayer.localScale;

        refreshRate = new WaitForSeconds(refreshRateTime);
        playerDeathAnimation = new WaitForSeconds(playerDeathAnimationTime);
        StartCoroutine(Record());
    }

    private IEnumerator Record()
    {
        while (true)
        {
            // Record player's position
            recordingPositions.Enqueue(_playerRb.position);
            // Record player's velocity
            recodringVelocities.Enqueue(playerMovement.Velocity);
            // Wait for refreshRateTime seconds
            yield return refreshRate;
        }
    }

    public void StopRecording()
    {
        _ghostPlayer.gameObject.SetActive(true);

        // We make sure nothing in the replay queues remains, we don't want sudden jumpy movement from previous ghost replay
        replayPositions.Clear();
        replayVelocities.Clear();

        // We copy everything from the recodings into the replaying
        replayPositions = new Queue<Vector3>(recordingPositions);
        replayVelocities = new Queue<Vector3>(recodringVelocities);

        // We clear recordings so we can start fresh
        recordingPositions.Clear();
        recodringVelocities.Clear();

        // We stop replaying if we are (The case the player lost a life - ghost player is playing - then before the ghost player is done, player loses another life)
        StopCoroutine(Replay());
        StartCoroutine(Replay());
    }

    private IEnumerator playDeathAnimation()
    {
        // We play the animations and wait for it to finish
        _animator.SetBool("Dead", true);
        yield return playerDeathAnimation;
        _animator.SetBool("Dead", false);
    }

    private IEnumerator Replay()
    {
        // We loop through all the replay positions
        foreach (var p in replayPositions)
        {
            _nextPosition = p;
            // Player didn't move, skip this
            if (Vector3.Distance(_nextPosition, _ghostPlayer.position) < .05f)
            {
                // We remove a velocity as well, to keep both queues balanced and even
                replayVelocities.Dequeue();
                continue;
            }

            // We get the velocity from the queue
            _velocity = replayVelocities.Dequeue();
            // Get the direction the player is facing based on the velocity
            _direction = _velocity.normalized;

            // We clamp its x, either -1 or 1, nothing in between
            _direction.x = (-_direction.x >= 0) ? 1 : -1;

            _playerLocalScale.x = _direction.x;

            _animator.SetFloat("Horizontal", _direction.x);
            _animator.SetFloat("Vertical", _direction.y);
            _animator.SetFloat("Velocity", _velocity.sqrMagnitude);

            _ghostPlayer.position = _nextPosition;
            _ghostPlayer.localScale = _playerLocalScale;

            yield return refreshRate;
        }
        yield return playDeathAnimation();
        // After animations finishes, we disable the ghost player since we don't want it sticking around, we enable it only when replaying
        _ghostPlayer.position = _respawnPoint.position;
        _ghostPlayer.gameObject.SetActive(false);

        // We clear any remains, extra caution
        replayPositions.Clear();
        replayVelocities.Clear();
    }

    private void OnDisable()
    {
        // We unsubscribe from the observer pattern of the player to avoid null exceptions
        PlayerHealthSystem.OnPlayerDeathEvent -= StopRecording;
    }
}
