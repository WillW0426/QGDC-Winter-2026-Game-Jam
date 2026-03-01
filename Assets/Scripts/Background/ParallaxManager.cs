using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using static UnityEngine.Rendering.DebugUI.Table;

public class ParallaxManager : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public string layerName;
        public float parallaxEffectX;
        public float parallaxEffectY;
        [HideInInspector] public GameObject[] sprites;
        [HideInInspector] public float[] startPosX;
        [HideInInspector] public float startPosY;
        [HideInInspector] public float spriteWidth;
    }

    public ParallaxLayer[] layers;
    public GameObject cam;

    void Start()
    {
        foreach (ParallaxLayer layer in layers)
        {
            // Find the child GameObject by name
            Transform layerParent = transform.Find(layer.layerName);

            // Grab all sprites that are children of that object
            layer.sprites = new GameObject[layerParent.childCount];
            for (int i = 0; i < layerParent.childCount; i++)
            {
                layer.sprites[i] = layerParent.GetChild(i).gameObject;
            }

            layer.spriteWidth = layer.sprites[0].GetComponent<SpriteRenderer>().bounds.size.x;
            layer.startPosY = layer.sprites[0].transform.position.y;
            layer.startPosX = new float[layer.sprites.Length];

            for (int i = 0; i < layer.sprites.Length; i++)
            {
                layer.startPosX[i] = layer.sprites[i].transform.position.x;
            }
        }
    }

    void Update()
    {
        foreach (ParallaxLayer layer in layers)
        {
            for (int i = 0; i < layer.sprites.Length; i++)
            {
                float distX = cam.transform.position.x * layer.parallaxEffectX;
                float distY = cam.transform.position.y * layer.parallaxEffectY;

                layer.sprites[i].transform.position = new Vector3(layer.startPosX[i] + distX, layer.startPosY + distY, layer.sprites[i].transform.position.z);

                float relativeX = cam.transform.position.x * (1 - layer.parallaxEffectX) - layer.startPosX[i];

                if (relativeX > layer.spriteWidth)
                    layer.startPosX[i] += layer.spriteWidth * 3;
                else if (relativeX < -layer.spriteWidth)
                    layer.startPosX[i] -= layer.spriteWidth * 3;
            }
        }
    }
}