using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ParticleManager : SingletonBase<ParticleManager>
{
    [SerializeField] private GameObject _victoryParticleSystem;
    [SerializeField] private GameObject _defeatParticleSystem;

    public void PlayVictoryParticleSystem(Vector3 position)
    {
        _victoryParticleSystem.transform.position = position;
        _victoryParticleSystem.GetComponent<ParticleSystem>().Play();
    }

    public void PlayDefeatParticleSystem(Vector3 position)
    {
        _defeatParticleSystem.transform.position = position;
        _defeatParticleSystem.GetComponent<ParticleSystem>().Play();
    }
}

