using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    CubeState cubeState;
    ReadCube readCube;

    //rayCast va porni de la mouse spre ecran
    //raza va detecta doar fetele cubului, si va ignora restul
    private int layerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
    }

    // Update is called once per frame
    void Update()
    {
        //daca butonul stang al mouse-ului a fost apasat
        //si daca cubul nu se roteste automat (shuffle)
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating)
        {
            //citeste starea crt a cubului
            readCube.ReadState();

            //-------arunca o raza dinspre mouse catre cub sa vada daca loveste o fata
            //stocheaza detalii despre ob lovite de raza
            RaycastHit hit;
            //raycast de la mouse catre ecran
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //daca raza dinspre mouse loveste vreo fata la dist max de 100f
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                //fata pe care a lovit-o (fata selectata)
                GameObject face = hit.collider.gameObject;

                //mapeaza fiecare fata la pivot
                Dictionary<List<GameObject>, Transform> faceToPivot = new Dictionary<List<GameObject>, Transform>()
                {
                    { cubeState.up, GameObject.Find("UpFacePivot").transform },
                    { cubeState.down, GameObject.Find("DownFacePivot").transform },
                    { cubeState.left, GameObject.Find("LeftFacePivot").transform },
                    { cubeState.right, GameObject.Find("RightFacePivot").transform },
                    { cubeState.front, GameObject.Find("FrontFacePivot").transform },
                    { cubeState.back, GameObject.Find("BackFacePivot").transform },
                };

                foreach (var entry in faceToPivot)
                {
                    List<GameObject> cubeSide = entry.Key;
                    Transform pivot = entry.Value;

                    if (cubeSide.Contains(face))
                    {
                        //ataseaza cubuletele fetei la pivotul ei
                        cubeState.PickUp(cubeSide, pivot);

                        //logica pt rotirea fetei!!!!!!!!!!!!!!!!
                        pivot.GetComponent<PivotRotation>().Rotate(cubeSide, pivot);
                    }
                }
            }
        }
    }
}
