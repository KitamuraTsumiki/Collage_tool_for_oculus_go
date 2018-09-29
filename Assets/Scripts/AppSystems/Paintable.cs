using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour {

    [SerializeField]
    private RenderTexture rt;
    private Material mt;

	private void Start () {
        rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        rt.Create();

        mt = GetComponent<Renderer>().material;

        mt.SetTexture("_Alpha", rt);
    }
	
	private void Update () {
		
	}

    public void PaintAlpha()
    {

    }
}
