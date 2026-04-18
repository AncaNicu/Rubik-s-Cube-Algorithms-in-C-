using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSAlg
{
    //pt operatiile ce se pot efectua asupra starii cubului
    private CubeHandler cubeHandler = new CubeHandler();

    //pt amestecarea aleatoare a mutarilor permise
    private static System.Random random = new System.Random();

    //adancimea maxima la care sa se efectueze cautarea
    private int maxDepth = 11;

    //nr maxim de pasi de executat
    private int maxSteps = 3000000;

    //clasa pt a retine starea cubului, secventa de mutari si adancimea conform DFS
    public class CubeStateForDFS
    {
        public string state; //starea cubului
        public List<string> moves; //mutarile care au generat starea
        public int depth; //adancimea
        public CubeStateForDFS(string _state, int _depth, List<string> _moves = null)
        {
            this.state = _state;
            this.depth = _depth;
            this.moves = _moves != null ? new List<string>(_moves) : new List<string>();
        }
    }

    //fct care aplica alg DFS si returneaza secventa de mutari determinata
    public List<string> DFSAlgorithm(string initialState)
    {
        //cub deja rezolvat => ret lista goala
        if (cubeHandler.IsCubeSolved(initialState))
        {
            return new List<string>();
        }

        //stiva si starile vizitate
        Stack<CubeStateForDFS> stack = new Stack<CubeStateForDFS>();
        HashSet<string> visitedStates = new HashSet<string>();

        //creeaza starea initiala, o adauga la stiva si o viziteaza
        CubeStateForDFS startState = new CubeStateForDFS(initialState, 0);
        stack.Push(startState);
        visitedStates.Add(initialState);

        //nr de pasi executati pana in acest moment
        int noOfSteps = 0;

        //cat timp mai sunt elemente in stiva
        while (stack.Count > 0)
        {
            //daca s-a ajuns la nr maxim de pasi => stop, nu s-a gasit solutie in nr max de pasi
            if(noOfSteps == maxSteps)
            {
                //Debug.Log($"Solution not found within {maxSteps} steps.");
                return null;
            }

            //se ia primul elem din stiva
            CubeStateForDFS crt = stack.Pop();

            //mareste nr de pasi
            noOfSteps++;

            //daca starea crt e cubul rezolvat => ret solutia
            if (cubeHandler.IsCubeSolved(crt.state))
            {
                return crt.moves;
            }

            //daca s-a depasit adancimea maxima => stop
            if (crt.depth >= maxDepth)
            {
                continue;
            }

            //determina ultima mutare efectuata
            //daca avem prima stare => lastMove ramane un sir gol
            string lastMove = null;
            if (crt.moves.Count > 0)
            {
                lastMove = crt.moves[crt.moves.Count - 1];
            }

            List<string> allowedMoves = cubeHandler.GetAllowedMoves(lastMove);
            List<string> shuffledAllowedMoves = ShuffleAllowedMoves(allowedMoves);

            //exploreaza starea crt => ii adauga descendentii in stiva
            foreach (string move in shuffledAllowedMoves)
            {
                //creeaza noua stare pt a fi adaugata in stiva
                //aplicand mutarea move asupra configuratiei crt
                string newStateString = cubeHandler.ApplyMove(crt.state, move); //noua stare sub forma de string

                //doar daca starea noua nu a fost deja vizitata => se adauga in stiva
                if (!visitedStates.Contains(newStateString))
                {
                    //noua secventa de mutari este vechea secventa, la care se adauga move
                    List<string> newSequenceOfMoves = new List<string>(crt.moves) { move };
                    int newDepth = crt.depth + 1;

                    //noua stare pe baza datelor de mai sus
                    CubeStateForDFS newState = new CubeStateForDFS(newStateString, newDepth, newSequenceOfMoves);
                    stack.Push(newState);
                }
            }

        }
        return null;
    }

    //fct care amesteca aleator mutarile permise
    private List<string> ShuffleAllowedMoves(List<string> moves)
    {
        List<string> movesCopy = new List<string>(moves);
        List<string> shuffledMoves = new List<string>();

        while(movesCopy.Count > 1)
        {
            int randomIndex = random.Next(movesCopy.Count);
            shuffledMoves.Add(movesCopy[randomIndex]);
            movesCopy.RemoveAt(randomIndex);
        }
        shuffledMoves.Add(movesCopy[0]);
        return shuffledMoves;
    }
}
