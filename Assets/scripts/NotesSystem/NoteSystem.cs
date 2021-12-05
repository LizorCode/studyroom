using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;



[Serializable()]
public struct UIElements
{
    [SerializeField] TextMeshProUGUI textobj; //хранит текст страницы
    public TextMeshProUGUI Textobj { get { return textobj; } }

    [SerializeField] TextMeshProUGUI subscript;
    public TextMeshProUGUI Subscript { get { return subscript; } } //хранит subscritt текст

    [SerializeField] CanvasGroup subscriptGroup;
    public CanvasGroup SubscriptGroup { get { return subscriptGroup; } }

    [SerializeField] Image page;  //изобр страницы
    public Image Page { get { return page; } }

    [SerializeField] Image lines;  //нужно ли отображать строки
    public Image Lines { get { return lines; } }

    [SerializeField] CanvasGroup noteCanvasGroup;
    public CanvasGroup NoteCanvasGroup { get { return noteCanvasGroup; } }

    [SerializeField] CanvasGroup listCanvasGroup;
    public CanvasGroup ListCanvasGroup { get { return listCanvasGroup; } }

    [SerializeField] CanvasGroup readButton;
    public CanvasGroup ReadButton { get { return readButton; } }

    [SerializeField] CanvasGroup nextButton;
    public CanvasGroup NextButton { get { return nextButton; } }

    [SerializeField] CanvasGroup previousButton;
    public CanvasGroup PreviousButton { get { return previousButton; } }

    [SerializeField] NoteData noteDataPrefab;
    public NoteData NoteDataPrefab { get { return noteDataPrefab; } }

    [SerializeField] RectTransform listRect;
    public RectTransform ListRect { get { return listRect; } }



}
public class NoteSystem : MonoBehaviour
{

    #region Data and Actions
    [SerializeField] UIElements UI = new UIElements();

    [SerializeField] Color color1 = Color.grey;

    [SerializeField] Color color2 = Color.grey;

    private static Dictionary<String, Note> Notes = new Dictionary<String, Note>();
    private List<NoteData> noteDatas = new List<NoteData>();
    private static Action<Note> A_Display = delegate { };

    #endregion

    #region Properties and Private

    private Note activeNote = null;
    private Page ActivePage
    {
        get
        {
            return activeNote.Pages[currentPage];
        }
    }
    private int currentPage = 0;
    private bool readSubscript = false;
    private Sprite defaultPageTexture = null;
    private bool usingNotesSystem;

    #endregion

    #region Unity's defolt methods

    private void OnEnable()
    {
        A_Display += DisplayNote;
    }
    private void OnDisable()
    {
        A_Display -= DisplayNote;
    }
    private void Start()
    {
        Close();
        defaultPageTexture = UI.Page.sprite;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            usingNotesSystem = !usingNotesSystem;
            switch (usingNotesSystem)
            {
                case true:
                    Open();
                    break;
                case false:
                    Close();
                    break;
            }
        }
    }
    #endregion

    public void Open()
    {
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().enableCameraMovement = false;
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = false;
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().autoCrosshair = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UpdateList();
        UpdateCanvasGroup(true, UI.ListCanvasGroup);
    }
    public void Close()
    {
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().enableCameraMovement = true;
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = true;
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().autoCrosshair = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CloseNote();
        UpdateCanvasGroup(false, UI.ListCanvasGroup);
    }

    public void DisplayNote(Note note)
    {
        if (noteDatas == null) { return; }

        UpdateCanvasGroup(true, UI.ListCanvasGroup);
        activeNote = note;

        DisplayPage(0);
    }
    public void DisplayPage(int page)
    {
        UI.ReadButton.interactable = activeNote.Pages[page].Type == PageType.Texture;

        if (activeNote.Pages[page].Type != PageType.Texture)
        { readSubscript = false; } else { if (readSubscript) { UpdateSubscript(); } }

        switch (activeNote.Pages[page].Type)
        {
            case PageType.Text:
                UI.Page.sprite = defaultPageTexture;
                UI.Textobj.text = activeNote.Pages[page].Text;
                break;
            case PageType.Texture:
                UI.Page.sprite = activeNote.Pages[page].Texture;
                UI.Textobj.text = string.Empty;
                break;
        }
        UpdateCanvasGroup(true, UI.NoteCanvasGroup);
        UpdateUI();
    }

    public static void Display(Note note)
    {
        A_Display(note);
    }
    public static void Display(string key)
    {
        var note = GetNote(key);
        A_Display(note);
    }

    public void CloseNote()
    {
        UpdateCanvasGroup(false, UI.NoteCanvasGroup);
        OnNoteClose();
    }

    private void UpdateUI()
    {
        UI.PreviousButton.interactable = !(currentPage == 0);
        UI.NextButton.interactable = !(currentPage == activeNote.Pages.Length - 1);

        var useSubscript = ActivePage.Type == PageType.Texture && ActivePage.UseSubscript;
        UI.ReadButton.alpha = useSubscript ? (readSubscript ? .5f : 1f) : 0f;

        UpdateCanvasGroup(readSubscript, UI.ListCanvasGroup);

        UI.Lines.enabled = ActivePage.DisplayLines;
    }
    private void UpdateList()
    {
        ClearList();

        //четные и нечетные элементы, если четные -color1 , если нечетные color2
        var index = 0;
        var height = 0.0f;
        foreach (var note in Notes)
        {
            var color = index % 2 == 0 ? color1 : color2;

            var newNotePrefab = Instantiate(UI.NoteDataPrefab, UI.ListRect);
            noteDatas.Add(newNotePrefab);

            newNotePrefab.UpdateInfo(note.Value, color);

            newNotePrefab.Rect.anchoredPosition = new Vector2(0, height);
            height -= newNotePrefab.Rect.sizeDelta.y;

            UI.ListRect.sizeDelta = new Vector2(UI.ListRect.sizeDelta.x, height * -1);

            index++;
            //это все нужно для обновления списка
        }
    }
    private void UpdateSubscript()
    {
        UI.Subscript.text = readSubscript ? ActivePage.Text : string.Empty;
    }

    public void Next()
    {
        currentPage++;
        DisplayPage(currentPage);
    }
    public void Previous()
    {
        currentPage--;
        DisplayPage(currentPage);
    }
    public void Read()
    {
        readSubscript = !readSubscript;

        UpdateSubscript();
        UpdateUI();
    }

    private void ClearList()
    {
        foreach (var note in noteDatas)
        {
            Destroy(note.gameObject);
        }
        noteDatas.Clear();
    }
    private void OnNoteClose()
    {
        activeNote = null;
        currentPage = 0;
        readSubscript = false;
    }
    private void UpdateCanvasGroup(bool state, CanvasGroup canvasGroup)
    {
        switch (state)
        {
            case true:
                canvasGroup.alpha = 1.0f;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
                break;
            case false:
                canvasGroup.alpha = 0.0f;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                break;
        }
    }

    public static void AddNote(string key, Note note)
    {
        if (Notes.ContainsKey(key) == false)
        {
            Notes.Add(key, note);
        }
    }
    public static Note GetNote(string key)
    {
        if (Notes.ContainsKey(key))
        {
            return Notes[key];
        }
        return null;
    }

}
