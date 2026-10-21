using SMKRestaurantAPI.Models;

namespace SMKRestaurantAPI.Services
{
    public interface IMenuService
    {
        Task<IReadOnlyList<MenuDto>> GetMenusAsync(CancellationToken cancellationToken = default);
    }
}
