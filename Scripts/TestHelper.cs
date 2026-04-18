using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Threading.Tasks;

//clasa pt generarea datelor de test si a rezultatelor
//fol pt comparatia intre algoritmi
public class TestHelper : MonoBehaviour
{
    private static System.Random rand = new System.Random();

    //=========================================================================CONSTRUIREA DATELOR DE TEST
    //clasa pt o inregistrare din fisierul de date de test (json in)
    [System.Serializable]
    public class TestDataRecord
    {
        public int id; //id unic pt fiecare inregistrare 
        public int shuffleLength; //lungimea secventei de amestecare determinata cu BFS
        public string shuffledCubeState; //cubul amestecat rezultat
    }

    //clasa ce contine lista tuturor inregistrarilor din json in
    [System.Serializable]
    public class TestDataList
    {
        public List<TestDataRecord> testData = new List<TestDataRecord>();
    }
    public void GenerateTestData()
    {
        //<lungimea sol det de BFS, lista de inregistrari pt acea lungime>
        Dictionary<int, List<TestDataRecord>> testData = new Dictionary<int, List<TestDataRecord>>();

        //initializeaza lista
        for(int i = 1; i <= 9; i++)
        {
            testData[i] = new List<TestDataRecord>();
        }

        BFSAlg bfs = new BFSAlg();

        while(!TestDataComplete(testData))
        {
            //nr de rotiri generat aleator
            int shuffleSequenceLen = rand.Next(1, 20);

            //genereaza secventa de mutari de lungime rand
            List<string> shuffleMoves = GenerateShuffleSequence(shuffleSequenceLen);

            //aplica mutarile si determina cubul amestecat
            string shuffledCube = GetShuffledCube(shuffleMoves);

            //aplica BFS si det sol
            List<string> BFSSol = bfs.BidirectionalBFSAlgorithm(shuffledCube);
            int solLen = BFSSol.Count;

            //daca sol este intre 1 si 9 si mai este loc in lista pt acea lung
            //=> o poate pune in lista de test
            if (solLen >= 1 && solLen <= 9 && testData[solLen].Count < 5 && !IsCubeStateAlreadyInList(testData[solLen], shuffledCube))
            {
                //creeaza inregistrarea cu noile date
                TestDataRecord record = new TestDataRecord()
                {
                    id = (solLen - 1) * 5 + testData[solLen].Count,
                    shuffleLength = solLen,
                    shuffledCubeState = shuffledCube
                };

                //adauga inreg la lista
                testData[solLen].Add(record);
            }
        }

        //creeaza lista finala de teste si adauga testele
        TestDataList testDataList = new TestDataList();
        foreach(var entry in testData)
        {
            testDataList.testData.AddRange(entry.Value);
        }

        //scrie datele de test in fisierul json
        string jsonIN = JsonUtility.ToJson(testDataList, true);

        string pathForTestData = Path.Combine(Application.persistentDataPath, "testData.json");
        File.WriteAllText(pathForTestData, jsonIN);
        Debug.Log("Test generation complete!");
    }

    //verifica daca s-au generat toate datele de test
    private bool TestDataComplete(Dictionary<int, List<TestDataRecord>> testData)
    {
        foreach (int length in testData.Keys)
        {
            if (testData[length].Count < 5)
                return false;
        }
        return true;
    }

    //fct care verifica daca starea cubului este deja in lista
    private bool IsCubeStateAlreadyInList(List<TestDataRecord> testList, string cubeState)
    {
        foreach(TestDataRecord test in testList)
        {
            if(test.shuffledCubeState == cubeState)
            {
                return true;
            }
        }
        return false;
    }

    //fct care genereaza o secventa de mutari pt amestecare de lungime length
    private List<string> GenerateShuffleSequence(int length)
    {
        //ultima mutare din secventa, pt a evita mutari de tip X, X' sau X2 dupa X, X' sau X2
        string lastMove = null;

        //lista de mutari pt amestecare
        List<string> shuffleMoves = new List<string>();

        //pt a obtine mutarile permise pt lastMove
        CubeHandler ch = new CubeHandler();

        for(int l = 0; l < length; l++)
        {
            List<string> allowedMoves = ch.GetAllowedMoves(lastMove);
            string randomMove = allowedMoves[rand.Next(allowedMoves.Count)];
            shuffleMoves.Add(randomMove);
            lastMove = randomMove;
        }

        return shuffleMoves;
    }

    //fct care aplica o secventa de mutari asupra cubului rezultat
    //si returneaza cubul amestecat rezultat sub forma de string
    private string GetShuffledCube(List<string> shuffleSequence)
    {
        //cubul amestecat rezultat (porneste de la cubul rezolvat)
        string shuffledCube = "UUUURRRRFFFFDDDDLLLLBBBB";

        CubeHandler ch = new CubeHandler();

        //aplica secventa de mutari asupra cubului rezolvat
        foreach(string move in shuffleSequence)
        {
            shuffledCube = ch.ApplyMove(shuffledCube, move);
        }

        return shuffledCube;
    }

