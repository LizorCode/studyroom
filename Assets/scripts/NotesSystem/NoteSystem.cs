using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

//этот скрипт включает в себя описание элементов пользлвательского интерфейса, их связь с данными

[Serializable()]
public struct UIElements    //элементы пользовательского интерфейса
{
    [SerializeField] TextMeshProUGUI textobj; //объект, куда добавится текст страницы
    public TextMeshProUGUI Textobj { get { return textobj; } }

    [SerializeField] TextMeshProUGUI subscript; //объект, куда добавится поясняющий текст
    public TextMeshProUGUI Subscript { get { return subscript; } } 

    [SerializeField] CanvasGroup subscriptGroup;    //показать поясняющий текст
    public CanvasGroup SubscriptGroup { get { return subscriptGroup; } }

    [SerializeField] Image page;  //изображение страницы (основа без текста)
    public Image Page { get { return page; } }

    [SerializeField] Image lines;  //активировать линии
    public Image Lines { get { return lines; } }

    //переменные связанные с элементом пользовательского элемента Canvas (делает элементы Note и List выдимыми)
    [SerializeField] CanvasGroup noteCanvasGroup;
    public CanvasGroup NoteCanvasGroup { get { return noteCanvasGroup; } }

    [SerializeField] CanvasGroup listCanvasGroup;
    public CanvasGroup ListCanvasGroup { get { return listCanvasGroup; } }

    //переменные связанные с элементом пользовательского элемента Canvas (делает элементы различные кнопки выдимыми)
    [SerializeField] CanvasGroup readButton;    //кнопка Read
    public CanvasGroup ReadButton { get { return readButton; } }

    [SerializeField] CanvasGroup nextButton;    //кнопка Next
    public CanvasGroup NextButton { get { return nextButton; } }

    [SerializeField] CanvasGroup previousButton;    //кнопка Previous
    public CanvasGroup PreviousButton { get { return previousButton; } }

    [SerializeField] NoteData noteDataPrefab;   //ссылка на префаб с данными
    public NoteData NoteDataPrefab { get { return noteDataPrefab; } }

    [SerializeField] RectTransform listRect;    //обновление высоты списка
    public RectTransform ListRect { get { return listRect; } }



}
public class NoteSystem : MonoBehaviour     //класс, в котором описаны основные функции для работы интерфейса
{

    #region Data and Actions
    [SerializeField] UIElements UI = new UIElements();  //массив, хранящий элементы пользовательского интерфейса

    //используемые цвета, для создания элемента списка
    [SerializeField] Color color1 = Color.grey;

    [SerializeField] Color color2 = Color.grey;

    private static Dictionary<String, Note> Notes = new Dictionary<String, Note>();     //хранит в себе элемент лекции
    private List<NoteData> noteDatas = new List<NoteData>();    //хранит в себе данные лекции
    private static Action<Note> A_Display = delegate { };  //делегат метода отображения лекции

    #endregion

    #region Properties and Private
    //переменные активации
    private Note activeNote = null;
    private Page ActivePage
    {
        get
        {
           return activeNote.Pages[currentPage];   //сделать активной текущую
        }
    }

    //инициализация
    private int currentPage = 0;        //номер текущей страницы
    private bool readSubscript = false;     //просматриваем ли мы вспомогательный текст
    private Sprite defaultPageTexture = null;   //изобрадение страницы по умолчанию
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
    private void Start()    //метод инициализации, выполняется по умолчанию
    {
        Close();
        defaultPageTexture = UI.Page.sprite;
    }
    private void Update()   //включение меню лекций по нажатию клавиши
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

    public void Open()      //метод отключающий управление персонажем, если активировано меню просмотра лекций
    {
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().enableCameraMovement = false; //отключить камеру
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = false; //отключить движение
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().autoCrosshair = false; //отключить прицел

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UpdateList();
        UpdateCanvasGroup(true, UI.ListCanvasGroup);   //обновить пользовательский интерфейс
    }
    public void Close()         //метод включающий управление персонажем, если деактивировано меню просмотра лекций
    {
        // GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().enableCameraMovement = true;  //включить камеру
        // GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = true;  //включить движение
        // GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().autoCrosshair = true;  //включить прицел
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CloseNote();
        UpdateCanvasGroup(false, UI.ListCanvasGroup);   //обновить пользовательский интерфейс
    }

