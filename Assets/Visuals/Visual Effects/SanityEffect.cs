using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityEffect : MonoBehaviour
{
    [Header("Color Adjustments")]
    [SerializeField] float colExposureMin;
    [SerializeField] float colExposureMax;

    [Space]
    [SerializeField] float contrastMin;
    [SerializeField] float contrastMax;

    [Header("Bloom")]
    [SerializeField] float bloomMin;
    [SerializeField] float bloomMax;

    [Header("Film Grain")]
    [SerializeField] float filmGrainMin;
    [SerializeField] float filmGrainMax;

    [Header("Vignette ")]
    [SerializeField] float vignetteMin;
    [SerializeField] float vignetteMax;

    [Header("Lens Distortion")]
    [SerializeField] float lensDistortionMin;
    [SerializeField] float lensDistortionMax;


    Volume volume;
    ColorAdjustments colorAdjustments;
    Bloom bloom;
    ChromaticAberration chromatic;
    FilmGrain filmGrain;
    Vignette vignette;
    LensDistortion lensDistortion;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out chromatic);
        volume.profile.TryGet(out filmGrain);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lensDistortion);
    }

    // Update is called once per frame
    void Update()
    {
        float sanityLoss = SanityManager.Instance.SanityLoss;
        colorAdjustments.postExposure.value = Mathf.Lerp(colExposureMin, colExposureMax, sanityLoss);
        colorAdjustments.contrast.value = Mathf.Lerp(contrastMin, contrastMax, sanityLoss);
        bloom.intensity.value = Mathf.Lerp(bloomMin, bloomMax, sanityLoss);
        chromatic.intensity.value = sanityLoss;
        filmGrain.intensity.value = Mathf.Lerp(filmGrainMin, filmGrainMax, sanityLoss);
        vignette.intensity.value = Mathf.Lerp(vignetteMin, vignetteMax, sanityLoss);
        lensDistortion.intensity.value = Mathf.Lerp(lensDistortionMin, lensDistortionMax, sanityLoss);
    }
}
