using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GeneralSolver : MonoBehaviour
{
    public bool isSolving = false;

    //pt a citi starea crt a cubului
    private CubeState cubeState;
    private ReadCube readCube;


    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void SolveCube(string algorithmName)
    {
        //butonul de shuffle
        Button shuffleBtn = GameObject.Find("ShuffleButton").GetComponent<Button>();

        //panoul de rezultate
        ResultsPanelController resultText = FindObjectOfType<ResultsPanelController>();

        //previne rularile multiple in ac timp
        if (isSolving)
        {
            Debug.Log("Another algorithm is already running!");
            return;
        }

        //pune isSolving pe true pt a arata ca deja ruleaza un alg
        isSolving = true;

        //inactiveaza btn de shuffle 
        shuffleBtn.interactable = false;

        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();

        //determina starea de start
        readCube.ReadState();
        string startState = cubeState.GetStateString();

        Debug.Log($"Starting {algorithmName} search...");
        resultText.ShowStartingMessage(algorithmName);

        Stopwatch sw = new Stopwatch();

        sw.Start();

        //ruleaza DFS pe un thread separat
        List<string> solution = await Task.Run(() => ApplyAlgorithm(algorithmName, startState));

        sw.Stop();

        float time = (float)sw.Elapsed.TotalMilliseconds;       

        if (solution != null)
        {
            if (solution.Count == 0)
            {
                Debug.Log("Cube already solved!");
                resultText.ShowCubeAlreadySolvedeResult(algorithmName, time);
                //activeaza btn de shuffle 
                shuffleBtn.interactable = true;
                isSolving = false;
            }
            else
            {
                Debug.Log("Solution found! Moves: " + string.Join(", ", solution));
                resultText.ShowSuccessResult(algorithmName, solution, solution.Count, time);
                Automate.isSolving = true;
                Automate.moveList.Clear();
                Automate.moveList = new List<string>(solution);
            }
        }
        else
        {
            //activeaza btn de shuffle 
            shuffleBtn.interactable = true;
            isSolving = false;

            resultText.ShowFailureResult(algorithmName, time);
            Debug.LogWarning("No solution found!");
        }
    }

    private List<string> ApplyAlgorithm(string algorithmName, string startState)
    {
        switch (algorithmName)
        {
            case "BFS":
                BFSAlg BFS = new BFSAlg();
                return BFS.BidirectionalBFSAlgorithm(startState);
            case "DFS":
                DFSAlg DFS = new DFSAlg();
                return DFS.DFSAlgorithm(startState);
            case "AStar":
                AStarAlg AStar = new AStarAlg();
                return AStar.AStarSearch(startState);
            case "MCTS":
                MCTSAlg MCTS = new MCTSAlg();
                return MCTS.MCTSAlgorithm(startState);
            case "QLearning":
                QLearningAlg QLearning = new QLearningAlg();
                return QLearning.QLearningAlgorithm(startState);
            case "LayerByLayer":
                LayerByLayerAlg LBL = new LayerByLayerAlg();
                return LBL.LayerByLayerAlgorithm(startState);
            default:
                return null;
        }
    }
}