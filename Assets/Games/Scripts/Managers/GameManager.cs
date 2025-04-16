using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlaceTower towerPlacer;
    public Button startButton;


    public void StartGame()
    {
        Debug.Log("Game started");
        towerPlacer.LockPlacement();
        startButton.GetComponent<ButtonVisibility>().OnGameStart();
    }
}
