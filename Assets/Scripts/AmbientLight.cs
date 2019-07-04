namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AmbientLight : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SetLuminosity(LuminosityManager.Instance.Luminosity);
            LuminosityManager.Instance.OnLuminosityChange += SetLuminosity;
        }
        
        public void SetLuminosity(float alpha)
        {
            Color originalColor = GetComponent<SpriteRenderer>().color;
            originalColor.a = Mathf.Lerp(0, 1, alpha / 255f);
            GetComponent<SpriteRenderer>().color = originalColor;
        }
    }
}
