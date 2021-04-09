using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{

    //Velocidade na qual o personagem se move at� seu destino
    public float moveSpeed;

    //� usado para saber o qu�o pr�ximo o personagem precisa estar do seu destino para que possa prosseguir at� o pr�ximo ponto
    public float accuracy;

    //Velocidade de rota��o do personagem
    public float rotationSpeed;

    //Gerenciador de WayPoints, que guarda os objetos para onde o Tanque se mover� e suas liga��es
    public WPManager wpManager;

    //Array de GameObjects utilizados como pontos, para os quais o personagem (objeto desse script) ir� se mover
    GameObject[] wayPoints;

    //Objeto que � o alvo atual do personagem
    public GameObject currentNode;

    //Vari�vel de �ndice, destaca qual ponto est� sendo utilizado atualmente no caminho at� o destino
    int wayPointIndex;

    //classe/script que gera visualmente os caminhos na janela "Scene" e faz os c�lculos que geram o caminho no qual o personagem se move
    Graph g;


    private void Start()
    {
        //Preenchendo a Array de pontos, utilizando a array de pontos do gerenciador
        wayPoints = wpManager.wayPoints;
        //Recebe o graph do gerenciador de pontos, que � criado l�
        g = wpManager.graph;
        //Determinando o alvo do personagem como o primeiro ponto da array
        currentNode = wayPoints[0];
    }

    // M�todo que transporta o personagem at� o Heliporto
    public void GoToHeli()
    {
        //Gera-se um caminho do atual alvo do personagem (primeiro argumento do m�todo) at� o ponto escolhido no segundo argumento
        g.AStar(currentNode, wayPoints[1]);
        //O �ndice de caminho � reiniciado, significa que o personagem ir� se direcionar ao primeiro objeto do caminho
        wayPointIndex = 0;
    }

    // M�todo que transporta o personagem at� as Ru�nas
    public void GoToRuin()
    {
        //Gera-se um caminho do atual alvo do personagem (primeiro argumento do m�todo) at� o ponto escolhido no segundo argumento
        g.AStar(currentNode, wayPoints[6]);
        //O �ndice de caminho � reiniciado, significa que o personagem ir� se direcionar ao primeiro objeto do caminho
        wayPointIndex = 0;
    }

    // M�todo que transporta o personagem at� a Ind�stria
    public void GoToIndustry()
    {
        //Gera-se um caminho do atual alvo do personagem (primeiro argumento do m�todo) at� o ponto escolhido no segundo argumento
        g.AStar(currentNode, wayPoints[8]);
        //O �ndice de caminho � reiniciado, significa que o personagem ir� se direcionar ao primeiro objeto do caminho
        wayPointIndex = 0;
    }

    //LateUpdate ocorre ap�s os outros Updates (Update padr�o ou FixedUpdate)
    private void LateUpdate()
    {
        //Caso o caminho a ser seguido seja inexistente ou caso o �ndice esteja no m�ximo, n�o ser�o executadas as linhas abaixo do if
        //A segunda condi��o mostra que o personagem chegou a seu destino
        if (g.getPathLength() == 0 || wayPointIndex == g.getPathLength())
        {
            //"return" � usado nesse caso para determinar onde o m�todo update ser� finalizado, caso as condi��es acima sejam verdadeiras
            return;
        }

        //Atualiza constantemente qual o objeto atual para onde o personagem se mover�
        //O objeto usado � o objeto para qual o personagem deve se mover de acordo com o caminho gerado na movimenta��o do personagem 
        currentNode = g.getPathPoint(wayPointIndex);

        
        //C�lcula a dist�ncia entre o personagem e seu destino, caso essa dist�ncia seja menor que o valor de acur�cia, significa que o personagem est� pr�ximo o suficiente para se mover at� o pr�ximo ponto
        if (Vector3.Distance(transform.position, g.getPathPoint(wayPointIndex).transform.position) < accuracy)
        {
            //O valor do �ndice de pontos � aumentado em 1, significa que o ponto utilizado ser� o pr�ximo no caminho
            wayPointIndex++;
        }

        // Executa c�lculos que geram a rota��o e movimenta��o do personagem caso o �ndice de caminho seja menor que o tamanho da array de caminho
        // Significa que as linhas abaixo ocorrer�o caso o personagem n�o tenha finalizado seu caminho
        if (wayPointIndex < g.getPathLength())
        {
            //Posi��o final para qual o personagem deve se rotacionar, utilizar a posi��o Y do personagem faz com que sua rota��o X (rota��o para cima e baixo) se mantenha a mesma na utiliza��o do vetor
            Vector3 lookAtGoal = new Vector3(g.getPathPoint(wayPointIndex).transform.position.x, transform.position.y, g.getPathPoint(wayPointIndex).transform.position.z);

            //Dire��o atual para qual o personagem vai se movimentar
            //Subtrair um Vetor por outro, gera um vetor que pode ser utilizado na dire��o de um vetor at� o outro
            Vector3 direction = lookAtGoal - transform.position;

            //Altera a rota��o do personagem para que ele se vire gradualmente at� seu destino
            //Os m�todos "Lerp" ou "Slerp" retornam a interpola��o entre um primeiro vetor e um segundo (nesse caso seria de um Quaternion)
            //O terceiro par�metro indica o qu�o alta � essa interpola��o, se for 0, o retorno ser� igual ao vetor do primeiro par�metro, se for 1, ser� igual ao segundo
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        }

        //O personagem adiciona movimento em rela��o ao seu pr�prio eixo Z, significa que ele estar� se movimentando para a frente de s� mesmo
        //Esse m�todo funciona para que o personagem se mova at� os pontos, pois devido � rota��o, a frente do personagem estar� sempre se direcionando at� seu ponto de destino
        //O valor "Time.deltaTime" � utilizado no c�lculo pois com ele, os m�todos podem acontecer corretamente indepentendemente do desempenho do jogo em m�quinas diferentes
        //Se "Time.deltaTime" n�o fosse utilizado, haveria a impress�o de que o personagem estaria se movendo mais devagar, caso o jogo estivesse sendo executado em com um desempenho muito baixo
        transform.Translate(0, 0, moveSpeed * Time.deltaTime);

    }
}
