using ControllerFirst.Contexts;
using ControllerFirst.DTO.Requests;
using ControllerFirst.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace ControllerFirst.Services.Classes;

public class UserElasticReindexService : IElasticReindexService<UserElasticDTO>
{
    private readonly IElasticClient _elastic;
    private readonly AuthContext _context;
    private readonly ILogger<UserElasticReindexService> _logger;

    public UserElasticReindexService(IElasticClient elastic, AuthContext context, ILogger<UserElasticReindexService> logger)
    {
        _elastic = elastic;
        _context = context;
        _logger = logger;
    }

    public async Task ReindexAllAsync()
    {
        var users = await _context.Users.Include(u => u.UserRoles).ToListAsync();

        var docs = users.Select(user => new UserElasticDTO
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,
            Email = user.Email,
            IsEmailConfirmed = user.IsEmailConfirmed,
            Roles = user.UserRoles.Select(r => r.RoleNameRef).ToList()
        });

        var bulk = await _elastic.BulkAsync(b => b
            .Index("users")
            .IndexMany(docs)
        );

        if (!bulk.IsValid)
        {
            _logger.LogError("Bulk reindexing failed: {Error}", bulk.ServerError?.Error?.Reason);
        }
        else
        {
            _logger.LogInformation("Reindexed {Count} users", docs.Count());
        }
    }
}