namespace LD51
{
    using System.Collections.Generic;
    using UnityEngine;


    [CreateAssetMenu()]
    public partial class SimpleEnemyBehavior : EnemyBehavior
    {
        private const float DestinationTolerance = 0.01f;

        public bool IsLightSensitive = true;
        public float NonHostileSpeed = 1f;
        public float HostileSpeed = 2f;
        public float AttackingSpeed = 4f;

        public List<Waypoint> Waypoints;

        private int currentWaypointIndex;
        private float idleTimer;
        private bool isTraversing;


        public override void Initialize(Enemy enemy)
        {
            SimpleEnemyBehavior instance = Instantiate(this);
            
            enemy.Behavior = instance;
        }


        public override void Process(Enemy enemy)
        {
            if (!enemy.SpriteRenderer.isVisible)
                return;
            
            // Lights off: Hostile
            if (this.IsLightSensitive
                && Game.LightsOff)
            {
                ProcessHostileBehavior(enemy);
            }
            
            // Lights on: Non-hostile
            else
            {
                ProcessNormalBehavior(enemy);
            }
        }


        private void ProcessNormalBehavior(Enemy enemy)
        {
            enemy.Movement.Speed = this.NonHostileSpeed;
            
            if (this.isTraversing)
            {
                MoveTowardWaypoint(enemy);
            }
            
            else if (this.idleTimer > 0)
            {
                // Do nothing, just wait for idle timer to run out.
                this.idleTimer -= Time.deltaTime;
            }

            else
            {
                IterateWaypoint();
            }
        }


        private void IterateWaypoint()
        {
            this.currentWaypointIndex++;
            if (this.currentWaypointIndex == this.Waypoints.Count)
                this.currentWaypointIndex = 0;
            this.isTraversing = true;
        }


        private void MoveTowardWaypoint(Enemy enemy)
        {
            Waypoint currentWaypoint = this.Waypoints[this.currentWaypointIndex];
            Vector2 waypointDestination = enemy.StartingPosition + currentWaypoint.Destination;
            
            float distanceToWaypoint = Vector2.Distance(
                enemy.transform.position,
                waypointDestination);

            // Reached destination, go idle.
            if (distanceToWaypoint < DestinationTolerance)
            {
                enemy.transform.position = waypointDestination;
                this.isTraversing = false;
                this.idleTimer = currentWaypoint.WaitDuration;
                return;
            }
            
            // Destination not reached, move toward it.
            if (waypointDestination.x < enemy.transform.position.x)
                enemy.Movement.MoveLeft();
            else
                enemy.Movement.MoveRight();
        }


        private void ProcessHostileBehavior(Enemy enemy)
        {
            enemy.Movement.Speed = this.HostileSpeed;
        }
    }
}


#if UNITY_EDITOR
namespace LD51
{
    using UnityEditor;
    using UnityEngine;


    partial class SimpleEnemyBehavior
    {
        public override void OnDrawEnemyGizmos(Enemy enemy)
        {
            if (this.Waypoints == null
                || this.Waypoints.Count == 0)
            {
                return;
            }

            for (int i = 0; i < this.Waypoints.Count; i++)
            {
                Waypoint waypoint = this.Waypoints[i];
                Transform enemyTransform = enemy.transform;
                Vector2 waypointDestination = enemy.StartingPosition + waypoint.Destination;

                Vector2 previousWaypointPosition = i == 0 
                                                       ?  enemy.StartingPosition
                                                       : enemy.StartingPosition + this.Waypoints[i - 1].Destination;
                Debug.DrawLine(previousWaypointPosition, waypointDestination, Color.magenta);
                Gizmos.DrawWireCube(waypointDestination, new Vector3(0, 0.1f, 0f));
                Handles.Label(waypointDestination, (i).ToString());
            }
        }
        
        [CustomEditor(typeof(SimpleEnemyBehavior))]
        public class Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                
                SimpleEnemyBehavior behavior = this.target as SimpleEnemyBehavior;
                EditorGUILayout.LabelField($"{nameof(SimpleEnemyBehavior.currentWaypointIndex)}: {behavior.currentWaypointIndex}");
                EditorGUILayout.Toggle(nameof(SimpleEnemyBehavior.isTraversing), behavior.isTraversing);
                EditorGUILayout.LabelField($"{nameof(SimpleEnemyBehavior.idleTimer)}: {behavior.idleTimer}");
            }
        }
    }
}
#endif