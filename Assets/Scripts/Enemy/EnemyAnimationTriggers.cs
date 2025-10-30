using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    private static readonly int IsTakingHit = Animator.StringToHash("IsTakingHit");

    [SerializeField]
    private Animator animator;


    public void ResetTakingHitAnimationTrigger()
    {
        animator.SetBool(IsTakingHit, false);
    }

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
