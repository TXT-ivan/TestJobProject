using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    public int UnitCounter;

    public Text FirstPlace;
    public Text SecondPlace;
    public Text ThirdPlace;
    public Text FourthPlace;
    public Text FifthPlace;

    public Camera MainCamera;
    public GameObject Unit;
    public GameObject Plane;

    private static System.Random rnd = new System.Random();

    private static ObservableCollection<UnitObject> UnitObjectCollection = new ObservableCollection<UnitObject>();
    private static ObservableCollection<NavMeshAgent> NavMeshAgentCollection = new ObservableCollection<NavMeshAgent>();

    void Start()
    {
        for(int i = 0; i < UnitCounter; i++)
        {
            GameObject unit = Instantiate(Unit);
            NavMeshAgent NMA = unit.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;

            unit.name = "Cube(" + i +")";
            unit.transform.position = new Vector3(UnityEngine.Random.Range(-10f,10f), 0.5f, UnityEngine.Random.Range(-10f, 10f));
            NavMeshAgentCollection.Add(NMA);
        }

        foreach(NavMeshAgent item in NavMeshAgentCollection)
        {
            Unit unit = item.gameObject.GetComponent(typeof(Unit)) as Unit;
            unit.Damage = rnd.Next(5, 50);
            unit.Health = rnd.Next(1, 100);
            unit.Speed = rnd.Next(2, 10);
            unit.Target = GetRandomEnemy(item);

            UnitObjectCollection.Add(new UnitObject { Agent = item, Unit = unit });
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            try
            {
                UnitObject new_agent = UnitObjectCollection.Where(x => x.Agent.gameObject.activeInHierarchy == false).ElementAt(rnd.Next(0, UnitObjectCollection.Count - 1));
                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit ray_hit))
                {
                    new_agent.Agent.gameObject.transform.position = ray_hit.point;

                    new_agent.Unit.Health = new_agent.Unit.MaxHealth;
                    new_agent.Unit.Target = GetRandomEnemy(new_agent.Agent);
                    new_agent.Unit.CollisionActive = false;

                    new_agent.Agent.gameObject.SetActive(true);
                }
            }
            catch { }
        }

        try
        {
            int counter = 1;

            foreach (UnitObject item in UnitObjectCollection.OrderByDescending(x => x.Unit.Score))
            {
                switch (counter)
                {
                    case 1:
                        FirstPlace.text = "1: " + item.Agent.gameObject.name + " - " + item.Unit.ScoreText.text;
                        break;
                    case 2:
                        SecondPlace.text = "2: " + item.Agent.gameObject.name + " - " + item.Unit.ScoreText.text;
                        break;
                    case 3:
                        ThirdPlace.text = "3: " + item.Agent.gameObject.name + " - " + item.Unit.ScoreText.text;
                        break;
                    case 4:
                        FourthPlace.text = "4: " + item.Agent.gameObject.name + " - " + item.Unit.ScoreText.text;
                        break;
                    case 5:
                        FifthPlace.text = "5: " + item.Agent.gameObject.name + " - " + item.Unit.ScoreText.text;
                        break;
                }

                counter++;
            }
        }
        catch { }
    }

    /// <summary>
    /// Послучение случайного противника для объекта
    /// </summary>
    /// <param name="agent">Объект</param>
    /// <returns></returns>
    public static NavMeshAgent GetRandomEnemy(NavMeshAgent agent)
    {
        try
        {
            int counter = NavMeshAgentCollection.Where(x => x.gameObject.activeInHierarchy == true).Count();

            if (counter > 1)
            {
                NavMeshAgent new_agent = NavMeshAgentCollection.Where(x => x.gameObject.activeInHierarchy == true).ElementAt(rnd.Next(0, NavMeshAgentCollection.Count - 1));

                if (agent != new_agent)
                {
                    return new_agent;
                }
                else
                {
                    return GetRandomEnemy(agent);
                }
            }
        }
        catch { }

        return null;
    }

    private class UnitObject
    {
        public NavMeshAgent Agent { get; set; }
        public Unit Unit { get; set; }
    }
}
