using OpenTK;

namespace PlantGeneration.Structures
{
    public interface IBone
    {
        ILink ParentLink { get; set; }
        ILink ChildLink { get; set; }
        double Length{ get; set; }
        Vector3 Direction { get; set; }
        string Name { get; set; }
        void AddBone(IBone bone);
    }
}