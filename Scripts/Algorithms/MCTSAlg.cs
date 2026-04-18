using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MCTSAlg
{
    //nr maxim de pasi 
    private int maxSteps = 30;
    private int iterations = 700;

    //fct care aplica MCTS si returneaza solutia gasita
    public List<string> MCTSAlgorithm(string startState)
    {
        CubeHandler ch = new CubeHandler();
        //cub deja rezolvat => ret lista goala
        if (ch.IsCubeSolved(startState))
        {
            return new List<string>();
        }

        //solutia
        List<string> solution = new List<string>();
        
        //starea crt 
        string crtState = startState;

        int maxDepth = 20;

        int noOfSteps = 0;

        //cat timp cubul nu e rezolvat si nu s-a facut nr max de pasi => aplica MCTSSearch
        while (!ch.IsCubeSolved(crtState) && noOfSteps < maxSteps)
        {
            MCTS mcts = new MCTS(new CubeStateNode(crtState), maxDepth);
            //determina cea mai buna mutare de facut din starea crtState
            string bestMove = mcts.MCTSSearch(iterations);
            //aplica mutarea si actualizeaza starea crt
            crtState = ch.ApplyMove(crtState, bestMove);
            solution.Add(bestMove);
            noOfSteps++;
        }

        if(!ch.IsCubeSolved(crtState))
        {
            return null;
        }

        return solution;
    }

    //clasa pt nodurile din MCTS
    public class CubeStateNode
    {
        public string cubeState;//starea cubului
        public CubeStateNode parent; //parintele nodului crt
        public List<CubeStateNode> children; //copiii nodului crt
        public int visits; //de cate ori a fost vizitata starea crt
        public double score;//arata cat de buna este starea crt
        public string lastMove; //mutarea care a determinat starea crt

        public CubeHandler cubeHandler = new CubeHandler();

        public CubeStateNode(string _cubeState, CubeStateNode _parent = null, string _lastMove = null)
        {
            this.cubeState = _cubeState;
            this.parent = _parent;
            this.children = new List<CubeStateNode>();
            this.visits = 0;
            this.score = 0.0;
            this.lastMove = _lastMove;
        }

        //fct care verifica daca nodul crt e terminal
        //(<=> cubeState e cubul rezolvat)
        public bool IsATerminalNode()
        {
            return cubeHandler.IsCubeSolved(cubeState);
        }

        //fct care arata daca nodul crt e complet expandat
        //(<=> nr de copii = nr de mutari permise din starea crt)
        public bool IsAFullyExpandedNode()
        {
            return children.Count == cubeHandler.GetMovesWithoutInverses(lastMove).Count;
        }

        //fct care alege c. m. bun copil al nodului crt folosind UCT
        //cu constanta de explorare cu val sqrt(2) = 1.41
        public CubeStateNode BestChild(double exploration = 1.41f)
        {
            return children.OrderByDescending(c => (c.score / (c.visits + 1e-6)) +
                                                exploration * Math.Sqrt(Math.Log(visits + 1) / (c.visits + 1e-6))).First();
        }
    }

    //clasa pt aplicarea alg MCTS
    public class MCTS
    {
        //radacina arborelui = starea initiala a cubului, de la care se porneste cautarea
        public CubeStateNode root;
        //adancimea maxima a cautarii
        public int maxDepth;
        public CubeHandler cubeHandler = new CubeHandler();
        private System.Random random = new System.Random();

        public MCTS(CubeStateNode _root, int _maxDepth)
        {
            this.root = _root;
            this.maxDepth = _maxDepth;
        }

        //fct care realizeaza cautarea efectiva
        //returneaza cea mai buna mutare care se poate face din radacina
        public string MCTSSearch(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                CubeStateNode node = SelectNode(root);
                double result = SimulateWithEpsilonGreedy(node);
                Backpropagate(node, result);
            }
            return BestMove();
        }

        //fct care selecteaza un nod pornind de la nodul node
        private CubeStateNode SelectNode(CubeStateNode node)
        {
            //cat timp nu s-a ajuns la un nod terminal
            while (!node.IsATerminalNode())
            {
                //daca nodul crt nu e complet expandat => il expandeaza
                if (!node.IsAFullyExpandedNode())
                {
                    return ExpandNode(node);
                }
                //nod complet expandat => merge pe ramura celor mai buni copii
                else
                {
                    node = node.BestChild();
                }
            }
            return node;
        }

        //fct care expandeaza un nod
        private CubeStateNode ExpandNode(CubeStateNode node)
        {
            //lista cu toate mutarile permise din starea crt
            List<string> permittedMoves = cubeHandler.GetMovesWithoutInverses(node.lastMove);

            //lista cu toate mutarile permise care nu sunt copii ai nodului crt
            List<string> untriedMoves = new List<string>();

            foreach (string permittedMove in permittedMoves)
            {
                //daca node nu are niciun copil cu mutarea permittedMove
                // => o adauga la lista de mutari neincercate
                if (!node.children.Any(child => child.lastMove == permittedMove))
                {
                    untriedMoves.Add(permittedMove);
                }
            }

            //selecteaza aleatoriu o mutare neincercata
            string randomMove = untriedMoves[random.Next(untriedMoves.Count)];

            //aplica mutarea asupra starii nodului crt
            string newState = cubeHandler.ApplyMove(node.cubeState, randomMove);

            //creeaza un copil cu acea stare si mutare
            CubeStateNode newChildNode = new CubeStateNode(newState, node, randomMove);

            //adauga copilul la nodul crt si il returneaza
            node.children.Add(newChildNode);

            return newChildNode;
        }

        //fct care simuleaza o secventa de mutari asupra starii din node
        //simularea e E-greedy, adica alege mutari aleatoare in 20% din cazuri
        //si mutari folosind euristica in 80% din cazuri
        private double SimulateWithEpsilonGreedy(CubeStateNode node)
        {
            //starea crt a cubului
            string crtState = node.cubeState;

            //starile vizitate in simularea crt
            HashSet<string> simVisited = new HashSet<string>();
            //adauga starea crt la vizitate
            simVisited.Add(crtState);

            //ultima mutare aplicata
            string prevMove = node.lastMove;

            //adancimea crt a simularii
            int depth = 0;
            //cat timp nu s-a ajuns la cubul rezolvat si nu s-a depasit adancimea max
            while (!cubeHandler.IsCubeSolved(crtState) && depth < maxDepth)
            {
                List<string> permittedMoves = cubeHandler.GetMovesWithoutInverses(prevMove);

                //cea mai buna stare dpdv al euristicii si valoarea euristicii ei
                string bestState = null;
                string moveForBestState = null;
                int bestHeuristicVal = int.MaxValue;

                //lista de mutari permise, care nu determina stari vizitate
                List<string> unvisitedPermittedMoves = new List<string>();

                //parcurge toate mutarile permise si det care e cea mai buna stare si mutarea care a determinat-o
                foreach (string permittedMove in permittedMoves)
                {
                    string nextState = cubeHandler.ApplyMove(crtState, permittedMove);

                    if(!simVisited.Contains(nextState))
                    {
                        unvisitedPermittedMoves.Add(permittedMove);

                        int h = cubeHandler.DetermineHValue(nextState);
                        if (h < bestHeuristicVal)
                        {
                            bestHeuristicVal = h;
                            bestState = nextState;
                            moveForBestState = permittedMove;
                        }
                    }
                }

                //vede daca acum alege  mutare aleatoare 
                //sau pe cea care are cea mai buna euristica
                bool chooseRandom = random.NextDouble() < 0.2;

                //chooseRandom = true => alegea aleator
                if (chooseRandom)
                {
                    //daca nu avem nicio mutare permisa si nevizitata => break
                    if(unvisitedPermittedMoves.Count == 0)
                    {
                        break;
                    }
                    //altfel, alege aleatoriu o mutare si o executa
                    else
                    {
                        string randomMove = unvisitedPermittedMoves[random.Next(unvisitedPermittedMoves.Count)];
                        string nextState = cubeHandler.ApplyMove(crtState, randomMove);
                        crtState = nextState;
                        prevMove = randomMove;
                        simVisited.Add(crtState);
                    }
                }
                //altfel => alege bestState
                else
                {
                    //daca starea c.m. buna nu este deja vizitata in simulare
                    //=> trece la acea stare si o viziteaza
                    if (bestState != null)
                    {
                        crtState = bestState;
                        prevMove = moveForBestState;
                        simVisited.Add(crtState);
                    }
                    else
                    {
                        break;
                    }
                }

                depth++;
            }

            //retueneaza recompensa pt simularea crt
            return cubeHandler.IsCubeSolved(crtState) ? 1 + (1.0 / (depth + 1)) : 
                1.0 / (cubeHandler.DetermineHValue(crtState) + 1);
        }

        //fct care actualizeaza visits si score in arbore, de la node la rad
        private void Backpropagate(CubeStateNode node, double result)
        {
            while (node != null)
            {
                node.visits++;
                node.score += result;
                node = node.parent;
            }
        }

        //fct care determina care este cea mai buna mutare 
        //cea mai buna mutare pt starea din root = c m vizitata
        private string BestMove()
        {
            return root.children.OrderByDescending(c => c.visits).First().lastMove;
        }
    }
}
