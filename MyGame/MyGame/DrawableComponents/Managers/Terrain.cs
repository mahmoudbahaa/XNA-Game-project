﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MyGame
{
    /// <summary>
    /// This class represent the Terrain that draw the terrain and has utility methods to deal with the terrain
    /// </summary>
    public class Terrain : DrawableGameComponent, IRenderable
    {
        VertexPositionNormalTexture[] vertices; // Vertex array
        VertexBuffer vertexBuffer; // Vertex buffer
        int[] indices; // Index array
        IndexBuffer indexBuffer; // Index buffer
        float[,] heights; // Array of vertex heights
        float height; // Maximum height of terrain
        float cellSize; // Distance between vertices on x and z axes
        int width, length; // Number of vertices on x and z axes
        int nVertices, nIndices; // Number of vertices and indices
        Effect effect; // Effect used for rendering
        Texture2D heightMap; // Heightmap texture

        Texture2D baseTexture;
        float textureTiling;
        Vector3 lightDirection;
        //Vector2 pos;

        public Texture2D RTexture, BTexture, GTexture, WeightMap;
        public Texture2D DetailTexture;
        public float DetailDistance = 2500;
        public float DetailTextureTiling = 100;

        Camera camera;
        public Terrain(Game game,Camera camera, Texture2D HeightMap, float CellSize, float Height,
            Texture2D BaseTexture, float TextureTiling, Vector3 LightDirection/*,Vector2 pos*/)
            : base(game)
        {
            this.baseTexture = BaseTexture;
            this.camera = camera;
            this.textureTiling = TextureTiling;
            this.lightDirection = LightDirection;
            this.heightMap = HeightMap;
            this.width = HeightMap.Width;
            this.length = HeightMap.Height;
            this.cellSize = CellSize;
            this.height = Height;
            //this.pos = pos;

            effect = Game.Content.Load<Effect>("TerrainEffect");

            // 1 vertex per pixel
            nVertices = width * length;

            // (Width-1) * (Length-1) cells, 2 triangles per cell, 3 indices per triangle
            nIndices = (width - 1) * (length - 1) * 6;

            vertexBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionNormalTexture),
                nVertices, BufferUsage.WriteOnly);

            indexBuffer = new IndexBuffer(Game.GraphicsDevice, IndexElementSize.ThirtyTwoBits,
                nIndices, BufferUsage.WriteOnly);

            getHeights();
            createVertices();
            createIndices();
            genNormals();

            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
            indexBuffer.SetData<int>(indices);
        }

        //To extract the height values from the heightmap.
        private void getHeights()
        {
            //// Extract pixel data
            Color[] heightMapData = new Color[width * length];
            heightMap.GetData<Color>(heightMapData);

            // Create heights[,] array
            heights = new float[width, length];

            // For each pixel
            for (int y = 0; y < length; y++)
                for (int x = 0; x < width; x++)
                {
                    // Get color value (0 - 255)
                    float amt = heightMapData[y * width + x].R;

                    // Scale to (0 - 1)
                    amt /= 255.0f;

                    // Multiply by max height to get final height
                    heights[x, y] = amt * height;
                }
        }

        // Returns the height and steepness of the terrain at point (X, Z)
        public float GetHeightAtPosition(float X, float Z)
        {
            // Clamp coordinates to locations on terrain
            X = MathHelper.Clamp(X, (-width / 2) * cellSize,
                (width / 2) * cellSize);
            Z = MathHelper.Clamp(Z, (-length / 2) * cellSize,
                (length / 2) * cellSize);

            // Map from (-Width/2->Width/2,-Length/2->Length/2) 
            // to (0->Width, 0->Length)
            X += (width / 2f) * cellSize;
            Z += (length / 2f) * cellSize;

            // Map to cell coordinates
            X /= cellSize;
            Z /= cellSize;

            // Truncate coordinates to get coordinates of top left cell vertex
            int x1 = (int)X;
            int z1 = (int)Z;

            x1 = x1 == width ? x1 - 1 : x1;
            z1 = z1 == width ? z1 - 1 : z1;
            // Try to get coordinates of bottom right cell vertex
            int x2 = x1 + 1 == width ? x1 : x1 + 1;
            int z2 = z1 + 1 == length ? z1 : z1 + 1;

            // Get the heights at the two corners of the cell
            float h1 = heights[x1, z1];
            float h2 = heights[x2, z2];

            // Interpolate between the corner vertices' heights
            //float h = MathHelper.Max(h1, h2);
            //if (h < 35) h = 35;
            return MathHelper.Max(h1, h2);
        }

        // Returns the height and steepness of the terrain at point (X, Z)
        public float GetHeightAtPosition(float X, float Z,
            out float Steepness)
        {
            // Clamp coordinates to locations on terrain
            X = MathHelper.Clamp(X, -((width / 2f)-1) * cellSize, ((width/ 2f)-1) * cellSize);
            Z = MathHelper.Clamp(Z, -((length/ 2f)-1) * cellSize, ((length / 2f)-1) * cellSize);

            //X = MathHelper.Clamp(X, (width - 1) * pos.X * cellSize,
            //    (width - 1) * pos.X * cellSize + (width - 1) * cellSize);
            //Z = MathHelper.Clamp(Z, (length - 1) * pos.Y * cellSize,
            //    (length - 1) * pos.Y * cellSize + (length - 1) * cellSize);

            // Map from (-Width/2->Width/2,-Length/2->Length/2) 
            // to (0->Width, 0->Length)
            X += (width / 2f) * cellSize;
            Z += (length / 2f) * cellSize;

            //X -= width * pos.X * cellSize;
            //Z -= length * pos.Y * cellSize;

            // Map to cell coordinates
            X /= cellSize;
            Z /= cellSize;

            // Truncate coordinates to get coordinates of top left cell vertex
            int x1 = ((int)X)%width;
            int z1 = ((int)Z)%length;

            // Try to get coordinates of bottom right cell vertex
            int x2 = x1 + 1 >= width -1 ? x1 : x1 + 1;
            int z2 = z1 + 1 >= length-1 ? z1 : z1 + 1;

            // Get the heights at the two corners of the cell
            float h1 = heights[x1, z1];
            float h2 = heights[x2, z2];

            // Determine steepness (angle between higher and lower vertex of cell)
            Steepness = (float)Math.Atan(Math.Abs((h1 - h2)) /
                (cellSize * Math.Sqrt(2)));

            // Find the average of the amounts lost from coordinates during 
            // truncation above
            float leftOver = ((X - x1) + (Z - z1)) / 2f;

            // Interpolate between the corner vertices' heights
            return MathHelper.Max(h1, h2);// MathHelper.Lerp(h1, h2, leftOver);
        }

        // create vertices for each pixel.
        private void createVertices()
        {
            vertices = new VertexPositionNormalTexture[nVertices];

            // Calculate the position offset that will center the terrain at (0, 0, 0)
            Vector3 offsetToCenter = -new Vector3(((float)width / 2.0f) * cellSize,
               0, ((float)length / 2.0f) * cellSize);

            //Vector3 offsetToCenter = new Vector3((width-3) * pos.X * cellSize,
            //    0, (length-3) *pos.Y * cellSize);

            // For each pixel in the image
            for (int z = 0; z < length; z++)
                for (int x = 0; x < width; x++)
                {
                    // Find position based on grid coordinates and height in heightmap
                    Vector3 position = new Vector3(x * cellSize, heights[x, z],
                        z * cellSize) + offsetToCenter;

                    // UV coordinates range from (0, 0) at grid location (0, 0) to (1, 1) at grid location (width, length)
                    Vector2 uv = new Vector2((float)x / width, (float)z / length);

                    // Create the vertex
                    vertices[z * width + x] = new VertexPositionNormalTexture(position,
                        Vector3.Zero, uv);
                }
        }

        // create indices of the vertices
        private void createIndices()
        {
            indices = new int[nIndices];
            int i = 0;

            // For each cell
            for (int x = 0; x < width - 1; x++)
                for (int z = 0; z < length - 1; z++)
                {
                    // Find the indices of the corners
                    int upperLeft = z * width + x;
                    int upperRight = upperLeft + 1;
                    int lowerLeft = upperLeft + width;
                    int lowerRight = lowerLeft + 1;

                    // Specify upper triangle
                    indices[i++] = upperLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerLeft;

                    // Specify lower triangle
                    indices[i++] = lowerLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerRight;
                }
        }

        private void genNormals()
        {
            // For each triangle
            for (int i = 0; i < nIndices; i += 3)
            {
                // Find the position of each corner of the triangle
                Vector3 v1 = vertices[indices[i]].Position;
                Vector3 v2 = vertices[indices[i + 1]].Position;
                Vector3 v3 = vertices[indices[i + 2]].Position;

                // Cross the vectors between the corners to get the normal
                Vector3 normal = Vector3.Cross(v1 - v2, v1 - v3);
                normal.Normalize();

                // Add the influence of the normal to each vertex in the triangle
                vertices[indices[i]].Normal += normal;
                vertices[indices[i + 1]].Normal += normal;
                vertices[indices[i + 2]].Normal += normal;
            }

            // Average the influences of the triangles touching each vertex
            for (int i = 0; i < nVertices; i++)
                vertices[i].Normal.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            Game.GraphicsDevice.Indices = indexBuffer;

            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["TextureTiling"].SetValue(textureTiling);
            effect.Parameters["LightDirection"].SetValue(lightDirection);
            effect.Parameters["BaseTexture"].SetValue(baseTexture);

            effect.Parameters["RTexture"].SetValue(RTexture);
            effect.Parameters["GTexture"].SetValue(GTexture);
            effect.Parameters["BTexture"].SetValue(BTexture);
            effect.Parameters["WeightMap"].SetValue(WeightMap);

            effect.Parameters["DetailTexture"].SetValue(DetailTexture);
            effect.Parameters["DetailDistance"].SetValue(DetailDistance);
            effect.Parameters["DetailTextureTiling"].SetValue(DetailTextureTiling);

            effect.Techniques[0].Passes[0].Apply();

            Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                nVertices, 0, nIndices / 3);

            base.Draw(gameTime);
        }

        public void Draw()
        {
            Game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            Game.GraphicsDevice.Indices = indexBuffer;

            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["TextureTiling"].SetValue(textureTiling);
            effect.Parameters["LightDirection"].SetValue(lightDirection);
            effect.Parameters["BaseTexture"].SetValue(baseTexture);

            effect.Parameters["RTexture"].SetValue(RTexture);
            effect.Parameters["GTexture"].SetValue(GTexture);
            effect.Parameters["BTexture"].SetValue(BTexture);
            effect.Parameters["WeightMap"].SetValue(WeightMap);

            effect.Parameters["DetailTexture"].SetValue(DetailTexture);
            effect.Parameters["DetailDistance"].SetValue(DetailDistance);
            effect.Parameters["DetailTextureTiling"].SetValue(DetailTextureTiling);

            effect.Techniques[0].Passes[0].Apply();

            Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                nVertices, 0, nIndices / 3);
        }


        public void SetClipPlane(Vector4? Plane)
        {
            effect.Parameters["ClipPlaneEnabled"].SetValue(Plane.HasValue);

            if (Plane.HasValue)
                effect.Parameters["ClipPlane"].SetValue(Plane.Value);
        }
    }
}
