using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;  // For TextMeshPro
using System.Collections.Generic;

public class LeaderboardScript : MonoBehaviour
{
    public TMP_Text[] usernameTextArray;
    public TMP_Text[] userIdTextArray;
    public TMP_Text[] countTextArray;

    public GameObject leaderboardEntryPrefab; // Prefab for leaderboard entry
    public Transform leaderboardParent; // Parent object to store instantiated entries

    private const string leaderboardUrl = "https://server.fivlog.space/api/user/leaderboard";

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string userId;
        public string username;
        public Count _count;
    }

    [System.Serializable]
    public class Count
    {
        public int winnings;
        public int rooms;
    }

    [System.Serializable]
    public class LeaderboardResponse
    {
        public List<LeaderboardEntry> leaderboard;
    }

    private float refreshInterval = 30f;
    private float nextRefreshTime = 0f;

    void Start()
    {
        StartCoroutine(GetLeaderboardData());
    }

    void Update()
    {
        if (Time.time >= nextRefreshTime)
        {
            nextRefreshTime = Time.time + refreshInterval;
            StartCoroutine(GetLeaderboardData());
        }
    }

    private IEnumerator GetLeaderboardData()
    {
        UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching leaderboard: " + request.error);
            yield break;
        }

        if (string.IsNullOrEmpty(request.downloadHandler.text))
        {
            Debug.LogWarning("Empty response from leaderboard API.");
            yield break;
        }

        LeaderboardResponse response = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);

        if (response.leaderboard == null || response.leaderboard.Count == 0)
        {
            Debug.Log("No players in the leaderboard.");
            yield break;
        }

        response.leaderboard.Sort((x, y) => y._count.winnings.CompareTo(x._count.winnings));

        // Populate the top 3 leaderboard UI elements
        for (int i = 0; i < usernameTextArray.Length; i++)
        {
            if (i >= response.leaderboard.Count) break;

            var entry = response.leaderboard[i];

            usernameTextArray[i].text = entry.username;
            userIdTextArray[i].text = entry.userId;
            countTextArray[i].text = $"Winnings: {entry._count.winnings}";
        }

        // Clear existing instantiated entries before creating new ones
        foreach (Transform child in leaderboardParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate prefabs for remaining leaderboard entries beyond the top 3
        for (int i = 3; i < response.leaderboard.Count; i++)
        {
            var entry = response.leaderboard[i];
            GameObject newEntry = Instantiate(leaderboardEntryPrefab, leaderboardParent);

            TMP_Text[] textComponents = newEntry.GetComponentsInChildren<TMP_Text>();

            if (textComponents.Length >= 3)
            {
                textComponents[0].text = entry.username;
                textComponents[1].text = entry.userId;
                textComponents[2].text = $"Winnings: {entry._count.winnings}";
            }
        }
    }
}
