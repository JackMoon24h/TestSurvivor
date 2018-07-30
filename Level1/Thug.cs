using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thug : Character
{

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () 
    {
        base.Start();
        this.SetParameter();
	}
	
	// Update is called once per frame
    protected override void Update () 
    {
		
	}

    void SetParameter()
    {
        this.Level = 1;
        this.Name = "Pavle";
        this.job = Job.Thug;
        this.MaxHealth = 30f;
        this.Health = this.MaxHealth;
        this.MaxMental = 100f;
        this.Mental = this.MaxMental;
        this.Damage = 6f;
        this.Protection = 4f;
        this.Endurance = 3f;
        this.Speed = 7f;
        this.Accuracy = 87f;
        this.Dodge = 12f;
        this.Critical = 13f;
        this.Virtue = 5f;
        this.StressRes = 20f;
        this.BleedRes = 35f;
        this.InfectRes = 16f;
        this.StunRes = 15f;
        this.MoveRes = 45f;
        this.DeathBlow = 33f;
    }
}