using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Automate : MonoBehaviour
{
    //lista de miscari
    public static List<string> moveList = new List<string>() { };

    //lista cu toate mutarile posibile
    private readonly List<string> allMoves = new List<string>()
    {
        "U", "D", "L", "R", "F", "B",
        "U2", "D2", "L2", "R2", "F2", "B2",
        "U'", "D'", "L'", "R'", "F'", "B'"
    };

    private CubeState cubeState;
    private ReadCube readCube;

    private GeneralSolver generalSolver;
    private Button shuffleBtn;

    private List<Button> algorithmsBtn = new List<Button>();

    //pt a inactiva/activa btn pt alg in timpul lui shuffle
    private bool isShuffling = false;

    public static bool isSolving = false;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
        generalSolver = FindObjectOfType<GeneralSolver>();
        shuffleBtn = GameObject.Find("ShuffleButton").GetComponent<Button>();

        List<string> algBtnNames = new List<string>()
        {
            "QLearningButton",
            "AStarButton",
            "BFSButton",
            "DFSButton",
            "MCTSButton",
            "LayerByLayerButton"
        };

        //populeaza lista de butoane pt alg
        foreach(string btnName in algBtnNames)
        {
            Button btn = GameObject.Find(btnName).GetComponent<Button>();
            algorithmsBtn.Add(btn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //daca mai sunt mutari in lista,
        //s-a terminat mutarea anterioara
        //si jocul s-a incarcat complet
        if (moveList.Count > 0 && !CubeState.autoRotating && CubeState.started)
        {
            //face prima miscare din lista
            DoMove(moveList[0]);

            //sterge mutarea realizata
            moveList.Remove(moveList[0]);
        }

        //daca s-au terminat de executat mutarile si nu se executa un alg => activeaza btn de shuffle
        if(moveList.Count == 0 && isSolving)
        {
            shuffleBtn.interactable = true;
            generalSolver.isSolving = false;
            isSolving = false;
        }

        //daca s-au terminat mutarile de shuffle=> activeaza btn si pune isShuffling pe false
        if(moveList.Count == 0 && isShuffling)
        {
            isShuffling = false;
            ActivateAlgButtons();
        }
    }

    //fct pt activarea butoanelor de solve
    private void ActivateAlgButtons()
    {
        foreach(Button btn in algorithmsBtn)
        {
            btn.interactable = true;
        }
    }

    //fct pt inactivarea butoanelor de solve
    private void InactivateAlgButtons()
    {
        foreach (Button btn in algorithmsBtn)
        {
            btn.interactable = false;
        }
    }

    //fct care genereaza o secventa de mutari posibile aleatoare
    public void Shuffle()
    {
        //daca deja se face shuffle
        if(isShuffling)
        {
            Debug.Log("The cube is already shuffling");
            return;
        }

        isShuffling = true;

        //inactiveaza butoanele de alg
        InactivateAlgButtons();

        List<string> moves = new List<string>();
        //nr de mutari generat aleator intre 10 si 19 (inclusiv)
        //int shuffleLength = Random.Range(10, 20);

        //7 mutari aleatoare
        int shuffleLength = 7;

        for (int i = 0; i < shuffleLength; i++)
        {
            //nr mutarii aleatoare
            int randomMove = Random.Range(0, allMoves.Count);
            //adauga mutarea aleatoare la lista de mutari aleatoare
            moves.Add(allMoves[randomMove]);
        }

        //salveaza lista de mutari aleatoare in moveList
        moveList = moves;
        Debug.Log("The cube was shuffled with moves: " + string.Join(" ", moves));
    }

    //pt testare, da spre executare o anumita secventa de mutari
    public void Test()
    {
        string cubeState = "UFBLDDFLRBDURLFURULBRFBD";

        BFSAlg bfs = new BFSAlg();

        List<string> moves = new List<string>();

        moves = bfs.BidirectionalBFSAlgorithm(cubeState);

        moveList = moves;
    }

    //fct pt rotirea fetei side cu unghiul angle
    void RotateSide(List<GameObject> side, float angle, Transform facePivot)
    {
        //gaseste componenta PivotRotation si porneste autorotirea
        PivotRotation pr = facePivot.GetComponent<PivotRotation>();

        pr.StartAutoRotate(side, angle, facePivot);
    }

    //fct care face miscarea automata move
    public void DoMove(string move)
    {
        //citeste starea crt a cubului pt a se asigura ca face miscarile pe configuratia actuala
        readCube.ReadState();
        //setam var autoRotate a cubeState pt a semnala autorotirea
        CubeState.autoRotating = true;
        //roteste o fata a cubului pe baza lui move
        if (move == "U")
        {
            RotateSide(cubeState.up, -90, GameObject.Find("UpFacePivot").transform);
        }
        if (move == "U'")
        {
            RotateSide(cubeState.up, 90, GameObject.Find("UpFacePivot").transform);
        }
        if (move == "U2")
        {
            RotateSide(cubeState.up, -180, GameObject.Find("UpFacePivot").transform);
        }
        if (move == "D")
        {
            RotateSide(cubeState.down, -90, GameObject.Find("DownFacePivot").transform);
        }
        if (move == "D'")
        {
            RotateSide(cubeState.down, 90, GameObject.Find("DownFacePivot").transform);
        }
        if (move == "D2")
        {
            RotateSide(cubeState.down, -180, GameObject.Find("DownFacePivot").transform);
        }
        if (move == "L")
        {
            RotateSide(cubeState.left, -90, GameObject.Find("LeftFacePivot").transform);
        }
        if (move == "L'")
        {
            RotateSide(cubeState.left, 90, GameObject.Find("LeftFacePivot").transform);
        }
        if (move == "L2")
        {
            RotateSide(cubeState.left, -180, GameObject.Find("LeftFacePivot").transform);
        }
        if (move == "R")
        {
            RotateSide(cubeState.right, -90, GameObject.Find("RightFacePivot").transform);
        }
        if (move == "R'")
        {
            RotateSide(cubeState.right, 90, GameObject.Find("RightFacePivot").transform);
        }
        if (move == "R2")
        {
            RotateSide(cubeState.right, -180, GameObject.Find("RightFacePivot").transform);
        }
        if (move == "F")
        {
            RotateSide(cubeState.front, -90, GameObject.Find("FrontFacePivot").transform);
        }
        if (move == "F'")
        {
            RotateSide(cubeState.front, 90, GameObject.Find("FrontFacePivot").transform);
        }
        if (move == "F2")
        {
            RotateSide(cubeState.front, -180, GameObject.Find("FrontFacePivot").transform);
        }
        if (move == "B")
        {
            RotateSide(cubeState.back, -90, GameObject.Find("BackFacePivot").transform);
        }
        if (move == "B'")
        {
            RotateSide(cubeState.back, 90, GameObject.Find("BackFacePivot").transform);
        }
        if (move == "B2")
        {
            RotateSide(cubeState.back, -180, GameObject.Find("BackFacePivot").transform);
        }
    }
}
