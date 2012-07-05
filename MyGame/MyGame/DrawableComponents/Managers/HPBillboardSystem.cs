using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace MyGame
{
    /// <summary>
    /// This class represent a special case of billboard system which is the hp billboard system in which 
    /// all billboards dont have the same texture and texture of each billboards change with time 
    /// </summary>
    public class HPBillboardSystem
    {
        // Vertex buffer and index buffer, particle
        // and index arrays
        VertexBuffer verts;
        IndexBuffer ints;
        VertexPositionTexture[] particles;
        List<Monster> monsters;
        public List<Texture2D> monstersTextures;
        int[] indices;

        // Billboard settings
        int nBillboards;
        Vector2 billboardSize;
        private static Texture2D HP000;
        private static Texture2D HP010;
        private static Texture2D HP020;
        private static Texture2D HP030;
        private static Texture2D HP040;
        private static Texture2D HP050;
        private static Texture2D HP060;
        private static Texture2D HP070;
        private static Texture2D HP080;
        private static Texture2D HP090;
        private static Texture2D HP100;

        // GraphicsDevice and Effect
        GraphicsDevice graphicsDevice;
        Effect effect;

        public bool EnsureOcclusion = true;

        public enum BillboardMode { Cylindrical, Spherical };
        public BillboardMode Mode = BillboardMode.Spherical;

        public HPBillboardSystem(GraphicsDevice graphicsDevice,
            ContentManager content, Vector2 billboardSize, List<Monster> monsters)
        {
            this.billboardSize = billboardSize;
            this.graphicsDevice = graphicsDevice;
            HP000 = content.Load<Texture2D>("HP//000");
            HP010 = content.Load<Texture2D>("HP//010");
            HP020 = content.Load<Texture2D>("HP//020");
            HP030 = content.Load<Texture2D>("HP//030");
            HP040 = content.Load<Texture2D>("HP//040");
            HP050 = content.Load<Texture2D>("HP//050");
            HP060 = content.Load<Texture2D>("HP//060");
            HP070 = content.Load<Texture2D>("HP//070");
            HP080 = content.Load<Texture2D>("HP//080");
            HP090 = content.Load<Texture2D>("HP//090");
            HP100 = content.Load<Texture2D>("HP//100");
            this.monsters = monsters;

            effect = content.Load<Effect>("BillboardEffect");

            monsters = new List<Monster>();
            monstersTextures = new List<Texture2D>();
        }

        public void generateParticles()
        {
            this.nBillboards = monsters.Count;
            // Create vertex and index arrays
            particles = new VertexPositionTexture[nBillboards * 4];
            indices = new int[nBillboards * 6];

            int x = 0;

            // For each billboard...
            for (int i = 0; i < nBillboards * 4; i += 4)
            {
                Vector3 pos = monsters[i / 4].unit.position + Constants.HP_OFFSET;

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
            verts = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), 
                nBillboards * 4, BufferUsage.WriteOnly);
            verts.SetData<VertexPositionTexture>(particles);

            // Create and set the index buffer
            ints = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, 
                nBillboards * 6, BufferUsage.WriteOnly);
            ints.SetData<int>(indices);
        }

        public static Texture2D getTexture(int health)
        {
            switch (health/10*10)
            {
                case 00 : return HP000; 
                case 10 : return HP010; 
                case 20 : return HP020; 
                case 30 : return HP030; 
                case 40 : return HP040; 
                case 50 : return HP050; 
                case 60 : return HP060; 
                case 70 : return HP070; 
                case 80 : return HP080; 
                case 90 : return HP090; 
                case 100: return HP100; 
            }

            return HP000;
        }

        public void setTexture(int j)
        {
            monstersTextures[j] = getTexture(monsters[j].health);
        }
        void setEffectParameters(Matrix View, Matrix Projection, Vector3 Up, Vector3 Right)
        {
            effect.Parameters["ParticleTexture"].SetValue(monstersTextures[0]);
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["Size"].SetValue(billboardSize / 2f);
            effect.Parameters["Up"].SetValue(Mode == BillboardMode.Spherical ? Up : Vector3.Up);
            effect.Parameters["Side"].SetValue(Right);
        }

        /// <summary>
        /// This method renders the current state.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public void Draw(Matrix View, Matrix Projection, Vector3 Up, Vector3 Right)
        {
            // Set the vertex and index buffer to the graphics card
            graphicsDevice.SetVertexBuffer(verts);
            graphicsDevice.Indices = ints;

            graphicsDevice.BlendState = BlendState.AlphaBlend;

            setEffectParameters(View, Projection, Up, Right);

            if (EnsureOcclusion)
            {
                drawOpaquePixels();
                drawTransparentPixels();
            }
            else
            {
                graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                effect.Parameters["AlphaTest"].SetValue(false);
                drawBillboards();
            }

            // Reset render states
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Un-set the vertex and index buffer
            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;
        }

        void drawOpaquePixels()
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            effect.Parameters["AlphaTest"].SetValue(true);
            effect.Parameters["AlphaTestGreater"].SetValue(true);

            drawBillboards();
        }

        void drawTransparentPixels()
        {
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            effect.Parameters["AlphaTest"].SetValue(true);
            effect.Parameters["AlphaTestGreater"].SetValue(false);

            drawBillboards();
        }

        void drawBillboards()
        {
            //graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
            //    4 * nBillboards, 0, nBillboards * 2);

            for (int i = 0; i < monsters.Count; i++)
            {
                //if(effect.Parameters["ParticleTexture"].GetValueTexture2D() != monstersTextures[i])
                effect.Parameters["ParticleTexture"].SetValue(monstersTextures[i]);
                drawBillboard(i);
            }
        }

        void drawBillboard(int monsterId)
        {
            //effect.CurrentTechnique.Passes[0].Apply();

            //graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
            //    4 * nBillboards, 0, nBillboards * 2);

            effect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 
                4, 6 * monsterId, 2);
        }
    }
}
