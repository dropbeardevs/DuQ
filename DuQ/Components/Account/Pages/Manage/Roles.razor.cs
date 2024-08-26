using System.Security.Claims;
using DuQ.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;

namespace DuQ.Components.Account.Pages.Manage;

public partial class Roles
{
    [Inject] private IdentityService IdentityService { get; set; } = null!;
    private int curTabIdx { get; set; } = 0;
    private string? TabClass(int idx) => curTabIdx == idx ? "active" : null;
    private Modal newModal { get; set; }
    private Modal editModal { get; set; }
    private Guid roleIdField;
    private string roleNameField = String.Empty;

    private record RoleList(int total, Role[] data);

    private record Role(Guid Id, string Name, KeyValuePair<string, string>[] Claims);

    private QuickGrid<Role> rolesGrid;
    private GridItemsProvider<Role>? rolesProvider;
    private int numResults;
    private Dictionary<string, string?> claimTypes = new Dictionary<string, string?>();
    private List<Claim> claims = new List<Claim>();
    private string claimTypeField = String.Empty;
    private string claimValueField = String.Empty;

    protected override async Task OnInitializedAsync()
    {
        //claimTypes = await Http.GetFromJsonAsync<Dictionary<string, string>>("/api/identity/claimslist");

        claimTypes = IdentityService.GetClaimsList;

        rolesProvider = async req =>
        {
            //var response = await Http.GetFromJsonAsync<RoleList>("/api/identity/rolelist", req.CancellationToken);

            var response = await IdentityService.GetRoleListAsync();

            numResults = response.total;
            this.StateHasChanged();
            return GridItemsProviderResult.From(items: response.data, totalItemCount: response.total);
        };
    }

    private void AddClaim()
    {
        if (!string.IsNullOrWhiteSpace(claimTypeField) && !string.IsNullOrWhiteSpace(claimValueField))
        {
            claims.Add(new Claim(claimTypeField, claimValueField));
            claimTypeField = String.Empty;
            claimValueField = String.Empty;
        }
    }

    private void NewRoleModal()
    {
        roleNameField = String.Empty;
        newModal.Open();
    }

    private async Task AddNewRoleAsync()
    {
        newModal.Close();
        //await Http.PostAsync($"/api/identity/CreateRole?name={roleNameField}", null);
        await IdentityService.CreateRole(roleNameField);

        await rolesGrid.RefreshDataAsync();
    }

    private void ChangeRole(Role r)
    {
        roleIdField = r.Id;
        roleNameField = r.Name;

        claims.Clear();
        foreach (var claim in r.Claims)
            claims.Add(new Claim(claim.Key, claim.Value));

        claimTypeField = String.Empty;
        claimValueField = String.Empty;

        curTabIdx = 0;
        editModal.Open();
    }

    private async Task UpdateRoleAsync()
    {
        editModal.Close();

        var dict = new Dictionary<string, object?>
                   {
                       ["id"] = roleIdField,
                       ["name"] = roleNameField
                   };

        var i = 0;
        foreach (var claim in claims)
        {
            dict.Add($"claims[{i}].key", claim.Type);
            dict.Add($"claims[{i}].value", claim.Value);
            i++;
        }

        var url = NavManager.GetUriWithQueryParameters("/api/identity/updaterole", dict);
        await Http.PostAsync(url, null);

        await rolesGrid.RefreshDataAsync();
    }

    private async Task DeleteRoleAsync(Role r)
    {
        if (await JsRuntime.InvokeAsync<bool>("confirm", "Are you sure?"))
        {
            //await Http.DeleteAsync($"/api/identity/DeleteRole?id={r.Id}");

            await IdentityService.DeleteRole(r.Id.ToString());

            await rolesGrid.RefreshDataAsync();
        }
    }
}
