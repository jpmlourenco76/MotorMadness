using UnityEngine;

public class BlowTrigger : MonoBehaviour
{
    public GameObject explosionPrefab;  // Arraste o objeto "explosion9" aqui

    void OnTriggerEnter(Collider other)
    {
        // Verifica se o collider que entrou em contato é o carro
        if (other.CompareTag("Car"))
        {
            ActivateExplosion();
        }
    }

    void ActivateExplosion()
    {
        // Ativa a explosão
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // Opcional: Destruir o objeto BlowTrigger após a explosão
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Prefab de explosão não atribuído ao BlowTrigger.");
        }
    }
}

