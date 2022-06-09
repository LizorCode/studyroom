using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SlideControl : NetworkBehaviour
{

    // public float distanseToSee;
    // RaycastHit whatIHit;

    public int NumSlide;
    public GameObject[] Slides;

    [SyncVar(hook = nameof(SyncSlide))] //задаем метод, который будет выполняться при синхронизации переменной
    int _SyncSlide;
    
    void Update()
    {
        // if (!hasAuthority) //проверяем, есть ли у нас права изменять этот объект
        // {

            if (NumSlide==0)
            {
                NumSlide=3;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (isServer)
                    { //если мы являемся сервером, то переходим к непосредственному изменению переменной
                    SetNumSlide(NumSlide - 1);
                    Debug.Log("Выполнился метод на сервере  "+ NumSlide); 
                    Debug.Log("1: "+ Slides[0].activeSelf + " 2: "+ Slides[1].activeSelf + " 3: " + Slides[2].activeSelf);
                    }
                else
                    {
                    CmdSetNumSlide(NumSlide - 1); 
                    Debug.Log("Выполнился метод на клиенте  " + NumSlide);
                    }
            }
        // }

        for (int i=0; i<Slides.Length; i++)
        {
            Slides[i].SetActive(!(NumSlide-1 < i));         
        }  

    }


    void SyncSlide(int oldValue, int newValue) 
    {
        NumSlide = newValue;
    }

    [Server] //обозначаем, что этот метод будет вызываться и выполняться только на сервере
    public void SetNumSlide(int newValue)
    {
      _SyncSlide = newValue;
    }

    [Command] //обозначаем, что этот метод должен будет выполняться на сервере по запросу клиента
    public void CmdSetNumSlide(int newValue) //обязательно ставим Cmd в начале названия метода
    {
        SetNumSlide(newValue); //переходим к непосредственному изменению переменной
    }
}
