using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clasa care se ocupa cu operatiile care se pot efectua asupra starii cubului
public class CubeHandler
{
    //fct pt determinarea euristicii (pentru A* si MCTS)   
    public int DetermineHValue(string stateString)
    {
        //dictionar ce contine <numele coltului, <locatia, costul aferent locatiei>>
        Dictionary<string, Dictionary<int, int>> cornersCostBasedOnLocation = new Dictionary<string, Dictionary<int, int>>()
    {
        { "OGY", new Dictionary<int, int>()
            {
                { 0, 1 }, { 1, 1 }, { 2, 0 }, { 3, 1 }, { 4, 1 }, { 5, 2 }, { 6, 2 }, { 7, 2 },
                { 8, 2 }, { 9, 2 }, { 10, 1 }, { 11, 2 }, { 12, 2 }, { 13, 1 }, { 14, 1 }, { 15, 2 },
                { 16, 2 }, { 17, 2 }, { 18, 2 }, { 19, 1 }, { 20, 2 }, { 21, 1 }, { 22, 2 }, { 23, 2 }
            }
        },
        { "OBY", new Dictionary<int, int>()
            {
                { 0, 1 }, { 1, 1 }, { 2, 1 }, { 3, 0 }, { 4, 2 }, { 5, 2 }, { 6, 1 }, { 7, 2 },
                { 8, 2 }, { 9, 2 }, { 10, 2 }, { 11, 1 }, { 12, 1 }, { 13, 2 }, { 14, 2 }, { 15, 1 },
                { 16, 2 }, { 17, 1 }, { 18, 2 }, { 19, 2 }, { 20, 1 }, { 21, 2 }, { 22, 2 }, { 23, 2 }
            }
        },
        { "GRY", new Dictionary<int, int>()
            {
                { 0, 0 }, { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 2 }, { 5, 1 }, { 6, 2 }, { 7, 2 },
                { 8, 1 }, { 9, 2 }, { 10, 2 }, { 11, 2 }, { 12, 1 }, { 13, 2 }, { 14, 2 }, { 15, 2 },
                { 16, 2 }, { 17, 2 }, { 18, 1 }, { 19, 2 }, { 20, 2 }, { 21, 2 }, { 22, 2 }, { 23, 1 }
            }
        },
        { "RBY", new Dictionary<int, int>()
            {
                { 0, 1 }, { 1, 0 }, { 2, 1 }, { 3, 1 }, { 4, 2 }, { 5, 2 }, { 6, 2 }, { 7, 1 },
                { 8, 2 }, { 9, 1 }, { 10, 2 }, { 11, 2 }, { 12, 2 }, { 13, 1 }, { 14, 1 }, { 15, 1 },
                { 16, 1 }, { 17, 2 }, { 18, 2 }, { 19, 2 }, { 20, 2 }, { 21, 2 }, { 22, 1 }, { 23, 2 }
            }
        },
        { "OGW", new Dictionary<int, int>()
            {
                { 0, 1 }, { 1, 2 }, { 2, 2 }, { 3, 1 }, { 4, 2 }, { 5, 2 }, { 6, 1 }, { 7, 2 },
                { 8, 1 }, { 9, 2 }, { 10, 2 }, { 11, 2 }, { 12, 0 }, { 13, 1 }, { 14, 1 }, { 15, 1 },
                { 16, 2 }, { 17, 1 }, { 18, 2 }, { 19, 2 }, { 20, 2 }, { 21, 2 }, { 22, 2 }, { 23, 1 }
            }
        },
        { "OBW", new Dictionary<int, int>()
            {
                { 0, 2 }, { 1, 1 }, { 2, 1 }, { 3, 2 }, { 4, 1 }, { 5, 2 }, { 6, 2 }, { 7, 2 },
                { 8, 2 }, { 9, 1 }, { 10, 2 }, { 11, 2 }, { 12, 1 }, { 13, 0 }, { 14, 1 }, { 15, 1 },
                { 16, 2 }, { 17, 2 }, { 18, 2 }, { 19, 1 }, { 20, 2 }, { 21, 2 }, { 22, 1 }, { 23, 2 }
            }
        },
        { "GRW", new Dictionary<int, int>()
            {
                { 0, 2 }, { 1, 1 }, { 2, 1 }, { 3, 2 }, { 4, 2 }, { 5, 2 }, { 6, 2 }, { 7, 1 },
                { 8, 2 }, { 9, 2 }, { 10, 1 }, { 11, 2 }, { 12, 1 }, { 13, 1 }, { 14, 0 }, { 15, 1 },
                { 16, 1 }, { 17, 2 }, { 18, 2 }, { 19, 2 }, { 20, 2 }, { 21, 1 }, { 22, 2 }, { 23, 2 }
            }
        },
        { "RBW", new Dictionary<int, int>()
            {
                { 0, 1 }, { 1, 2 }, { 2, 2 }, { 3, 1 }, { 4, 2 }, { 5, 1 }, { 6, 2 }, { 7, 2 },
                { 8, 2 }, { 9, 2 }, { 10, 2 }, { 11, 1 }, { 12, 1 }, { 13, 1 }, { 14, 1 }, { 15, 0 },
                { 16, 2 }, { 17, 2 }, { 18, 1 }, { 19, 2 }, { 20, 1 }, { 21, 2 }, { 22, 2 }, { 23, 2 }
            }
        }
    };
        int hVal = 0;
        CubeHandler cubeHandler = new CubeHandler();
        //determina locatiile celor 4 colturi galbene
        int OGYLocation = FindOGYLocation(stateString);
        int OBYLocation = FindOBYLocation(stateString);
        int GRYLocation = FindGRYLocation(stateString);
        int RBYLocation = FindRBYLocation(stateString);

        //determina locatiile celor 4 colturi albe
        int OGWLocation = FindOGWLocation(stateString);
        int OBWLocation = FindOBWLocation(stateString);
        int GRWLocation = FindGRWLocation(stateString);
        int RBWLocation = FindRBWLocation(stateString);


        //determina euristica adunand costurile pt cele 4 colturi
        hVal = cornersCostBasedOnLocation["OGY"][OGYLocation] + cornersCostBasedOnLocation["OBY"][OBYLocation] +
            cornersCostBasedOnLocation["GRY"][GRYLocation] + cornersCostBasedOnLocation["RBY"][RBYLocation] +
            cornersCostBasedOnLocation["OGW"][OGWLocation] + cornersCostBasedOnLocation["OBW"][OBWLocation] +
            cornersCostBasedOnLocation["GRW"][GRWLocation] + cornersCostBasedOnLocation["RBW"][RBWLocation];

        return hVal;
    }

