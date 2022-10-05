namespace LD51
{
    using UnityEngine;


    public abstract class EnemyBehavior : ScriptableObject
    {
        public abstract void Initialize(Enemy enemy);
        public abstract void Process(Enemy enemy);
        
        public virtual void OnDrawEnemyGizmos(Enemy enemy) { }
        public virtual void OnDrawEnemyGizmosSelected(Enemy enemy) { }
    }
}