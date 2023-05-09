using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigSpawner : MonoBehaviour
{
    public GameObject pigPrefab; // Reference to the pig prefab
    public int numPigsToSpawn = 10; // The number of pigs to spawn
    public float spawnRadius = 10f; // The radius within which to spawn the pigs
    public RuntimeAnimatorController animatorController; // Reference to the animator controller
    public AudioClip myAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the pigs
        for (int i = 0; i < numPigsToSpawn; i++)
        {
            // Instantiate a new pig at the specified position
            GameObject newPig = Instantiate(pigPrefab, new Vector3(0f, 0f, 105.5f), Quaternion.identity);
            newPig.tag = "Pig";
            
            // Randomize the pig's rotation
            newPig.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            
            // Add an Animator component to the new pig
            Animator animator = newPig.AddComponent<Animator>();
            
            // Set the animator's controller to the assigned animator controller
            animator.runtimeAnimatorController = animatorController;
            
            // Add the animal_movement component to the new pig
            animal_movement animalMovement = newPig.AddComponent<animal_movement>();
            
            // Add a BoxCollider to the pig
            BoxCollider boxCollider = newPig.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(1f, 1f, 1f);
            boxCollider.center = new Vector3(0f, 0.5f, 0f);
            
            // Add an AudioSource component to the pig
            AudioSource audioSource = newPig.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // Make the sound 3D
            audioSource.maxDistance = 10f; // Set the maximum distance at which the sound can be heard
            audioSource.clip = myAudioClip; // Assign the audio clip you want to play
            audioSource.loop = true;
            audioSource.Play();



            
        }
    }

}
