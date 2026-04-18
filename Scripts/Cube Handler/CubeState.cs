using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    //fetele cubului (cubuletele dintr-o fata) sunt retinute sub forma de liste
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    //pt a vedea daca pivotul se roteste automat!!!!!!!!
    public static bool autoRotating = false;

    //pt a ne asigura ca jocul a fost incarcat complet inainte de a incerca mutarile automate!!!!
    public static bool started = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //fct care face toate cubuletele fetei cubeSide copii ai pivotului fetei
    //pt a roti toata fata odata
    public void PickUp(List<GameObject> cubeSide, Transform facePivot)
    {
        //parcurge toate cubuletele fetei si le ataseaza la pivot
        foreach (GameObject piece in cubeSide)
        {
            piece.transform.parent.transform.parent = facePivot;
        }
    }

    //fct care detaseaza toate cubuletele fetei de la pivot
    //dupa efectuarea rotirii
    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        //parcurge toate cubuletele si le dezataseaza de la pivot
        foreach (GameObject littleCube in littleCubes)
        {
            littleCube.transform.parent.transform.parent = pivot;
        }
    }

    //fct care returneaza starea unei fete a cubului sun forma de string???
    public string GetSideString(List<GameObject> side)
    {
        string sideString = "";

        foreach (GameObject face in side)
        {
            sideString += face.name[0].ToString();
        }
        return sideString;
    }

    //returneaza starea intregului cub sub forma de string
    //stringul returnat are fetele in ordinea URFDLB
    public string GetStateString()
    {
        string stateString = "";
        stateString += GetSideString(up);
        stateString += GetSideString(right);
        stateString += GetSideString(front);
        stateString += GetSideString(down);
        stateString += GetSideString(left);
        stateString += GetSideString(back);

        return stateString;
    }

    //afiseaza continutul fetelor
    public void PrintFaces()
    {
        Debug.Log("=================================CUBE 1");
        Debug.Log("Up: " + GetSideString(up));
        Debug.Log("Down: " + GetSideString(down));
        Debug.Log("Front: " + GetSideString(front));
        Debug.Log("Right: " + GetSideString(right));
        Debug.Log("Back: " + GetSideString(back));
        Debug.Log("Left: " + GetSideString(left));
    }
}
