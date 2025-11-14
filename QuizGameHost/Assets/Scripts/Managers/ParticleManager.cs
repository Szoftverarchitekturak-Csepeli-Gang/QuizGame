using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ParticleManager : SingletonBase<ParticleManager>
{
    [SerializeField] private ParticleSystem _victoryParticleSystem;
    [SerializeField] private ParticleSystem _defeatParticleSystem;

    public void PlayVictoryParticleSystem()
    {
        _victoryParticleSystem.Play();
    }

    public void PlayDefeatParticleSystem()
    {
        _defeatParticleSystem.Play();
    }
}

