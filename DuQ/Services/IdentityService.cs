using System.Reflection;
using System.Security.Claims;
using DuQ.Contexts;
using DuQ.Models.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DuQ.Services;

public class IdentityService
{
    private readonly UserManager<DuQIdentityUser> _userManager;
    private readonly RoleManager<DuQIdentityRole> _roleManager;

    private Dictionary<string, string?> _roles;
    private Dictionary<string, string?> _claimTypes;

    public IdentityService(UserManager<DuQIdentityUser> userManager, RoleManager<DuQIdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;

        _roles = roleManager.Roles.OrderBy(r => r.Name).ToDictionary(r => r.Id, r => r.Name);
        var fldInfo = typeof(ClaimTypes).GetFields(BindingFlags.Static | BindingFlags.Public);
        _claimTypes = fldInfo.OrderBy(c => c.Name).ToDictionary(c => c.Name, c => (string?)c.GetValue(null));
    }

    public Dictionary<string, string?> GetRoles => _roles;

    public Dictionary<string, string?> GetClaimsList => _claimTypes;

    public async Task<(int, IEnumerable<dynamic>)> GetRoleListAsync()
    {
        var qry = _roleManager.Roles.Include(r => r.Claims).OrderBy(r => r.Name);

        int total = await qry.CountAsync();

        var data = (await qry.ToArrayAsync()).Select(r => new
                                                            {
                                                                Id = r.Id,
                                                                Name = r.Name,
                                                                Claims = r.Claims.Select(c => new KeyValuePair<string, string?>(_claimTypes.Single(x => x.Value == c.ClaimType).Key, c.ClaimValue!)),
                                                            });

        return (total, data);
    }

    public async Task<IResult> CreateRole(string name)
    {
        try
        {
            var role = new DuQIdentityRole(name);

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                Log.Information("Created role {name}.", name);
                return Results.NoContent();
            }
            else
                return Results.BadRequest(result.Errors.First().Description);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failure creating role {name}.", name);
            return Results.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
        }
    }

    public async Task<IResult> UpdateRole(string id, string name, List<KeyValuePair<string, string>> claims)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return Results.NotFound("Role not found.");

            role.Name = name;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                Log.Information("Updated role {name}.", role.Name);

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var kvp in claims.Where(a => !roleClaims.Any(b => _claimTypes[a.Key] == b.Type && a.Value == b.Value)))
                    await _roleManager.AddClaimAsync(role, new Claim(_claimTypes[kvp.Key]!, kvp.Value));

                foreach (var claim in roleClaims.Where(a => !claims.Any(b => a.Type == _claimTypes[b.Key] && a.Value == b.Value)))
                    await _roleManager.RemoveClaimAsync(role, claim);

                return Results.NoContent();
            }
            else
            {
                return Results.BadRequest(result.Errors.First().Description);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failure updating role {roleId}.", id);
            return Results.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
        }
    }

    public async Task<IResult> DeleteRole(string id)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return Results.NotFound("Role not found.");

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                Log.Information("Deleted role {name}.", role.Name);
                return Results.NoContent();
            }
            else
                return Results.BadRequest(result.Errors.First().Description);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failure deleting role {roleId}.", id);
            return Results.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
        }
    }

    // public async Task<dynamic> UserList(int skip, int limit, string? sort, string? search)
    // {
    //     var qry = _userManager.Users.Include(u => u.Roles).Include(u => u.Claims)
    //                           .Where(u => string.IsNullOrWhiteSpace(search) || u.Email!.Contains(search));
    //
    //     int total = await qry.CountAsync();
    //
    //     if (sort?.Split(' ') is [var col, var dir])
    //     {
    //         if (dir == "ASC")
    //             qry = qry.OrderBy(x => EF.Property<string>(x, col));
    //         else
    //             qry = qry.OrderByDescending(x => EF.Property<string>(x, col));
    //     }
    //
    //     var data = (await qry.Skip(skip).Take(limit).ToArrayAsync()).Select(u => new
    //         {
    //             u.Id,
    //             u.Email,
    //             LockedOut = u.LockoutEnd == null ? string.Empty : "Yes",
    //             Roles = u.Roles.Select(r => _roles[r.RoleId]),
    //             Claims = u.Claims.Select(c => new KeyValuePair<string, string>(_claimTypes.Single(x => x.Value == c.ClaimType).Key, c.ClaimValue!)),
    //             DisplayName = u.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Name)?.ClaimValue,
    //             u.UserName
    //         });
    //
    //     return new { total, data };
    // }
}
