using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap : MonoBehaviour
{
    //starea cubului
    CubeState cubeState;

    //fetele cubului in format 2D
    public Transform up;
    public Transform down;
    public Transform front;
    public Transform back;
    public Transform left;
    public Transform right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //fct pt a seta culorile hartii cubului
    public void Set()
    {
        cubeState = FindObjectOfType<CubeState>();

        //actualizeaza fetele cubului desfasurat
        UpdateMap(cubeState.front, front);
        UpdateMap(cubeState.back, back);
        UpdateMap(cubeState.left, left);
        UpdateMap(cubeState.right, right);
        UpdateMap(cubeState.up, up);
        UpdateMap(cubeState.down, down);
    }

    //actualizeaza culorile fetelor cubului desfasurat 
    //ca sa se potriveasca cubului 3D
    //face e reprezentarea unei fete 3D
    //side e reprezentarea unei fete 2D
    void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;

        //pt fiecare patratel din reprezentarea 2D
        foreach (Transform map in side)
        {
            //ia corespondentul din reprezentarea 3D
            //si vede de ce fata apartine
            //pt a det cum sa-l coloreze
            if (face[i].name[0] == 'F')//front cu portocaliu
            {
                map.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1);
            }
            if (face[i].name[0] == 'B')//back cu rosu
            {
                map.GetComponent<Image>().color = Color.red;
            }
            if (face[i].name[0] == 'U')//up cu galben
            {
                map.GetComponent<Image>().color = Color.yellow;
            }
            if (face[i].name[0] == 'D')//down cu alb
            {
                map.GetComponent<Image>().color = Color.white;
            }
            if (face[i].name[0] == 'L')//left cu verde
            {
                map.GetComponent<Image>().color = Color.green;
            }
            if (face[i].name[0] == 'R')//right cu albastru
            {
                map.GetComponent<Image>().color = Color.blue;
            }

            i++;
        }
    }
}
