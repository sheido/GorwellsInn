﻿using UnityEngine;
using System.Collections;

public class AcidMan : IA {

	/* Mouvement */
	private NavMeshAgent path;
	private float dist;
	private int randDes;
    public bool firstTime;
	//public Transform[] destinations;
	private DataBase pathFinding;
	private string room;
	/**/

	/* Acide */
	public Transform[] acidPrefab;
	private float spawnTime = 1f;
	private int randAcid;
	public Transform spawn;
	private bool spawning;
	/**/

	/* Mort */
	public Animator die;
	/**/

	/* Animation */
	private Personnage player;
	public Animator feet;
	public Animator body;
	/**/

	public override void Start () 
	{
		base.Start ();
        base.salleNumber();
		room = "Salle " + base.salle;
        pathFinding = GameObject.Find (room).GetComponent<DataBase>();
		path = GetComponent<NavMeshAgent> ();
		player = GameObject.Find ("Player").GetComponent<Personnage> ();
		feet = this.transform.FindChild ("AcidMan/Pied").GetComponent<Animator>();
		body = this.transform.FindChild ("AcidMan/HautDuCorps Monstres").GetComponent<Animator>();
		spawning = true;
		die = this.transform.FindChild ("AcidMan").GetComponent<Animator>();
		StartCoroutine (SpawnAcid());
	}

	public override void Update () 
	{
		base.Update ();
		Movement ();
		Acid ();
		die.SetBool("Mort", mort);

		if (mort == true) 
		{
			path.enabled = false;
			spawning = false;
		}
			
		if (this.gameObject.tag != "Dead") 
		{
			if (player.transform.position.z > this.gameObject.transform.position.z)
			{
				feet.SetBool("Vertical", true);
				body.SetBool("Vertical", true);
			}
			else
			{
				feet.SetBool("Vertical", false);
				body.SetBool("Vertical", false);
			}
		}
	}

	void Movement()
	{
		randDes = Random.Range (0, pathFinding.salleAléa.Length);

		if (this.gameObject.tag != "Dead") 
		{
			dist = Vector3.Distance (this.transform.position, pathFinding.salleAléa [randDes].transform.position);

			if (dist < 2 && path.enabled == true || firstTime == false) 
			{
                firstTime = true;
				randDes = Random.Range (0, pathFinding.salleAléa.Length);
				path.destination = pathFinding.salleAléa [randDes].position;
			}
		}
	}

	void Acid()
	{
		if (this.gameObject.tag != "Dead") 
		{
			randAcid = Random.Range (0, acidPrefab.Length);
		}
	}

	IEnumerator SpawnAcid()
	{
		while (spawning = true && this.gameObject.tag != "Dead") 
		{
			yield return new WaitForSeconds (spawnTime);
			Instantiate (acidPrefab [randAcid], spawn.position, spawn.rotation);	
		}
	}
}
