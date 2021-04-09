using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe de Link
//Essa classe representa as liga��es de um primeiro ponto com um segundo, assim como se essa liga��o � apenas de ida ou tamb�m de volta
[System.Serializable]
public struct Link
{
    //Enum que define se a liga��o � apenas de ida (UNI) ou tamb�m de volta (BI)
    public enum direction {UNI, BI}
    //Primeiro objeto da liga��o
    public GameObject node1;
    //Segundo objeto da liga��o
    public GameObject node2;
    //Vari�vel do enum "Direction", explicado nas linhas anteriores
    public direction dir;
}


public class WPManager : MonoBehaviour
{
    //Array de objetos que representam os pontos pelos quais o personagem pode se mover
    public GameObject[] wayPoints;
    //Array de liga��es, classe explicada nas primeiras linhas do script
    public Link[] links;
    //Cria um novo "Graph", classe/script que gera visualmente os caminhos na janela "Scene" e faz os c�lculos que geram o caminho no qual o personagem se move
    public Graph graph = new Graph();

    private void Start()
    {
        //Executa as linhas abaixo caso a array de pontos n�o esteja vazia
        if (wayPoints.Length > 0)
        {
            //Utiliza o m�todo foreach (identifica cada vari�vel em uma array ou lista) para preencher os nodes do "g" (Graphs) com base nos pontos contidos na array desse script
            foreach (GameObject wp in wayPoints)
            {
                //Adicionando os nodes no objeto da classe graph contida nesse script
                graph.AddNode(wp);
            }

            //Utiliza o m�todo foreach para passar as liga��es contidas nesse script at� o objeto da classe graph
            foreach (Link l in links)
            {
                //Adiciona um poss�vel caminho do primeiro objeto da liga��o at� o segundo
                graph.AddEdge(l.node1, l.node2);
                //Se a dire��o da liga��o for "BI", adiciona um poss�vel caminho do segundo objeto da liga��o at� o primeiro
                //Significa que adiciona um caminho de retorno
                if (l.dir == Link.direction.BI)
                {
                    //Adicionando um caminho do segundo objeto da liga��o at� o primeiro (conforme explicado nas linhas acima)
                    graph.AddEdge(l.node2, l.node1);
                }
            }
        }
    }

    private void Update()
    {
        //Desenhando as linhas entre os caminhos
        graph.debugDraw();
    }
}