    //fct care incarca datele de test
    public TestDataList LoadTestData()
    {
        string pathForTestData = Path.Combine(Application.persistentDataPath, "testData.json");
        Debug.Log(pathForTestData);

        if (File.Exists(pathForTestData))
        {
            string json = File.ReadAllText(pathForTestData);
            TestDataList data = JsonUtility.FromJson<TestDataList>(json);
            return data;
        }
        else
        {
            Debug.LogWarning("Test data file not found!");
            return null;
        }
    }


    //=========================================================================GENERAREA REZULTATELOR
    //clasa pt o inregistrare din fisierul cu rezultatele (json out)
    [System.Serializable]
    public class ResultDataRecord
    {
        public int testId; //id-ul datei de test
        public string algorithmName; //numele alg folosit

        //datele cubului de la care am pornit
        public int shuffleLengthByBFS; //lungimea secventei de amestecare det cu BFS
        public string shuffledCubeState; //cubul amestecat rezultat

        //rezultatele
        public float avgExecutionTime; //media timpilor de executie pt cubul crt
        public float avgMemoryUsed; //media memoriei fol pt cubul crt
        public float avgSolutionLength; //media nr de mutari pt cubul crt (se iau in calcul doar succesele)
        public float successRate; //de cate ori s-a gasit sol  pt cubul crt / nr de rulari
    }

    //clasa ce contine lista tuturor inregistrarilor din json out
    [System.Serializable]
    public class ResultDataList
    {
        public List<ResultDataRecord> resultData = new List<ResultDataRecord>();
    }

    //clasa pt o inregistrare din fisierul cu rezultatele (json out)
    [System.Serializable]
    public class GroupedResultDataRecord
    {
        public string algorithmName; //numele alg folosit
        public int shuffleLengthByBFS; //lungimea secventei de amestecare det cu BFS

        //rezultatele
        public float avgExecutionTime; //media timpilor de executie pt cubul crt
        public float avgMemoryUsed; //media memoriei fol pt cubul crt
        public float avgSolutionLength; //media nr de mutari pt cubul crt (se iau in calcul doar succesele)
        public float successRate; //de cate ori s-a gasit sol  pt cubul crt / nr de rulari
    }

    //clasa ce contine lista tuturor inregistrarilor din json out
    [System.Serializable]
    public class GroupedResultDataList
    {
        public List<GroupedResultDataRecord> resultData = new List<GroupedResultDataRecord>();
    }

