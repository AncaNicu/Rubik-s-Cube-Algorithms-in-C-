using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSAlg
{
    //pt operatiile ce se pot efectua asupra starii cubului
    private CubeHandler cubeHandler = new CubeHandler();

    //cubul rezolvat
    private string solvedCube = "UUUURRRRFFFFDDDDLLLLBBBB";

    //clasa pt a retine starea cubului si secventa de mutari conform BFS
    public class CubeStateForBFS
    {
        public string state; //starea cubului
        public List<string> moves; //mutarile care au generat starea

        public CubeStateForBFS(string _state, List<string> _moves = null)
        {
            this.state = _state;
            this.moves = _moves != null ? new List<string>(_moves) : new List<string>();
        }
    }

    public List<string> BidirectionalBFSAlgorithm(string initialState)
    {
        //cub deja rezolvat => ret lista goala
        if (cubeHandler.IsCubeSolved(initialState))
        {
            return new List<string>();
        }

        //coada si starile vizitate pt parcurgerea de la start spre goal
        //adica de la cub amestecat spre cub rezolvat
        Queue<CubeStateForBFS> queueForward = new Queue<CubeStateForBFS>();
        Dictionary<string, List<string>> visitedStatesForward = new Dictionary<string, List<string>>();

        //coada si starile vizitate pt parcurgerea de la goal la start
        //adica de la cub rezolvat spre cub amestecat
        Queue<CubeStateForBFS> queueBackward = new Queue<CubeStateForBFS>();
        Dictionary<string, List<string>> visitedStatesBackward = new Dictionary<string, List<string>>();

        //creeaza starea initiala (cub amestecat), o adauga la coada si o viziteaza
        CubeStateForBFS startState = new CubeStateForBFS(initialState);
        queueForward.Enqueue(startState);
        visitedStatesForward[initialState] = new List<string>();

        //creeaza starea tinta (cub rezolvat), o adauga la coada si o viziteaza
        CubeStateForBFS goalState = new CubeStateForBFS(solvedCube);
        queueBackward.Enqueue(goalState);
        visitedStatesBackward[solvedCube] = new List<string>();

        //cat timp mai sunt elemente in cozi
        while (queueBackward.Count > 0 && queueForward.Count > 0)
        {
            //extinde nivelul crt pt forward si, daca s-a gasit sol, construieste si ret sol
            if (ExpandLevel(queueForward, visitedStatesForward, visitedStatesBackward, "forward", out List<string> pathForward))
                return pathForward;

            //extinde nivelul crt pt backward si, daca s-a gasit sol, construieste si ret sol
            if (ExpandLevel(queueBackward, visitedStatesBackward, visitedStatesForward, "backward", out List<string> pathBackward))
                return pathBackward;
        }
        return null;
    }

    //fct care expandeaza un intreg nivel din arbore folosind BFS
    //queue, visitedThis = coada si starile vizitate pt directia crt
    //visited other = starile vizitate pt cealalta directie
    //path = calea rezultata
    private bool ExpandLevel(Queue<CubeStateForBFS> queue, Dictionary<string, List<string>> visitedThis,
                         Dictionary<string, List<string>> visitedOther, string direction,
                         out List<string> path)
    {
        //determina lungimea crt a cozii, pt ca ea inseamna nr de noduri din nivel
        int levelSize = queue.Count;
        string[] allMoves = { "U2", "U'", "U", "D2", "D'", "D", "L2", "L'", "L", "R2", "R'", "R", "F2", "F'", "F", "B2", "B'", "B" };

        //parcurge toate nodurile din nivelul anterior si le expandeaza pe cele cu o stare nevizitata 
        for (int i = 0; i < levelSize; i++)
        {
            CubeStateForBFS current = queue.Dequeue();

            foreach (string move in allMoves)
            {
                string newState = cubeHandler.ApplyMove(current.state, move);

                if (visitedThis.ContainsKey(newState))
                    continue;

                //mutarile aferente nodului crt sunt mutarile nodului parinte + mutarea care l-a determinat
                List<string> newMoves = new List<string>(current.moves) { move };
                CubeStateForBFS newCubeState = new CubeStateForBFS(newState, newMoves);

                //viziteaza starea crt si o adauga la coada
                visitedThis[newState] = newMoves;
                queue.Enqueue(newCubeState);

                //daca starea crt este deja vizitata de cealalta directie => determina calea si ret true
                if (visitedOther.ContainsKey(newState))
                {
                    if(direction == "forward")
                    {
                        path = BuildPath(newMoves, visitedOther[newState]);
                    }
                    else
                    {
                        path = BuildPath(visitedOther[newState], newMoves);
                    }
                    return true;
                }
            }
        }

        //inca nu s-a gasit o solutie => calea e null si ret false
        path = null;
        return false;
    }

    //fct care determina solutia reunind
    //mutarile de la cub amestecat la rezolvat
    //cu cele de la cub rezolvat la amestecat
    private List<string> BuildPath(List<string> movesForward, List<string> movesBackward)
    {
        //calea
        //mutarile dinspre amestecat spre rezolvat se iau in ordinea in care sunt
        List<string> path = new List<string>(movesForward);

        //mutarile dinspre rezolvat spre amestecat
        //se iau in ordine inversa si inversate
        for (int i = movesBackward.Count - 1; i >= 0; i--)
        {
            path.Add(cubeHandler.GetInverseMove(movesBackward[i]));
        }
        return path;
    }

}
