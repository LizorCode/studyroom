using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "new note", menuName = "Notes System/new Note")]
    public class Note : ScriptableObject
    {
        [SerializeField] string label = string.Empty;
        public string Label { get { return label; } }
        [SerializeField] Page[] pages = new Page[0];  //массив страниц 
        public Page[] Pages { get { return pages; } }
    }
