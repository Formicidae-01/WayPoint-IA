using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{

    //Velocidade na qual o personagem se move até seu destino
    public float moveSpeed;

    //É usado para saber o quão próximo o personagem precisa estar do seu destino para que possa prosseguir até o próximo ponto
    public float accuracy;

    //Velocidade de rotação do personagem
    public float rotationSpeed;

    //Gerenciador de WayPoints, que guarda os objetos para onde o Tanque se moverá e suas ligações
    public WPManager wpManager;

    //Array de GameObjects utilizados como pontos, para os quais o personagem (objeto desse script) irá se mover
    GameObject[] wayPoints;

    //Objeto que é o alvo atual do personagem
    public GameObject currentNode;

    //Variável de índice, destaca qual ponto está sendo utilizado atualmente no caminho até o destino
    int wayPointIndex;

    //classe/script que gera visualmente os caminhos na janela "Scene" e faz os cálculos que geram o caminho no qual o personagem se move
    Graph g;


    private void Start()
    {
        //Preenchendo a Array de pontos, utilizando a array de pontos do gerenciador
        wayPoints = wpManager.wayPoints;
        //Recebe o graph do gerenciador de pontos, que é criado lá
        g = wpManager.graph;
        //Determinando o alvo do personagem como o primeiro ponto da array
        currentNode = wayPoints[0];
    }

    // Método que transporta o personagem até o Heliporto
    public void GoToHeli()
    {
        //Gera-se um caminho do atual alvo do personagem (primeiro argumento do método) até o ponto escolhido no segundo argumento
        g.AStar(currentNode, wayPoints[1]);
        //O índice de caminho é reiniciado, significa que o personagem irá se direcionar ao primeiro objeto do caminho
        wayPointIndex = 0;
    }

    // Método que transporta o personagem até as Ruínas
    public void GoToRuin()
    {
        //Gera-se um caminho do atual alvo do personagem (primeiro argumento do método) até o ponto escolhido no segundo argumento
        g.AStar(currentNode, wayPoints[6]);
        //O índice de caminho é reiniciado, significa que o personagem irá se direcionar ao primeiro objeto do caminho
        wayPointIndex = 0;
    }

    // Método que transporta o personagem até a Indústria
    public void GoToIndustry()
    {
        //Gera-se um caminho do atual alvo do personagem (primeiro argumento do método) até o ponto escolhido no segundo argumento
        g.AStar(currentNode, wayPoints[8]);
        //O índice de caminho é reiniciado, significa que o personagem irá se direcionar ao primeiro objeto do caminho
        wayPointIndex = 0;
    }

    //LateUpdate ocorre após os outros Updates (Update padrão ou FixedUpdate)
    private void LateUpdate()
    {
        //Caso o caminho a ser seguido seja inexistente ou caso o índice esteja no máximo, não serão executadas as linhas abaixo do if
        //A segunda condição mostra que o personagem chegou a seu destino
        if (g.getPathLength() == 0 || wayPointIndex == g.getPathLength())
        {
            //"return" é usado nesse caso para determinar onde o método update será finalizado, caso as condições acima sejam verdadeiras
            return;
        }

        //Atualiza constantemente qual o objeto atual para onde o personagem se moverá
        //O objeto usado é o objeto para qual o personagem deve se mover de acordo com o caminho gerado na movimentação do personagem 
        currentNode = g.getPathPoint(wayPointIndex);

        
        //Cálcula a distância entre o personagem e seu destino, caso essa distância seja menor que o valor de acurácia, significa que o personagem está próximo o suficiente para se mover até o próximo ponto
        if (Vector3.Distance(transform.position, g.getPathPoint(wayPointIndex).transform.position) < accuracy)
        {
            //O valor do índice de pontos é aumentado em 1, significa que o ponto utilizado será o próximo no caminho
            wayPointIndex++;
        }

        // Executa cálculos que geram a rotação e movimentação do personagem caso o índice de caminho seja menor que o tamanho da array de caminho
        // Significa que as linhas abaixo ocorrerão caso o personagem não tenha finalizado seu caminho
        if (wayPointIndex < g.getPathLength())
        {
            //Posição final para qual o personagem deve se rotacionar, utilizar a posição Y do personagem faz com que sua rotação X (rotação para cima e baixo) se mantenha a mesma na utilização do vetor
            Vector3 lookAtGoal = new Vector3(g.getPathPoint(wayPointIndex).transform.position.x, transform.position.y, g.getPathPoint(wayPointIndex).transform.position.z);

            //Direção atual para qual o personagem vai se movimentar
            //Subtrair um Vetor por outro, gera um vetor que pode ser utilizado na direção de um vetor até o outro
            Vector3 direction = lookAtGoal - transform.position;

            //Altera a rotação do personagem para que ele se vire gradualmente até seu destino
            //Os métodos "Lerp" ou "Slerp" retornam a interpolação entre um primeiro vetor e um segundo (nesse caso seria de um Quaternion)
            //O terceiro parâmetro indica o quão alta é essa interpolação, se for 0, o retorno será igual ao vetor do primeiro parâmetro, se for 1, será igual ao segundo
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        }

        //O personagem adiciona movimento em relação ao seu próprio eixo Z, significa que ele estará se movimentando para a frente de sí mesmo
        //Esse método funciona para que o personagem se mova até os pontos, pois devido à rotação, a frente do personagem estará sempre se direcionando até seu ponto de destino
        //O valor "Time.deltaTime" é utilizado no cálculo pois com ele, os métodos podem acontecer corretamente indepentendemente do desempenho do jogo em máquinas diferentes
        //Se "Time.deltaTime" não fosse utilizado, haveria a impressão de que o personagem estaria se movendo mais devagar, caso o jogo estivesse sendo executado em com um desempenho muito baixo
        transform.Translate(0, 0, moveSpeed * Time.deltaTime);

    }
}
