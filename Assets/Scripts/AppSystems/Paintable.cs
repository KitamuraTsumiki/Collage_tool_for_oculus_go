using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class controls alpha map of the paintable materials
/// </summary>
[RequireComponent(typeof(Collider))]
public class Paintable : MonoBehaviour {

    [SerializeField]
    private RenderTexture rt;
    private Material mt;
    private Collider collider;

	private void Start () {
        rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        rt.Create();

        mt = GetComponent<Renderer>().material;

        mt.SetTexture("_Alpha", rt);

        collider = GetComponent<Collider>();
    }
	
	private void Update () {
		
	}

    public void PaintAlpha()
    {

    }

    public void PaintAlpha(Vector3 _point, Texture _brush, float _scale = 0.01f)
    {
        Vector2 scale = new Vector2(_scale, _scale);

        RaycastHit hit;
        Vector3 rayDir = collider.ClosestPoint(_point) - _point;
        Vector2 offset_temp = Vector2.zero;
        
        if (!Physics.Raycast(_point, rayDir, out hit)) { return; }
        
        Vector2 offset = hit.textureCoord;
        Graphics.Blit(_brush, rt, scale, offset);
    }
}
