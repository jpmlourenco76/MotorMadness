using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Start()
    {
        // Obtém o componente ParticleSystem no mesmo objeto
        particleSystem = GetComponent<ParticleSystem>();

        // Certifica-se de que o sistema de partículas está desativado no início
        particleSystem.Stop();
    }

    public void ActivateExplosion()
    {
        // Ativa o sistema de partículas
        particleSystem.Play();

        // Adicione lógica adicional aqui, se necessário
    }

    public void DeactivateExplosion()
    {
        // Desativa o sistema de partículas
        particleSystem.Stop();

        // Adicione lógica adicional aqui, se necessário
    }
}
