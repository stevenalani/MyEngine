using OpenTK;

namespace MyEngine
{
    public interface IEngineModel
    {
        string series { get; set; }
        bool purgesiblings { get; set; }
        Matrix4 Modelmatrix { get; set; }
    }
}