    //=============================FUNCTII PT. LOCALIZAREA COLTURILOR
    //locatia colturilor e poz piesei cu galben/alb in state
    private int FindOGYLocation(string state)
    {
        //------------CAZ 1 - coltul galben pe fata laterala, stanga jos
        if (state[10] == 'U' && state[19] == 'L' && state[12] == 'F')//pe fata portocalie
        {
            return 10;
        }
        if (state[6] == 'U'&& state[11] == 'L' && state[13] == 'F')//pe fata albastra
        {
            return 6;
        }
        if (state[22] == 'U' && state[7] == 'L' && state[15] == 'F')//pe fata rosie
        {
            return 22;
        }
        if (state[18] == 'U' && state[23] == 'L' && state[14] == 'F')//pe fata verde
        {
            return 18;
        }

        //------------CAZ 2 - coltul galben pe fata laterala, dreapta jos
        if (state[11] == 'U' && state[6] == 'F' && state[13] == 'L')//pe fata portocalie
        {
            return 11;
        }
        if (state[7] == 'U' && state[22] == 'F' && state[15] == 'L')//pe fata albastra
        {
            return 7;
        }
        if (state[23] == 'U' && state[18] == 'F' && state[14] == 'L')//pe fata rosie
        {
            return 23;
        }
        if (state[19] == 'U' && state[10] == 'F' && state[12] == 'L')//pe fata verde
        {
            return 19;
        }

        //------------CAZ 3 - coltul galben pe fata laterala, stanga sus
        if (state[8] == 'U' && state[17] == 'F' && state[2] == 'L')//pe fata portocalie
        {
            return 8;
        }
        if (state[4] == 'U' && state[9] == 'F' && state[3] == 'L')//pe fata albastra
        {
            return 4;
        }
        if (state[20] == 'U' && state[5] == 'F' && state[1] == 'L')//pe fata rosie
        {
            return 20;
        }
        if (state[16] == 'U' && state[21] == 'F' && state[0] == 'L')//pe fata verde
        {
            return 16;
        }

        //------------CAZ 4 - coltul galben pe fata laterala, dreapta sus
        if (state[9] == 'U' && state[4] == 'L' && state[3] == 'F')//pe fata portocalie
        {
            return 9;
        }
        if (state[5] == 'U' && state[20] == 'L' && state[1] == 'F')//pe fata albastra
        {
            return 5;
        }
        if (state[21] == 'U' && state[16] == 'L' && state[0] == 'F')//pe fata rosie
        {
            return 21;
        }
        if (state[17] == 'U' && state[8] == 'L' && state[2] == 'F')//pe fata verde
        {
            return 17;
        }

        //------------CAZ 5 - coltul galben pe fata alba
        if (state[12] == 'U' && state[10] == 'L' && state[19] == 'F')//pe poz 0
        {
            return 12;
        }
        if (state[13] == 'U' && state[6] == 'L' && state[11] == 'F')//pe poz 1
        {
            return 13;
        }
        if (state[14] == 'U' && state[18] == 'L' && state[23] == 'F')//pe poz 2
        {
            return 14;
        }
        if (state[15] == 'U' && state[22] == 'L' && state[7] == 'F')//pe poz 3
        {
            return 15;
        }

        //------------CAZ 6 - coltul galben pe fata galbena
        if (state[0] == 'U' && state[16] == 'F' && state[21] == 'L')//pe poz 0
        {
            return 0;
        }
        if (state[1] == 'U' && state[20] == 'F' && state[5] == 'L')//pe poz 1
        {
            return 1;
        }
        if (state[2] == 'U' && state[8] == 'F' && state[17] == 'L')//pe poz 2
        {
            return 2;
        }
        if (state[3] == 'U' && state[4] == 'F' && state[9] == 'L')//pe poz 3
        {
            return 3;
        }

        return -1;
    }

    private int FindOBYLocation(string state)
    {
        //------------CAZ 1 - coltul galben pe fata laterala, stanga jos
        if (state[10] == 'U' && state[19] == 'F' && state[12] == 'R')//pe fata portocalie
        {
            return 10;
        }
        if (state[6] == 'U' && state[11] == 'F' && state[13] == 'R')//pe fata albastra
        {
            return 6;
        }
        if (state[22] == 'U' && state[7] == 'F' && state[15] == 'R')//pe fata rosie
        {
            return 22;
        }
        if (state[18] == 'U' && state[23] == 'F' && state[14] == 'R')//pe fata verde
        {
            return 18;
        }

        //------------CAZ 2 - coltul galben pe fata laterala, dreapta jos
        if (state[11] == 'U' && state[6] == 'R' && state[13] == 'F')//pe fata portocalie
        {
            return 11;
        }
        if (state[7] == 'U' && state[22] == 'R' && state[15] == 'F')//pe fata albastra
        {
            return 7;
        }
        if (state[23] == 'U' && state[18] == 'R' && state[14] == 'F')//pe fata rosie
        {
            return 23;
        }
        if (state[19] == 'U' && state[10] == 'R' && state[12] == 'F')//pe fata verde
        {
            return 19;
        }

        //------------CAZ 3 - coltul galben pe fata laterala, stanga sus
        if (state[8] == 'U' && state[17] == 'R' && state[2] == 'F')//pe fata portocalie
        {
            return 8;
        }
        if (state[4] == 'U' && state[9] == 'R' && state[3] == 'F')//pe fata albastra
        {
            return 4;
        }
        if (state[20] == 'U' && state[5] == 'R' && state[1] == 'F')//pe fata rosie
        {
            return 20;
        }
        if (state[16] == 'U' && state[21] == 'R' && state[0] == 'F')//pe fata verde
        {
            return 16;
        }

        //------------CAZ 4 - coltul galben pe fata laterala, dreapta sus
        if (state[9] == 'U' && state[4] == 'F' && state[3] == 'R')//pe fata portocalie
        {
            return 9;
        }
        if (state[5] == 'U' && state[20] == 'F' && state[1] == 'R')//pe fata albastra
        {
            return 5;
        }
        if (state[21] == 'U' && state[16] == 'F' && state[0] == 'R')//pe fata rosie
        {
            return 21;
        }
        if (state[17] == 'U' && state[8] == 'F' && state[2] == 'R')//pe fata verde
        {
            return 17;
        }

        //------------CAZ 5 - coltul galben pe fata alba
        if (state[12] == 'U' && state[10] == 'F' && state[19] == 'R')//pe poz 0
        {
            return 12;
        }
        if (state[13] == 'U' && state[6] == 'F' && state[11] == 'R')//pe poz 1
        {
            return 13;
        }
        if (state[14] == 'U' && state[18] == 'F' && state[23] == 'R')//pe poz 2
        {
            return 14;
        }
        if (state[15] == 'U' && state[22] == 'F' && state[7] == 'R')//pe poz 3
        {
            return 15;
        }

        //------------CAZ 6 - coltul galben pe fata galbena
        if (state[0] == 'U' && state[16] == 'R' && state[21] == 'F')//pe poz 0
        {
            return 0;
        }
        if (state[1] == 'U' && state[20] == 'R' && state[5] == 'F')//pe poz 1
        {
            return 1;
        }
        if (state[2] == 'U' && state[8] == 'R' && state[17] == 'F')//pe poz 2
        {
            return 2;
        }
        if (state[3] == 'U' && state[4] == 'R' && state[9] == 'F')//pe poz 3
        {
            return 3;
        }
        
        return -1;
    }

