using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Reactor;

namespace DoomScroll
{
    // Resposible for loading images embedded in current assembly
    public static class ImageLoader
    {
        public static Sprite ReadImageFromAssembly(Assembly assembly, string resource)
        {
            // uses ARGB32 as texture format, asset needs to be .png!!
            Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            try
            {
                Stream texture = assembly.GetManifestResourceStream(resource);
                byte[] imageByte = new byte[texture.Length];
                texture.Read(imageByte, 0, (int)texture.Length);
                ImageConversion.LoadImage(tex, imageByte, false);
            }
            catch (Exception e)
            {
                Logger<DoomScrollPlugin>.Error("Failed to load image from assembly: " + e);
                throw e;
            }

            return Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );

        }
    }
}
