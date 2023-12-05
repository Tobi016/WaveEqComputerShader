using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaveManager : MonoBehaviour
{
    public Material WaveEqMaterial;
    public ComputeShader WaveEq;
    public RenderTexture NState, Nm1State, Np1State;
    public RenderTexture ObstacleTex;
    public Vector2Int resolution;
    public Vector3 effect; //x cord, y cord, strenght
    public float dispersion=0.98f;
    // Start is called before the first frame update
    void Start()
    {
        InitializeTexture(ref NState);
        InitializeTexture(ref Nm1State);
        InitializeTexture(ref Np1State);
        ObstacleTex.enableRandomWrite = true;
        Debug.Assert(ObstacleTex.width == resolution.x && ObstacleTex.height == resolution.y); 
        WaveEqMaterial.mainTexture = NState;
    }
    private void InitializeTexture(ref RenderTexture tex)
    {
        tex = new RenderTexture(resolution.x,resolution.y,1,UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SNorm);
        tex.enableRandomWrite = true;
        tex.Create();
    }
    // Update is called once per frame
    void Update()
    {

        Graphics.CopyTexture(NState,Nm1State);
        Graphics.CopyTexture(Np1State, NState);
        WaveEq.SetTexture(0,"NState",NState);
        WaveEq.SetTexture(0, "Nm1State", Nm1State);
        WaveEq.SetTexture(0, "Np1State", Np1State);
        //WaveEq.SetInts("resolution",new int[] { resolution.x,resolution.y});
        WaveEq.SetVector("effect",effect);
        WaveEq.SetVector("resolution",new Vector2( resolution.x,resolution.y));
        WaveEq.SetFloat("dispersion",dispersion);
        WaveEq.SetTexture(0,"ObstacleTex",ObstacleTex);
        WaveEq.Dispatch(0,resolution.x/8,resolution.y/8,1);
    }
}
