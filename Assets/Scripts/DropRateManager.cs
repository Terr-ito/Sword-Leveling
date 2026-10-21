using NUnit.Framework;
using System.Collections.Generic;
using Unity.AppUI.Core;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{

    [System.Serializable]
    public class Drops
    {
        public string itemName;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    void OnDestroy()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List <Drops> possibleDrops = new List<Drops>();

        foreach (Drops drop in drops)
        {
            if (randomNumber <= drop.dropRate)
            {
                possibleDrops.Add(drop);
                break;
            }
        }

        // Check if there are any possible drops and instantiate one randomly
        if (possibleDrops.Count > 0)
        {
            Drops drop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
