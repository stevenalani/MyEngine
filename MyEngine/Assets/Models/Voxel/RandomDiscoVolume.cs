using System;
using MyEngine.ShaderImporter;
using OpenTK;

namespace MyEngine.Assets.Models.Voxel
{
    internal class RandomDiscoVolume : Volume
    {
        public RandomDiscoVolume(Vector3 dimensions) : base(dimensions)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < dimensions.X; i++)
            for (var j = 0; j < dimensions.Y; j++)
            for (var k = 0; k < dimensions.Z; k++)
                SetVoxel(new Vector3(i, j, k),
                    new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));
            Update();
        }

        public RandomDiscoVolume(int witdh, int height, int depth) : base(witdh, height, depth)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            
            Update();
        }

        

        public void Update()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < dimensions.X; i++)
            for (var j = 0; j < dimensions.Y; j++)
            for (var k = 0; k < dimensions.Z; k++)
                SetVoxel(new Vector3(i, j, k),
                    new Vector4(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));
            Init();
        }
    }

    internal class PositionColorModelCustom : PositionColorModel
    {
        RandomDiscoVolume volume;
        private int drawings = 0;

        public PositionColorModelCustom(RandomDiscoVolume volume) : base(volume.Vertices,volume.Indices)
        {
            this.volume = volume;
            this.Vertices = volume.Vertices;
            this.Indices = volume.Indices;
            //this.OnDraw += volume.Update;
        }

        public PositionColorModelCustom(int f, int f1, int f2) : base(null,null)
        {
            volume = new RandomDiscoVolume(f,f1,f2);
            Vertices = volume.Vertices;
            Indices = volume.Indices;
        }

        private event Action OnDraw;


        public override void Draw(ShaderProgram shader)
        {
            base.Draw(shader);
            update();

        }

        public void update()
        {
            drawings++;
            if (drawings == 100)
            {
                volume.Update();
                this.Vertices = volume.Vertices;
                this.Indices = volume.Indices;
                IsInitialized = false;
                drawings = 0;
            }
        }
    }
}