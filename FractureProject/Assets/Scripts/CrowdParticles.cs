using UnityEngine;

public class CrowdParticles : MonoBehaviour
{
    public GameObject goodParticles, badParticles;
    
    public ParticleSystem[] goodParticleSystems, badParticleSystems;

    public void ChangeState(bool good)
    {
        goodParticles.SetActive(good);
        badParticles.SetActive(!good);
    }

    public void SetSize(float size, Quaternion rotation)
    {
        foreach (ParticleSystem ps in goodParticleSystems)
        {
            UnityEngine.ParticleSystem.ShapeModule shape = ps.shape;
            UnityEngine.ParticleSystem.EmissionModule emission = ps.emission;

            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(size,1,1);
            emission.rateOverTime = size;
        }
        foreach (ParticleSystem ps in badParticleSystems)
        {
            UnityEngine.ParticleSystem.ShapeModule shape = ps.shape;
            UnityEngine.ParticleSystem.EmissionModule emission = ps.emission;

            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(size,1,1);
            emission.rateOverTime = size;
        }
    }
    
}
