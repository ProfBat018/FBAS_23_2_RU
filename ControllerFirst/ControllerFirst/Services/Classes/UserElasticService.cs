using ControllerFirst.Contexts;
using ControllerFirst.Data.Models;
using ControllerFirst.DTO.Requests;
using ControllerFirst.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace ControllerFirst.Services.Classes;

public class UserElasticService : IUserElasticService
{
    private readonly IElasticClient _elastic;
    private readonly AuthContext _context;

    public UserElasticService(IElasticClient elastic, AuthContext context)
    {
        _elastic = elastic;
        _context = context;
    }

    public async Task IndexUserAsync(User user)
    {
        var roles = _context.UserRoles
            .Where(r => r.UserRef == user.Id)
            .Select(r => r.RoleNameRef)
            .ToList();

        var doc = new UserElasticDTO
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,
            Email = user.Email,
            IsEmailConfirmed = user.IsEmailConfirmed,
            Roles = roles
        };

        var response = await _elastic.IndexDocumentAsync(doc);

        if (!response.IsValid)
        {
            Console.WriteLine($"Elastic indexing failed for user {user.UserName}: {response.OriginalException?.Message}");
        }
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await _elastic.DeleteAsync<UserElasticDTO>(id.ToString());
    }


    public async Task<IEnumerable<UserElasticDTO>> SearchAsync(string query)
    {
        var response = await _elastic.SearchAsync<UserElasticDTO>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(u => u.UserName)
                        .Field(u => u.Email)
                    )
                    .Query(query)
                )
            )
        );

        return response.Documents;
    }

    public async Task ReindexAllUsersAsync()
    {
        var users = await _context.Users.Include(u => u.UserRoles).ToListAsync();
        foreach (var user in users)
        {
            await IndexUserAsync(user);
        }
    }
}