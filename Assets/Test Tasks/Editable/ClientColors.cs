using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Editable
{
    public class ClientColors : MonoBehaviour
    {
        [SerializeField] private Transform _swatchContainer;
        [SerializeField] private Image _swatchPrefab;
        [SerializeField] private int _maxDisplayedSwatches = 20;

        public List<Color> ReceivedColors { get; private set; } = new List<Color>();
        public event Action ColorsChanged;

        public void RequestColors()
        {
            ClientPacketsHandler.SendColorRequest();
        }

        public void SetColors(List<Color> colors)
        {
            ReceivedColors = colors;

            foreach (Transform child in _swatchContainer)
                Destroy(child.gameObject);

            int displayCount = Mathf.Min(colors.Count, _maxDisplayedSwatches);
            for (int i = 0; i < displayCount; i++)
            {
                var swatch = Instantiate(_swatchPrefab, _swatchContainer);
                swatch.color = colors[i];
            }

            ColorsChanged?.Invoke();
        }
    }
}
