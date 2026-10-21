using Microsoft.AspNetCore.Mvc;
using SMKRestaurantAPI.Models;
using SMKRestaurantAPI.Services;

namespace SMKRestaurantAPI.Controllers
{
    [ApiController]
    [Route("menu")]
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// GET /menu atau GET /api/menu
        /// Header: Authorization (wajib ada)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<MenuDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMenu(CancellationToken cancellationToken)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Unauthorized(new ApiErrorResponse
                {
                    Message = "Authorization required"
                });
            }

            var menus = await _menuService.GetMenusAsync(cancellationToken);

            return Ok(ApiResponse<IReadOnlyList<MenuDto>>.Ok(
                menus,
                message: "Menu berhasil diambil.",
                count: menus.Count));
        }
    }
}
