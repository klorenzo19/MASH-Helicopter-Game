using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshPro
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ---- UI References (drag these in the Inspector) ----
    public TextMeshProUGUI soldiersInHelicopterText;
    public TextMeshProUGUI soldiersRescuedText;
    public TextMeshProUGUI totalSoldiersText;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;

    // ---- Sound ----
    public AudioSource pickupSound;

    // ---- Private tracking ----
    private int totalSoldiers;
    private int soldiersRescued = 0;
    private int soldiersOnBoard = 0;
    public bool isGameOver = false;

    // All soldier GameObjects in the scene
    private GameObject[] allSoldiers;

    void Start()
    {
        // Find all soldiers in the scene at game start
        allSoldiers = GameObject.FindGameObjectsWithTag("Soldier");
        totalSoldiers = allSoldiers.Length;

        // Hide panels at start
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (youWinPanel != null) youWinPanel.SetActive(false);

        UpdateUI();
    }

    // Called when helicopter picks up a soldier
    public void PickUpSoldier(GameObject soldierObj)
    {
        soldiersOnBoard++;

        // Play pickup sound
        if (pickupSound != null) pickupSound.Play();

        UpdateUI();
    }

    // Called when helicopter reaches the hospital
    public void DropOffSoldiers(int count)
    {
        soldiersRescued += count;
        soldiersOnBoard = 0;
        UpdateUI();
        CheckWinCondition();
    }

    // Check if all soldiers have been rescued
    void CheckWinCondition()
    {
        if (soldiersRescued >= totalSoldiers)
        {
            isGameOver = true;
            if (youWinPanel != null) youWinPanel.SetActive(true);
        }
    }

    // Called when helicopter hits a tree
    public void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // Reset the entire game
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update all the UI text
    void UpdateUI()
    {
        if (soldiersInHelicopterText != null)
            soldiersInHelicopterText.text = "In Helicopter: " + soldiersOnBoard + "/3";

        if (soldiersRescuedText != null)
            soldiersRescuedText.text = "Rescued: " + soldiersRescued;

        if (totalSoldiersText != null)
            totalSoldiersText.text = "Total Soldiers: " + totalSoldiers;
    }
}