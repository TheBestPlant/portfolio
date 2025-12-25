using UnityEngine;

public class UIToggleManager : MonoBehaviour
{
    public GameObject uiImage;

    private bool isUIOpen = false;

    void Start(){
        HideUI();
    }

    void Update()
    {
        if (isUIOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            HideUI();
        }
    }

    public void ShowUI()
    {
        uiImage.SetActive(true);
        isUIOpen = true;
    }

    public void HideUI()
    {
        uiImage.SetActive(false);
        isUIOpen = false;
    }
}