    private int FindGRYLocation(string state)
    {
        //------------CAZ 1 - coltul galben pe fata laterala, stanga jos
        if (state[10] == 'U' && state[19] == 'B' && state[12] == 'L')//pe fata portocalie
        {
            return 10;
        }
        if (state[6] == 'U' && state[11] == 'B' && state[13] == 'L')//pe fata albastra
        {
            return 6;
        }
        if (state[22] == 'U' && state[7] == 'B' && state[15] == 'L')//pe fata rosie
        {
            return 22;
        }
        if (state[18] == 'U' && state[23] == 'B' && state[14] == 'L')//pe fata verde
        {
            return 18;
        }

        //------------CAZ 2 - coltul galben pe fata laterala, dreapta jos
        if (state[11] == 'U' && state[6] == 'L' && state[13] == 'B')//pe fata portocalie
        {
            return 11;
        }
        if (state[7] == 'U' && state[22] == 'L' && state[15] == 'B')//pe fata albastra
        {
            return 7;
        }
        if (state[23] == 'U' && state[18] == 'L' && state[14] == 'B')//pe fata rosie
        {
            return 23;
        }
        if (state[19] == 'U' && state[10] == 'L' && state[12] == 'B')//pe fata verde
        {
            return 19;
        }

        //------------CAZ 3 - coltul galben pe fata laterala, stanga sus
        if (state[8] == 'U' && state[17] == 'L' && state[2] == 'B')//pe fata portocalie
        {
            return 8;
        }
        if (state[4] == 'U' && state[9] == 'L' && state[3] == 'B')//pe fata albastra
        {
            return 4;
        }
        if (state[20] == 'U' && state[5] == 'L' && state[1] == 'B')//pe fata rosie
        {
            return 20;
        }
        if (state[16] == 'U' && state[21] == 'L' && state[0] == 'B')//pe fata verde
        {
            return 16;
        }

        //------------CAZ 4 - coltul galben pe fata laterala, dreapta sus
        if (state[9] == 'U' && state[4] == 'B' && state[3] == 'L')//pe fata portocalie
        {
            return 9;
        }
        if (state[5] == 'U' && state[20] == 'B' && state[1] == 'L')//pe fata albastra
        {
            return 5;
        }
        if (state[21] == 'U' && state[16] == 'B' && state[0] == 'L')//pe fata rosie
        {
            return 21;
        }
        if (state[17] == 'U' && state[8] == 'B' && state[2] == 'L')//pe fata verde
        {
            return 17;
        }

        //------------CAZ 5 - coltul galben pe fata alba
        if (state[12] == 'U' && state[10] == 'B' && state[19] == 'L')//pe poz 0
        {
            return 12;
        }
        if (state[13] == 'U' && state[6] == 'B' && state[11] == 'L')//pe poz 1
        {
            return 13;
        }
        if (state[14] == 'U' && state[18] == 'B' && state[23] == 'L')//pe poz 2
        {
            return 14;
        }
        if (state[15] == 'U' && state[22] == 'B' && state[7] == 'L')//pe poz 3
        {
            return 15;
        }

        //------------CAZ 6 - coltul galben pe fata galbena
        if (state[0] == 'U' && state[16] == 'L' && state[21] == 'B')//pe poz 0
        {
            return 0;
        }
        if (state[1] == 'U' && state[20] == 'L' && state[5] == 'B')//pe poz 1
        {
            return 1;
        }
        if (state[2] == 'U' && state[8] == 'L' && state[17] == 'B')//pe poz 2
        {
            return 2;
        }
        if (state[3] == 'U' && state[4] == 'L' && state[9] == 'B')//pe poz 3
        {
            return 3;
        }
        
        return -1;
    }

