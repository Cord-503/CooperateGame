using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text winerText;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameManager != null && gameManager.canOpenChest && collision.gameObject.CompareTag("Player2"))
        {
            Debug.Log("You Win!");
            ShowVictoryText();
            Destroy(gameObject);
            PauseGame();
        }
    }

    private void ShowVictoryText()
    {
        if (winerText != null)
        {
            winerText.gameObject.SetActive(true); 
        }
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
    }
}