    //fct care aplica algoritmii asupra datelor de test si incarca rezultatele in json out
    public async void GenerateAlgResults()
    {
        //obtine datele de test din fisier
        TestDataList testRecords = LoadTestData();

        if (testRecords != null)
        {
            int noOfRuns = 4; //de cate ori sa ruleze fiecare alg pt fiecare data de test

            //lista de algoritmi
            List<string> algorithms = new List<string>{ "BFS", "DFS", "AStar", "MCTS", "QLearning", "LayerByLayer" };
            Debug.Log("INCEPEEEEEEEEEEEEEEEEEE");

            foreach (string algorithm in algorithms)
            {
                //lista de rezultate pt algoritmul crt
                ResultDataList resultsList = new ResultDataList();

                //ruleaza fiecare data de test de cate 4 ori si face media rezultatelor
                for (int i = 0; i < testRecords.testData.Count; i++)
                {
                    //starea cubului amestecat luata din fisierul de test
                    string testCubeState = testRecords.testData[i].shuffledCubeState;
                    int shuffleLength = testRecords.testData[i].shuffleLength;

                    float avgTime = 0f; //timpul de executie mediu
                    float avgMemory = 0f; //cantitatea medie de memorie
                    float avgSolLength = 0f; //lungimea medie a solutiei
                    float successes = 0f; // rata succesului de a determina o solutie

                    for (int j = 1; j <= noOfRuns; j++)
                    {
                        GC.Collect(); //curata memoria
                        Stopwatch sw = new Stopwatch();

                        long memStart = GC.GetTotalMemory(true);

                        sw.Start();
                        List<string> result = await Task.Run(() => ApplyAlgorithm(algorithm, testCubeState));
                        sw.Stop();

                        long memEnd = GC.GetTotalMemory(false);

                        //timpul si memoria pt rularea crt
                        float time = (float)sw.Elapsed.TotalMilliseconds;
                        float mem = (memEnd - memStart) / 1024f; //memoria in KB

                        //adauga timpul si mem pt a putea calcula apoi media
                        avgTime += time;
                        avgMemory += mem;

                        //pt cazul in care gaseste o sol, se adauga lungimea sol la medie
                        //si se creste rata succesului
                        if (result != null)
                        {
                            avgSolLength += result.Count;
                            successes++;
                        }

                        Debug.Log($"S-a rulat {j} din testul {i}");
                    }

                    //adauga rezultatul la lista
                    ResultDataRecord resultRecord = new ResultDataRecord
                    {
                        testId = testRecords.testData[i].id,
                        algorithmName = algorithm,
                        shuffledCubeState = testRecords.testData[i].shuffledCubeState,
                        shuffleLengthByBFS = testRecords.testData[i].shuffleLength,
                        avgExecutionTime = avgTime / noOfRuns,
                        avgMemoryUsed = avgMemory / noOfRuns,
                        avgSolutionLength = (successes > 0) ? avgSolLength / successes : 0f,
                        successRate = successes / noOfRuns
                    };

                    resultsList.resultData.Add(resultRecord);

                    Debug.Log($"Test id = {resultRecord.testId}, alg = {resultRecord.algorithmName}, " +
                              $"length = {resultRecord.shuffleLengthByBFS}, avg time = {resultRecord.avgExecutionTime} ms, " +
                              $"avg mem = {resultRecord.avgMemoryUsed} KB, avg sol length = {resultRecord.avgSolutionLength}, success rate = {resultRecord.successRate}");
                }

                //scrie rezultatele in fisierul json out
                string jsonOUT = JsonUtility.ToJson(resultsList, true);

                string pathForResults = Path.Combine(Application.persistentDataPath, algorithm + "Results2.json");
                File.WriteAllText(pathForResults, jsonOUT);

                Debug.Log("GATAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA pentru " + algorithm);

                //grupeaza rezultatele dupa lungimea sol optime
                var groupedByLength = new Dictionary<int, List<ResultDataRecord>>();

                foreach (var result in resultsList.resultData)
                {
                    if (!groupedByLength.ContainsKey(result.shuffleLengthByBFS))
                    {
                        groupedByLength[result.shuffleLengthByBFS] = new List<ResultDataRecord>();
                    }
                    groupedByLength[result.shuffleLengthByBFS].Add(result);
                }

                //rezultatele pt fiecare lungime
                GroupedResultDataList averagedByLength = new GroupedResultDataList();

                foreach (var entry in groupedByLength)
                {
                    int length = entry.Key;
                    var group = entry.Value;

                    float avgTime = 0f, avgMem = 0f, avgSolLen = 0f, avgSuccess = 0f;
                    int count = group.Count;
                    float totalSuccessRate = 0f;

                    foreach (var r in group)
                    {
                        avgTime += r.avgExecutionTime;
                        avgMem += r.avgMemoryUsed;
                        avgSolLen += r.avgSolutionLength * r.successRate;
                        totalSuccessRate += r.successRate;
                        avgSuccess += r.successRate;
                    }

                    averagedByLength.resultData.Add(new GroupedResultDataRecord
                    {
                        algorithmName = algorithm,
                        shuffleLengthByBFS = length,
                        avgExecutionTime = avgTime / count,
                        avgMemoryUsed = avgMem / count,
                        avgSolutionLength = avgSolLen / totalSuccessRate,
                        successRate = avgSuccess / count
                    });
                }

                //scrie datele grupate dupa lungimea sol intr-un fisier json
                string pathForAvgResults = Path.Combine(Application.persistentDataPath, algorithm + "_AvgByLength2.json");
                string avgJson = JsonUtility.ToJson(averagedByLength, true);
                File.WriteAllText(pathForAvgResults, avgJson);
            }

        }
    }

    //fct care aplica alg din parametru si returneaza lista de mutari determinata cu el
    private List<string> ApplyAlgorithm(string algorithmName, string cubeState)
    {
        switch (algorithmName)
        {
            case "BFS":
                BFSAlg BFS = new BFSAlg();
                return BFS.BidirectionalBFSAlgorithm(cubeState);
            case "DFS":
                DFSAlg DFS = new DFSAlg();
                return DFS.DFSAlgorithm(cubeState);
            case "AStar":
                AStarAlg AStar = new AStarAlg();
                return AStar.AStarSearch(cubeState);
            case "MCTS":
                MCTSAlg MCTS = new MCTSAlg();
                return MCTS.MCTSAlgorithm(cubeState);
            case "QLearning":
                QLearningAlg QLearning = new QLearningAlg();
                return QLearning.QLearningAlgorithm(cubeState);
            case "LayerByLayer":
                LayerByLayerAlg LBL = new LayerByLayerAlg();
                return LBL.LayerByLayerAlgorithm(cubeState);
            default:
                return null;
        }
    }

}