using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    //lista cu cubuletele din fata pe care o rotim
    private List<GameObject> activeSide;

    //in jurul carei axe se roteste fata
    private Vector3 localForward;

    //poz de la care porneste mouse-ul cand e apasat
    private Vector3 mouseRef;

    //daca mouse-ul roteste fata in acest moment
    private bool dragging = false;

    //daca fata se roteste automat
    private bool autoRotating = false;

    //unghiul catre care sa se roteasca automat
    private Quaternion targetQuaternion;

    //referinte la cubul 3d si la harta pt a le actualiza automat dupa rotire
    private ReadCube readCube;
    private CubeState cubeState;

    //cat sa se roteasca fata in fct de cat s-a miscat mouse-ul
    private float sensitivity = 0.4f;

    //cat sa se roteasca fata la fiecare cadru
    private Vector3 rotation;

    //viteza de rotire automata
    private float speed = 300f;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(dragging && !autoRotating)
        {
            SpinSide(activeSide);
            if (Input.GetMouseButtonUp(0))
            {
                //=> det cel mai apropiat unghi de 90 de grade
                dragging = false;
                RotateToRightAngle();
            }
        }

        if(autoRotating)
        {
            AutoRotate();
        }
    }

    //fct care gestioneaza rotirea fetei side pe baza miscarii mouse-ului
    private void SpinSide(List<GameObject> side)
    {
        //reseteaza rotatia
        rotation = Vector3.zero;

        //calculeaza diferenta intre poz finala si cea initiala a mouse-ului
        //ca sa vada cat sa roteasca fata
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);

        //pe baza fetei selectate, se det in jurul carei axe 
        //sa se roteasca fata si in ce directie
        if (side == cubeState.front)
        {
            //se roteste in jurul axei Ox
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * (-1);
        }

        if (side == cubeState.back)
        {
            //se roteste in jurul axei Ox
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity;
        }

        if (side == cubeState.up)
        {
            //se roteste in jurul axei Oy
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity;
        }

        if (side == cubeState.down)
        {
            //se roteste in jurul axei Oy
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * (-1);
        }

        if (side == cubeState.left)
        {
            //se roteste in jurul axei Oz
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity;
        }

        if (side == cubeState.right)
        {
            //se roteste in jurul axei Oz
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * (-1);
        }

        //roteste fata
        transform.Rotate(rotation, Space.Self);

        //actualizeaza poz de start a mouse-ului
        mouseRef = Input.mousePosition;
    }

    //seteaza variabilele la inceputul rotirii manuale a fetei side
    public void Rotate(List<GameObject> side, Transform facePivot)
    {
        //side devine fata activa, adica cea pe care o rotim
        activeSide = side;

        //cat sa rotim fata in fct de mutarea mouse-ului
        mouseRef = Input.mousePosition;

        //fata e trasa de mouse
        dragging = true;

        //vect in jurul caruia sa se roteasca fata pe baza
        //poz locale a piesei pe care o rotim si a pivotului
        localForward = Vector3.zero - facePivot.transform.localPosition;
    }

    //fct care det unghiul tinta pt rotirea automata a fetei 
    //dupa eliberarea mouse-ului
    //nu efectueaza rotirea !!!!
    public void RotateToRightAngle()
    {
        //det cel mai apropiat unghi de 90 de grade unde sa se sfarseasca rotirea
        Vector3 vec = transform.localEulerAngles;
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        //seteaza unghiul de rotire final conform lui vec
        targetQuaternion.eulerAngles = vec;
        //porneste autorotirea in Update()
        autoRotating = true;
    }

    //fct pt rotirea automata a fetei in fiecare cadru
    public void AutoRotate()
    {
        //gradul de rotire pt fiecare cadru
        var step = speed * Time.deltaTime;

        //pivotul se roteste automat cu step
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        //daca diferenta dintre poz crt a fetei si cea tinta este <= 1 => finalizeaza rotatia
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            //aduce fata la unghiul tinta
            transform.localRotation = targetQuaternion;

            //cubuletele nu mai sunt acum copii ai pivotului
            cubeState.PutDown(activeSide, transform.parent);
            //actualizeaza harta pe baza poz finale a fetei
            readCube.ReadState();

            //seteaza var globala de autorotire ca fiind false
            CubeState.autoRotating = false;

            autoRotating = false;
            dragging = false;
        }
    }

    //fct care gestioneaza autorotirea automata a fetei side cu unghiul angle (pt. Automate)
    public void StartAutoRotate(List<GameObject> side, float angle, Transform facePivot)
    {
        //ataseaza cubuletele fetei la pivot pt fata side
        cubeState.PickUp(side, facePivot);

        //axa in jurul careia sa se roteasca
        Vector3 localForward = Vector3.zero - facePivot.transform.localPosition;//2x2

        //starea finala de rotire
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;

        activeSide = side;
        autoRotating = true;
    }
}
