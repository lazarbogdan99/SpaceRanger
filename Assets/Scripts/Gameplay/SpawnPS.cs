using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPS : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public ParticleSystem dustEffect;

    private void Awake()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        // Makes sure we have a GameObject with tag player and with a RigidBody2D component in the Scene
        // Else we exit
        if (playerMovement == null)
        {
            Debug.LogError("Player has no PlayerMovement script");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if the player isn't moving, we stop the particle system
        if (playerMovement.Velocity.sqrMagnitude <= 0.4f)
            dustEffect.Stop();
        // if the particle system is already playing, we don't have to play it again
        else if (!dustEffect.isPlaying)
            dustEffect.Play();
    }
}
