namespace LD51
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;


    public class SpriteImporter
    {
        [MenuItem("Assets/Conform All Sprite Assets")]
        private static void ConformSpriteTextures()
        {
            int count = 0;
            string[] paths = AssetDatabase.GetAllAssetPaths()
                .Where(x => 
                    System.IO.Path.GetExtension(x).ToLower().Contains("png") && x.Contains("Assets/Sprites/"))
                .ToArray();
            
            foreach (string path in paths)
            {
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                if (textureImporter != null)
                {
                    textureImporter.filterMode = FilterMode.Point;
                    textureImporter.spritePixelsPerUnit = 24;
                    textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
                    AssetDatabase.ImportAsset(path);
                    count++;
                }
            }

            Debug.Log($"Processed and conformed {count} sprites.");
        }
    }
}