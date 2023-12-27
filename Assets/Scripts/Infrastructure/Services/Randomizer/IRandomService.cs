﻿namespace Assets.Scripts.Infrastructure.Services.Randomizer
{
  public interface IRandomService : IService
  {
    int Next(int minValue, int maxValue);
  }
}