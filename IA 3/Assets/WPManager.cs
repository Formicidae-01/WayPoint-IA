using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe de Link
//Essa classe representa as ligações de um primeiro ponto com um segundo, assim como se essa ligação é apenas de ida ou também de volta
[System.Serializable]
public struct Link
{
    //Enum que define se a ligação é apenas de ida (UNI) ou também de volta (BI)
    public enum direction {UNI, BI}
    //Primeiro objeto da ligação
    public GameObject node1;
    //Segundo objeto da ligação
    public GameObject node2;
    //Variável do enum "Direction", explicado nas linhas anteriores
    public direction dir;
}


public class WPManager : MonoBehaviour
{
    //Array de objetos que representam os pontos pelos quais o personagem pode se mover
    public GameObject[] wayPoints;
    //Array de ligações, classe explicada nas primeiras linhas do script
    public Link[] links;
    //Cria um novo "Graph", classe/script que gera visualmente os caminhos na janela "Scene" e faz os cálculos que geram o caminho no qual o personagem se move
    public Graph graph = new Graph();

    private void Start()
    {
        //Executa as linhas abaixo caso a array de pontos não esteja vazia
        if (wayPoints.Length > 0)
        {
            //Utiliza o método foreach (identifica cada variável em uma array ou lista) para preencher os nodes do "g" (Graphs) com base nos pontos contidos na array desse script
            foreach (GameObject wp in wayPoints)
            {
                //Adicionando os nodes no objeto da classe graph contida nesse script
                graph.AddNode(wp);
            }

            //Utiliza o método foreach para passar as ligações contidas nesse script até o objeto da classe graph
            foreach (Link l in links)
            {
                //Adiciona um possível caminho do primeiro objeto da ligação até o segundo
                graph.AddEdge(l.node1, l.node2);
                //Se a direção da ligação for "BI", adiciona um possível caminho do segundo objeto da ligação até o primeiro
                //Significa que adiciona um caminho de retorno
                if (l.dir == Link.direction.BI)
                {
                    //Adicionando um caminho do segundo objeto da ligação até o primeiro (conforme explicado nas linhas acima)
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
