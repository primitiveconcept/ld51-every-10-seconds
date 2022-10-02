namespace LD51
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;


    public class SpriteImporter : AssetPostprocessor
    {
        public void OnPreprocessTexture()
        {
            if (System.IO.Path.GetExtension(this.assetPath).ToLower().Contains("png")
                && this.assetPath.Contains("Assets/Sprites/"))
            {
                ConformTexture(this.assetPath);    
            }
        }


        [MenuItem("Tools/Conform All Sprite Assets")]
        private static void ConformSpriteTextures()
        {
            int count = 0;
            string[] paths = AssetDatabase.GetAllAssetPaths()
                .Where(x => 
                    System.IO.Path.GetExtension(x).ToLower().Contains("png") && x.Contains("Assets/Sprites/"))
                .ToArray();
            
            foreach (string path in paths)
            {
                ConformTexture(path);
                count++;
            }

            Debug.Log($"Processed and conformed {count} sprites.");
        }


        private static void ConformTexture(string path)
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.filterMode = FilterMode.Point;
                textureImporter.spritePixelsPerUnit = 24;
                textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
                textureImporter.mipmapEnabled = false;
                AssetDatabase.ImportAsset(path);
            }
        }
    }
}