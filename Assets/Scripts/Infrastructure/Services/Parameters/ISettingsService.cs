using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services.Parameters
{
    public interface ISettingsService : IService
    {
        Settings Settings { get; set; }
    }
}