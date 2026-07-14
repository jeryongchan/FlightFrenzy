using System;
using UnityEngine;
using UnityEngine.Video;

public enum GameState
{
    Start,
    HowToPlay,
    FlyIn, // plane flies in and the world speeds up before timer start
    Playing,
    FlyOut // timer done, world speeds down and plane still flying briefly before next start screen
}

// owns game state, countdown and score
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<GameState> StateChanged;

    [SerializeField]
    float gameDuration = 10f;

    [SerializeField]
    float flyInDuration = 1.5f;

    [SerializeField]
    float flyOutDuration = 1.5f;

    [SerializeField]
    RingSpawner ringSpawner;

    [SerializeField]
    PlaneController plane;

    [SerializeField]
    AudioSource ringCollectedSound;

    [SerializeField]
    AudioSource countdownSound;

    [SerializeField]
    AudioSource jetSound;

    [SerializeField]
    VideoPlayer skyBackgroundVideo;

    [SerializeField]
    ParticleSystem speedLineParticles;

    [SerializeField]
    float countdownLeadTime = 4f; // comes in 4 beeps, with the final one higher pitched on zeroth second.

    [SerializeField]
    float playingWorldSpeed = 2f;

    [SerializeField]
    float idleWorldSpeed = 1f; // rings and sky video slow down together during the fly in and fly out

    [SerializeField]
    float worldSpeedChangeRate = 2f;

    [SerializeField]
    float idleJetVolume = 0.2f; // lower engine volume when not playing

    [SerializeField]
    float playingJetVolume = 0.5f;

    public GameState State { get; private set; }
    public int Score { get; private set; }
    public float TimeLeft { get; private set; }
    public float WorldSpeed { get; private set; } = 1f;

    bool countdownStarted;
    float phaseTimeLeft; // only used for fly in and fly out

    void Awake()
    {
        Instance = this;
        ringSpawner.enabled = false;
        plane.enabled = false;

        jetSound.volume = idleJetVolume; // loop runs the whole time, only the volume changes
        jetSound.Play();

        // webgl (itch io) cannot play imported video clips, so stream from StreamingAssets instead
        skyBackgroundVideo.url = Application.streamingAssetsPath + "/sky_bg_video.mp4";
        skyBackgroundVideo.Play();
    }

    void Update()
    {
        if (State == GameState.FlyIn)
        {
            EaseWorldSpeed(playingWorldSpeed);

            phaseTimeLeft -= Time.deltaTime;
            if (phaseTimeLeft <= 0f)
            {
                EnterPlaying();
            }
        }
        else if (State == GameState.Playing)
        {
            TimeLeft -= Time.deltaTime;

            if (!countdownStarted && TimeLeft <= countdownLeadTime)
            {
                countdownStarted = true;
                countdownSound.Play(); // warn player near the end
            }

            if (TimeLeft <= 0f)
            {
                TimeLeft = 0f;
                EnterFlyOut();
            }
        }
        else if (State == GameState.FlyOut)
        {
            EaseWorldSpeed(idleWorldSpeed);

            phaseTimeLeft -= Time.deltaTime;
            if (phaseTimeLeft <= 0f)
            {
                EnterStart();
            }
        }
    }

    public void EnterFlyIn()
    {
        Score = 0;
        TimeLeft = gameDuration;
        countdownStarted = false; // rearm the countdown
        phaseTimeLeft = flyInDuration;

        WorldSpeed = idleWorldSpeed;
        skyBackgroundVideo.playbackSpeed = idleWorldSpeed;

        plane.ResetPosition();
        plane.FlyIn(flyInDuration);
        SetState(GameState.FlyIn);
    }

    void EnterFlyOut()
    {
        phaseTimeLeft = flyOutDuration;
        ringSpawner.enabled = false; // existing rings keep flying past
        speedLineParticles.Stop(); // in flight lines finish their lifetime, so no pop
        SetState(GameState.FlyOut);
    }

    public void EnterStart()
    {
        ClearRings();
        plane.enabled = false;
        plane.ResetPosition();
        SetState(GameState.Start);
    }

    public void EnterHowToPlay()
    {
        SetState(GameState.HowToPlay);
    }

    void EnterPlaying()
    {
        plane.enabled = true; // player takes over where the fly in left off
        ringSpawner.enabled = true;
        speedLineParticles.Play();
        SetState(GameState.Playing);
    }

    void SetState(GameState state)
    {
        State = state;
        jetSound.volume = state == GameState.Playing ? playingJetVolume : idleJetVolume;
        StateChanged?.Invoke(state);
    }

    public void AddScore()
    {
        if (State != GameState.Playing)
        {
            return; // rings collected during the fly out no longer count
        }
        Score++;
        ringCollectedSound.PlayOneShot(ringCollectedSound.clip);
    }

    void ClearRings()
    {
        RingController[] rings = FindObjectsByType<RingController>(FindObjectsSortMode.None);
        foreach (RingController ring in rings)
        {
            Destroy(ring.gameObject);
        }
    }

    void EaseWorldSpeed(float target)
    {
        WorldSpeed = Mathf.Lerp(WorldSpeed, target, worldSpeedChangeRate * Time.deltaTime);
        skyBackgroundVideo.playbackSpeed = WorldSpeed;
    }
}
