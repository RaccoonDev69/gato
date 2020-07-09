using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Node
{
    public int x;
    public int y;
    public int player;
}

public class MoveryAnotar
{
    public int score;
    public Node punto;

    public MoveryAnotar(int _score, Node _punto)
    {
        this.score = _score;
        this.punto = _punto;
    }
}

public class script : MonoBehaviour
{
    public GameObject modeloX;
    public GameObject modeloO;
    Node[,] gato = new Node[3, 3];
    public List<MoveryAnotar> hijosScore;
    public GameObject ui;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition.y);
            if ((Input.mousePosition.x <155 && Input.mousePosition.y > 323) && movimientoValido(0,0))
            {
                Debug.Log("entro");
                Node nodo = new Node();
                nodo.x= 0;
                nodo.y = 0;
                nodo.player = 1;
                gato[nodo.x, nodo.y] = nodo;
                callMiniMax(0, 1);

                Node mejorMovimiento = returnMejorMovimiento();
                if (movimientoValido(mejorMovimiento.x, mejorMovimiento.y))
                {
                    Instantiate(modeloO, new Vector3(mejorMovimiento.x, 1, mejorMovimiento.y), Quaternion.identity);
                    mejorMovimiento.player = 2;
                    gato[mejorMovimiento.x, mejorMovimiento.y] = mejorMovimiento;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(Input.mousePosition);
        }
    }

    // metodo GameOver
    public bool seAcabo()
    {
        Text texto = ui.GetComponent<Text>();

        if(getMovimientos().Capacity == 0)
        {
            texto.text = "empate";
            return true;
        }
        if (ganoO())
        {
            texto.text = "O gana";
            return true;
        }
        if (ganoX())
        {
            texto.text = "X gana";
            return true;
        }
        return false;

    }

    // verifica si es un movimiento valido
    public bool movimientoValido(int x, int y)
    {
        if(gato[x,y] != null)
        {
            return true;
        }
        return false;
    }

    // metodo Anotar movimientos
    List<Node> getMovimientos()
    {
        List<Node> resultado = new List<Node>();
        for(int i = 0; i < 3; i++)
        {
            for(int j=0; j < 3; j++)
            {
                if (gato[i,j] == null)
                {
                    Node nodo = new Node();
                    nodo.x = i;
                    nodo.y = j;
                    resultado.Add(nodo);
                }
            }
        }
        return resultado;
    }

    // verificar X gano 
    public bool ganoX()
    {
        //gano la diagonal
        if (gato[0, 0] != null && gato[1, 1] != null && gato[2, 2] != null)
        {
            if (gato[0, 0].player == 2 && gato[1, 1].player == 2 && gato[2, 2].player == 2)
            {
                return true;
            }
        }
        // Gano la diagonal 
        if (gato[0, 2] != null && gato[1, 1] != null && gato[2, 0] != null)
        {
            if (gato[0, 2].player == 2 && gato[1, 1].player == 2 && gato[2, 0].player == 2)
            {
                return true;
            }
        }

        //gano las filas o columnas
        for (int i = 0; i < 3; i++)
        {
            if (gato[i, 0] != null && gato[i, 1] != null && gato[i, 2] != null)
            {
                if (gato[i, 0].player == 2 && gato[i, 1].player == 2 && gato[i, 2].player == 2)
                {
                    return true;
                }
            }

            if (gato[0, i] != null && gato[1, i] != null && gato[2, i] != null)
            {
                if (gato[0, i].player == 2 && gato[1, i].player == 2 && gato[2, i].player == 2)
                {
                    return true;
                }
            }
        }

        return false;

    }

    //verificar O gano 
    public bool ganoO()
    {
        //gano la diagonal
        if (gato[0,0] != null && gato[1,1]!= null && gato[2,2] != null)
        {
            if(gato[0,0].player == 1 && gato[1,1].player == 1 && gato[2,2].player == 1)
            {
                return true;
            }
        }
           // Gano la diagonal 
        if (gato[0, 2] != null && gato[1, 1] != null && gato[2, 0] != null)
        {
            if (gato[0, 2].player == 1 && gato[1, 1].player == 1 && gato[2, 0].player == 1)
            {
                return true;
            }
        }

        //gano las filas o columnas
        for(int i =0; i<3; i++)
        {
            if (gato[i,0]!=null && gato[i,1] != null && gato[i,2] != null)
            {
                if (gato[i,0].player == 1 && gato[i,1].player == 1 && gato[i,2].player == 1)
                {
                    return true;
                }
            }

            if (gato[0, i] != null && gato[1, i] != null && gato[2, i] != null)
            {
                if (gato[0, i].player == 1 && gato[1, i].player == 1 && gato[2, i].player == 1)
                {
                    return true;
                }
            }
        }

        return false;

    }

    // metodo para llamar al algoritmo minimax dado en un turno y profundidad especifica
    public void callMiniMax(int _profundidad, int _turno)
    {
        hijosScore = new List<MoveryAnotar>();
        miniMax(_profundidad, _turno);
    }

    // algoritmo Minimax
    public int miniMax( int _profundidad, int _turno)
    {
        if (ganoX()) { return +1; }
        if (ganoO()) { return -1; }

        List<Node> movimientosDisponibles = getMovimientos();
        if (movimientosDisponibles.Capacity == 0) { return 0; }

        List<int> scores = new List<int>();

        for (int i=0; i< movimientosDisponibles.Count; i++)
        {
            Node punto = movimientosDisponibles[i];

            if (_turno == 1)
            {
                Node x = new Node();
                x.x = punto.x;
                x.y = punto.y;
                x.player = 2;
                gato[punto.x, punto.y] = x;

                int scoreActual = miniMax(_profundidad + 1, 2);
                scores.Add(scoreActual);

                if(_profundidad == 0)
                {
                    MoveryAnotar m = new MoveryAnotar(scoreActual, punto);
                    m.punto = punto;
                    m.score = scoreActual;
                    hijosScore.Add(m);
                }
            }

            else if (_turno == 2)
            {
                Node o = new Node();
                o.x = punto.x;
                o.y = punto.y;
                o.player = 1;
                gato[punto.x, punto.y] = o;
                int scoreActual = miniMax(_profundidad + 1, 1);
                scores.Add(scoreActual);
            }
            gato[punto.x, punto.y] = null;
        }
        return _turno == 1 ? returnMax(scores) : returnMin(scores);
    }
    // regresa el minimo elemento de la lista pasado por ella
    public int returnMin(List<int> _lista)
    {
        int min = 100000;
        int index = -1;

        for(int i=0; i< _lista.Count; i++)
        {
            if (_lista[i] < min)
            {
                min = _lista[i];
                index = i;
            }
        }
        return _lista[index];
    }
    // regresa el maximo elemento de la lista dada 
    public int returnMax(List<int> _lista)
    {
        int max = -100000;
        int index = -1;
        for (int i=0; i< _lista.Count; i++)
        {
            if(_lista[i] > max)
            {
                max = _lista[i];
                index = i; 
            }
        }
        return _lista[index];
    }

    public Node returnMejorMovimiento()
    {
        int MAX = -100000;
        int best = -1;

        for (int i =0; i < hijosScore.Count; i++)
        {
            if(MAX < hijosScore[i].score && movimientoValido(hijosScore[i].punto.x, hijosScore[i].punto.y))
            {
                MAX = hijosScore[i].score;
                best = i;
            }
        }
        if(best > -1)
        {
            return hijosScore[best].punto;
        }
        Node vacio = new Node();
        vacio.x = 0;
        vacio.y = 0;
        return vacio;
    }
}
