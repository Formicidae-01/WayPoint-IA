using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script onde um objeto simplesmente segue a posi��o de outro objeto (Usado na c�mera)
public class Follow : MonoBehaviour
{
    // Transform alvo para seguir
    public Transform target;

    // Valor que suaviza a seguida.
    public float smooth;

    void Update()
    {
        // Posi��o atualizada com um Lerp feito entre os dois objetos
        transform.position = Vector3.Lerp(transform.position, target.position, smooth);
    }
}
