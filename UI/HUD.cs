using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public string retriesFormat = "00";
    public string healthFormat = "0";
    public string coinsFormat = "000";

    [Header("UI Elements")]
    public Text retires;
    public Text health;
    public Text coins;
    public Text timer;
    public Image[] starsImages;

    protected Game m_game;
    protected LevelScore m_score;
    protected Player m_player;

    protected float timerStep;
    protected static float timerRefreshRate = 0.1f;

    protected virtual void UpdateCoins(int value)
    {
        coins.text = value.ToString(coinsFormat);
    }

    protected virtual void UpdateRetries(int value)
    {
        retires.text = value.ToString(retriesFormat);
    }

    protected virtual void UpdateHealth()
    {
        health.text = m_player.health.current.ToString(healthFormat);
    }

    protected virtual void UpdateStars(bool[] value)
    {
        for (int i = 0; i < starsImages.Length; i++)
        {
            starsImages[i].enabled = value[i];
        }
    }

    protected virtual void UpdateTimer()
    {
        timerStep += Time.deltaTime;

        if (timerStep >= timerRefreshRate)
        {
            timer.text = GameLevel.FormattedTime(m_score.time);
            timerStep = 0;
        }
    }

    public virtual void Refresh()
    {
        UpdateCoins(m_score.coins);
        UpdateRetries(m_game.retries);
        UpdateHealth();
        UpdateStars(m_score.stars);
    }

    protected virtual void Awake()
    {
        m_game = Game.instance;
        m_score = LevelScore.instance;
        m_player = FindObjectOfType<Player>();

        m_score.OnScoreLoaded.AddListener(() =>
        {
            m_score.OnCoinsSet.AddListener(UpdateCoins);
            m_score.OnStarsSet.AddListener(UpdateStars);
            m_game.OnRetriesSet.AddListener(UpdateRetries);
            m_player.health.onChange.AddListener(UpdateHealth);
            Refresh();
        });
    }

    protected virtual void Update() => UpdateTimer();
}
