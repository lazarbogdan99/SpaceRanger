using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/**
 * Replay System
 * 
 *      The system starts a never ending coroutine at the start of each level, it records each (refreshRateTime) a frame into a Queue
 *      The data recorded each frame is the position of the player and the velocity it had
 *      Then, when player loses a life, the system dumps the recorded data into another Queue 
 *      that will be used to replay the playthrough using the ghost player
 *      While, at the same time, records the current playthrough
 *     
 */
public class ReplaySystem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRb;                     // Rigidbody of the object I want to record
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

        // I make sure nothing in the replay queues remains, I don't want sudden jumpy movement from previous ghost replay
        replayPositions.Clear();
        replayVelocities.Clear();

        // I copy everything from the recodings into the replaying
        replayPositions = new Queue<Vector3>(recordingPositions);
        replayVelocities = new Queue<Vector3>(recodringVelocities);

        // I clear recordings so I can start fresh
        recordingPositions.Clear();
        recodringVelocities.Clear();

        // I stop replaying if I am 
        StopCoroutine(Replay());
        StartCoroutine(Replay());
    }

    private IEnumerator playDeathAnimation()
    {
        // I play the animations and wait for it to finish
        _animator.SetBool("Dead", true);
        yield return playerDeathAnimation;
        _animator.SetBool("Dead", false);
    }

    private IEnumerator Replay()
    {
        // I loop through all the replay positions
        foreach (var p in replayPositions)
        {
            _nextPosition = p;
            // Player didn't move, skip this
            if (Vector3.Distance(_nextPosition, _ghostPlayer.position) < .05f)
            {
                // I remove a velocity as well, to keep both queues balanced and even
                replayVelocities.Dequeue();
                continue;
            }

            // I get the velocity from the queue
            _velocity = replayVelocities.Dequeue();
            // Get the direction the player is facing based on the velocity
            _direction = _velocity.normalized;

            // I clamp its x, either -1 or 1, nothing in between
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
        // After animations finishes, I disable the ghost player since I don't want it sticking around,
       // I enable it only when replaying
        _ghostPlayer.position = _respawnPoint.position;
        _ghostPlayer.gameObject.SetActive(false);

        // I clear any remains
        replayPositions.Clear();
        replayVelocities.Clear();
    }

    private void OnDisable()
    {
        // I unsubscribe from the observer pattern of the player to avoid null exceptions
        PlayerHealthSystem.OnPlayerDeathEvent -= StopRecording;
    }
}
