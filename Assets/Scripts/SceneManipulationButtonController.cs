using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManipulationButtonController : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClickRestartScene);
    }

    private void OnButtonClickRestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
