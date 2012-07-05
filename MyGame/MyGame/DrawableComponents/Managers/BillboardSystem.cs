using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    /// <summary>
    /// This class represent a Billboard System a group of 
    /// 2D textures(pictures) that are always drawn facing the camera
    /// </summary>
    public class BillboardSystem : DrawableGameComponent
    {
        // Vertex buffer and index buffer, particle
        // and index arrays
        VertexBuffer verts;
        IndexBuffer ints;
        VertexPositionTexture[] particles;
        int[] indices;

        // Billboard settings
        int nBillboards;
        Vector2 billboardSize;
        Texture2D texture;

        // GraphicsDevice and Effect
        Effect effect;

        public bool EnsureOcclusion = true;

        public enum BillboardMode { Cylindrical, Spherical };
        public BillboardMode Mode = BillboardMode.Spherical;

        public BillboardSystem(Game game, Texture2D texture,
            Vector2 billboardSize, Vector3[] particlePositions):base(game)
        {
            this.nBillboards = particlePositions.Length;
            this.billboardSize = billboardSize;
            this.texture = texture;

            effect = Game.Content.Load<Effect>("BillboardEffect");

            generateParticles(particlePositions);
        }

        void generateParticles(Vector3[] particlePositions)
        {
            // Create vertex and index arrays
            particles = new VertexPositionTexture[nBillboards * 4];
            indices = new int[nBillboards * 6];

            int x = 0;

            // For each billboard...
            for (int i = 0; i < nBillboards * 4; i += 4)
            {
                Vector3 pos = particlePositions[i / 4];

                // Add 4 vertices at the billboard's position
                particles[i + 0] = new VertexPositionTexture(pos, new Vector2(0, 0));
                particles[i + 1] = new VertexPositionTexture(pos, new Vector2(0, 1));
                particles[i + 2] = new VertexPositionTexture(pos, new Vector2(1, 1));
                particles[i + 3] = new VertexPositionTexture(pos, new Vector2(1, 0));

                // Add 6 indices to form two triangles
                indices[x++] = i + 0;
                indices[x++] = i + 3;
                indices[x++] = i + 2;
                indices[x++] = i + 2;
                indices[x++] = i + 1;
                indices[x++] = i + 0;
            }

            // Create and set the vertex buffer
            verts = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionTexture), 
                nBillboards * 4, BufferUsage.WriteOnly);
            verts.SetData<VertexPositionTexture>(particles);

            // Create and set the index buffer
            ints = new IndexBuffer(Game.GraphicsDevice, IndexElementSize.ThirtyTwoBits, 
                nBillboards * 6, BufferUsage.WriteOnly);
            ints.SetData<int>(indices);
        }

        void setEffectParameters()
        {
            ChaseCamera camera = (ChaseCamera)((MyGame)Game).camera;
            effect.Parameters["ParticleTexture"].SetValue(texture);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["Size"].SetValue(billboardSize / 2f);
            effect.Parameters["Up"].SetValue(Mode == BillboardMode.Spherical ? camera.Up : Vector3.Up);
            effect.Parameters["Side"].SetValue(camera.Right);
        }

        public override void Draw(GameTime gameTime)
        {
            // Set the vertex and index buffer to the graphics card
            Game.GraphicsDevice.SetVertexBuffer(verts);
            Game.GraphicsDevice.Indices = ints;

            Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            setEffectParameters();

            if (EnsureOcclusion)
            {
                drawOpaquePixels();
                drawTransparentPixels();
            }
            else
            {
                Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                effect.Parameters["AlphaTest"].SetValue(false);
                drawBillboards();
            }

            // Reset render states
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Un-set the vertex and index buffer
            Game.GraphicsDevice.SetVertexBuffer(null);
            Game.GraphicsDevice.Indices = null;
        }

        void drawOpaquePixels()
        {
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            effect.Parameters["AlphaTest"].SetValue(true);
            effect.Parameters["AlphaTestGreater"].SetValue(true);

            drawBillboards();
        }

        void drawTransparentPixels()
        {
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            effect.Parameters["AlphaTest"].SetValue(true);
            effect.Parameters["AlphaTestGreater"].SetValue(false);

            drawBillboards();
        }

        void drawBillboards()
        {
            effect.CurrentTechnique.Passes[0].Apply();

            Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                4 * nBillboards, 0, nBillboards * 2);
        }
    }
}
