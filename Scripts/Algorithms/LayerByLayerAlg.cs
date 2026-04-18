using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerByLayerAlg
{
    //starea cubului
    private string state;

    //pt operatiile ce se pot efectua asupra starii cubului
    private CubeHandler cubeHandler = new CubeHandler();

    //lista de mutari ce trebuie efectuate pt rezolvarea cubului
    public List<string> moves = new List<string>() { };

    //fct care ruleaza algoritmul si returneaza lista de mutari
    public List<string> LayerByLayerAlgorithm(string initialState)
    {
        //cub deja rezolvat => ret lista goala
        if (cubeHandler.IsCubeSolved(initialState))
        {
            return new List<string>();
        }

        state = initialState;
        moves.Clear();
        SolveYellowFace();
        OLL();
        PLL();
        AllignThe2Layers();
        return moves;
    }

    //==========================================REZOLVAREA FETEI GALBENE

    //----------------------------CAZUL 1 -> galben pe fata laterala, stanga jos

    bool SolveOGYCase1()
    {
        //arata daca coltul este in cazul 1
        bool cornerInCase1 = false;

        //aduce coltul pe fata laterala potrivita
        if (state[10] == 'U' && state[19] == 'L' && state[12] == 'F')
        {
            state = cubeHandler.ApplyMove(state, "D");
            moves.Add("D");
            cornerInCase1 = true;
        }
        else
        {
            if (state[6] == 'U' && state[11] == 'L' && state[13] == 'F')
            {
                cornerInCase1 = true;
            }
            else
            {
                if (state[22] == 'U' && state[7] == 'L' && state[15] == 'F')
                {
                    state = cubeHandler.ApplyMove(state, "D'");
                    moves.Add("D'");
                    cornerInCase1 = true;
                }
                else
                {
                    if (state[18] == 'U' && state[23] == 'L' && state[14] == 'F')
                    {
                        state = cubeHandler.ApplyMove(state, "D2");
                        moves.Add("D2");
                        cornerInCase1 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 1
        if (cornerInCase1)
        {
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L'");
            moves.Add("L");
            moves.Add("D'");
            moves.Add("L'");
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 1
        return cornerInCase1;
    }

    bool SolveOBYCase1()
    {
        //arata daca coltul este in cazul 1
        bool cornerInCase1 = false;

        //aduce coltul pe fata laterala potrivita
        if (state[10] == 'U' && state[19] == 'F' && state[12] == 'R')
        {
            state = cubeHandler.ApplyMove(state, "D2");
            moves.Add("D2");
            cornerInCase1 = true;
        }
        else
        {
            if (state[6] == 'U' && state[11] == 'F' && state[13] == 'R')
            {
                state = cubeHandler.ApplyMove(state, "D");
                moves.Add("D");
                cornerInCase1 = true;
            }
            else
            {
                if (state[22] == 'U' && state[7] == 'F' && state[15] == 'R')
                {
                    cornerInCase1 = true;
                }
                else
                {
                    if (state[18] == 'U' && state[23] == 'F' && state[14] == 'R')
                    {
                        state = cubeHandler.ApplyMove(state, "D'");
                        moves.Add("D'");
                        cornerInCase1 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 1
        if (cornerInCase1)
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F'");
            moves.Add("F");
            moves.Add("D'");
            moves.Add("F'");
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 1
        return cornerInCase1;
    }

    bool SolveGRYCase1()
    {
        //arata daca coltul este in cazul 1
        bool cornerInCase1 = false;

        //aduce coltul pe fata laterala potrivita
        if (state[10] == 'U' && state[19] == 'B' && state[12] == 'L')
        {
            cornerInCase1 = true;
        }
        else
        {
            if (state[6] == 'U' && state[11] == 'B' && state[13] == 'L')
            {
                state = cubeHandler.ApplyMove(state, "D'");
                moves.Add("D'");
                cornerInCase1 = true;
            }
            else
            {
                if (state[22] == 'U' && state[7] == 'B' && state[15] == 'L')
                {
                    state = cubeHandler.ApplyMove(state, "D2");
                    moves.Add("D2");
                    cornerInCase1 = true;
                }
                else
                {
                    if (state[18] == 'U' && state[23] == 'B' && state[14] == 'L')
                    {
                        state = cubeHandler.ApplyMove(state, "D");
                        moves.Add("D");
                        cornerInCase1 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 1
        if (cornerInCase1)
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B'");
            moves.Add("B");
            moves.Add("D'");
            moves.Add("B'");
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 1
        return cornerInCase1;
    }

    bool SolveRBYCase1()
    {
        //arata daca coltul este in cazul 1
        bool cornerInCase1 = false;

        //aduce coltul pe fata laterala potrivita
        if (state[10] == 'U' && state[19] == 'R' && state[12] == 'B')
        {
            state = cubeHandler.ApplyMove(state, "D'");
            moves.Add("D'");
            cornerInCase1 = true;
        }
        else
        {
            if (state[6] == 'U' && state[11] == 'R' && state[13] == 'B')
            {
                state = cubeHandler.ApplyMove(state, "D2");
                moves.Add("D2");
                cornerInCase1 = true;
            }
            else
            {
                if (state[22] == 'U' && state[7] == 'R' && state[15] == 'B')
                {
                    state = cubeHandler.ApplyMove(state, "D");
                    moves.Add("D");
                    cornerInCase1 = true;
                }
                else
                {
                    if (state[18] == 'U' && state[23] == 'R' && state[14] == 'B')
                    {
                        cornerInCase1 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 1
        if (cornerInCase1)
        {
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R'");
            moves.Add("R");
            moves.Add("D'");
            moves.Add("R'");
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 1
        return cornerInCase1;
    }

    //----------------------------CAZUL 2 -> galben pe fata laterala, dreapta jos
    bool SolveOGYCase2()
    {
        //arata daca coltul este in cazul 2
        bool cornerInCase2 = false;

        //aduce coltul pe fata laterala potrivita
        if (state[11] == 'U' && state[6] == 'F' && state[13] == 'L')
        {
            state = cubeHandler.ApplyMove(state, "D2");
            moves.Add("D2");
            cornerInCase2 = true;
        }
        else
        {
            if (state[7] == 'U' && state[22] == 'F' && state[15] == 'L')
            {
                state = cubeHandler.ApplyMove(state, "D");
                moves.Add("D");
                cornerInCase2 = true;
            }
            else
            {
                if (state[23] == 'U' && state[18] == 'F' && state[14] == 'L')
                {
                    cornerInCase2 = true;
                }
                else
                {
                    if (state[19] == 'U' && state[10] == 'F' && state[12] == 'L')
                    {
                        state = cubeHandler.ApplyMove(state, "D'");
                        moves.Add("D'");
                        cornerInCase2 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 2
        if (cornerInCase2)
        {
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F");
            moves.Add("F'");
            moves.Add("D");
            moves.Add("F");
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 2
        return cornerInCase2;
    }

    bool SolveOBYCase2()
    {
        //arata daca coltul este in cazul 2
        bool cornerInCase2 = false;

        //aduce coltul pe fata laterala potrivita
        if (state[11] == 'U' && state[6] == 'R' && state[13] == 'F')
        {
            state = cubeHandler.ApplyMove(state, "D'");
            moves.Add("D'");
            cornerInCase2 = true;
        }
        else
        {
            if (state[7] == 'U' && state[22] == 'R' && state[15] == 'F')
            {
                state = cubeHandler.ApplyMove(state, "D2");
                moves.Add("D2");
                cornerInCase2 = true;
            }
            else
            {
                if (state[23] == 'U' && state[18] == 'R' && state[14] == 'F')
                {
                    state = cubeHandler.ApplyMove(state, "D");
                    moves.Add("D");
                    cornerInCase2 = true;
                }
                else
                {
                    if (state[19] == 'U' && state[10] == 'R' && state[12] == 'F')
                    {
                        cornerInCase2 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 2
        if (cornerInCase2)
        {
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R");
            moves.Add("R'");
            moves.Add("D");
            moves.Add("R");
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 2
        return cornerInCase2;
    }

    bool SolveGRYCase2()
    {
        //arata daca coltul este in cazul 2
        bool cornerInCase2 = false;

        //aduce coltul pe fata laterala potrivita
        if (state[11] == 'U' && state[6] == 'L' && state[13] == 'B')
        {
            state = cubeHandler.ApplyMove(state, "D");
            moves.Add("D");
            cornerInCase2 = true;
        }
        else
        {
            if (state[7] == 'U' && state[22] == 'L' && state[15] == 'B')
            {
                cornerInCase2 = true;
            }
            else
            {
                if (state[23] == 'U' && state[18] == 'L' && state[14] == 'B')
                {
                    state = cubeHandler.ApplyMove(state, "D'");
                    moves.Add("D'");
                    cornerInCase2 = true;
                }
                else
                {
                    if (state[19] == 'U' && state[10] == 'L' && state[12] == 'B')
                    {
                        state = cubeHandler.ApplyMove(state, "D2");
                        moves.Add("D2");
                        cornerInCase2 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 2
        if (cornerInCase2)
        {
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "L");
            moves.Add("L'");
            moves.Add("D");
            moves.Add("L");
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 2
        return cornerInCase2;
    }

    bool SolveRBYCase2()
    {
        //arata daca coltul este in cazul 2
        bool cornerInCase2 = false;

        //aduce coltul pe fata laterala potrivita
        if (state[11] == 'U' && state[6] == 'B' && state[13] == 'R')
        {
            cornerInCase2 = true;
        }
        else
        {
            if (state[7] == 'U' && state[22] == 'B' && state[15] == 'R')
            {
                state = cubeHandler.ApplyMove(state, "D'");
                moves.Add("D'");
                cornerInCase2 = true;
            }
            else
            {
                if (state[23] == 'U' && state[18] == 'B' && state[14] == 'R')
                {
                    state = cubeHandler.ApplyMove(state, "D2");
                    moves.Add("D2");
                    cornerInCase2 = true;
                }
                else
                {
                    if (state[19] == 'U' && state[10] == 'B' && state[12] == 'R')
                    {
                        state = cubeHandler.ApplyMove(state, "D");
                        moves.Add("D");
                        cornerInCase2 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 2
        if (cornerInCase2)
        {
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B");
            moves.Add("B'");
            moves.Add("D");
            moves.Add("B");
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 2
        return cornerInCase2;
    }

    //----------------------------CAZUL 3 -> galben pe fata laterala, stanga sus

    bool SolveOGYCase3()
    {
        //arata daca coltul este in cazul 3
        bool cornerInCase3 = false;

        //aduce coltul in cazul 1
        if (state[8] == 'U' && state[17] == 'F' && state[2] == 'L')
        {
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F");
            moves.Add("F'");
            moves.Add("D2");
            moves.Add("F");
            cornerInCase3 = true;
        }
        else
        {
            if (state[4] == 'U' && state[9] == 'F' && state[3] == 'L')
            {
                state = cubeHandler.ApplyMove(state, "R'");
                state = cubeHandler.ApplyMove(state, "D2");
                state = cubeHandler.ApplyMove(state, "R");
                moves.Add("R'");
                moves.Add("D2");
                moves.Add("R");
                cornerInCase3 = true;
            }
            else
            {
                if (state[20] == 'U' && state[5] == 'F' && state[1] == 'L')
                {
                    state = cubeHandler.ApplyMove(state, "B'");
                    state = cubeHandler.ApplyMove(state, "D2");
                    state = cubeHandler.ApplyMove(state, "B");
                    moves.Add("B'");
                    moves.Add("D2");
                    moves.Add("B");
                    cornerInCase3 = true;
                }
                else
                {
                    if (state[16] == 'U' && state[21] == 'F' && state[0] == 'L')
                    {
                        state = cubeHandler.ApplyMove(state, "L'");
                        state = cubeHandler.ApplyMove(state, "D2");
                        state = cubeHandler.ApplyMove(state, "L");
                        moves.Add("L'");
                        moves.Add("D2");
                        moves.Add("L");
                        cornerInCase3 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 3
        if (cornerInCase3)
        {
            SolveOGYCase1();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 3
        return cornerInCase3;
    }

    bool SolveOBYCase3()
    {
        //arata daca coltul este in cazul 3
        bool cornerInCase3 = false;

        //aduce coltul in cazul 1
        if (state[8] == 'U' && state[17] == 'R' && state[2] == 'F')
        {
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F");
            moves.Add("F'");
            moves.Add("D2");
            moves.Add("F");
            cornerInCase3 = true;
        }
        else
        {
            if (state[4] == 'U' && state[9] == 'R' && state[3] == 'F')
            {
                state = cubeHandler.ApplyMove(state, "R'");
                state = cubeHandler.ApplyMove(state, "D2");
                state = cubeHandler.ApplyMove(state, "R");
                moves.Add("R'");
                moves.Add("D2");
                moves.Add("R");
                cornerInCase3 = true;
            }
            else
            {
                if (state[20] == 'U' && state[5] == 'R' && state[1] == 'F')
                {
                    state = cubeHandler.ApplyMove(state, "B'");
                    state = cubeHandler.ApplyMove(state, "D2");
                    state = cubeHandler.ApplyMove(state, "B");
                    moves.Add("B'");
                    moves.Add("D2");
                    moves.Add("B");
                    cornerInCase3 = true;
                }
                else
                {
                    if (state[16] == 'U' && state[21] == 'R' && state[0] == 'F')
                    {
                        state = cubeHandler.ApplyMove(state, "L'");
                        state = cubeHandler.ApplyMove(state, "D2");
                        state = cubeHandler.ApplyMove(state, "L");
                        moves.Add("L'");
                        moves.Add("D2");
                        moves.Add("L");
                        cornerInCase3 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 3
        if (cornerInCase3)
        {
            SolveOBYCase1();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 3
        return cornerInCase3;
    }

    bool SolveGRYCase3()
    {
        //arata daca coltul este in cazul 3
        bool cornerInCase3 = false;

        //aduce coltul in cazul 1
        if (state[8] == 'U' && state[17] == 'L' && state[2] == 'B')
        {
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F");
            moves.Add("F'");
            moves.Add("D2");
            moves.Add("F");
            cornerInCase3 = true;
        }
        else
        {
            if (state[4] == 'U' && state[9] == 'L' && state[3] == 'B')
            {
                state = cubeHandler.ApplyMove(state, "R'");
                state = cubeHandler.ApplyMove(state, "D2");
                state = cubeHandler.ApplyMove(state, "R");
                moves.Add("R'");
                moves.Add("D2");
                moves.Add("R");
                cornerInCase3 = true;
            }
            else
            {
                if (state[20] == 'U' && state[5] == 'L' && state[1] == 'B')
                {
                    state = cubeHandler.ApplyMove(state, "B'");
                    state = cubeHandler.ApplyMove(state, "D2");
                    state = cubeHandler.ApplyMove(state, "B");
                    moves.Add("B'");
                    moves.Add("D2");
                    moves.Add("B");
                    cornerInCase3 = true;
                }
                else
                {
                    if (state[16] == 'U' && state[21] == 'L' && state[0] == 'B')
                    {
                        state = cubeHandler.ApplyMove(state, "L'");
                        state = cubeHandler.ApplyMove(state, "D2");
                        state = cubeHandler.ApplyMove(state, "L");
                        moves.Add("L'");
                        moves.Add("D2");
                        moves.Add("L");
                        cornerInCase3 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 3
        if (cornerInCase3)
        {
            SolveGRYCase1();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 3
        return cornerInCase3;
    }

    bool SolveRBYCase3()
    {
        //arata daca coltul este in cazul 3
        bool cornerInCase3 = false;

        //aduce coltul in cazul 1
        if (state[8] == 'U' && state[17] == 'B' && state[2] == 'R')
        {
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F");
            moves.Add("F'");
            moves.Add("D2");
            moves.Add("F");
            cornerInCase3 = true;
        }
        else
        {
            if (state[4] == 'U' && state[9] == 'B' && state[3] == 'R')
            {
                state = cubeHandler.ApplyMove(state, "R'");
                state = cubeHandler.ApplyMove(state, "D2");
                state = cubeHandler.ApplyMove(state, "R");
                moves.Add("R'");
                moves.Add("D2");
                moves.Add("R");
                cornerInCase3 = true;
            }
            else
            {
                if (state[20] == 'U' && state[5] == 'B' && state[1] == 'R')
                {
                    state = cubeHandler.ApplyMove(state, "B'");
                    state = cubeHandler.ApplyMove(state, "D2");
                    state = cubeHandler.ApplyMove(state, "B");
                    moves.Add("B'");
                    moves.Add("D2");
                    moves.Add("B");
                    cornerInCase3 = true;
                }
                else
                {
                    if (state[16] == 'U' && state[21] == 'B' && state[0] == 'R')
                    {
                        state = cubeHandler.ApplyMove(state, "L'");
                        state = cubeHandler.ApplyMove(state, "D2");
                        state = cubeHandler.ApplyMove(state, "L");
                        moves.Add("L'");
                        moves.Add("D2");
                        moves.Add("L");
                        cornerInCase3 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 3
        if (cornerInCase3)
        {
            SolveRBYCase1();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 3
        return cornerInCase3;
    }

    //----------------------------CAZUL 4 -> galben pe fata laterala, dreapta sus

    bool SolveOGYCase4()
    {
        //arata daca coltul este in cazul 4
        bool cornerInCase4 = false;

        //aduce coltul in cazul 2
        if (state[9] == 'U' && state[4] == 'L' && state[3] == 'F')
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F'");
            moves.Add("F");
            moves.Add("D2");
            moves.Add("F'");
            cornerInCase4 = true;
        }
        else
        {
            if (state[5] == 'U' && state[20] == 'L' && state[1] == 'F')
            {
                state = cubeHandler.ApplyMove(state, "R");
                state = cubeHandler.ApplyMove(state, "D2");
                state = cubeHandler.ApplyMove(state, "R'");
                moves.Add("R");
                moves.Add("D2");
                moves.Add("R'");
                cornerInCase4 = true;
            }
            else
            {
                if (state[21] == 'U' && state[16] == 'L' && state[0] == 'F')
                {
                    state = cubeHandler.ApplyMove(state, "B");
                    state = cubeHandler.ApplyMove(state, "D2");
                    state = cubeHandler.ApplyMove(state, "B'");
                    moves.Add("B");
                    moves.Add("D2");
                    moves.Add("B'");
                    cornerInCase4 = true;
                }
                else
                {
                    if (state[17] == 'U' && state[8] == 'L' && state[2] == 'F')
                    {
                        state = cubeHandler.ApplyMove(state, "L");
                        state = cubeHandler.ApplyMove(state, "D2");
                        state = cubeHandler.ApplyMove(state, "L'");
                        moves.Add("L");
                        moves.Add("D2");
                        moves.Add("L'");
                        cornerInCase4 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 4
        if (cornerInCase4)
        {
            SolveOGYCase2();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 4
        return cornerInCase4;
    }

    bool SolveOBYCase4()
    {
        //arata daca coltul este in cazul 4
        bool cornerInCase4 = false;

        //aduce coltul in cazul 2
        if (state[9] == 'U' && state[4] == 'F' && state[3] == 'R')
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F'");
            moves.Add("F");
            moves.Add("D2");
            moves.Add("F'");
            cornerInCase4 = true;
        }
        else
        {
            if (state[5] == 'U' && state[20] == 'F' && state[1] == 'R')
            {
                state = cubeHandler.ApplyMove(state, "R");
                state = cubeHandler.ApplyMove(state, "D2");
                state = cubeHandler.ApplyMove(state, "R'");
                moves.Add("R");
                moves.Add("D2");
                moves.Add("R'");
                cornerInCase4 = true;
            }
            else
            {
                if (state[21] == 'U' && state[16] == 'F' && state[0] == 'R')
                {
                    state = cubeHandler.ApplyMove(state, "B");
                    state = cubeHandler.ApplyMove(state, "D2");
                    state = cubeHandler.ApplyMove(state, "B'");
                    moves.Add("B");
                    moves.Add("D2");
                    moves.Add("B'");
                    cornerInCase4 = true;
                }
                else
                {
                    if (state[17] == 'U' && state[8] == 'F' && state[2] == 'R')
                    {
                        state = cubeHandler.ApplyMove(state, "L");
                        state = cubeHandler.ApplyMove(state, "D2");
                        state = cubeHandler.ApplyMove(state, "L'");
                        moves.Add("L");
                        moves.Add("D2");
                        moves.Add("L'");
                        cornerInCase4 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 4
        if (cornerInCase4)
        {
            SolveOBYCase2();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 4
        return cornerInCase4;
    }

    bool SolveGRYCase4()
    {
        //arata daca coltul este in cazul 4
        bool cornerInCase4 = false;

        //aduce coltul in cazul 2
        if (state[9] == 'U' && state[4] == 'B' && state[3] == 'L')
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F'");
            moves.Add("F");
            moves.Add("D2");
            moves.Add("F'");
            cornerInCase4 = true;
        }
        else
        {
            if (state[5] == 'U' && state[20] == 'B' && state[1] == 'L')
            {
                state = cubeHandler.ApplyMove(state, "R");
                state = cubeHandler.ApplyMove(state, "D2");
                state = cubeHandler.ApplyMove(state, "R'");
                moves.Add("R");
                moves.Add("D2");
                moves.Add("R'");
                cornerInCase4 = true;
            }
            else
            {
                if (state[21] == 'U' && state[16] == 'B' && state[0] == 'L')
                {
                    state = cubeHandler.ApplyMove(state, "B");
                    state = cubeHandler.ApplyMove(state, "D2");
                    state = cubeHandler.ApplyMove(state, "B'");
                    moves.Add("B");
                    moves.Add("D2");
                    moves.Add("B'");
                    cornerInCase4 = true;
                }
                else
                {
                    if (state[17] == 'U' && state[9] == 'B' && state[2] == 'L')
                    {
                        state = cubeHandler.ApplyMove(state, "L");
                        state = cubeHandler.ApplyMove(state, "D2");
                        state = cubeHandler.ApplyMove(state, "L'");
                        moves.Add("L");
                        moves.Add("D2");
                        moves.Add("L'");
                        cornerInCase4 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 4
        if (cornerInCase4)
        {
            SolveGRYCase2();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 4
        return cornerInCase4;
    }

    bool SolveRBYCase4()
    {
        //arata daca coltul este in cazul 4
        bool cornerInCase4 = false;

        //aduce coltul in cazul 2
        if (state[9] == 'U' && state[4] == 'R' && state[3] == 'B')
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F'");
            moves.Add("F");
            moves.Add("D2");
            moves.Add("F'");
            cornerInCase4 = true;
        }
        else
        {
            if (state[5] == 'U' && state[20] == 'R' && state[1] == 'B')
            {
                state = cubeHandler.ApplyMove(state, "R");
                state = cubeHandler.ApplyMove(state, "D2");
                state = cubeHandler.ApplyMove(state, "R'");
                moves.Add("R");
                moves.Add("D2");
                moves.Add("R'");
                cornerInCase4 = true;
            }
            else
            {
                if (state[21] == 'U' && state[16] == 'R' && state[0] == 'B')
                {
                    state = cubeHandler.ApplyMove(state, "B");
                    state = cubeHandler.ApplyMove(state, "D2");
                    state = cubeHandler.ApplyMove(state, "B'");
                    moves.Add("B");
                    moves.Add("D2");
                    moves.Add("B'");
                    cornerInCase4 = true;
                }
                else
                {
                    if (state[17] == 'U' && state[8] == 'R' && state[2] == 'B')
                    {
                        state = cubeHandler.ApplyMove(state, "L");
                        state = cubeHandler.ApplyMove(state, "D2");
                        state = cubeHandler.ApplyMove(state, "L'");
                        moves.Add("L");
                        moves.Add("D2");
                        moves.Add("L'");
                        cornerInCase4 = true;
                    }
                }
            }
        }

        //pune coltul la locul lui pe fata galbena, daca e in cazul 4
        if (cornerInCase4)
        {
            SolveRBYCase2();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 4
        return cornerInCase4;
    }

    //----------------------------CAZUL 5 -> galben pe fata alba

    bool SolveOGYCase5()
    {
        //arata daca coltul este in cazul 5
        bool cornerInCase5 = false;

        //aduce coltul sub locul unde ar trebui plasat
        if (state[12] == 'U' && state[10] == 'L' && state[19] == 'F')
        {
            cornerInCase5 = true;
        }
        else
        {
            if (state[13] == 'U' && state[6] == 'L' && state[11] == 'F')
            {
                state = cubeHandler.ApplyMove(state, "D'");
                moves.Add("D'");
                cornerInCase5 = true;
            }
            else
            {
                if (state[14] == 'U' && state[18] == 'L' && state[23] == 'F')
                {
                    state = cubeHandler.ApplyMove(state, "D");
                    moves.Add("D");
                    cornerInCase5 = true;
                }
                else
                {
                    if (state[15] == 'U' && state[22] == 'L' && state[7] == 'F')
                    {
                        state = cubeHandler.ApplyMove(state, "D2");
                        moves.Add("D2");
                        cornerInCase5 = true;
                    }
                }
            }
        }

        //daca e in cazul 5 => aduce coltul in cazul 2, apoi rezolva cazul 2
        if (cornerInCase5)
        {
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "L'");
            moves.Add("L");
            moves.Add("D2");
            moves.Add("L'");
            SolveOGYCase2();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 5
        return cornerInCase5;
    }

    bool SolveOBYCase5()
    {
        //arata daca coltul este in cazul 5
        bool cornerInCase5 = false;

        //aduce coltul sub locul unde ar trebui plasat
        if (state[12] == 'U' && state[10] == 'F' && state[19] == 'R')
        {
            state = cubeHandler.ApplyMove(state, "D");
            moves.Add("D");
            cornerInCase5 = true;
        }
        else
        {
            if (state[13] == 'U' && state[6] == 'F' && state[11] == 'R')
            {
                cornerInCase5 = true;
            }
            else
            {
                if (state[14] == 'U' && state[18] == 'F' && state[23] == 'R')
                {
                    state = cubeHandler.ApplyMove(state, "D2");
                    moves.Add("D2");
                    cornerInCase5 = true;
                }
                else
                {
                    if (state[15] == 'U' && state[22] == 'F' && state[7] == 'R')
                    {
                        state = cubeHandler.ApplyMove(state, "D'");
                        moves.Add("D'");
                        cornerInCase5 = true;
                    }
                }
            }
        }

        //daca e in cazul 5 => aduce coltul in cazul 2, apoi rezolva cazul 2
        if (cornerInCase5)
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F'");
            moves.Add("F");
            moves.Add("D2");
            moves.Add("F'");
            SolveOBYCase2();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 5
        return cornerInCase5;
    }

    bool SolveGRYCase5()
    {
        //arata daca coltul este in cazul 5
        bool cornerInCase5 = false;

        //aduce coltul sub locul unde ar trebui plasat
        if (state[12] == 'U' && state[10] == 'B' && state[19] == 'L')
        {
            state = cubeHandler.ApplyMove(state, "D'");
            moves.Add("D'");
            cornerInCase5 = true;
        }
        else
        {
            if (state[13] == 'U' && state[6] == 'B' && state[11] == 'L')
            {
                state = cubeHandler.ApplyMove(state, "D2");
                moves.Add("D2");
                cornerInCase5 = true;
            }
            else
            {
                if (state[14] == 'U' && state[18] == 'B' && state[23] == 'L')
                {
                    cornerInCase5 = true;
                }
                else
                {
                    if (state[15] == 'U' && state[22] == 'B' && state[7] == 'L')
                    {
                        state = cubeHandler.ApplyMove(state, "D");
                        moves.Add("D");
                        cornerInCase5 = true;
                    }
                }
            }
        }

        //daca e in cazul 5 => aduce coltul in cazul 2, apoi rezolva cazul 2
        if (cornerInCase5)
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "B'");
            moves.Add("B");
            moves.Add("D2");
            moves.Add("B'");
            SolveGRYCase2();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 5
        return cornerInCase5;
    }

    bool SolveRBYCase5()
    {
        //arata daca coltul este in cazul 5
        bool cornerInCase5 = false;

        //aduce coltul sub locul unde ar trebui plasat
        if (state[12] == 'U' && state[10] == 'R' && state[19] == 'B')
        {
            state = cubeHandler.ApplyMove(state, "D2");
            moves.Add("D2");
            cornerInCase5 = true;
        }
        else
        {
            if (state[13] == 'U' && state[6] == 'R' && state[11] == 'B')
            {
                state = cubeHandler.ApplyMove(state, "D");
                moves.Add("D");
                cornerInCase5 = true;
            }
            else
            {
                if (state[14] == 'U' && state[18] == 'R' && state[23] == 'B')
                {
                    state = cubeHandler.ApplyMove(state, "D'");
                    moves.Add("D'");
                    cornerInCase5 = true;
                }
                else
                {
                    if (state[15] == 'U' && state[22] == 'R' && state[7] == 'B')
                    {
                        cornerInCase5 = true;
                    }
                }
            }
        }

        //daca e in cazul 5 => aduce coltul in cazul 2, apoi rezolva cazul 2
        if (cornerInCase5)
        {
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "R'");
            moves.Add("R");
            moves.Add("D2");
            moves.Add("R'");
            SolveRBYCase2();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 5
        return cornerInCase5;
    }

    //----------------------------CAZUL 6 -> galben pe fata galbena, dar nu la locul lui

    bool SolveOGYCase6()
    {
        //arata daca coltul este in cazul 6
        bool cornerInCase6 = false;

        //aduce coltul este pe fata galbena, dar nu unde trebuie => se reduce la cazul 1
        if (state[0] == 'U' && state[16] == 'F' && state[21] == 'L')
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B'");
            moves.Add("B");
            moves.Add("D");
            moves.Add("B'");
            cornerInCase6 = true;
        }
        else
        {
            if (state[1] == 'U' && state[20] == 'F' && state[5] == 'L')
            {
                state = cubeHandler.ApplyMove(state, "R");
                state = cubeHandler.ApplyMove(state, "D");
                state = cubeHandler.ApplyMove(state, "R'");
                moves.Add("R");
                moves.Add("D");
                moves.Add("R'");
                cornerInCase6 = true;
            }
            else
            {
                if (state[3] == 'U' && state[4] == 'F' && state[9] == 'L')
                {
                    state = cubeHandler.ApplyMove(state, "F");
                    state = cubeHandler.ApplyMove(state, "D");
                    state = cubeHandler.ApplyMove(state, "F'");
                    moves.Add("F");
                    moves.Add("D");
                    moves.Add("F'");
                    cornerInCase6 = true;
                }
            }
        }

        //daca e in cazul 6 => rezolva cazul 1
        if (cornerInCase6)
        {
            SolveOGYCase1();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 5
        return cornerInCase6;
    }

    bool SolveOBYCase6()
    {
        //arata daca coltul este in cazul 6
        bool cornerInCase6 = false;

        //aduce coltul este pe fata galbena, dar nu unde trebuie => se reduce la cazul 1
        if (state[0] == 'U' && state[16] == 'R' && state[21] == 'F')
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B'");
            moves.Add("B");
            moves.Add("D");
            moves.Add("B'");
            cornerInCase6 = true;
        }
        else
        {
            if (state[1] == 'U' && state[20] == 'R' && state[5] == 'F')
            {
                state = cubeHandler.ApplyMove(state, "R");
                state = cubeHandler.ApplyMove(state, "D");
                state = cubeHandler.ApplyMove(state, "R'");
                moves.Add("R");
                moves.Add("D");
                moves.Add("R'");
                cornerInCase6 = true;
            }
            else
            {
                if (state[2] == 'U' && state[8] == 'R' && state[17] == 'F')
                {
                    state = cubeHandler.ApplyMove(state, "L");
                    state = cubeHandler.ApplyMove(state, "D");
                    state = cubeHandler.ApplyMove(state, "L'");
                    moves.Add("L");
                    moves.Add("D");
                    moves.Add("L'");
                    cornerInCase6 = true;
                }
            }
        }

        //daca e in cazul 6 => rezolva cazul 1
        if (cornerInCase6)
        {
            SolveOBYCase1();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 5
        return cornerInCase6;
    }
    bool SolveGRYCase6()
    {
        //arata daca coltul este in cazul 6
        bool cornerInCase6 = false;

        //aduce coltul este pe fata galbena, dar nu unde trebuie => se reduce la cazul 1
        if (state[1] == 'U' && state[20] == 'L' && state[5] == 'B')
        {
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R'");
            moves.Add("R");
            moves.Add("D");
            moves.Add("R'");
            cornerInCase6 = true;
        }
        else
        {
            if (state[2] == 'U' && state[8] == 'L' && state[17] == 'B')
            {
                state = cubeHandler.ApplyMove(state, "L");
                state = cubeHandler.ApplyMove(state, "D");
                state = cubeHandler.ApplyMove(state, "L'");
                moves.Add("L");
                moves.Add("D");
                moves.Add("L'");
                cornerInCase6 = true;
            }
            else
            {
                if (state[3] == 'U' && state[4] == 'L' && state[9] == 'B')
                {
                    state = cubeHandler.ApplyMove(state, "F");
                    state = cubeHandler.ApplyMove(state, "D");
                    state = cubeHandler.ApplyMove(state, "F'");
                    moves.Add("F");
                    moves.Add("D");
                    moves.Add("F'");
                    cornerInCase6 = true;
                }
            }
        }

        //daca e in cazul 6 => rezolva cazul 1
        if (cornerInCase6)
        {
            SolveGRYCase1();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 5
        return cornerInCase6;
    }

    bool SolveRBYCase6()
    {
        //arata daca coltul este in cazul 6
        bool cornerInCase6 = false;

        //aduce coltul este pe fata galbena, dar nu unde trebuie => se reduce la cazul 1
        if (state[0] == 'U' && state[16] == 'B' && state[21] == 'R')
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B'");
            moves.Add("B");
            moves.Add("D");
            moves.Add("B'");
            cornerInCase6 = true;
        }
        else
        {
            if (state[2] == 'U' && state[8] == 'B' && state[17] == 'R')
            {
                state = cubeHandler.ApplyMove(state, "L");
                state = cubeHandler.ApplyMove(state, "D");
                state = cubeHandler.ApplyMove(state, "L'");
                moves.Add("L");
                moves.Add("D");
                moves.Add("L'");
                cornerInCase6 = true;
            }
            else
            {
                if (state[3] == 'U' && state[4] == 'B' && state[9] == 'R')
                {
                    state = cubeHandler.ApplyMove(state, "F");
                    state = cubeHandler.ApplyMove(state, "D");
                    state = cubeHandler.ApplyMove(state, "F'");
                    moves.Add("F");
                    moves.Add("D");
                    moves.Add("F'");
                    cornerInCase6 = true;
                }
            }
        }

        //daca e in cazul 6 => rezolva cazul 1
        if (cornerInCase6)
        {
            SolveRBYCase1();
        }

        //returneaza cornerInCase1 ca sa arate daca coltul este in cazul 5
        return cornerInCase6;
    }

    //----------------------------REZOLVAREA COLTURILOR GALENE

    //pune coltul OGY la locul lui din poz sa initiala (toate cazurile)
    void SolveOGYCorner()
    {
        if (SolveOGYCase1())
        {
            return;
        }
        if (SolveOGYCase2())
        {
            return;
        }
        if (SolveOGYCase3())
        {
            return;
        }
        if (SolveOGYCase4())
        {
            return;
        }
        if (SolveOGYCase5())
        {
            return;
        }
        if (SolveOGYCase6())
        {
            return;
        }
    }

    //pune coltul OBY la locul lui din poz sa initiala (toate cazurile)
    void SolveOBYCorner()
    {
        if (SolveOBYCase1())
        {
            return;
        }
        if (SolveOBYCase2())
        {
            return;
        }
        if (SolveOBYCase3())
        {
            return;
        }
        if (SolveOBYCase4())
        {
            return;
        }
        if (SolveOBYCase5())
        {
            return;
        }
        if (SolveOBYCase6())
        {
            return;
        }
    }

    //pune coltul GRY la locul lui din poz sa initiala (toate cazurile)
    void SolveGRYCorner()
    {
        if (SolveGRYCase1())
        {
            return;
        }
        if (SolveGRYCase2())
        {
            return;
        }
        if (SolveGRYCase3())
        {
            return;
        }
        if (SolveGRYCase4())
        {
            return;
        }
        if (SolveGRYCase5())
        {
            return;
        }
        if (SolveGRYCase6())
        {
            return;
        }
    }

    //pune coltul RBY la locul lui din poz sa initiala (toate cazurile)
    void SolveRBYCorner()
    {
        if (SolveRBYCase1())
        {
            return;
        }
        if (SolveRBYCase2())
        {
            return;
        }
        if (SolveRBYCase3())
        {
            return;
        }
        if (SolveRBYCase4())
        {
            return;
        }
        if (SolveRBYCase5())
        {
            return;
        }
        if (SolveRBYCase6())
        {
            return;
        }
    }

    //----------------------------REZOLVAREA FETEI GALENE

    //rezolva intreaga fata galbena, cate un colt pe rand
    void SolveYellowFace()
    {
        int contor = 0;
        while (!IsYellowFaceAndFirstRowSolved(state) && contor < 10)
        {
            SolveOGYCorner();
            SolveOBYCorner();
            SolveGRYCorner();
            SolveRBYCorner();
            contor++;
        }
    }

    //-----------------------------ORIENTAREA FETEI ALBE (OLL)

    //rezolva cazul Little Fish
    bool SolveLittleFish()
    {

        if (state[13] == 'D' && state[10] == 'D' && state[18] == 'D' && state[22] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "L'");

            moves.Add("L");
            moves.Add("D");
            moves.Add("L'");
            moves.Add("D");
            moves.Add("L");
            moves.Add("D2");
            moves.Add("L'");

            return true;
        }

        if (state[12] == 'D' && state[18] == 'D' && state[22] == 'D' && state[6] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "B'");

            moves.Add("B");
            moves.Add("D");
            moves.Add("B'");
            moves.Add("D");
            moves.Add("B");
            moves.Add("D2");
            moves.Add("B'");

            return true;
        }

        if (state[14] == 'D' && state[22] == 'D' && state[6] == 'D' && state[10] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "R'");

            moves.Add("R");
            moves.Add("D");
            moves.Add("R'");
            moves.Add("D");
            moves.Add("R");
            moves.Add("D2");
            moves.Add("R'");

            return true;
        }

        if (state[15] == 'D' && state[6] == 'D' && state[10] == 'D' && state[18] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F'");

            moves.Add("F");
            moves.Add("D");
            moves.Add("F'");
            moves.Add("D");
            moves.Add("F");
            moves.Add("D2");
            moves.Add("F'");

            return true;
        }

        return false;
    }

    //rezolva cazul Big Fish
    bool SolveBigFish()
    {
        if (state[14] == 'D' && state[11] == 'D' && state[19] == 'D' && state[7] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L'");

            moves.Add("L");
            moves.Add("D2");
            moves.Add("L'");
            moves.Add("D'");
            moves.Add("L");
            moves.Add("D'");
            moves.Add("L'");

            return true;
        }

        if (state[15] == 'D' && state[11] == 'D' && state[19] == 'D' && state[23] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B'");

            moves.Add("B");
            moves.Add("D2");
            moves.Add("B'");
            moves.Add("D'");
            moves.Add("B");
            moves.Add("D'");
            moves.Add("B'");

            return true;
        }

        if (state[13] == 'D' && state[19] == 'D' && state[23] == 'D' && state[7] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R'");

            moves.Add("R");
            moves.Add("D2");
            moves.Add("R'");
            moves.Add("D'");
            moves.Add("R");
            moves.Add("D'");
            moves.Add("R'");

            return true;
        }

        if (state[12] == 'D' && state[23] == 'D' && state[7] == 'D' && state[11] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F'");

            moves.Add("F");
            moves.Add("D2");
            moves.Add("F'");
            moves.Add("D'");
            moves.Add("F");
            moves.Add("D'");
            moves.Add("F'");

            return true;
        }

        return false;
    }

    //rezolva cazul Double Fish
    bool SolveDoubleFish()
    {
        if (state[10] == 'D' && state[11] == 'D' && state[22] == 'D' && state[23] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "R2");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "R2");

            moves.Add("R2");
            moves.Add("D2");
            moves.Add("R");
            moves.Add("D2");
            moves.Add("R2");

            return true;
        }

        if (state[18] == 'D' && state[19] == 'D' && state[6] == 'D' && state[7] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "F2");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F2");

            moves.Add("F2");
            moves.Add("D2");
            moves.Add("F");
            moves.Add("D2");
            moves.Add("F2");

            return true;
        }

        return false;
    }

    //rezolva cazul No Fish
    bool SolveNoFish()
    {
        if (state[6] == 'D' && state[7] == 'D' && state[19] == 'D' && state[23] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F'");

            moves.Add("F");
            moves.Add("L");
            moves.Add("D");
            moves.Add("L'");
            moves.Add("D'");
            moves.Add("L");
            moves.Add("D");
            moves.Add("L'");
            moves.Add("D'");
            moves.Add("F'");

            return true;
        }

        if (state[10] == 'D' && state[11] == 'D' && state[18] == 'D' && state[7] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L'");

            moves.Add("L");
            moves.Add("B");
            moves.Add("D");
            moves.Add("B'");
            moves.Add("D'");
            moves.Add("B");
            moves.Add("D");
            moves.Add("B'");
            moves.Add("D'");
            moves.Add("L'");

            return true;
        }

        if (state[18] == 'D' && state[18] == 'D' && state[22] == 'D' && state[11] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B'");

            moves.Add("B");
            moves.Add("R");
            moves.Add("D");
            moves.Add("R'");
            moves.Add("D'");
            moves.Add("R");
            moves.Add("D");
            moves.Add("R'");
            moves.Add("D'");
            moves.Add("B'");

            return true;
        }

        if (state[22] == 'D' && state[23] == 'D' && state[6] == 'D' && state[19] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R'"); ;

            moves.Add("R");
            moves.Add("F");
            moves.Add("D");
            moves.Add("F'");
            moves.Add("D'");
            moves.Add("F");
            moves.Add("D");
            moves.Add("F'");
            moves.Add("D'");
            moves.Add("R'");

            return true;
        }

        return false;
    }

    //rezolva cazul Chameleon
    bool SolveChameleon()
    {
        if (state[15] == 'D' && state[13] == 'D' && state[23] == 'D' && state[10] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "F");

            moves.Add("R'");
            moves.Add("D'");
            moves.Add("R");
            moves.Add("D");
            moves.Add("L");
            moves.Add("D'");
            moves.Add("L'");
            moves.Add("F");

            return true;
        }

        if (state[13] == 'D' && state[12] == 'D' && state[7] == 'D' && state[18] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "L");

            moves.Add("F'");
            moves.Add("D'");
            moves.Add("F");
            moves.Add("D");
            moves.Add("B");
            moves.Add("D'");
            moves.Add("B'");
            moves.Add("L");

            return true;
        }

        if (state[12] == 'D' && state[14] == 'D' && state[11] == 'D' && state[22] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "B");

            moves.Add("L'");
            moves.Add("D'");
            moves.Add("L");
            moves.Add("D");
            moves.Add("R");
            moves.Add("D'");
            moves.Add("R'");
            moves.Add("B");

            return true;
        }

        if (state[14] == 'D' && state[15] == 'D' && state[19] == 'D' && state[6] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "R");

            moves.Add("B'");
            moves.Add("D'");
            moves.Add("B");
            moves.Add("D");
            moves.Add("F");
            moves.Add("D'");
            moves.Add("F'");
            moves.Add("R");

            return true;
        }

        return false;
    }

    //rezolva cazul Cam's Ugly Bro
    bool SolveCamsUglyBro1()
    {
        if (state[15] == 'D' && state[12] == 'D' && state[6] == 'D' && state[23] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D");

            moves.Add("R'");
            moves.Add("D'");
            moves.Add("R'");
            moves.Add("D");
            moves.Add("L");
            moves.Add("D'");
            moves.Add("R");
            moves.Add("D");

            return true;
        }

        if (state[13] == 'D' && state[14] == 'D' && state[10] == 'D' && state[7] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D");

            moves.Add("F'");
            moves.Add("D'");
            moves.Add("F'");
            moves.Add("D");
            moves.Add("B");
            moves.Add("D'");
            moves.Add("F");
            moves.Add("D");

            return true;
        }

        if (state[12] == 'D' && state[15] == 'D' && state[18] == 'D' && state[11] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D");

            moves.Add("L'");
            moves.Add("D'");
            moves.Add("L'");
            moves.Add("D");
            moves.Add("R");
            moves.Add("D'");
            moves.Add("L");
            moves.Add("D");

            return true;
        }

        if (state[14] == 'D' && state[13] == 'D' && state[22] == 'D' && state[19] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");

            moves.Add("B'");
            moves.Add("D'");
            moves.Add("B'");
            moves.Add("D");
            moves.Add("F");
            moves.Add("D'");
            moves.Add("B");
            moves.Add("D");

            return true;
        }

        return false;
    }

    bool SolveCamsUglyBro()
    {
        if ((state[15] == 'D' && state[12] == 'D' && state[6] == 'D' && state[23] == 'D') ||
            (state[13] == 'D' && state[14] == 'D' && state[10] == 'D' && state[7] == 'D') ||
            (state[12] == 'D' && state[15] == 'D' && state[18] == 'D' && state[11] == 'D') ||
            (state[14] == 'D' && state[13] == 'D' && state[22] == 'D' && state[19] == 'D'))
        {
            //aplica Little Fish si ajunge la unul din cazurile
            //Little Fish, Big Fish, Double Fish sau No Fish
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D2");
            state = cubeHandler.ApplyMove(state, "F'");

            moves.Add("F");
            moves.Add("D");
            moves.Add("F'");
            moves.Add("D");
            moves.Add("F");
            moves.Add("D2");
            moves.Add("F'");

            //verifica in ce caz OLL e si il rezolva
            if (SolveLittleFish())
            {
                return true;
            }

            if (SolveBigFish())
            {
                return true;
            }

            if (SolveDoubleFish())
            {
                return true;
            }

            if (SolveNoFish())
            {
                return true;
            }

        }

        return false;
    }

    //rezolva cazul Bug Eyes
    bool SolveBugEyes()
    {
        if (state[14] == 'D' && state[12] == 'D' && state[6] == 'D' && state[7] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "L'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "F'");

            moves.Add("F");
            moves.Add("L");
            moves.Add("D");
            moves.Add("L'");
            moves.Add("D'");
            moves.Add("F'");

            return true;
        }

        if (state[15] == 'D' && state[14] == 'D' && state[10] == 'D' && state[11] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "L");
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "B'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "L'");

            moves.Add("L");
            moves.Add("B");
            moves.Add("D");
            moves.Add("B'");
            moves.Add("D'");
            moves.Add("L'");

            return true;
        }

        if (state[13] == 'D' && state[15] == 'D' && state[18] == 'D' && state[19] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "B");
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "R'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "B'");

            moves.Add("B");
            moves.Add("R");
            moves.Add("D");
            moves.Add("R'");
            moves.Add("D'");
            moves.Add("B'");

            return true;
        }

        if (state[15] == 'D' && state[13] == 'D' && state[22] == 'D' && state[23] == 'D')
        {
            state = cubeHandler.ApplyMove(state, "R");
            state = cubeHandler.ApplyMove(state, "F");
            state = cubeHandler.ApplyMove(state, "D");
            state = cubeHandler.ApplyMove(state, "F'");
            state = cubeHandler.ApplyMove(state, "D'");
            state = cubeHandler.ApplyMove(state, "R'");

            moves.Add("R");
            moves.Add("F");
            moves.Add("D");
            moves.Add("F'");
            moves.Add("D'");
            moves.Add("R'");

            return true;
        }

        return false;
    }

    //aplica OLL potrivit in fct de configuratia cubului
    void OLL()
    {
        //daca e deja rezolvata fata alba => nu mai e necesar OLL
        if (IsWhiteFaceSolved(state))
        {
            //UnityEngine.Debug.Log("FATA ALBA DEJA REZOLVATA");
            return;
        }
        //altfel, se aplica algoritmii OLL
        if (SolveLittleFish())
        {
            //UnityEngine.Debug.Log("LITTLE FISH");
            return;
        }
        if (SolveBigFish())
        {
            //UnityEngine.Debug.Log("BIG FISH");
            return;
        }
        if (SolveDoubleFish())
        {
            //UnityEngine.Debug.Log("DOUBLE FISH");
            return;
        }
        if (SolveNoFish())
        {
            //UnityEngine.Debug.Log("NO FISH");
            return;
        }
        if (SolveChameleon())
        {
            //UnityEngine.Debug.Log("CHAMELEON");
            return;
        }
        if (SolveBugEyes())
        {
            //UnityEngine.Debug.Log("BUG EYES");
            return;
        }
        if (SolveCamsUglyBro())
        {
            //UnityEngine.Debug.Log("CAM'S UGLY BRO");
            return;
        }

        //daca nu e in niciunul dintre cazurile de mai sus -> aplica Little Fish 
        //si apoi din nou un algoritm din OLL
        state = cubeHandler.ApplyMove(state, "F");
        state = cubeHandler.ApplyMove(state, "D");
        state = cubeHandler.ApplyMove(state, "F'");
        state = cubeHandler.ApplyMove(state, "D");
        state = cubeHandler.ApplyMove(state, "F");
        state = cubeHandler.ApplyMove(state, "D2");
        state = cubeHandler.ApplyMove(state, "F'");

        moves.Add("F");
        moves.Add("D");
        moves.Add("F'");
        moves.Add("D");
        moves.Add("F");
        moves.Add("D2");
        moves.Add("F'");

        OLL();
    }

    //-----------------------------REZOLVAREA CUBULUI (PLL + TERMINAREA REZOLVARII)

    //aplica algoritmul pt PLL cu fata galbena in fata si cea rosie sus
    void PLLMoves()
    {
        state = cubeHandler.ApplyMove(state, "R'");
        state = cubeHandler.ApplyMove(state, "B");
        state = cubeHandler.ApplyMove(state, "R'");
        state = cubeHandler.ApplyMove(state, "F2");
        state = cubeHandler.ApplyMove(state, "R");
        state = cubeHandler.ApplyMove(state, "B'");
        state = cubeHandler.ApplyMove(state, "R'");
        state = cubeHandler.ApplyMove(state, "F2");
        state = cubeHandler.ApplyMove(state, "R2");

        moves.Add("R'");
        moves.Add("B");
        moves.Add("R'");
        moves.Add("F2");
        moves.Add("R");
        moves.Add("B'");
        moves.Add("R'");
        moves.Add("F2");
        moves.Add("R2");
    }

    //aplica algoritmul pt PLL cu fata galbena in fata si cea rosie sus
    //cazul 2 (in care sunt 2 piese la fel pe randul 2 al cubului)
    void PLLCase2()
    {
        //aduce cele 2 piese la fel pe fata front
        if (state[6] == state[7])
        {
            state = cubeHandler.ApplyMove(state, "D'");
            moves.Add("D'");
        }
        else
        {
            if (state[22] == state[23])
            {
                state = cubeHandler.ApplyMove(state, "D2");
                moves.Add("D2");
            }
            else
            {
                if (state[18] == state[19])
                {
                    state = cubeHandler.ApplyMove(state, "D");
                    moves.Add("D");
                }
            }
        }
        //aplica algoritmul PLL
        PLLMoves();

    }

    //gestioneaza toate cazurile din PLL
    void PLL()
    {
        //daca randurile sunt deja rezolvate => nu mai e nevoie de PLL
        //=> doar se aliniaza randurile
        if (AreLayersSolved(state))
        {
            return;
        }

        //daca nu sunt 2 piese la fel pe o fata laterala, randul 2 
        //=> aplica PLLMoves() a.i. fata rosie e sus si galbena e in fata
        if (state[10] != state[11] && state[18] != state[19] &&
            state[6] != state[7] && state[22] != state[23])
        {
            PLLMoves();
            //dupa aplicarea lui PLLMoves() se reduce la cazul in care sunt 2 piese la fel pe o fata
            //=> se aplica cazul 2
            PLLCase2();
        }
        else
        {
            //daca sunt 2 piese la fel => se aplica cazul 2
            PLLCase2();
        }
    }

    //termina rezolvarea cubului (aliniaza cele 2 randuri)
    void AllignThe2Layers()
    {
        //se uita ce piese sunt pe fata portocalie, randul2
        //si roteste fata alba pana se aliniaza cele 2 randuri
        if (state[10] == state[11] && state[10] == 'R')
        {
            state = cubeHandler.ApplyMove(state, "D");
            moves.Add("D");
            return;
        }
        if (state[10] == state[11] && state[10] == 'B')
        {
            state = cubeHandler.ApplyMove(state, "D2");
            moves.Add("D2");
            return;
        }
        if (state[10] == state[11] && state[10] == 'L')
        {
            state = cubeHandler.ApplyMove(state, "D'");
            moves.Add("D'");
            return;
        }
    }


    //-----------------------------FUNCTII AUXILIARE

    //verifica daca cele 2 randuri sunt rezolvate complet
    //chiar daca nu sunt aliniate
    //=> adica mai trebuie doar aliniate
    public bool AreLayersSolved(string crtState)
    {
        return (crtState[8] == crtState[9] && crtState[10] == crtState[11] &&
            crtState[4] == crtState[5] && crtState[6] == crtState[7] &&
            crtState[20] == crtState[21] && crtState[22] == crtState[23] &&
            crtState[16] == crtState[17] && crtState[18] == crtState[19] &&
            crtState[4] == crtState[5] && crtState[6] == crtState[7] &&
            crtState[0] == crtState[1] && crtState[2] == crtState[3] &&
            crtState[12] == crtState[13] && crtState[14] == crtState[15]);
    }

    //verifica daca fata galbena si primul rand sunt rezolvate
    public bool IsYellowFaceAndFirstRowSolved(string crtState)
    {
        return (crtState[0] == 'U' && crtState[1] == 'U' && crtState[2] == 'U' && crtState[3] == 'U' &&
            crtState[4] == 'R' && crtState[5] == 'R' && crtState[8] == 'F' && crtState[9] == 'F' &&
            crtState[16] == 'L' && crtState[17] == 'L' && crtState[20] == 'B' && crtState[21] == 'B');
    }

    //verifica daca fata alba e rezolvata (doar ea, nu si pe fetele laterale)
    public bool IsWhiteFaceSolved(string crtState)
    {
        return (crtState[12] == 'D' && crtState[13] == 'D' && crtState[14] == 'D' && crtState[15] == 'D');
    }
}