    public void DisplayNote(Note note)  //показать название лекции в списке
    {
        if (noteDatas == null) { return; }

        UpdateCanvasGroup(true, UI.ListCanvasGroup);    //обновить пользовательский интерфейс
        activeNote = note;

        DisplayPage(0);     //показывать 1 страницу лекции
    }
    public void DisplayPage(int page)   //показать страницу
    {
        UI.ReadButton.interactable = activeNote.Pages[page].Type == PageType.Texture;   //кнопка чтения включена, если тип равен текстуре

        if (activeNote.Pages[page].Type != PageType.Texture)
        { readSubscript = false; } else { if (readSubscript) { UpdateSubscript(); } }   //если тип не текстура, то можно прочитать вспомогательный текст

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
        UpdateCanvasGroup(true, UI.NoteCanvasGroup);    //обновить интерфейс
        UpdateUI();     //обновить переключение страниц
    }

    public static void Display(Note note) 
    {
        A_Display(note);    //вывести на экран в список лекций название
    }
    public static void Display(string key)
    {
        var note = GetNote(key);    //получить данные
        A_Display(note);        //вывести на экран в список лекций название
    }

    public void CloseNote() //закрыть лекцию
    {
        UpdateCanvasGroup(false, UI.NoteCanvasGroup);
        OnNoteClose();
    }

    private void UpdateUI()     //переключение между страницами лекции
    {
        UI.PreviousButton.interactable = !(currentPage == 0);   //листаем назад
        UI.NextButton.interactable = !(currentPage == activeNote.Pages.Length - 1);   //листаем вперед

        var useSubscript = ActivePage.Type == PageType.Texture && ActivePage.UseSubscript;
        UI.ReadButton.alpha = useSubscript ? (readSubscript ? .5f : 1f) : 0f;

        UpdateCanvasGroup(readSubscript, UI.ListCanvasGroup);   //обновить интерфейс

        UI.Lines.enabled = ActivePage.DisplayLines;     //выводить разлиновынный текст
    }
    private void UpdateList()   //обновить список лекций
    {
        ClearList();

        var index = 0;
        var height = 0.0f;
        foreach (var note in Notes)
        {
            var color = index % 2 == 0 ? color1 : color2;   //четные лекции будут принимать первый цвет, нечетные - второй

            var newNotePrefab = Instantiate(UI.NoteDataPrefab, UI.ListRect);     //сгенерировать префаб
            noteDatas.Add(newNotePrefab);       //добавить в список

            newNotePrefab.UpdateInfo(note.Value, color);

            newNotePrefab.Rect.anchoredPosition = new Vector2(0, height);     //координаты, где будет высвечиваться лекция
            height -= newNotePrefab.Rect.sizeDelta.y;

            UI.ListRect.sizeDelta = new Vector2(UI.ListRect.sizeDelta.x, height * -1);

            index++;
        }
    }
    private void UpdateSubscript()      //обновить вспомогательный текст
    {
        UI.Subscript.text = readSubscript ? ActivePage.Text : string.Empty;
    }

    public void Next()      //листать страницы вперед
    {
        currentPage++;
        DisplayPage(currentPage);
    }
    public void Previous()     //листать страницы назад
    {
        currentPage--;
        DisplayPage(currentPage);
    }
    public void Read()     //читать вспомогательный текст
    {
        readSubscript = !readSubscript;

        UpdateSubscript();
        UpdateUI();     //обновить переключение страниц
    }

    private void ClearList()    //удалить объект и очистить список
    {
        foreach (var note in noteDatas)
        {
            Destroy(note.gameObject);
        }
        noteDatas.Clear();
    }
    private void OnNoteClose()  //мтод при закрытии лекции
    {
        activeNote = null;
        currentPage = 0;    //текущая страница 0
        readSubscript = false;
        if (!usingNotesSystem)
        {
            //SwitchGameControls(true);
        }
    }
    
    private void UpdateCanvasGroup(bool state, CanvasGroup canvasGroup)     //обновить группу UI элементов
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
