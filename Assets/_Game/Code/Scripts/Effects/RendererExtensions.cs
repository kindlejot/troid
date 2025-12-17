using System.Collections;
using UnityEngine;

public static class RendrererExtensions
{
    public static IEnumerator HitFlash(this MeshRenderer renderer, float duration = .1f)
    {
        return HitFlashRoutine(renderer, duration);
    }

    private static IEnumerator HitFlashRoutine(MeshRenderer renderer, float duration)
    {
        renderer.material.EnableKeyword("_EMISSION");                        // AI Remove solution
        renderer.material.SetColor("_EmissionColor", new Color(5f, .5f, .5f)); // AI Remove solution
        yield return new WaitForSeconds(duration);
        renderer.material.DisableKeyword("_EMISSION");                       // AI Remove solution
    }
}