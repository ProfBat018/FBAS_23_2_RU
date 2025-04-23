using ControllerFirst.Data.Models;
using ControllerFirst.DTO.Requests;

namespace ControllerFirst.Services.Interfaces;

public interface IUserElasticService
{
    Task IndexUserAsync(User user);
    Task DeleteUserAsync(Guid id);
    public Task ReindexAllUsersAsync();
    Task<IEnumerable<UserElasticDTO>> SearchAsync(string query);
}