    private int FindRBYLocation(string state)
    {
        //------------CAZ 1 - coltul galben pe fata laterala, stanga jos
        if (state[10] == 'U' && state[19] == 'R' && state[12] == 'B')//pe fata portocalie
        {
            return 10;
        }
        if (state[6] == 'U' && state[11] == 'R' && state[13] == 'B')//pe fata albastra
        {
            return 6;
        }
        if (state[22] == 'U' && state[7] == 'R' && state[15] == 'B')//pe fata rosie
        {
            return 22;
        }
        if (state[18] == 'U' && state[23] == 'R' && state[14] == 'B')//pe fata verde
        {
            return 18;
        }

        //------------CAZ 2 - coltul galben pe fata laterala, dreapta jos
        if (state[11] == 'U' && state[6] == 'B' && state[13] == 'R')//pe fata portocalie
        {
            return 11;
        }
        if (state[7] == 'U' && state[22] == 'B' && state[15] == 'R')//pe fata albastra
        {
            return 7;
        }
        if (state[23] == 'U' && state[18] == 'B' && state[14] == 'R')//pe fata rosie
        {
            return 23;
        }
        if (state[19] == 'U' && state[10] == 'B' && state[12] == 'R')//pe fata verde
        {
            return 19;
        }

        //------------CAZ 3 - coltul galben pe fata laterala, stanga sus
        if (state[8] == 'U' && state[17] == 'B' && state[2] == 'R')//pe fata portocalie
        {
            return 8;
        }
        if (state[4] == 'U' && state[9] == 'B' && state[3] == 'R')//pe fata albastra
        {
            return 4;
        }
        if (state[20] == 'U' && state[5] == 'B' && state[1] == 'R')//pe fata rosie
        {
            return 20;
        }
        if (state[16] == 'U' && state[21] == 'B' && state[0] == 'R')//pe fata verde
        {
            return 16;
        }

        //------------CAZ 4 - coltul galben pe fata laterala, dreapta sus
        if (state[9] == 'U' && state[4] == 'R' && state[3] == 'B')//pe fata portocalie
        {
            return 9;
        }
        if (state[5] == 'U' && state[20] == 'R' && state[1] == 'B')//pe fata albastra
        {
            return 5;
        }
        if (state[21] == 'U' && state[16] == 'R' && state[0] == 'B')//pe fata rosie
        {
            return 21;
        }
        if (state[17] == 'U' && state[8] == 'R' && state[2] == 'B')//pe fata verde
        {
            return 17;
        }

        //------------CAZ 5 - coltul galben pe fata alba
        if (state[12] == 'U' && state[10] == 'R' && state[19] == 'B')//pe poz 0
        {
            return 12;
        }
        if (state[13] == 'U' && state[6] == 'R' && state[11] == 'B')//pe poz 1
        {
            return 13;
        }
        if (state[14] == 'U' && state[18] == 'R' && state[23] == 'B')//pe poz 2
        {
            return 14;
        }
        if (state[15] == 'U' && state[22] == 'R' && state[7] == 'B')//pe poz 3
        {
            return 15;
        }

        //------------CAZ 6 - coltul galben pe fata galbena
        if (state[0] == 'U' && state[16] == 'B' && state[21] == 'R')//pe poz 0
        {
            return 0;
        }
        if (state[1] == 'U' && state[20] == 'B' && state[5] == 'R')//pe poz 1
        {
            return 1;
        }
        if (state[2] == 'U' && state[8] == 'B' && state[17] == 'R')//pe poz 2
        {
            return 2;
        }
        if (state[3] == 'U' && state[4] == 'B' && state[9] == 'R')//pe poz 3
        {
            return 3;
        }
        
        return -1;
    }

    //pentru fata alba, cazurile sunt construite considerand ca fata alba e in sus
    private int FindOGWLocation(string state)
    {
        //------------CAZ 1 - coltul alb pe fata laterala, stanga jos
        if (state[9] == 'D' && state[4] == 'F' && state[3] == 'L')//pe fata portocalie
        {
            return 9;
        }
        if (state[5] == 'D' && state[20] == 'F' && state[1] == 'L')//pe fata albastra
        {
            return 5;
        }
        if (state[21] == 'D' && state[16] == 'F' && state[0] == 'L')//pe fata rosie
        {
            return 21;
        }
        if (state[17] == 'D' && state[8] == 'F' && state[2] == 'L')//pe fata verde
        {
            return 17;
        }

        //------------CAZ 2 - coltul alb pe fata laterala, dreapta jos
        if (state[8] == 'D' && state[17] == 'L' && state[2] == 'F')//pe fata portocalie
        {
            return 8;
        }
        if (state[4] == 'D' && state[9] == 'L' && state[3] == 'F')//pe fata albastra
        {
            return 4;
        }
        if (state[20] == 'D' && state[5] == 'L' && state[1] == 'F')//pe fata rosie
        {
            return 20;
        }
        if (state[16] == 'D' && state[21] == 'L' && state[0] == 'F')//pe fata verde
        {
            return 16;
        }

        //------------CAZ 3 - coltul alb pe fata laterala, stanga sus
        if (state[11] == 'D' && state[6] == 'L' && state[13] == 'F')//pe fata portocalie
        {
            return 11;
        }
        if (state[7] == 'D' && state[22] == 'L' && state[15] == 'F')//pe fata albastra
        {
            return 7;
        }
        if (state[23] == 'D' && state[18] == 'L' && state[14] == 'F')//pe fata rosie
        {
            return 23;
        }
        if (state[19] == 'D' && state[10] == 'L' && state[12] == 'F')//pe fata verde
        {
            return 19;
        }

        //------------CAZ 4 - coltul alb pe fata laterala, dreapta sus
        if (state[10] == 'D' && state[19] == 'F' && state[12] == 'L')//pe fata portocalie
        {
            return 10;
        }
        if (state[6] == 'D' && state[11] == 'F' && state[13] == 'L')//pe fata albastra
        {
            return 6;
        }
        if (state[22] == 'D' && state[7] == 'F' && state[15] == 'L')//pe fata rosie
        {
            return 22;
        }
        if (state[18] == 'D' && state[23] == 'F' && state[14] == 'L')//pe fata verde
        {
            return 18;
        }

        //------------CAZ 5 - coltul alb pe fata alba
        if (state[12] == 'D' && state[10] == 'F' && state[19] == 'L')//pe poz 0
        {
            return 12;
        }
        if (state[13] == 'D' && state[6] == 'F' && state[11] == 'L')//pe poz 1
        {
            return 13;
        }
        if (state[14] == 'D' && state[18] == 'F' && state[23] == 'L')//pe poz 2
        {
            return 14;
        }
        if (state[15] == 'D' && state[22] == 'F' && state[7] == 'L')//pe poz 3
        {
            return 15;
        }

        //------------CAZ 6 - coltul alb pe fata galbena
        if (state[0] == 'D' && state[16] == 'L' && state[21] == 'F')//pe poz 0
        {
            return 0;
        }
        if (state[1] == 'D' && state[20] == 'L' && state[5] == 'F')//pe poz 1
        {
            return 1;
        }
        if (state[2] == 'D' && state[8] == 'L' && state[17] == 'F')//pe poz 2
        {
            return 2;
        }
        if (state[3] == 'D' && state[4] == 'L' && state[9] == 'F')//pe poz 3
        {
            return 3;
        }
        
        return -1;
    }

