using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// [CreateAssetMenu(fileName = "new note", menuName = "Notes System/new Note")]    //создать атрибут контекстного меню
public class NoteData : MonoBehaviour
{

    [SerializeField] Image bgImage = null;  //изображение страницы
    [SerializeField] TextMeshProUGUI label = null;      //ссылка на элемент пользовательского интерфейса, куда будет выведен текст

    private Note note = null;
    private RectTransform rect = null;
    public RectTransform Rect
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>();   //получить компонент, если rect = 0
                if (rect == null) { rect = gameObject.AddComponent<RectTransform>(); }      //добавить объекту компонент RectTransform
            }
            return rect;
        }
    }

    public void UpdateInfo(Note note, Color color)  //обновляет информацию в этом классе
    {
        this.note = note;

        label.text = note.Label;
        bgImage.color = color;

    }
    public void Display()       //показать лекцию
    {
        NoteSystem.Display(note);
    }

}
