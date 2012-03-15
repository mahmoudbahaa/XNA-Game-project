using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// TODO: replace this with the type you want to import.
//using TImport = System.String;

namespace XNAnimationPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".tif", ".tiff", DisplayName = "TIFF Importer", DefaultProcessor = "TextureProcessor")]
    public class TiffImporter : ContentImporter<Texture2DContent>
    {
        public override Texture2DContent Import(string filename, ContentImporterContext context)
        {
            Bitmap bitmap = Image.FromFile(filename) as Bitmap;
            var bitmapContent = new PixelBitmapContent<Microsoft.Xna.Framework.Color>(bitmap.Width, bitmap.Height);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    System.Drawing.Color from = bitmap.GetPixel(i, j);
                    Microsoft.Xna.Framework.Color to = new Microsoft.Xna.Framework.Color(from.R, from.G, from.B, from.A);
                    bitmapContent.SetPixel(i, j, to);
                }
            }

            return new Texture2DContent()
            {
                Mipmaps = new MipmapChain(bitmapContent)
            };
        }
    }
}
