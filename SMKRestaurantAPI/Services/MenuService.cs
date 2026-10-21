using SMKRestaurantAPI.Data;
using SMKRestaurantAPI.Models;

namespace SMKRestaurantAPI.Services
{
    /// <summary>
    /// Business logic menu. Controller tidak boleh berisi SQL.
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public Task<IReadOnlyList<MenuDto>> GetMenusAsync(CancellationToken cancellationToken = default)
        {
            return _menuRepository.GetAllAsync(cancellationToken);
        }
    }
}
