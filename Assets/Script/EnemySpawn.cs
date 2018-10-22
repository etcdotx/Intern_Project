using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public Transform[] location;//0= L1, 1= L2, 3= L3
    public GameObject enemy;
    private int previous = -1;

    public EnemyStats SpawnEnemy() {
        int RIndex;
        do
        {
            RIndex = Random.Range(0, location.Length);
        } while (RIndex == previous);

        previous = RIndex;
        GameObject obj =  Instantiate(enemy);
        obj.transform.SetParent(null);
        obj.transform.position = location[RIndex].position;
        EnemyStats stat = obj.GetComponent<EnemyStats>();
        return stat;        
    }
 
}
