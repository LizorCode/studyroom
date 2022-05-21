using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "new note", menuName = "Notes System/new Note")]    //создать атрибут контекстного меню
    public class Note : ScriptableObject    //класс, позволяющий хранить большой объем данных
    {
        [SerializeField] string label = string.Empty;   //создание поля для ввода тэга на объект в инспекторе
        public string Label { get { return label; } }
        [SerializeField] Page[] pages = new Page[0];  //создание поля для ввода элементов массива на объект в инспекторе
        public Page[] Pages { get { return pages; } }
    }
