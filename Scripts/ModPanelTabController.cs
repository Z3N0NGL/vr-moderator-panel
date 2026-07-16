using UnityEngine;

// Simple tab switcher. Assign the 5 tab root objects and 5 tab buttons
// in the Inspector, in the same order. No icons - use plain TextMeshPro
// labels on the buttons ("MOVEMENT", "LOCAL", "PLAYERS", "SESSION", "SETTINGS").

public class ModPanelTabController : MonoBehaviour
{
    [System.Serializable]
    public struct Tab
    {
        public string tabName;
        public GameObject tabRoot;
        public UnityEngine.UI.Button tabButton;
    }

    public Tab[] tabs;
    public int defaultTabIndex = 0;

    private int _activeIndex = -1;

    private void Start()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i; // capture for closure
            if (tabs[i].tabButton != null)
                tabs[i].tabButton.onClick.AddListener(() => ShowTab(index));
        }

        ShowTab(defaultTabIndex);
    }

    public void ShowTab(int index)
    {
        if (index == _activeIndex)
            return;

        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabs[i].tabRoot != null)
                tabs[i].tabRoot.SetActive(i == index);
        }

        _activeIndex = index;
    }
}
