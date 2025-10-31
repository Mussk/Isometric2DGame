using UnityEngine;

namespace Enemy
{
    public class EnemyAnimationTriggers : MonoBehaviour
    {
        
        [SerializeField]
        private Animator animator;

        //This method is triggered by animation
        public void ResetTakingHitAnimationTrigger()
        {
            //animator.SetBool(IsTakingHit, false);
        }
        
        //This method is triggered by animation
        public void PlaySoundOnTakingDamage(string enemyType)
        {
            switch(enemyType) { 

                case "skeleton":
                    //AudioManager.PlaySound(AudioManager.AudioLibrary.TribeSceneSounds.SkeletonHit);
                    break;
                case "bat":
                    // AudioManager.PlaySound(AudioManager.AudioLibrary.TribeSceneSounds.BatHit);
                    break;

            }
        }
        
        //This method is triggered by animation
        public void PlaySoundOnDealingDamage(string enemyType)
        {
            switch (enemyType)
            {

                case "skeleton":
                    //  AudioManager.PlaySound(AudioManager.AudioLibrary.TribeSceneSounds.SkeletonAttack);
                    break;
                case "bat":
                    //   AudioManager.PlaySound(AudioManager.AudioLibrary.TribeSceneSounds.BatAttack);
                    break;

            }
        }
        
        //This method is triggered by animation
        public void PlaySoundOnDeath(string enemyType)
        {
            switch (enemyType)
            {

                case "skeleton":
                    //AudioManager.PlaySound(AudioManager.AudioLibrary.TribeSceneSounds.SkeletonDeath);
                    break;
                case "bat":
                    // AudioManager.PlaySound(AudioManager.AudioLibrary.TribeSceneSounds.BatDeath);
                    break;

            }
        }
    }
}
