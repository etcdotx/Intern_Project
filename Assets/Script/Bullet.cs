using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject target;
    public bool HaveTarget = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (HaveTarget) {
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
            transform.Translate(0, 2f * Time.deltaTime, 0);
        }
	}

    public void getTarget(GameObject _target) {
        target = _target;
        HaveTarget = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "target" && GameObject.ReferenceEquals(target,collision.transform.parent.gameObject)) {
            Destroy(collision.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }
}