    private int FindOBWLocation(string state)
    {
        //------------CAZ 1 - coltul alb pe fata laterala, stanga jos
        if (state[9] == 'D' && state[4] == 'R' && state[3] == 'F')//pe fata portocalie
        {
            return 9;
        }
        if (state[5] == 'D' && state[20] == 'R' && state[1] == 'F')//pe fata albastra
        {
            return 5;
        }
        if (state[21] == 'D' && state[16] == 'R' && state[0] == 'F')//pe fata rosie
        {
            return 21;
        }
        if (state[17] == 'D' && state[8] == 'R' && state[2] == 'F')//pe fata verde
        {
            return 17;
        }

        //------------CAZ 2 - coltul alb pe fata laterala, dreapta jos
        if (state[8] == 'D' && state[17] == 'F' && state[2] == 'R')//pe fata portocalie
        {
            return 8;
        }
        if (state[4] == 'D' && state[9] == 'F' && state[3] == 'R')//pe fata albastra
        {
            return 4;
        }
        if (state[20] == 'D' && state[5] == 'F' && state[1] == 'R')//pe fata rosie
        {
            return 20;
        }
        if (state[16] == 'D' && state[21] == 'F' && state[0] == 'R')//pe fata verde
        {
            return 16;
        }

        //------------CAZ 3 - coltul alb pe fata laterala, stanga sus
        if (state[11] == 'D' && state[6] == 'F' && state[13] == 'R')//pe fata portocalie
        {
            return 11;
        }
        if (state[7] == 'D' && state[22] == 'F' && state[15] == 'R')//pe fata albastra
        {
            return 7;
        }
        if (state[23] == 'D' && state[18] == 'F' && state[14] == 'R')//pe fata rosie
        {
            return 23;
        }
        if (state[19] == 'D' && state[10] == 'F' && state[12] == 'R')//pe fata verde
        {
            return 19;
        }

        //------------CAZ 4 - coltul alb pe fata laterala, dreapta sus
        if (state[10] == 'D' && state[19] == 'R' && state[12] == 'F')//pe fata portocalie
        {
            return 10;
        }
        if (state[6] == 'D' && state[11] == 'R' && state[13] == 'F')//pe fata albastra
        {
            return 6;
        }
        if (state[22] == 'D' && state[7] == 'R' && state[15] == 'F')//pe fata rosie
        {
            return 22;
        }
        if (state[18] == 'D' && state[23] == 'R' && state[14] == 'F')//pe fata verde
        {
            return 18;
        }

        //------------CAZ 5 - coltul alb pe fata alba
        if (state[12] == 'D' && state[10] == 'R' && state[19] == 'F')//pe poz 0
        {
            return 12;
        }
        if (state[13] == 'D' && state[6] == 'R' && state[11] == 'F')//pe poz 1
        {
            return 13;
        }
        if (state[14] == 'D' && state[18] == 'R' && state[23] == 'F')//pe poz 2
        {
            return 14;
        }
        if (state[15] == 'D' && state[22] == 'R' && state[7] == 'F')//pe poz 3
        {
            return 15;
        }

        //------------CAZ 6 - coltul alb pe fata galbena
        if (state[0] == 'D' && state[16] == 'F' && state[21] == 'R')//pe poz 0
        {
            return 0;
        }
        if (state[1] == 'D' && state[20] == 'F' && state[5] == 'R')//pe poz 1
        {
            return 1;
        }
        if (state[2] == 'D' && state[8] == 'F' && state[17] == 'R')//pe poz 2
        {
            return 2;
        }
        if (state[3] == 'D' && state[4] == 'F' && state[9] == 'R')//pe poz 3
        {
            return 3;
        }
        
        return -1;
    }

    private int FindGRWLocation(string state)
    {
        //------------CAZ 1 - coltul alb pe fata laterala, stanga jos
        if (state[9] == 'D' && state[4] == 'L' && state[3] == 'B')//pe fata portocalie
        {
            return 9;
        }
        if (state[5] == 'D' && state[20] == 'L' && state[1] == 'B')//pe fata albastra
        {
            return 5;
        }
        if (state[21] == 'D' && state[16] == 'L' && state[0] == 'B')//pe fata rosie
        {
            return 21;
        }
        if (state[17] == 'D' && state[8] == 'L' && state[2] == 'B')//pe fata verde
        {
            return 17;
        }

        //------------CAZ 2 - coltul alb pe fata laterala, dreapta jos
        if (state[8] == 'D' && state[17] == 'B' && state[2] == 'L')//pe fata portocalie
        {
            return 8;
        }
        if (state[4] == 'D' && state[9] == 'B' && state[3] == 'L')//pe fata albastra
        {
            return 4;
        }
        if (state[20] == 'D' && state[5] == 'B' && state[1] == 'L')//pe fata rosie
        {
            return 20;
        }
        if (state[16] == 'D' && state[21] == 'B' && state[0] == 'L')//pe fata verde
        {
            return 16;
        }

        //------------CAZ 3 - coltul alb pe fata laterala, stanga sus
        if (state[11] == 'D' && state[6] == 'B' && state[13] == 'L')//pe fata portocalie
        {
            return 11;
        }
        if (state[7] == 'D' && state[22] == 'B' && state[15] == 'L')//pe fata albastra@
        {
            return 7;
        }
        if (state[23] == 'D' && state[18] == 'B' && state[14] == 'L')//pe fata rosie
        {
            return 23;
        }
        if (state[19] == 'D' && state[10] == 'B' && state[12] == 'L')//pe fata verde
        {
            return 19;
        }

        //------------CAZ 4 - coltul alb pe fata laterala, dreapta sus
        if (state[10] == 'D' && state[19] == 'L' && state[12] == 'B')//pe fata portocalie
        {
            return 10;
        }
        if (state[6] == 'D' && state[11] == 'L' && state[13] == 'B')//pe fata albastra
        {
            return 6;
        }
        if (state[22] == 'D' && state[7] == 'L' && state[15] == 'B')//pe fata rosie
        {
            return 22;
        }
        if (state[18] == 'D' && state[23] == 'L' && state[14] == 'B')//pe fata verde
        {
            return 18;
        }

        //------------CAZ 5 - coltul alb pe fata alba
        if (state[12] == 'D' && state[10] == 'L' && state[19] == 'B')//pe poz 0
        {
            return 12;
        }
        if (state[13] == 'D' && state[6] == 'L' && state[11] == 'B')//pe poz 1
        {
            return 13;
        }
        if (state[14] == 'D' && state[18] == 'L' && state[23] == 'B')//pe poz 2
        {
            return 14;
        }
        if (state[15] == 'D' && state[22] == 'L' && state[7] == 'B')//pe poz 3
        {
            return 15;
        }

        //------------CAZ 6 - coltul alb pe fata galbena
        if (state[0] == 'D' && state[16] == 'B' && state[21] == 'L')//pe poz 0
        {
            return 0;
        }
        if (state[1] == 'D' && state[20] == 'B' && state[5] == 'L')//pe poz 1
        {
            return 1;
        }
        if (state[2] == 'D' && state[8] == 'B' && state[17] == 'L')//pe poz 2
        {
            return 2;
        }
        if (state[3] == 'D' && state[4] == 'B' && state[9] == 'L')//pe poz 3
        {
            return 3;
        }
        
        return -1;
    }

