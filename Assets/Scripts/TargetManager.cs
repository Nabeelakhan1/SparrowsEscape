using System.Collections;
using UnityEngine;
using TMPro;

public class TargetManager : MonoBehaviour
{
    public GameObject[] TargetPrefabs; // Array to hold different target prefabs
    public Transform[] SpawnPoints;
    public Transform MidPoint;
    public float SpawnDelay = 1.0f;
    public GameManager gameManager;
    public int numberOfTargets = 5; // Set this in the Inspector
    public TextMeshProUGUI targetCounterText; // Reference to the TextMeshProUGUI component

    private int targetsSpawned = 0;
    private int targetsHit = 0;

    private void Start()
    {
        targetCounterText.text = $"{numberOfTargets}"; // Initialize counter text
        StartCoroutine(SpawnTargets());
    }

    private IEnumerator SpawnTargets()
    {
        while (targetsSpawned < numberOfTargets)
        {
            SpawnTarget();
            targetsSpawned++;
            Debug.Log($"Target spawned: {targetsSpawned}/{numberOfTargets}");
            yield return new WaitForSeconds(SpawnDelay);
        }
    }

    private void SpawnTarget()
    {
        int spawnIndex = Random.Range(0, SpawnPoints.Length);
        Transform spawnPoint = SpawnPoints[spawnIndex];
        GameObject targetPrefab = TargetPrefabs[spawnIndex]; // Use the corresponding prefab for the spawn point

        GameObject newTarget = Instantiate(targetPrefab, spawnPoint.position, Quaternion.identity);
        Target targetComponent = newTarget.GetComponent<Target>();

        if (targetComponent != null)
        {
            targetComponent.SetMidPoint(MidPoint.position);
            targetComponent.OnTargetReachMidpointEvent += TargetReachedMidpointHandler;
        }

        targetComponent.OnTargetHitEvent += TargetHitHandler;
    }

    private void TargetHitHandler(Target target)
    {
        target.OnTargetHitEvent -= TargetHitHandler;
        Destroy(target.gameObject);

        targetsHit++;
        targetCounterText.text = $"{numberOfTargets - targetsHit}"; // Update counter text
        Debug.Log($"Target hit: {targetsHit}/{numberOfTargets}");
        if (targetsHit >= numberOfTargets)
        {
            Debug.Log("All targets hit, showing level completion panel.");
            gameManager.ShowLevelCompletionPanel();
        }
    }

    private void TargetReachedMidpointHandler(Target target)
    {
        target.OnTargetReachMidpointEvent -= TargetReachedMidpointHandler;
        gameManager.ShowGameOverPanel();
    }
}
