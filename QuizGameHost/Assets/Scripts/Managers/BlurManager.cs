using UnityEngine;

public class BlurManager : SingletonBase<BlurManager>
{
    [SerializeField] private GameObject _blurEffect;

    public void ActivateBlurEffect()
    { 
        _blurEffect.SetActive(true);
    }

    public void DeactivateBlurEffect()
    {
        _blurEffect.SetActive(false);
    }
}
