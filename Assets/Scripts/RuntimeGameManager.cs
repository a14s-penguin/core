using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RuntimeGameManager : MonoBehaviour
{
    private PlayerController player;
    private Text scoreLabel;
    private Text infoLabel;
    private GameObject winTextObject;
    private int targetScore;
    private bool gameWon;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetupForScene();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerController.ScoreChanged -= OnScoreChanged;
        PlayerController.WinReached -= OnWinReached;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupForScene();
    }

    private void SetupForScene()
    {
        player = FindObjectOfType<PlayerController>();
        winTextObject = GameObject.Find("Wintext");
        if (winTextObject != null)
        {
            winTextObject.SetActive(false);
        }

        if (player != null)
        {
            PlayerController.ScoreChanged -= OnScoreChanged;
            PlayerController.WinReached -= OnWinReached;
            PlayerController.ScoreChanged += OnScoreChanged;
            PlayerController.WinReached += OnWinReached;
            targetScore = player.winScore;
        }

        if (scoreLabel == null || infoLabel == null)
        {
            CreateHud();
        }

        if (player != null)
        {
            UpdateScoreLabel(player.score);
        }
        else
        {
            UpdateScoreLabel(0);
        }

        UpdateInfoLabel("Use arrow keys or WASD to move. Press R to restart.");
        gameWon = false;
    }

    private void CreateHud()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var canvasObject = new GameObject("RuntimeCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        scoreLabel = CreateTextElement("ScoreText", canvas.transform, new Vector2(10, -10), 24);
        infoLabel = CreateTextElement("InfoText", canvas.transform, new Vector2(10, -40), 20);
    }

    private Text CreateTextElement(string name, Transform parent, Vector2 anchoredPosition, int fontSize)
    {
        var textObject = new GameObject(name);
        textObject.transform.SetParent(parent, false);
        var rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(800, 80);

        var text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (text.font == null)
        {
            text.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
        }
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.UpperLeft;
        text.text = string.Empty;
        text.raycastTarget = false;
        return text;
    }

    private void OnScoreChanged(int score)
    {
        UpdateScoreLabel(score);
    }

    private void OnWinReached()
    {
        gameWon = true;
        UpdateInfoLabel("You won! Press R to restart.");
        if (winTextObject != null)
        {
            winTextObject.SetActive(true);
        }
    }

    private void UpdateScoreLabel(int score)
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = $"Score: {score}/{targetScore}";
        }
    }

    private void UpdateInfoLabel(string text)
    {
        if (infoLabel != null)
        {
            infoLabel.text = text;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public static class RuntimeGameManagerInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        if (Object.FindObjectOfType<RuntimeGameManager>() == null)
        {
            var managerObject = new GameObject("RuntimeGameManager");
            managerObject.AddComponent<RuntimeGameManager>();
        }
    }
}
