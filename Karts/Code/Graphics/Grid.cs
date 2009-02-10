using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;


namespace Karts.Code
{

    class Grid : DrawableGameComponent
    {
        //---------------------------------------------------
        // Class Members
        //---------------------------------------------------

        GraphicsDevice graphics;
        VertexBuffer vertexBuffer = null;
        Random rnd = new Random();
        int numberOfLines = 0;

        public Grid(Game game) : base(game)
        {
        }

        int gridSpacing = 100;

        public int GridSpacing
        {
            get { return gridSpacing; }
            set { gridSpacing = value; }
        }

        int gridSize = 2000;

        public int GridSize
        {
            get { return gridSize; }
            set { gridSize = value; }
        }

        bool showGrid = true;

        public bool ShowGrid
        {
            get { return showGrid; }
            set { showGrid = value; }
        }

        //---------------------------------------------------
        // Class Methods
        //---------------------------------------------------

        public void Start()
        {
            if (!showGrid)
                return;

            // Get a reference to the graphics service so we can draw
            graphics = ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice;
            

            // Populate the vertex buffer for our grid settings
            numberOfLines = (gridSize / gridSpacing) * 2;

            PopulateVertexBuffer();
        }

        public override void Update(GameTime gameTime)
        {
            // We don't need to update anything here, since our grid never changes
        }

        public override void Draw(GameTime gameTime)
        {
            if (!showGrid)
                return;

            // Set the buffer, and then draw our lines
            graphics.VertexDeclaration = new VertexDeclaration(graphics, VertexPositionColor.VertexElements);
            graphics.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionColor.SizeInBytes);
            graphics.DrawPrimitives(PrimitiveType.LineList, 0, numberOfLines);
        }

        private void PopulateVertexBuffer()
        {
            // Make sure if the buffer was disposed (window minimised etc.) we recreated it
            if (vertexBuffer == null || vertexBuffer.IsDisposed)
                vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), numberOfLines * 2, BufferUsage.WriteOnly);

            // Create a list of vertices equal to the number of lines * 2 (since that's how many points we'll have)
            VertexPositionColor[] verts = new VertexPositionColor[numberOfLines * 4];

            // Add the points for each line
            int maxPos = gridSize / 2;

            for (int i = 0; i < numberOfLines; i += 2)
            {
                int lineOffset = gridSpacing * (i / 2);

                // Move along the X axis for first line
                verts[i].Position = new Vector3(-maxPos + lineOffset, 0, -maxPos);
                verts[i].Color = Color.White;
                verts[i + 1].Position = new Vector3(-maxPos + lineOffset, 0, maxPos);
                verts[i + 1].Color = Color.White;

                // And move along the Z axis for second line
                verts[i + numberOfLines].Position = new Vector3(-maxPos, 0, -maxPos + lineOffset);
                verts[i + numberOfLines].Color = Color.White;
                verts[i + numberOfLines + 1].Position = new Vector3(maxPos, 0, -maxPos + lineOffset);
                verts[i + numberOfLines + 1].Color = Color.White;
            }

            vertexBuffer.SetData<VertexPositionColor>(verts, 0, numberOfLines * 2);
        }
    }
}