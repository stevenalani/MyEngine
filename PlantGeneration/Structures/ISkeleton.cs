namespace PlantGeneration.Structures
{
    public interface ISkeleton
    {
        IBone Root { get; }
        IBone[] Bones { get; }
    }
}