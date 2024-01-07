using Random = UnityEngine.Random;

namespace Assets.Scripts.Infrastructure.Services.Randomizer
{
  public class UnityRandomService : IRandomService
  {
    public int Next(int min, int max) =>
      Random.Range(min, max);
  }
}