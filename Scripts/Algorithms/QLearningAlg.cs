using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QLearningAlg
{

    //Q-table cu (stare, <mutarea aplicata, Q-value>)
    private Dictionary<string, Dictionary<string, double>> QTable = new Dictionary<string, Dictionary<string, double>>();
    private System.Random random = new System.Random();
    private CubeHandler cubeHandler = new CubeHandler();

    private double alpha = 0.2; // Learning rate
    private double gamma = 0.9; // Discount factor
    private double epsilon = 1; // Exploration rate

    //pt a scadea exponential epsilon pe parcursul episoadelor
    private double minEpsilon = 0.05;
    private double decayRate = 0.00004;

    //adancimea maxima = 30 pt a descuraja solutiile lungi
    private int max_depth = 30;

    //nr de epidoade (un episod = pana cand se ajunge intr-o stare terminala)
    private int noOfEpisodes = 50000;

    //toate actiunile posibile
    private List<string> allActions = new List<string> { "U2", "U'", "U", "D2", "D'", "D", "L2", "L'", "L", "R2", "R'", "R", "F2", "F'", "F", "B2", "B'", "B" };

    //fct pt antrenarea agentului 
    //pornind de la cubul in startState
    private void Train(string startState)
    {
        for (int episode = 0; episode < noOfEpisodes; episode++)
        {
            //episodul porneste cu starea de start si actiunile pe null
            string state = startState;
            //ultima mutare efectuata
            string lastAction = null;
            for (int step = 0; step < max_depth; step++)
            {
                //alege o actiune pe baza politicii epsilon-greedy
                string action = ChooseAction(state, lastAction);

                //determina starea rezultat in urma aplicarii lui action
                string newState = cubeHandler.ApplyMove(state, action);

                //determina recompensa
                int heuristic_before = cubeHandler.DetermineHValue(state);
                int heuristic_after = cubeHandler.DetermineHValue(newState);

                //double reward = heuristic_before - heuristic_after;

                double reward;

                //daca prin mutarea action, s-a rez cubul => reward = 100
                if (heuristic_after == 0)
                {
                    reward = 100;
                }
                else
                {
                    reward = heuristic_before - heuristic_after;
                }               

                //actualizeaza Q-val pt starea state si actiunea action
                UpdateQTable(state, action, newState, reward);

                //trece la starea rezultata
                state = newState;

                //determina noua ultima mutare
                lastAction = action;

                //termina episodul daca am ajuns la cubul rezolvat
                if (heuristic_after == 0)
                {
                    break;
                }
            }

            //scade epsilon exponential
            epsilon = minEpsilon + (1.0 - minEpsilon) * Math.Exp(-decayRate * episode);
        }
    }

    //alege o actiune pt starea state, tinand cont de politica epsilon-greedy
    private string ChooseAction(string state, string lastAction)
    {
        //daca starea state nu este inca in tabel
        //=> creeaza inregistrarea in tabel
        // si initializeaza Q-values pt toate actiunile cu 0
        if (!QTable.ContainsKey(state))
        {
            QTable[state] = InitializeActionValues();
        }

        //determina mutarile valide 
        //List<string> allowedActions = cubeHandler.GetAllowedMoves(lastAction);
        List<string> allowedActions = cubeHandler.GetMovesWithoutInverses(lastAction);

        //Explorare <=> alege aleator o actiune
        if (random.NextDouble() < epsilon)
        {
            return allowedActions[random.Next(allowedActions.Count)];
        }
        //Exploatare <=> alege actiunea care are cea mai mare Q-val
        else
        {
            return GetBestActionFromState(state, allowedActions);
        }
    }

    //creeaza si returneaza un dictionar <mutare/actiune, Q-value>
    //in care Q-value este 0 pt toate actiunile
    //fol in cazul in care o stare nu este inca in Q-table
    private Dictionary<string, double> InitializeActionValues()
    {
        Dictionary<string, double> actions = new Dictionary<string, double>();
        foreach (string action in allActions)
        {
            actions[action] = 0.0;
        }
        return actions;
    }

    //fct care actualizeaza Q-Table folosind ecuatia Temporal Difference (TD)
    private void UpdateQTable(string state, string action, string newState, double reward)
    {
        //daca nu exista inregistrare in Q-table pt state sau newState
        //o creeaza si o initializeaza
        if (!QTable.ContainsKey(state))
        {
            QTable[state] = InitializeActionValues();
        }
        if (!QTable.ContainsKey(newState))
        {
            QTable[newState] = InitializeActionValues();
        }

        //determina c.m. mare Q-val pt newState
        double maxFutureQVal = double.MinValue;
        foreach (double qVal in QTable[newState].Values)
        {
            if (qVal > maxFutureQVal)
            {
                maxFutureQVal = qVal;
            }
        }

        //vechea Q-val pt starea state si actiunea action
        double oldQVal = QTable[state][action];

        //determina noua Q-val pt starea state si actiunea action folosind TD
        double newQVal = oldQVal + alpha * (reward + gamma * maxFutureQVal - oldQVal);

        //actualizeaza Q-val in tabel
        QTable[state][action] = newQVal;
    }

    //fct care antreneaza modelul, construieste Q-table
    //si folosind valorile din Q-table, determina o solutie
    public List<string> QLearningAlgorithm(string startState)
    {
        //cub deja rezolvat => ret lista goala
        if (cubeHandler.IsCubeSolved(startState))
        {
            return new List<string>();
        }

        //secventa de mutari rezultata
        List<string> moves = new List<string>();

        //antreneaza modelul
        Train(startState);

        string state = startState;

        int noOfMovesApplied = 0;

        //ultima mutare
        string lastAction = null;

        //starile vizitate
        HashSet<string> visitedStates = new HashSet<string>();
        visitedStates.Add(startState);

        while (!cubeHandler.IsCubeSolved(state) && noOfMovesApplied < max_depth)
        {
            //determina mutarile valide 
            //List<string> allowedActions = GetAllowedMoves(state, cubeHandler.GetAllowedMoves(lastAction), visitedStates);
            List<string> allowedActions = GetAllowedMoves(state, cubeHandler.GetMovesWithoutInverses(lastAction), visitedStates);

            //daca toate starile sunt vizitate deja => returneaza null
            if (allowedActions.Count == 0)
            {
                return null;
            }

            //determina c.m. buna actiune pt starea state
            string bestAction = GetBestActionFromState(state, allowedActions);

            //adauga mutarea la lista de mutari
            moves.Add(bestAction);

            //aplica mutarea
            string newState = cubeHandler.ApplyMove(state, bestAction);

            //pt a evita posibilitatea ca o stare sa nu fie in dictionar
            while (!QTable.ContainsKey(newState) && allowedActions.Count > 0)
            {
                //daca newState nu este explorata => alege urmatoarea c.m. buna actiune si o executa
                allowedActions.Remove(bestAction);
                if (allowedActions.Count == 0)
                {
                    return null;
                }
                bestAction = GetBestActionFromState(state, allowedActions);
                newState = cubeHandler.ApplyMove(state, bestAction);
            }

            //daca niciuna din starile urmatoare nu este explorata => af msg de eroare
            if (allowedActions.Count == 0)
            {
                return null;
            }

            //actualizeaza lista de mutari, starea si adauga starea la vizitate
            state = newState;
            lastAction = bestAction;
            visitedStates.Add(newState);
            noOfMovesApplied++;
        }

        //s-a depasit nr max de mutari
        if (noOfMovesApplied == max_depth)
        {
            return null;
        }

        return moves;
    }

    //determina lista de mutari permise tinand cont de starile vizitate
    private List<string> GetAllowedMoves(string cubeState, List<string> moves, HashSet<string> visitedStates)
    {
        List<string> allowedMoves = new List<string>();
        foreach(string move in moves)
        {
            string newState = cubeHandler.ApplyMove(cubeState, move);
            if(!visitedStates.Contains(newState))
            {
                allowedMoves.Add(move);
            }
        }
        return allowedMoves;
    }

    //determina cea mai buna actiune de luat din starea state
    private string GetBestActionFromState(string state, List<string> allowedActions)
    {
        string bestActionToTake = null;//cea mai buna actiune
        double bestQVal = double.NegativeInfinity;//Q-val a ei

        //determina actiunea cu c. m. mare val din actiunile permise
        foreach (string action in allowedActions)
        {
            double value = QTable[state][action];
            if (value > bestQVal)
            {
                bestActionToTake = action;
                bestQVal = value;
            }
        }

        return bestActionToTake;
    }
}
