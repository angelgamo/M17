using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prova : MonoBehaviour
{
    [SerializeField]
    GameObject[] cubes;
    [SerializeField]
    int size, seedM, seedB, seedT;
    [SerializeField]
    float freqM, freqB, freqT;
    [SerializeField] float ampM;    
    [SerializeField] float ampB ,ampT;    
    [SerializeField] AnimationCurve amplitudBiome;
    [SerializeField] AnimationCurve biomeTransition;
    public void GeneracioDeMapa() {
        Clear();
        seedB = Random.Range(100000, 999999);
        seedM = Random.Range(100000, 999999);
        seedT = Random.Range(100000, 999999);
        Vector3 pos = this.transform.position;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float bioma = Mathf.PerlinNoise((pos.x + i + seedB) / freqB, (pos.z + j) / freqB);
                float y = Mathf.PerlinNoise((pos.x + i + seedM) / freqM, (pos.z + j) / freqM);
                float tree = Mathf.PerlinNoise((pos.x + i + seedT) / freqT, (pos.z + j) / freqT);
                //tree = tree * ampT;
                int cPos = findBiome(bioma);
                y = y * amplitudBiome.Evaluate(bioma) * ampB;
                
                //newBlock.transform.position = new Vector3(pos.x + i, y, pos.z + j);
                y = y * ampM;
                //Debug.Log(y);
                int ymenos = (int)y - 5;
                for (int j2 = ymenos; j2 < y; j2++)
                {
                    if (j2>14)
                    {
                        cPos = 3;
                    }
                    //Debug.Log(y+" "+j2);
                    if (j2 >= y-1 && cPos == 0) {
                        cPos = 1;
                    }
                    GameObject newBlock = Instantiate(cubes[cPos]);
                    newBlock.transform.position = new Vector3(pos.x + i, j2, pos.z + j);
                    newBlock.transform.parent = this.transform;
                }
                Debug.Log(tree+" "+cPos);
                if (tree > .85 && cPos == 1) {
                    GameObject newBlock = Instantiate(cubes[6]);
                    newBlock.transform.position = new Vector3(pos.x + i, y, pos.z + j);
                    newBlock.transform.parent = this.transform;
                    Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                }
                
            }
        }
    }
    int findBiome(float a) {
        if (a < .4f)
            return 0;
        else if (a < .5f)
        {
            var f = biomeTransition.Evaluate(Mathf.InverseLerp(.4f, .5f, a));
            return Random.Range(0f, 1f) < f ? 2 : 0;
        }

        return 2;
    }
    public void Clear()
    {
        for (int i = transform.childCount; i > 0; i--)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }
}