    private int FindRBWLocation(string state)
    {
        //------------CAZ 1 - coltul alb pe fata laterala, stanga jos
        if (state[9] == 'D' && state[4] == 'B' && state[3] == 'R')//pe fata portocalie
        {
            return 9;
        }
        if (state[5] == 'D' && state[20] == 'B' && state[1] == 'R')//pe fata albastra
        {
            return 5;
        }
        if (state[21] == 'D' && state[16] == 'B' && state[0] == 'R')//pe fata rosie
        {
            return 21;
        }
        if (state[17] == 'D' && state[8] == 'B' && state[2] == 'R')//pe fata verde
        {
            return 17;
        }

        //------------CAZ 2 - coltul alb pe fata laterala, dreapta jos
        if (state[8] == 'D' && state[17] == 'R' && state[2] == 'B')//pe fata portocalie
        {
            return 8;
        }
        if (state[4] == 'D' && state[9] == 'R' && state[3] == 'B')//pe fata albastra
        {
            return 4;
        }
        if (state[20] == 'D' && state[5] == 'R' && state[1] == 'B')//pe fata rosie
        {
            return 20;
        }
        if (state[16] == 'D' && state[21] == 'R' && state[0] == 'B')//pe fata verde
        {
            return 16;
        }

        //------------CAZ 3 - coltul alb pe fata laterala, stanga sus
        if (state[11] == 'D' && state[6] == 'R' && state[13] == 'B')//pe fata portocalie
        {
            return 11;
        }
        if (state[7] == 'D' && state[22] == 'R' && state[15] == 'B')//pe fata albastra
        {
            return 7;
        }
        if (state[23] == 'D' && state[18] == 'R' && state[14] == 'B')//pe fata rosie
        {
            return 23;
        }
        if (state[19] == 'D' && state[10] == 'R' && state[12] == 'B')//pe fata verde
        {
            return 19;
        }

        //------------CAZ 4 - coltul alb pe fata laterala, dreapta sus
        if (state[10] == 'D' && state[19] == 'B' && state[12] == 'R')//pe fata portocalie
        {
            return 10;
        }
        if (state[6] == 'D' && state[11] == 'B' && state[13] == 'R')//pe fata albastra
        {
            return 6;
        }
        if (state[22] == 'D' && state[7] == 'B' && state[15] == 'R')//pe fata rosie
        {
            return 22;
        }
        if (state[18] == 'D' && state[23] == 'B' && state[14] == 'R')//pe fata verde
        {
            return 18;
        }

        //------------CAZ 5 - coltul alb pe fata alba
        if (state[12] == 'D' && state[10] == 'B' && state[19] == 'R')//pe poz 0
        {
            return 12;
        }
        if (state[13] == 'D' && state[6] == 'B' && state[11] == 'R')//pe poz 1
        {
            return 13;
        }
        if (state[14] == 'D' && state[18] == 'B' && state[23] == 'R')//pe poz 2
        {
            return 14;
        }
        if (state[15] == 'D' && state[22] == 'B' && state[7] == 'R')//pe poz 3
        {
            return 15;
        }

        //------------CAZ 6 - coltul alb pe fata galbena
        if (state[0] == 'D' && state[16] == 'R' && state[21] == 'B')//pe poz 0
        {
            return 0;
        }
        if (state[1] == 'D' && state[20] == 'R' && state[5] == 'B')//pe poz 1
        {
            return 1;
        }
        if (state[2] == 'D' && state[8] == 'R' && state[17] == 'B')//pe poz 2
        {
            return 2;
        }
        if (state[3] == 'D' && state[4] == 'R' && state[9] == 'B')//pe poz 3
        {
            return 3;
        }
        
        return -1;
    }

    //=============================FUNCTII AJUTATOARE

    //fct care returneaza toate mutarile in afara de cea inversa lui lastMove
    public List<string> GetMovesWithoutInverses(string lastMove)
    {
        string[] allMoves = { "U2", "U'", "U", "D2", "D'", "D", "L2", "L'", "L", "R2", "R'", "R", "F2", "F'", "F", "B2", "B'", "B" };
        List<string> allowedMoves = new List<string>(allMoves);

        if (lastMove == null)
        {
            return allowedMoves;
        }

        allowedMoves.Remove(GetInverseMove(lastMove));
        return allowedMoves;
    }

    //determina mutarea inversa lui move
    public string GetInverseMove(string move)
    {
        //pt mutarile de tip X' => ret X
        if (move.EndsWith("'")) return move.TrimEnd('\'');
        //pt mutarile de tip X2 => ret X2
        if (move.EndsWith("2")) return move;
        //pt mutarile de tip X => ret X'
        return move + "'";
    }

    //fct care determina mutarile permise avand in vedere ultima mutare efectuata
    public List<string> GetAllowedMoves(string lastMove)
    {
        string[] allMoves = { "U2", "U'", "U", "D2", "D'", "D", "L2", "L'", "L", "R2", "R'", "R", "F2", "F'", "F", "B2", "B'", "B" };
        List<string> allowedMoves = new List<string>(allMoves);

        if(lastMove == null)
        {
            return allowedMoves;
        }

        string face = lastMove.Substring(0, 1);

        string clockwiseMove = face;
        string counterclockwise = face + "'";
        string doubleMove = face + "2";

        //elimina mutarile irelevante
        allowedMoves.Remove(clockwiseMove);
        allowedMoves.Remove(counterclockwise);
        allowedMoves.Remove(doubleMove);

        return allowedMoves;
    }

    //fct care determina daca cubul este rezolvat
    public bool IsCubeSolved(string crtState)
    {
        return crtState == "UUUURRRRFFFFDDDDLLLLBBBB";
    }

    //=============================SIMULAREA MUTARILOR

