using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/MainMenu")]
    public static void ShowExample()
    {
        MainMenu wnd = GetWindow<MainMenu>();
        wnd.titleContent = new GUIContent("MainMenu");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        root.Clear();

        if (m_VisualTreeAsset == null)
        {
            m_VisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Editor/MainMenu.uxml"
            );
        }

        if (m_VisualTreeAsset == null)
        {
            root.Add(new Label("MainMenu.uxml missing. Check Assets/Editor/MainMenu.uxml"));
            return;
        }

        VisualElement menu = m_VisualTreeAsset.Instantiate();
        root.Add(menu);

        BindButtons(menu);
    }

    private void BindButtons(VisualElement root)
    {
        BindButton(root, "play-button", "Play pressed");
        BindButton(root, "quick-button", "Quick Match pressed");
        BindButton(root, "tourney-button", "Tournament pressed");
        BindButton(root, "options-button", "Options pressed");
        BindButton(root, "quit-button", "Quit pressed");
    }

    private void BindButton(VisualElement root, string name, string message)
    {
        Button button = root.Q<Button>(name);
        if (button == null)
        {
            return;
        }

        button.clicked += () => Debug.Log(message);
    }
}
