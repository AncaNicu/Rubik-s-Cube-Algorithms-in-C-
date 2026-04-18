using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlg
{
    //coada cu prioritati care retine starile cubului
    //ordonate dupa costul total (valoarea lui f)
    private PriorityQueue priorityQueue = new PriorityQueue();

    //un dictionar care pastreaza starile vizitate si costul lor minim
    //pt a evita revizitarea unei stari cu un cost mai mare 
    private Dictionary<string, int> visitedStates = new Dictionary<string, int>();

    //pt operatiile ce se pot efectua asupra starii cubului
    private CubeHandler cubeHandler = new CubeHandler();

    //clasa care se ocupa de starea cubului pt prima faza a algoritmului
    public class CubeStateForAStar
    {
        //starea crt a cubului sub forma de string 
        //fetele sunt in ordinea yellow, blue, orange, white, green, red
        public string currentState;

        //secventa de mutari care s-au aplicat pana in acest moment
        public List<string> sequenceOfMoves;

        //g(n) = nr de mutari facute pana acum
        public int gValue;

        //h(n) = euristica = nr estimat de mutari pt a completa faza 1
        public int hValue;

        //f(n) = g(n) + h(n) = costul unei mutari
        public int fValue;

        //constructorul cu parametrii
        public CubeStateForAStar(string _currentState, int _gValue, int _hValue, List<string> _sequenceOfMoves = null)
        {
            this.currentState = _currentState;
            this.gValue = _gValue;
            this.hValue = _hValue;
            DetermineTotalCost(); //valoarea lui f se determina prin insumarea lui h cu g
            this.sequenceOfMoves = _sequenceOfMoves != null ? new List<string>(_sequenceOfMoves) : new List<string>();
        }

        //fct care determina costul total al unei mutari (adica val lui f)
        private void DetermineTotalCost()
        {
            fValue = gValue + hValue;
        }
    }

    //clasa pt coada cu prioritati in care ordinea elem e stabilita in fct de cost
    public class PriorityQueue
    {
        //pastreaza elem in ordinea data de min-heap
        private List<CubeStateForAStar> heap = new List<CubeStateForAStar>();

        //fct care schimba intre ele 2 elemente din coada
        //folosita la ordonarea elem (la enqueue sau dequeue)
        private void Swap(int i, int j)
        {
            CubeStateForAStar temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        //reorganizeaza elementele din coada dupa o insertie
        //astfel incat sa aiba costurile in ordine crescatoare
        //parintele unui nod trebuie sa aiba costul mai mic decat copilul
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (heap[index].fValue >= heap[parent].fValue) break;

                Swap(index, parent);
                index = parent;
            }
        }

        //reorganizeaza elementele din coada dupa o stergere
        private void HeapifyDown(int index)
        {
            int leftChild, rightChild, smallest;
            while (true)
            {
                leftChild = 2 * index + 1;
                rightChild = 2 * index + 2;
                smallest = index;

                if (leftChild < heap.Count && heap[leftChild].fValue < heap[smallest].fValue)
                    smallest = leftChild;

                if (rightChild < heap.Count && heap[rightChild].fValue < heap[smallest].fValue)
                    smallest = rightChild;

                if (smallest == index) break;

                Swap(index, smallest);
                index = smallest;
            }
        }

        //fct pt adaugarea unui element in coada
        public void Enqueue(CubeStateForAStar item)
        {
            heap.Add(item);
            HeapifyUp(heap.Count - 1);
        }

        //fct pt eliminarea elementului cu costul cel mai mic din coada
        public CubeStateForAStar Dequeue()
        {
            if (heap.Count == 0) throw new InvalidOperationException("Queue is empty");

            CubeStateForAStar minItem = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);

            return minItem;
        }

        //fct care goleste coada 
        public void Clear()
        {
            heap.Clear();
        }

        //fct care returneaza nr de elemente din coada
        public int Count()
        {
            return heap.Count;
        }
    }

    //fct care aplica alg A* si returneaza secventa de mutari determinata
    public List<string> AStarSearch(string initialState)
    {
        //cub deja rezolvat => ret lista goala
        if (cubeHandler.IsCubeSolved(initialState))
        {
            return new List<string>();
        }

        //goleste coada si lista de stari vizitate
        priorityQueue.Clear();
        visitedStates.Clear();

        //creeaza starea initiala si o adauga la coada
        CubeStateForAStar startState = new CubeStateForAStar(initialState, 0, cubeHandler.DetermineHValue(initialState));
        priorityQueue.Enqueue(startState);

        //cat timp mai sunt elemente in coada
        while (priorityQueue.Count() > 0)
        {
            //se ia din coada elem cu f cel mai mic
            CubeStateForAStar crt = priorityQueue.Dequeue();

            //daca starea crt e cubul rezolvat => ret solutia
            if (cubeHandler.IsCubeSolved(crt.currentState))
            {
                return crt.sequenceOfMoves;
            }

            //daca starea crt este deja vizitata si are un cost mai mic sau egal
            //=> nu o mai exploreaza
            if (visitedStates.ContainsKey(crt.currentState) &&
                visitedStates[crt.currentState] <= crt.fValue)
            {
                continue;
            }

            //daca starea nu a fost vizitata => o adauga la visitedStates
            //sau daca este deja vizitata, dar acum are f mai mic => actualizeaza costul in visitedStates
            visitedStates[crt.currentState] = crt.fValue;

            //determina ultima mutare efectuata
            //daca avem prima stare => lastMove ramane un sir gol
            string lastMove = null;
            if (crt.sequenceOfMoves.Count > 0)
            {
                lastMove = crt.sequenceOfMoves[crt.sequenceOfMoves.Count - 1];
            }

            //exploreaza starea crt => ii adauga descendentii in coada
            foreach (string move in cubeHandler.GetAllowedMoves(lastMove))
            {
                //creeaza noua stare pt a fi adaugata in coada
                //aplicand mutarea move asupra configuratiei crt
                string newStateString = cubeHandler.ApplyMove(crt.currentState, move); //noua stare sub forma de string

                //noua secventa de mutari este vechea secventa, la care se adauga move
                List<string> newSequenceOfMoves = new List<string>(crt.sequenceOfMoves);
                newSequenceOfMoves.Add(move);

                //noua stare pe baza datelor de mai sus
                CubeStateForAStar newState = new CubeStateForAStar(newStateString, crt.gValue + 1, cubeHandler.DetermineHValue(newStateString), newSequenceOfMoves);

                //doar daca starea noua nu a fost deja vizitata
                //sau daca a fost vizitata cu un cost mai mic
                //se adauga in coada
                if (!visitedStates.ContainsKey(newStateString) || visitedStates[newStateString] > newState.fValue)
                {
                    priorityQueue.Enqueue(newState);
                }
            }

        }
        return null;
    }
}