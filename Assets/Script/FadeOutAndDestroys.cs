using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAndDestroys : MonoBehaviour
{
    private IEnumerator FadeOutAndDestroy(GameObject attackObject, float duration)
    {
        Renderer renderer = attackObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            Destroy(attackObject, duration); // אם אין רנדרר, פשוט הורסים
            yield break;
        }

        Material material = renderer.material;
        Color startColor = material.color;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            material.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        Destroy(attackObject);
    }
}
