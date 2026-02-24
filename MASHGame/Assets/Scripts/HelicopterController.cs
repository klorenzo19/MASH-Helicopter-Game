using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    // ---- Settings you can change in the Unity Inspector ----
    public float moveSpeed = 5f;          // How fast the helicopter moves
    public int maxSoldiers = 3;           // Max soldiers allowed on board

    // ---- Private variables ----
    private int soldiersOnBoard = 0;      // Current soldiers on helicopter
    private Rigidbody2D rb;               // Reference to physics component
    private GameManager gameManager;      // Reference to the GameManager

    // Start() runs once when the game begins
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update() runs every single frame
    void Update()
    {
        // Don't allow movement if the game is over
        if (gameManager != null && gameManager.isGameOver) return;

        // Read arrow key input (-1, 0, or 1)
        float moveX = Input.GetAxis("Horizontal");   // Left/Right arrow keys
        float moveY = Input.GetAxis("Vertical");     // Up/Down arrow keys

        // Apply movement to the Rigidbody
        rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);

        // Press R to reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameManager.ResetGame();
        }
    }

    // OnTriggerEnter2D() fires when this object touches a trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // --- HIT A SOLDIER ---
        if (other.CompareTag("Soldier"))
        {
            if (soldiersOnBoard < maxSoldiers)
            {
                soldiersOnBoard++;
                // Tell the GameManager we picked someone up
                gameManager.PickUpSoldier(other.gameObject);
                // Hide the soldier (they're now on the helicopter)
                other.gameObject.SetActive(false);
            }
        }

        // --- REACHED THE HOSPITAL ---
        if (other.CompareTag("Hospital"))
        {
            if (soldiersOnBoard > 0)
            {
                // Tell the GameManager how many we delivered
                gameManager.DropOffSoldiers(soldiersOnBoard);
                soldiersOnBoard = 0;
            }
        }

        // --- HIT A TREE ---
        if (other.CompareTag("Tree"))
        {
            gameManager.GameOver();
        }
    }

    // Public getter so UI or GameManager can read soldier count
    public int GetSoldiersOnBoard()
    {
        return soldiersOnBoard;
    }

    // Reset helicopter state
    public void ResetHelicopter()
    {
        soldiersOnBoard = 0;
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
    }
}