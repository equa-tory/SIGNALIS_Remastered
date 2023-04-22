using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [SerializeField] private Texture2D currentCursorTexture;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D dotTexture;

    private Vector2 cursorHotspot;

    public float maxScopeTimer;
    private float scopeTimer;

    bool scoping;

    private int choosedScope;

    public bool showCursor;
    public bool showDot;

    private void Awake()
    {
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        currentCursorTexture = cursorTexture;
        SetCursor();
    }

    private void Update()
    {
        SetCursorVisible();
    }

    private void SetCursor()
    {
        cursorHotspot = new Vector2(currentCursorTexture.width / 2, currentCursorTexture.height / 2);
        Cursor.SetCursor(currentCursorTexture, cursorHotspot, CursorMode.Auto);
    }

    private void SetCursorVisible()
    {
        if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (showDot)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            currentCursorTexture = dotTexture;
            SetCursor();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
