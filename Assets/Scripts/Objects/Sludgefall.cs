using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sludgefall : Toggleable
{
    ParticleSystem particles;
    ParticleSystem.EmissionModule emissionModule;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        emissionModule = particles.emission;
    }

    protected override void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        base.Start();
    }

    public override void SetOn(bool state)
    {
        base.SetOn(state);

        emissionModule.enabled = state;
    }

    protected override void SetOnEditor()
    {
        Awake();
        base.SetOnEditor();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            DamageHandler.instance.Respawn();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Sludgefall.png", true);
    }
}
