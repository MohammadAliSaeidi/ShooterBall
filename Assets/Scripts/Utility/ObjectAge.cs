using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAge : MonoBehaviour
{
    public float Age = 1;
    [Tooltip("if Target is null then Target is this gameObject")]
    public GameObject Target;

    public static ObjectAge AddObjectAge(GameObject whereToAdd, float age)
	{
        if(whereToAdd.GetComponent<ObjectAge>())
		{
            return null;
		}
        ObjectAge oa = whereToAdd.AddComponent<ObjectAge>();
        oa.Age = age;
        return oa;
    }

    void Start()
    {
        if(Target == null)
		{
            Target = gameObject;
		}

        StartCoroutine(Co_Age(Age, Target));
    }

    IEnumerator Co_Age(float age, GameObject target)
	{
        yield return new WaitForSeconds(age);
        Destroy(target);
	}
}
