using UnityEngine;

public class LoadLevelButtonBridge : MonoBehaviour
{
    public LoadLevelAction loadLevelAction;

    public void LoadLevel()
    {
        if (loadLevelAction != null)
        {
            loadLevelAction.ExecuteAction(gameObject);
        }
    }
}
