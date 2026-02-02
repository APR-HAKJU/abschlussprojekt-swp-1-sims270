using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ilumisoft.Minesweeping.UI
{
    [RequireComponent(typeof(Button))]
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField]
        string sceneName = string.Empty;

        SceneLoader sceneLoader;

        Button button;

        private void Awake()
        {
            sceneLoader = FindAnyObjectByType<SceneLoader>();

            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

private void OnButtonClick()
        {
            // 1. Minesweeper-Objekt deaktivieren
        GameObject minesweeperObject = GameObject.FindGameObjectWithTag("minesweeper");
            if (minesweeperObject != null)
                {
                minesweeperObject.SetActive(false);
                }

            // There are two Main Cameras one for Minesweeper and one for the UI
            // 2. Set the UI Camera as the Main Camera it has tag "MainCamera2"
            var uiCamera = GameObject.FindGameObjectWithTag("Maincamera2");
            if (uiCamera != null)
                {
                //Camera.main.gameObject.tag = "Untagged"; // Remove the MainCamera tag from the current main camera
                uiCamera.tag = "MainCamera"; // Set the UI camera as the main camera
                }

            
        
            
        }
    }
}