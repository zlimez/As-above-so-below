using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chronellium.SceneSystem
{
    public class LoadingProgressBar : MonoBehaviour
    {
        private Image image;

        private void Awake()
        {
            image = transform.GetComponent<Image>();
        }

        private void Update()
        {
            image.fillAmount = SceneLoader.Instance.GetLoadingProgress();
        }
    }
}