using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script onde um objeto simplesmente segue a posição de outro objeto (Usado na câmera)
public class Follow : MonoBehaviour
{
    // Transform alvo para seguir
    public Transform target;

    // Valor que suaviza a seguida.
    public float smooth;

    void Update()
    {
        // Posição atualizada com um Lerp feito entre os dois objetos
        transform.position = Vector3.Lerp(transform.position, target.position, smooth);
    }
}
