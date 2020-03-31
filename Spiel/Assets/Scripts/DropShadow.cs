using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DropShadow : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject oShadow;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform. position, Vector3.down, out hit, 100f, LayerMask.GetMask("Default"));

        if (isHit)
        {
            oShadow.transform.position = hit.point + Vector3.up * 0.01f;
            float shadowsize = (1 - hit.distance * 0.4f + 0.5f);
            shadowsize = Mathf.Min(1, shadowsize);
            shadowsize = Mathf.Max(0.7f, shadowsize);
            oShadow.transform.localScale = Vector3.one * shadowsize;
        }
       
    }
}
