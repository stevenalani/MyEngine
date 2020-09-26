using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEngine.HgtImporter
{
    public class HgtMapVolume : ColorVolume
    {
        private const float Water = 0.1f;
        private const float Sand = 0.2f;
        private const float Dirt = 0.3f;
        private const float Gras = 0.6f;
        private const float Rock = 0.8f;
        private const float Snow = 0.9f;

        private Vector4 waterl = new Vector4(0, 151, 255, 127);
        private Vector4 water = new Vector4(177, 159, 144, 255);
        private Vector4 sand = new Vector4(134, 100, 71, 255);
        private Vector4 dirt = new Vector4(0, 136, 0, 255);
        private Vector4 gras = new Vector4(170, 170, 170, 255);
        private Vector4 rock = new Vector4(190, 190, 190, 255);
        private Vector4 snow = new Vector4(200, 200, 200, 255);

        public HgtMapVolume(int sizeX, int sizeY, int sizeZ) : base(sizeX, sizeY, sizeZ)
        {
        }
    }
}
