using UnityEngine;

namespace CropNamespace{
    public class CropScript : MonoBehaviour
    {
        // The time it takes for the crop to grow in seconds
        public float growthTime = 20f;

        // Reference to the particle system component on the crop game object
        private ParticleSystem particleSystem;

        // The time at which the crop was planted
        private float plantedTime;

        // Whether the crop is ready to harvest
        private bool isReadyToHarvest = false;

        // The original scale of the crop
        private Vector3 originalScale;

        // Start is called before the first frame update
        void Start()
        {
            plantedTime = Time.time;

            // Get the particle system component on the crop game object
            particleSystem = GetComponent<ParticleSystem>();
            particleSystem.Stop();
            originalScale = transform.localScale / 2;
        }

        // Update is called once per frame
        void Update()
        {
            // Check if the crop is ready to harvest
            if (!isReadyToHarvest && Time.time >= plantedTime + growthTime)
            {
                isReadyToHarvest = true;

                // Play the particle effect
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
        // Increase the scale of the crop as it grows
        float growthPercentage = Mathf.Clamp01((Time.time - plantedTime) / growthTime);
        transform.localScale = originalScale + originalScale * growthPercentage;
        }

        // Harvest the crop and destroy the game object
        public void Harvest()
        {
            if (isReadyToHarvest)
            {
                Destroy(gameObject);
            }
        }
    }
}
