using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCicleManager : MonoBehaviour
{
    [SerializeField]
    private float timeCicle;

    [SerializeField]
    private Material skybox;

    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private AudioSource ForestDay, ForestNight;

    [SerializeField]  private bool night;

    void Start()
    {
        night = true;
        skybox.SetFloat("_CubemapTransition", 0);
        StartCoroutine(CicleSkybox(timeCicle));
    }

    public bool IsNight()
    {
        return night;
    }

    IEnumerator CicleSkybox(float time)
    {
        if (night)
        {
            ForestDay.Play();
            ForestNight.Stop();
            night = false;
        }
        else
        {
            ForestNight.Play();
            ForestDay.Stop();
            night = true;
        }
        yield return new WaitForSeconds(time);
        StartCoroutine(transitionSkybox(time / 2));
        yield break;
    }
    IEnumerator transitionSkybox(float time)
    {
        for (int x = 0; x < 100; x++)
        {
            yield return new WaitForSeconds(time / 100);
            float _CubemapTransition = skybox.GetFloat("_CubemapTransition");
            float _LightIntensity = sunLight.intensity;
            if (night)
            {
               if (_LightIntensity < 1) sunLight.intensity = _LightIntensity + 0.01f;
               if (_CubemapTransition > 0) skybox.SetFloat("_CubemapTransition", _CubemapTransition + -0.01f);
            }
            else
            {
                if (_LightIntensity > 0) sunLight.intensity = _LightIntensity + -0.01f;
                if (_CubemapTransition < 1) skybox.SetFloat("_CubemapTransition", _CubemapTransition + 0.01f);
            }
        }
        StartCoroutine(CicleSkybox(time * 2));
        yield break;
    }
}
