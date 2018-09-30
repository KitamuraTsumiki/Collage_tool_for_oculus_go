using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class PaintPoint : MonoBehaviour {

    [SerializeField]
    private Texture brush;

    private void OnTriggerStay(Collider other)
    {
        var paintable = other.GetComponent<Paintable>();
        if (paintable == null) { return; }

        //float dist = GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        float dist = transform.lossyScale.x;
        paintable.PaintAlpha(transform.position, brush, dist);
    }
}
