using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieA : Enemies 
{
    protected override void Awake()
    {
        base.Awake();
    }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        this.SetParameter();
    }

    // Update is called once per frame
    protected override void Update()
    {

    }

    void SetParameter()
    {
        this.Level = 1;
        this.Name = "Zombie Type A";
        this.job = Job.ZombieA;
        this.MaxHealth = 20f;
        this.Health = this.MaxHealth;
        this.MaxMental = 100f;
        this.Mental = this.MaxMental;
        this.Damage = 4f;
        this.Protection = 3f;
        this.Endurance = 3f;
        this.Speed = 5f;
        this.Accuracy = 83f;
        this.Dodge = 5f;
        this.Critical = 12f;
        this.Virtue = 0f;
        this.StressRes = 10f;
        this.BleedRes = 29f;
        this.InfectRes = 34f;
        this.StunRes = 10f;
        this.MoveRes = 22f;
        this.DeathBlow = 0f;
    }
}
