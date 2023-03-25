using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletTimeAlive;
    [SerializeField] public float bulletPower;

    [SerializeField] public float entitySpeed;
    [SerializeField] public float entityPower;
    [SerializeField] public float entityHealth;
    [SerializeField] public int radiusSpawn;

    [SerializeField] public float dificulty;
    [SerializeField] public int countEntity;

    [SerializeField] public int coinCount;
    [SerializeField] public int dimondCount;
    [SerializeField] public float animationSpeed;

    [SerializeField] public float playerHealth;
    [SerializeField] public float timeHealing;
    [SerializeField] public int countBullets;
    [SerializeField] public float timeReload;

    public float MC_180_PI = 180 / Mathf.PI;
}
