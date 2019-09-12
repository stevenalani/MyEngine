namespace MyEngine.DataStructures
{
    public interface IVertextype
    {
        
        int getStructSize();
    }

    public interface IBoundinBox
    {
        BoundingBox BoundingBox { get; set; }
    }
}