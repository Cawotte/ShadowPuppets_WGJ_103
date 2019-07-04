namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AmbientLight : MonoBehaviour
    {
        public void SetLuminosity(float alpha)
        {
            Color originalColor = GetComponent<SpriteRenderer>().color;
            originalColor.a = Mathf.Lerp(0, 1, alpha / 255f);
            GetComponent<SpriteRenderer>().color = originalColor;
        }
    }
}
