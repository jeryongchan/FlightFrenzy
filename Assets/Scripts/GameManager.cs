using System;
using UnityEngine;

public enum GameState
{
    Start,
    HowToPlay,
    Playing
}

// owns game state, countdown and score
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<GameState> StateChanged;

    [SerializeField]
    float gameDuration = 10f;

    [SerializeField]
    RingSpawner ringSpawner;

    [SerializeField]
    PlaneController plane;

    public GameState State { get; private set; }
    public int Score { get; private set; }
    public float TimeLeft { get; private set; }

    void Awake()
    {
        Instance = this;
        ringSpawner.enabled = false;
        plane.enabled = false;
    }

    void Update()
    {
        if (State == GameState.Playing)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0f)
            {
                TimeLeft = 0f;
                EnterStart();
            }
        }
    }

    public void EnterStart()
    {
        ClearRings();
        ringSpawner.enabled = false;
        plane.enabled = false;
        SetState(GameState.Start);
    }

    public void EnterHowToPlay()
    {
        SetState(GameState.HowToPlay);
    }

    public void EnterPlaying()
    {
        Score = 0;
        TimeLeft = gameDuration;
        plane.ResetPosition();
        plane.enabled = true;
        ringSpawner.enabled = true;
        SetState(GameState.Playing);
    }

    void SetState(GameState state)
    {
        State = state;
        StateChanged?.Invoke(state);
    }

    public void AddScore()
    {
        Score++;
    }

    void ClearRings()
    {
        RingController[] rings = FindObjectsByType<RingController>(FindObjectsSortMode.None);
        foreach (RingController ring in rings)
        {
            Destroy(ring.gameObject);
        }
    }
}
