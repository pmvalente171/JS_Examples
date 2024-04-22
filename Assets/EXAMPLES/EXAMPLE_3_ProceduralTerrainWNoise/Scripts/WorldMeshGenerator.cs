using System.IO;
using _Scripts;
using UnityEngine;

namespace EXAMPLES.EXAMPLE_3_ProceduralTerrainWNoise.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class WorldMeshGenerator : MonoBehaviour
    {
        private static readonly float[,] QuadVertices =
        {
            {0f, 0f, 0f, 0f},
            {0f, 1f, 0f, 0f},
            {0f, 1f, 1f, 0f},
            {0f, 1f, 1f, 0f},
            {0f, 0f, 0f, 1f}
        };

        [Header("World values")] public Vector2Int worldTileSize;
        public Vector2 worldRes = new Vector2(0.5f, 0.5f);
        public Vector2 minMaxHeight = new Vector2(-1, 1);

        [Header("Field Texture")] public int TextureLength = 1024;

        public Texture2D texture = null;
        public Material material;

        [Space] [InspectorButton("OnButtonClicked")]
        public bool generate;

        [Space] [InspectorButton("OnGenerateTexture")]
        public bool generateTexture;

        private Mesh _mesh;

        private void OnGenerateTexture()
        {
            RenderTexture buffer = new RenderTexture(
                TextureLength,
                TextureLength,
                0, // No depth/stencil buffer
                RenderTextureFormat.ARGB32, // Standard colour format
                RenderTextureReadWrite.sRGB // No sRGB conversions
            );

            texture = new Texture2D(TextureLength, TextureLength, TextureFormat.ARGB32, true);

            MeshRenderer render = GetComponent<MeshRenderer>();

            Graphics.Blit(null, buffer, material);
            RenderTexture.active = buffer;

            texture.ReadPixels(
                new Rect(0, 0, TextureLength, TextureLength), 
                0, 0, 
                false); 

            File.WriteAllBytes(Application.dataPath + "/" + "SkinLut.png", texture.EncodeToPNG());
            Debug.Log($"Image saved: {Application.dataPath + "/" + "SkinLut.png"}");
        }


        private void OnButtonClicked()
        {
            transform.position = new Vector3();
            _mesh = new Mesh();
            _mesh.Clear();
            _mesh = CreatMesh();

            GetComponent<MeshFilter>().mesh = _mesh;
            GetComponent<MeshCollider>().sharedMesh = _mesh;
            _mesh.RecalculateNormals();
            
            transform.position += 
                Vector3.back * worldTileSize.y * worldRes.y / 2+ 
                Vector3.left * worldTileSize.x * worldRes.x / 2;
        }

        private Mesh CreatMesh()
        {
            int width = worldTileSize.x, height = worldTileSize.y;
            Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
            Vector2[] uvs = new Vector2[(width + 1) * (height + 1)];
            int[] triangles = new int[width * height * 6];

            for (int i = 0, y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++, i++)
                {
                    var uv = new Vector2(x * 1.0f / width, y * 1.0f / height);
                    float f = texture.GetPixelBilinear(uv.x, uv.y).grayscale;
                    float value = Mathf.Lerp(minMaxHeight.x, minMaxHeight.y, f);
                    vertices[i] = new Vector3(x * worldRes.x, value, y * worldRes.y);
                }
            }

            for (int vi = 0, ti = 0, y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
                    triangles[ti + 5] = vi + width + 2;
                }

                vi++;
            }

            for (int i = 0, y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++, i++)
                {
                    uvs[i] = new Vector2((x * 1.0f) / width, (y * 1.0f) / height);
                }
            }

            Mesh mesh = new Mesh {vertices = vertices, triangles = triangles, uv = uvs};
            return mesh;
        }
    }
}