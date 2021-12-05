using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickUp : MonoBehaviour, IInteractable
{
    [SerializeField] Note note = null;
    [SerializeField] bool autoDisplay = false;
    [SerializeField] bool add = true;
  public void Interact()
    {
        if (autoDisplay)
        {
            NoteSystem.Display(note);
        }
        if (add)
        {
            NoteSystem.AddNote(note.Label, note);
            Destroy(gameObject);
        }
    }
}
