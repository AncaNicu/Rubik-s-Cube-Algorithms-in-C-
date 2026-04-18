using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBigCube : MonoBehaviour
{
    //pozitia de la care se porneste
    Vector2 firstPressPos;
    //pozitia la care se ajunge in urma rotirii
    Vector2 secondPressPos;
    //directia de rotire
    Vector2 currentSwipe;

    Vector3 previousMousePosition;
    Vector3 mouseDelta; //dif dintre poz crt a mouse-ului si poz anterioara

    //viteza de rotire
    float speed = 200f;

    public GameObject target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Swipe();

        Drag();
    }

    //fct pt gestionarea rotirii in fct de apasarea mouse-ului
    //tine cont de cat s-a mutat mouse-ul cat timp a fost apasat
    void Drag()
    {
        //daca butonul drept a fost apasat (continuu!!!)
        if (Input.GetMouseButton(1))
        {
            //cat timp mouse ul e apasat, cubul poate fi rotit in jurul axei sale
            mouseDelta = Input.mousePosition - previousMousePosition;

            mouseDelta *= 0.1f; //pt a reduce viteza de rotatie a cubului

            //gradul de rotire al cubului e controlat de cat de mult s-a miscat mouse-ul cat timp a fost apasat
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;

        }
        //la eliberarea butonului => se roteste catre target
        else
        {
            //atunci cand se face swipe, cubul se va roti cu o anumita viteza automat catre target
            if (transform.rotation != target.transform.rotation)
            {
                var step = speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
            }
        }

        //actualizeaza pozitia cubului la fiecare cadru
        //indiferent de apasarea mouse-ului
        previousMousePosition = Input.mousePosition;
    }

    //fct pt gestionarea rotirii - discret
    void Swipe()
    {
        //daca mouse dreapta a fost apasat
        //=> se retine pozitia de la care se porneste in firstPressPos
        if (Input.GetMouseButtonDown(1))
        {
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        //daca mouse dreapta a fost eliberat
        if (Input.GetMouseButtonUp(1))
        {
            //obtine pozitia finala a mouse-ului
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //creeaza un vector ca distanta dintre poz init si cea finala
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalizeaza currentSwipe pt a tine cont doar de directia de swipe, nu si de magnitudinea swipe-ului
            currentSwipe.Normalize();

            //daca e rotire la stanga => se roteste pe axa Oy cu 90 de grade
            if (LeftSwipe(currentSwipe))
            {
                target.transform.RotateAround(target.transform.position, Vector3.up, 90);
            }
            else
            {
                //daca e rotire la dreapta => se roteste pe axa Oy cu -90 de grade
                if (RightSwipe(currentSwipe))
                {
                    target.transform.RotateAround(target.transform.position, Vector3.up, -90);
                }
                else
                {
                    if (UpLeftSwipe(currentSwipe))
                    {
                        target.transform.RotateAround(target.transform.position, Vector3.right, 90);
                    }
                    else
                    {
                        if (UpRightSwipe(currentSwipe))
                        {
                            target.transform.RotateAround(target.transform.position, Vector3.forward, -90);
                        }
                        else
                        {
                            if (DownLeftSwipe(currentSwipe))
                            {
                                target.transform.RotateAround(target.transform.position, Vector3.forward, 90);
                            }
                            else
                            {
                                if (DownRightSwipe(currentSwipe))
                                {
                                    target.transform.RotateAround(target.transform.position, Vector3.right, -90);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //pt a determina tipul de rotire

    //fct care determina daca s-a facut swipe la stanga cu mouse ul sau nu
    bool LeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    //fct care determina daca s-a facut swipe la dreapta cu mouse ul sau nu
    bool RightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    //fct care determina daca s-a facut swipe spre stanga sus cu mouse ul sau nu
    bool UpLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x < 0f;
    }

    //fct care determina daca s-a facut swipe spre dreapta sus cu mouse ul sau nu
    bool UpRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x > 0f;
    }

    //fct care determina daca s-a facut swipe spre stanga jos cu mouse ul sau nu
    bool DownLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x < 0f;
    }

    //fct care determina daca s-a facut swipe spre dreapta jos cu mouse ul sau nu
    bool DownRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x > 0f;
    }
}
