using System.Collections;
using UnityEngine;

public class SequentialActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToEnable;
    [SerializeField] private float startDelayTime = 1f;
    [SerializeField] private float intervalTime = 0.5f;

    [SerializeField] private AudioSource popSfxPlayer;
    [SerializeField] private AudioClip[] Popsfx;

    private void OnEnable()
    {
        foreach (var obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        if(Popsfx == null)
        {
            popSfxPlayer = GetComponent<AudioSource>();
        }


        StartCoroutine(EnableObjectsWithDelay());


    }

    private IEnumerator EnableObjectsWithDelay()
    {
        yield return new WaitForSeconds(startDelayTime);

        foreach (var obj in objectsToEnable)
        {

            if (obj != null)
            {
                if (popSfxPlayer != null)
                {
                    int num = Random.Range(0, Popsfx.Length);
                    popSfxPlayer.PlayOneShot(Popsfx[num]);
                }

                obj.SetActive(true);
                
                yield return new WaitForSeconds(intervalTime);
            }
        }
    }
}