    //fct care aplica mutarea move asupra starii cubului
    public string ApplyMove(string crtState, string move)
    {
        //starea rezultata in urma aplicarii mutarii
        string resultedState = "";

        //fetele sub forma de char[]
        char[] yellowFace = crtState.Substring(0, 4).ToCharArray();
        char[] blueFace = crtState.Substring(4, 4).ToCharArray();
        char[] orangeFace = crtState.Substring(8, 4).ToCharArray();
        char[] whiteFace = crtState.Substring(12, 4).ToCharArray();
        char[] greenFace = crtState.Substring(16, 4).ToCharArray();
        char[] redFace = crtState.Substring(20, 4).ToCharArray();

        //in fct de move, aplica mutarea asupra fetelor
        switch (move)
        {
            case "U":
                resultedState = ApplyUMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "F":
                resultedState = ApplyFMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "R":
                resultedState = ApplyRMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "L":
                resultedState = ApplyLMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "B":
                resultedState = ApplyBMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "D":
                resultedState = ApplyDMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "U'":
                resultedState = ApplyUPrimeMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "F'":
                resultedState = ApplyFPrimeMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "R'":
                resultedState = ApplyRPrimeMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "L'":
                resultedState = ApplyLPrimeMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "B'":
                resultedState = ApplyBPrimeMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "D'":
                resultedState = ApplyDPrimeMove(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "U2":
                resultedState = ApplyU2Move(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "F2":
                resultedState = ApplyF2Move(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "R2":
                resultedState = ApplyR2Move(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "L2":
                resultedState = ApplyL2Move(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "B2":
                resultedState = ApplyB2Move(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            case "D2":
                resultedState = ApplyD2Move(yellowFace, blueFace, orangeFace, whiteFace, greenFace, redFace);
                break;
            default:
                break;
        }

        return resultedState;
    }

    //pt a roti fata face in sensul acelor de ceasornic (L, D, etc)
    private void RotateFaceClockwise(ref char[] face)
    {
        char[] newFace = new char[4];
        newFace[0] = face[2];
        newFace[1] = face[0];
        newFace[2] = face[3];
        newFace[3] = face[1];
        face = newFace;
    }

    //pt a roti fata face de doua ori (L2, D2, etc)
    private void DoubleRotateFace(ref char[] face)
    {
        char[] newFace = new char[4];
        newFace[0] = face[3];
        newFace[1] = face[2];
        newFace[2] = face[1];
        newFace[3] = face[0];
        face = newFace;
    }

    //pt a roti fata face in sensul invers acelor de ceasornic (L', D', etc)
    private void RotateFaceCounterClockwise(ref char[] face)
    {
        char[] newFace = new char[4];
        newFace[0] = face[1];
        newFace[1] = face[3];
        newFace[2] = face[0];
        newFace[3] = face[2];
        face = newFace;
    }

    //fct care transforma configuratia sub forma de fete char[] in string
    private string CreateFacesString(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        string cubeStateString = string.Concat(upFace) + string.Concat(rightFace) +
                   string.Concat(frontFace) + string.Concat(downFace) +
                   string.Concat(leftFace) + string.Concat(backFace);
        return cubeStateString;
    }

    //fct care aplica mutarea U
    private string ApplyUMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = frontFace[0];
        temp[1] = frontFace[1];

        //roteste fata up clockwise
        RotateFaceClockwise(ref upFace);

        //roteste celelalte fete conform mutarii U
        frontFace[0] = rightFace[0];
        frontFace[1] = rightFace[1];

        rightFace[0] = backFace[0];
        rightFace[1] = backFace[1];

        backFace[0] = leftFace[0];
        backFace[1] = leftFace[1];

        leftFace[0] = temp[0];
        leftFace[1] = temp[1];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea R
    private string ApplyRMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = upFace[1];
        temp[1] = upFace[3];

        //roteste fata right clockwise
        RotateFaceClockwise(ref rightFace);

        //roteste celelalte fete conform mutarii R
        upFace[1] = frontFace[1];
        upFace[3] = frontFace[3];

        frontFace[1] = downFace[1];
        frontFace[3] = downFace[3];

        downFace[1] = backFace[2];
        downFace[3] = backFace[0];

        backFace[0] = temp[1];
        backFace[2] = temp[0];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea F
    private string ApplyFMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = upFace[2];
        temp[1] = upFace[3];

        //roteste fata front clockwise
        RotateFaceClockwise(ref frontFace);

        //roteste celelalte fete conform mutarii F
        upFace[2] = leftFace[3];
        upFace[3] = leftFace[1];

        leftFace[1] = downFace[0];
        leftFace[3] = downFace[1];

        downFace[0] = rightFace[2];
        downFace[1] = rightFace[0];

        rightFace[0] = temp[0];
        rightFace[2] = temp[1];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea B
    private string ApplyBMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = upFace[0];
        temp[1] = upFace[1];

        //roteste fata back clockwise
        RotateFaceClockwise(ref backFace);

        //roteste celelalte fete conform mutarii B
        upFace[0] = rightFace[1];
        upFace[1] = rightFace[3];

        rightFace[1] = downFace[3];
        rightFace[3] = downFace[2];

        downFace[2] = leftFace[0];
        downFace[3] = leftFace[2];

        leftFace[0] = temp[1];
        leftFace[2] = temp[0];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea L
    private string ApplyLMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = upFace[0];
        temp[1] = upFace[2];

        //roteste fata left clockwise
        RotateFaceClockwise(ref leftFace);

        //roteste celelalte fete conform mutarii L
        upFace[0] = backFace[3];
        upFace[2] = backFace[1];

        backFace[1] = downFace[2];
        backFace[3] = downFace[0];

        downFace[0] = frontFace[0];
        downFace[2] = frontFace[2];

        frontFace[0] = temp[0];
        frontFace[2] = temp[1];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea D
    private string ApplyDMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = frontFace[2];
        temp[1] = frontFace[3];

        //roteste fata down clockwise
        RotateFaceClockwise(ref downFace);

        //roteste celelalte fete conform mutarii D
        frontFace[2] = leftFace[2];
        frontFace[3] = leftFace[3];

        leftFace[2] = backFace[2];
        leftFace[3] = backFace[3];

        backFace[2] = rightFace[2];
        backFace[3] = rightFace[3];

        rightFace[2] = temp[0];
        rightFace[3] = temp[1];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea U'
    private string ApplyUPrimeMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = frontFace[0];
        temp[1] = frontFace[1];

        //roteste fata up counter clockwise
        RotateFaceCounterClockwise(ref upFace);

        //roteste celelalte fete conform mutarii U'
        frontFace[0] = leftFace[0];
        frontFace[1] = leftFace[1];

        leftFace[0] = backFace[0];
        leftFace[1] = backFace[1];

        backFace[0] = rightFace[0];
        backFace[1] = rightFace[1];

        rightFace[0] = temp[0];
        rightFace[1] = temp[1];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea R'
    private string ApplyRPrimeMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = upFace[1];
        temp[1] = upFace[3];

        //roteste fata right counter clockwise
        RotateFaceCounterClockwise(ref rightFace);

        //roteste celelalte fete conform mutarii R'
        upFace[1] = backFace[2];
        upFace[3] = backFace[0];

        backFace[0] = downFace[3];
        backFace[2] = downFace[1];

        downFace[1] = frontFace[1];
        downFace[3] = frontFace[3];

        frontFace[1] = temp[0];
        frontFace[3] = temp[1];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea F'
    private string ApplyFPrimeMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = upFace[2];
        temp[1] = upFace[3];

        //roteste fata front counter clockwise
        RotateFaceCounterClockwise(ref frontFace);

        //roteste celelalte fete conform mutarii F'
        upFace[2] = rightFace[0];
        upFace[3] = rightFace[2];

        rightFace[0] = downFace[1];
        rightFace[2] = downFace[0];

        downFace[0] = leftFace[1];
        downFace[1] = leftFace[3];

        leftFace[1] = temp[1];
        leftFace[3] = temp[0];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea B'
    private string ApplyBPrimeMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = upFace[0];
        temp[1] = upFace[1];

        //roteste fata back counter counter clockwise
        RotateFaceCounterClockwise(ref backFace);

        //roteste celelalte fete conform mutarii B'
        upFace[0] = leftFace[2];
        upFace[1] = leftFace[0];

        leftFace[0] = downFace[2];
        leftFace[2] = downFace[3];

        downFace[2] = rightFace[3];
        downFace[3] = rightFace[1];

        rightFace[1] = temp[0];
        rightFace[3] = temp[1];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea L'
    private string ApplyLPrimeMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = upFace[0];
        temp[1] = upFace[2];

        //roteste fata left counter clockwise
        RotateFaceCounterClockwise(ref leftFace);

        //roteste celelalte fete conform mutarii L'
        upFace[0] = frontFace[0];
        upFace[2] = frontFace[2];

        frontFace[0] = downFace[0];
        frontFace[2] = downFace[2];

        downFace[0] = backFace[3];
        downFace[2] = backFace[1];

        backFace[1] = temp[1];
        backFace[3] = temp[0];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea D'
    private string ApplyDPrimeMove(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[2];
        temp[0] = frontFace[2];
        temp[1] = frontFace[3];

        //roteste fata down counter clockwise
        RotateFaceCounterClockwise(ref downFace);

        //roteste celelalte fete conform mutarii D
        frontFace[2] = rightFace[2];
        frontFace[3] = rightFace[3];

        rightFace[2] = backFace[2];
        rightFace[3] = backFace[3];

        backFace[2] = leftFace[2];
        backFace[3] = leftFace[3];

        leftFace[2] = temp[0];
        leftFace[3] = temp[1];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea U2
    private string ApplyU2Move(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[4];
        temp[0] = frontFace[0];
        temp[1] = frontFace[1];
        temp[2] = rightFace[0];
        temp[3] = rightFace[1];

        //roteste fata up de 2 ori
        DoubleRotateFace(ref upFace);

        //roteste celelalte fete conform mutarii U2
        frontFace[0] = backFace[0];
        frontFace[1] = backFace[1];

        rightFace[0] = leftFace[0];
        rightFace[1] = leftFace[1];

        backFace[0] = temp[0];
        backFace[1] = temp[1];

        leftFace[0] = temp[2];
        leftFace[1] = temp[3];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea D2
    private string ApplyD2Move(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[4];
        temp[0] = frontFace[2];
        temp[1] = frontFace[3];
        temp[2] = rightFace[2];
        temp[3] = rightFace[3];

        //roteste fata down de 2 ori
        DoubleRotateFace(ref downFace);

        //roteste celelalte fete conform mutarii D2
        frontFace[2] = backFace[2];
        frontFace[3] = backFace[3];

        rightFace[2] = leftFace[2];
        rightFace[3] = leftFace[3];

        backFace[2] = temp[0];
        backFace[3] = temp[1];

        leftFace[2] = temp[2];
        leftFace[3] = temp[3];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea F2
    private string ApplyF2Move(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[4];
        temp[0] = upFace[2];
        temp[1] = upFace[3];
        temp[2] = rightFace[0];
        temp[3] = rightFace[2];

        //roteste fata front de 2 ori
        DoubleRotateFace(ref frontFace);

        //roteste celelalte fete conform mutarii F2
        upFace[2] = downFace[1];
        upFace[3] = downFace[0];

        downFace[0] = temp[1];
        downFace[1] = temp[0];

        rightFace[0] = leftFace[3];
        rightFace[2] = leftFace[1];

        leftFace[1] = temp[3];
        leftFace[3] = temp[2];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea R2
    private string ApplyR2Move(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[4];
        temp[0] = upFace[1];
        temp[1] = upFace[3];
        temp[2] = frontFace[1];
        temp[3] = frontFace[3];

        //roteste fata right de 2 ori
        DoubleRotateFace(ref rightFace);

        //roteste celelalte fete conform mutarii R2
        upFace[1] = downFace[1];
        upFace[3] = downFace[3];

        downFace[1] = temp[0];
        downFace[3] = temp[1];

        frontFace[1] = backFace[2];
        frontFace[3] = backFace[0];

        backFace[0] = temp[3];
        backFace[2] = temp[2];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea B2
    private string ApplyB2Move(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[4];
        temp[0] = upFace[0];
        temp[1] = upFace[1];
        temp[2] = rightFace[1];
        temp[3] = rightFace[3];

        //roteste fata back de 2 ori
        DoubleRotateFace(ref backFace);

        //roteste celelalte fete conform mutarii B2
        upFace[0] = downFace[3];
        upFace[1] = downFace[2];

        downFace[2] = temp[1];
        downFace[3] = temp[0];

        rightFace[1] = leftFace[2];
        rightFace[3] = leftFace[0];

        leftFace[0] = temp[3];
        leftFace[2] = temp[2];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }

    //fct care aplica mutarea L2
    private string ApplyL2Move(char[] upFace, char[] rightFace, char[] frontFace,
        char[] downFace, char[] leftFace, char[] backFace)
    {
        char[] temp = new char[4];
        temp[0] = upFace[0];
        temp[1] = upFace[2];
        temp[2] = frontFace[0];
        temp[3] = frontFace[2];

        //roteste fata left de 2 ori
        DoubleRotateFace(ref leftFace);

        //roteste celelalte fete conform mutarii L2
        upFace[0] = downFace[0];
        upFace[2] = downFace[2];

        downFace[0] = temp[0];
        downFace[2] = temp[1];

        frontFace[0] = backFace[3];
        frontFace[2] = backFace[1];

        backFace[1] = temp[3];
        backFace[3] = temp[2];

        //creeaza si returneaza string-ul rezultat
        return CreateFacesString(upFace, rightFace, frontFace, downFace, leftFace, backFace);
    }
}
