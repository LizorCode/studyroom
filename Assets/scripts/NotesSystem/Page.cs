using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PageType { Text, Texture }  //перечисление необходимых структур

[CreateAssetMenu(fileName = "new page", menuName = "Notes System/new Page")]    //создать атрибут контекстного меню
public class Page : ScriptableObject    //класс, позволяющий хранить большой объем данных
{
    [SerializeField] PageType type = PageType.Text;     //тип страницы
    public PageType Type { get { return type; } }

    [TextArea(8, 16)]
    [SerializeField] string text = string.Empty;    //текст страницы
    public string Text { get { return text; } }

    [SerializeField] Sprite texture = null;     //изображение страницы
    public Sprite Texture { get { return texture; } }

    [SerializeField] bool useSubscript = true;      //активация поясняющего текста
    public bool UseSubscript { get { return useSubscript; } }

    [SerializeField] bool displayLines = true;      //нужно ли отображать линии
    public bool DisplayLines { get { return displayLines; } }
}

