using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the pooling of sound effect holders on scene.
/// </summary>
public class SoundPool : MonoBehaviour
{
    public static SoundPool Instance;
    [Tooltip("The pool of sound effect holders.")]
    public List<GameObject> PooledSounds;
    [Tooltip("Sound effect holder (preferably a prefab.")]
    public GameObject SoundToPool;
    [Tooltip("How many pooled objects should be pre-instantiated on scene.")]
    public int PooledSoundCount;

    /// <summary>
    /// Returns a sound from the pool if available, null otherwise.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledSound()
    {
        for (int i = 0; i < PooledSoundCount; i++)
        {
            if (!PooledSounds[i].activeInHierarchy)
                return PooledSounds[i];
        }
        return null;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PooledSounds = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < PooledSoundCount; i++)
        {
            tmp = Instantiate(SoundToPool);
            tmp.SetActive(false);
            PooledSounds.Add(tmp);
        }
    }
}
