using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanSpawner : MonoBehaviour
{

    public List<GameObject> humansInTile;
    void OnEnable()
    {
        StartCoroutine(waitMethod());

    }

    IEnumerator waitMethod()
    {
        yield return new WaitForSeconds(0.01f);
        foreach (Transform obj in transform)
        {

            GameObject human = HumanObjectPool.instance.GetObjectForType("Man03", true);
            human.transform.parent = obj;
            human.transform.position = obj.position;

            humansInTile.Add(human);

        }
    }
}
