using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CMF;

[System.Serializable]
public class AnnouncerSoundList
{
    public AudioClip ready;
    public AudioClip start;
    public AudioClip end;
}

public class GameScript : MonoBehaviour
{
    public AnnouncerSoundList soundList;
    public GameObject[] players;
    public Sprite rockIcon;
    public Sprite paperIcon;
    public Sprite scissorsIcon;

    public GameStatus status = GameStatus.NotStarted;
    public Color blueColor;
    GameObject gameStartUI;
    GameObject gameOverUI;
    AudioSource audioSource;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        gameStartUI = GameObject.Find("GameStart_UI");
        gameOverUI = GameObject.Find("GameEnd_UI");
        gameOverUI.SetActive(false);
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Start()
    {
        PlayAudio(soundList.ready, 3f);
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2.5f);
        StartGame();
    }

    void StartGame()
    {
        status = GameStatus.InProgress;
        gameStartUI.SetActive(false);
        PlayAudio(soundList.start, 3f);
    }

    public void EndGame(Player loser)
    {
        // Idempotency
        if (status == GameStatus.End) return;

        PlayAudio(soundList.end, 3f);

        // Set status
        status = GameStatus.End;

        // Turn on UI
        gameOverUI.SetActive(true);
        Text winnerText = gameOverUI.transform.Find("Winner").GetComponent<Text>();
        winnerText.color = loser == Player.Player1 ? blueColor : Color.red;
        string playerString = loser == Player.Player1 ? "Player 2" : "Player 1";
        winnerText.text = playerString + " Wins!";

        //Restart game
        StartCoroutine("RestartGame");
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public enum GameStatus
    {
        NotStarted,
        InProgress,
        End,
    }

    public void PlayAudio(AudioClip clip, float volume = 1)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
