using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScaler : MonoBehaviour
{

    #region Variables
    [SerializeField] private bool enable = true;

    public RectTransform Scaler;
    public Image Cursor;

    public bool CanInteract;
    public float LerpSpeed = 5.0F;
    public Color CameraON, CameraOFF;
    public Vector2 StartSize;
    public Vector2 InteractSize;
    public Vector2 CurrentSize;

    InputManager input;

    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Awake()
    {
        Scaler.sizeDelta = StartSize;
        input = GetComponent<InputManager>();

        GameSettings GameData = FindObjectOfType<GameSettings>();
        if (GameData != null)
        {
            enable = GameData.ShowCrosshair;
        }
        else
        {
            enable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!enable)
        {
            Cursor.color = CameraON;
            return;
        }

        interact();

        Scaler.sizeDelta = CurrentSize;
        if(input.CameraOn)
        {
            Cursor.color = Color.Lerp(Cursor.color,CameraON, LerpSpeed * Time.deltaTime);
        }
        else
        {
            Cursor.color = Color.Lerp(Cursor.color, CameraOFF, LerpSpeed * Time.deltaTime);

        }

        if (CanInteract)
        {
            CurrentSize = Vector2.Lerp(CurrentSize, InteractSize, LerpSpeed * Time.deltaTime);
        }
        else
            CurrentSize = Vector2.Lerp(CurrentSize, StartSize, LerpSpeed * Time.deltaTime);

    }
    #endregion

    #region CustomMethods
    void interact()
    {
        CanInteract = input.CanInteract;
    }
    #endregion

}
