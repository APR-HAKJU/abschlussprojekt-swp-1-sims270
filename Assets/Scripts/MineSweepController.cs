using UnityEngine;

public class MineSweepController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //On M Press open "Game Easy" Scene && distance to Mineseep Start Button Object less than 3
        if (Input.GetKeyDown(KeyCode.M) && Vector3.Distance(GameObject.FindWithTag("Player").transform.position, GameObject.FindWithTag("minesweeper_start_button").transform.position) < 3f)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game Easy");
        }
        // if current Name is "Game Easy" and N is pressed load "Main Menu" Scene
        if (Input.GetKeyDown(KeyCode.N) && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game Easy")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("room2");
        }
    }
}
