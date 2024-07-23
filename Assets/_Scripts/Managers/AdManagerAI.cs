using UnityEditor;
using UnityEngine;

public class AdManagerAI

    : Singleton<AdManagerAI>
{
 
    public float adInterval = 60f * 3f; // Time interval to show ads in seconds
    public bool isInGameView = false;
    public bool shouldShowAd = false;
    public float adTimer;
    public int matchCount = 0;
    public int matchesBeforeAd = 4; // Number of matches before showing an ad

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        // Initialize the ad timer
        adTimer = adInterval;
    }

    private void Update()
    {
        if (isInGameView)
        {
            // If in-game, decrement the ad timer but do not reset it
            adTimer -= Time.deltaTime;

            if (adTimer <= 0)
            {
                // Set the flag to show ad after exiting game view
                shouldShowAd = true;
            }
        }
        else
        {
            // If not in-game, continue decrementing the ad timer
            adTimer -= Time.deltaTime;

            if (adTimer <= 0 && !shouldShowAd)
            {
                ShowVideoAd();
                ResetAdTimerAndMatchCount();
            }

            // Check if the player has played enough matches within the ad interval
            if (matchCount >= matchesBeforeAd && !shouldShowAd)
            {
                ShowVideoAd();
                ResetAdTimerAndMatchCount();
            }
        }
    }

    public void EnterGameView()
    {
        isInGameView = true;
        matchCount++; // Increase the match count when entering game view
        // Additional logic when entering game view
    }


    public void ExitGameView()
    {
        isInGameView = false;

        if (shouldShowAd)
        {
            ShowVideoAd();
            shouldShowAd = false;
            ResetAdTimerAndMatchCount();
        }
    }

    private void ShowVideoAd()
    {
        Debug.Log("Showing video ad...");
        // Replace with actual ad showing logic
        // AdProvider.ShowVideoAd();
    }

    private void ResetAdTimerAndMatchCount()
    {
        adTimer = adInterval; // Reset the ad timer
        matchCount = 0; // Reset the match count
    }
}
