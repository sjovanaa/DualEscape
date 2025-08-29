using UnityEngine;
using UnityEngine.UI;
public class RulesUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject mainMenuPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Start()
    {
        Hide();
    }

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Hide();
            mainMenuPanel.SetActive(true);
        });
    }

}
