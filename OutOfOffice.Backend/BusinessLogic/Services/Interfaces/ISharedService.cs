using BusinessLogic.Options;

namespace BusinessLogic.Services.Interfaces
{
    public interface ISharedService
    {
        List<T> SortEntities<T>(List<T> entities, SortOptions options);
        List<T> FilterEntities<T>(List<T> entities, FilterOptions options);
    }
}
