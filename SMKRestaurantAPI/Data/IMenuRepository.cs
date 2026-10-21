using SMKRestaurantAPI.Models;

namespace SMKRestaurantAPI.Data
{
    public interface IMenuRepository
    {
        Task<IReadOnlyList<MenuDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
