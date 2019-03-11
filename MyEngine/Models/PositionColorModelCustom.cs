using System;
using MyEngine.Assets.Models.Voxel;
using MyEngine.ShaderImporter;

namespace MyEngine.Models
{
    internal class PositionColorModelCustom : PositionColorModel
    {
        RandomDiscoVolume volume;
        private int drawings = 0;

        public PositionColorModelCustom(RandomDiscoVolume volume) : base(null, null)
        {
            this.volume = volume;
            this.Vertices = volume.Vertices;
            this.Indices = volume.Indices;
        }

        public PositionColorModelCustom(int f, int f1, int f2) : base(null,null)
        {
            volume = new RandomDiscoVolume(f,f1,f2);
            Vertices = volume.Vertices;
            Indices = volume.Indices;
        }

        public override void Draw(ShaderProgram shader)
        {
            base.Draw(shader);
            update();

        }

        public void update()
        {
            drawings++;
            if (drawings == 50)
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