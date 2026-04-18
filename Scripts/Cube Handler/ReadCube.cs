using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    //de unde va fi aruncata raza pt fiecare fata (centrul fiecarei fete)
    public Transform tUp;
    public Transform tDown;
    public Transform tFront;
    public Transform tBack;
    public Transform tLeft;
    public Transform tRight;

    //listele cu razele fiecarei fete
    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();

    //doar pt fetele cubului (doar pt layer8), si ignora restul
    private int layerMask = 1 << 8;

    //referinta la starea cubului 3D
    CubeState cubeState;
    //referinta la starea hartii
    CubeMap cubeMap;

    public GameObject emptyGO;


    // Start is called before the first frame update
    void Start()
    {
        //stabileste raze pt fiecare fata a cubului
        SetRayTransforms();

        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();

        //face o citire initiala a starii cubului !!!!!
        ReadState();
        //si arata ca jocul s-a incarcat complet
        CubeState.started = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //fct care citeste starea fetelor 3D si actualizeaza harta 2D
    public void ReadState()
    {
        //seteaza starea fiecarei fete a.i. sa stim culorile de pe acea fata
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        //actualizeaza harta conform configuratiei cubului 3D
        cubeMap.Set();
    }

    //fct care seteaza cele 4 raze pt fiecare fata a cubului
    void SetRayTransforms()
    {
        //populeaza listele de raze cu raze indreptate spre cub
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }

    //fct care creeaza un grid 2x2 de pct de pornire ale razelor pt o fata
    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        //pt a numi diferit fiecare raza
        int rayCount = 0;
        //lista de raze pt o fata
        List<GameObject> rays = new List<GameObject>();

        //creeaza cele 4 raze sub forma unui grid 2x2 
        for (float y = 0.5f; y >= -0.5f; y--)
        {
            for (float x = -0.5f; x <= 0.5f; x++)
            {

                //calc poz pt punctul din care sa porneasca fiecare raza
                Vector3 startPos = new Vector3(
                    rayTransform.position.x + x,
                    rayTransform.position.y + y,
                    rayTransform.position.z
                );

                //instantiaza un GameObject gol pt poz de start a razei
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);

                //ii da un nume unic razei
                rayStart.name = rayCount.ToString();

                //adauga raza la lista de raze
                rays.Add(rayStart);
                rayCount++;
            }
        }

        //seteaza directia razei parinte a.i. razele copil sa fie trimisein dir corecta, conform fetei de care apartin
        rayTransform.localRotation = Quaternion.Euler(direction);

        return rays;
    }

    //determina si returneaza toate obiectele pe care le detecteaza razele pe o fata
    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        //toate piesele lovite de razele asociate fetei
        List<GameObject> facesHit = new List<GameObject>();

        //arunca cate o raza pornind din punctele rayStart, corespunzatoare unei fete
        foreach (GameObject rayStart in rayStarts)
        {
            //seteaza pct de start al razei
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            //daca se intersecteaza ray cu vreun obiect din layerMask
            //ray = pct de start al razei
            //tFront.right = directia razei 
            //out hit = daca raza loveste ceva, il pastreaza in hit
            //Mathf.Infinity = raza se opreste cand intalneste ceva
            //layerMask se asigura ca doar fetele cubului sunt detectate de raza
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                //deseneaza o linie galbena in editor
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);

                //adauga piesa lovita de raza la facesHit
                facesHit.Add(hit.collider.gameObject);
                //print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }
        return facesHit;
    }
}
