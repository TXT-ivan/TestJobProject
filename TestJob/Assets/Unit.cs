using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets
{
    public class Unit: MonoBehaviour
    {
        public int Damage;
        public int Health;
        public int Speed;
        public int Score = 0;
        public int MaxHealth;

        public Text ScoreText;
        public Slider slider;
        public Vector3 offset;

        public NavMeshAgent Target;
        private NavMeshAgent agent;

        public bool CollisionActive = false;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            MaxHealth = Health;
            agent.speed = Speed;
            slider.maxValue = MaxHealth;
        }

        private void Update()
        {
            if (Target != null && Target.gameObject.activeSelf == true)
            {
                agent.gameObject.transform.LookAt(Target.transform);
                agent.destination = Target.transform.position;
            }
            else
            {
                CollisionActive = false;
                Target = StartScript.GetRandomEnemy(agent);
            }

            ScoreText.text = "Socre: " + Score.ToString();
            slider.value = Health;

            if (Health <= 0)
            {
                CollisionActive = false;
                this.gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.GetComponent(typeof(NavMeshAgent)) == Target)
            {
                if (Target != null)
                {
                    CollisionActive = true;
                    StartCoroutine(DamageCoroutine());
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent(typeof(NavMeshAgent)) == Target)
            {
                if (Target != null)
                {
                    CollisionActive = false;
                }
            }
        }

        /// <summary>
        /// Нанесение урона
        /// </summary>
        IEnumerator DamageCoroutine()
        {
            while(CollisionActive == true)
            {
                if (Target != null)
                {
                    Unit unit = Target.gameObject.GetComponent(typeof(Unit)) as Unit;
                    unit.Health = unit.Health - Damage;

                    if(unit.Health <= 0)
                    {
                        Score++;

                        if((Damage + unit.Damage) < 50)
                        {
                            Damage = Damage + unit.Damage;
                        }
                        else
                        {
                            Damage = 50;
                        }
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}
