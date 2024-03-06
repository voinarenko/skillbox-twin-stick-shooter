using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyAudio : MonoBehaviour
    {
        [SerializeField] private EnemyAttack _enemyAttack;

        public void FootStep()
        {
            switch (_enemyAttack.Type)
            {
                case EnemyType.SmallMelee:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/SmallMeleeEnemy/Move/SmallEnemyFootStep", GetComponent<Transform>().position);
                    break;
                case EnemyType.BigMelee:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/BigMeleeEnemy/Move/BigEnemyFootStep", GetComponent<Transform>().position);
                    break;
                case EnemyType.Ranged:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/RangedEnemy/Move/RangedEnemyFootStep", GetComponent<Transform>().position);
                    break;
            }
        }

        public void Attack()
        {
            switch (_enemyAttack.Type)
            {
                case EnemyType.SmallMelee:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/SmallMeleeEnemy/Attack/SmallEnemyAttack", GetComponent<Transform>().position);
                    break;
                case EnemyType.BigMelee:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/BigMeleeEnemy/Attack/BigEnemyAttack", GetComponent<Transform>().position);
                    break;
            }
        }

        public void Shoot() => 
            FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/RangedEnemy/Attack/RangedEnemyShoot", GetComponent<Transform>().position);

        public void Reload() => 
            FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/RangedEnemy/Attack/RangedEnemyReload", GetComponent<Transform>().position);
    }
}