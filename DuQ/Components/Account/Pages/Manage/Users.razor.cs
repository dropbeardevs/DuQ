using System.Security.Claims;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace DuQ.Components.Account.Pages.Manage;

public partial class Users
{
    private int curTabIdx { get; set; } = 0;
    private string? TabClass(int idx) => curTabIdx == idx ? "active" : null;
    private Timer searchTimer = new Timer(1000);
    private Modal newModal { get; set; }
    private Modal editModal { get; set; }
    private Modal pwdModal { get; set; }
    private string newError = string.Empty;
    private string editError = string.Empty;
    private string pwdError = string.Empty;
    private Guid userIdField;
    private string userNameField = String.Empty;
    private string fullNameField = String.Empty;
    private string emailField = String.Empty;
    private string passwordField = String.Empty;
    private string verifyField = String.Empty;
    private bool lockedField = false;

    private record UserList(int total, User[] data);

    private record User(
        Guid Id,
        string Email,
        string LockedOut,
        string[] Roles,
        KeyValuePair<string, string>[] Claims,
        string DisplayName,
        string UserName);

    private QuickGrid<User> usersGrid;
    private GridItemsProvider<User>? usersProvider;
    private PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
    private Dictionary<string, string> allRoles = new Dictionary<string, string>();
    private Dictionary<string, string> claimTypes = new Dictionary<string, string>();
    private List<string> roles = new List<string>();
    private List<Claim> claims = new List<Claim>();
    private string claimTypeField = String.Empty;
    private string claimValueField = String.Empty;
    private string nameSearch;

    private string NameSearch
    {
        get => nameSearch;
        set
        {
            if (nameSearch != value)
            {
                nameSearch = value;
                searchTimer.Stop();
                searchTimer.Start();
            }
        }
    }

    private async void Elapsed_SearchAsync(object? sender, ElapsedEventArgs e)
    {
        await usersGrid.RefreshDataAsync();
    }

    private void CheckboxChanged(string value, object isChecked)
    {
        if ((bool)isChecked)
            roles.Add(value);
        else
            roles.Remove(value);
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

    private void NewUserModal()
    {
        userNameField = String.Empty;
        fullNameField = String.Empty;
        emailField = String.Empty;
        passwordField = String.Empty;
        newModal.Open();
    }

    private async Task AddNewUserAsync()
    {
        var response =
            await Http.PostAsync(
                $"/api/identity/CreateUser?userName={userNameField}&name={fullNameField}&email={emailField}&password={passwordField}",
                null);
        if (response.IsSuccessStatusCode)
        {
            newModal.Close();
            await usersGrid.RefreshDataAsync();
        }
        else
            newError = await response.Content.ReadAsStringAsync();
    }

    private void ChangeUser(User u)
    {
        userIdField = u.Id;
        emailField = u.Email;
        lockedField = u.LockedOut == "Yes";
        roles = new List<string>(u.Roles);

        claims.Clear();
        foreach (var claim in u.Claims)
            claims.Add(new Claim(claim.Key, claim.Value));

        claimTypeField = String.Empty;
        claimValueField = String.Empty;

        curTabIdx = 0;
        editModal.Open();
    }

    private async Task UpdateUserAsync()
    {
        var dict = new Dictionary<string, object?>
                   {
                       ["id"] = userIdField,
                       ["email"] = emailField,
                       ["locked"] = lockedField
                   };

        var i = 0;
        foreach (var claim in claims)
        {
            dict.Add($"claims[{i}].key", claim.Type);
            dict.Add($"claims[{i}].value", claim.Value);
            i++;
        }

        var j = 0;
        foreach (var role in roles)
            dict.Add($"roles[{j++}]", role);

        var url = NavManager.GetUriWithQueryParameters("/api/identity/updateuser", dict);

        var response = await Http.PostAsync(url, null);
        if (response.IsSuccessStatusCode)
        {
            editModal.Close();
            await usersGrid.RefreshDataAsync();
        }
        else
            editError = await response.Content.ReadAsStringAsync();
    }

    private async Task DeleteUserAsync(User u)
    {
        if (await JsRuntime.InvokeAsync<bool>("confirm", "Are you sure?"))
        {
            await Http.DeleteAsync($"/api/identity/DeleteUser?id={u.Id}");
            await usersGrid.RefreshDataAsync();
        }
    }

    private void ChangePassword(User u)
    {
        userIdField = u.Id;
        passwordField = String.Empty;
        verifyField = String.Empty;
        pwdModal.Open();
    }

    private async Task ResetPasswordAsync()
    {
        var response =
            await Http.PostAsync(
                $"/api/identity/ResetPassword?id={userIdField}&password={passwordField}&verify={verifyField}", null);
        if (response.IsSuccessStatusCode)
            pwdModal.Close();
        else
            pwdError = await response.Content.ReadAsStringAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        searchTimer.AutoReset = false;
        searchTimer.Elapsed += Elapsed_SearchAsync;

        allRoles = await Http.GetFromJsonAsync<Dictionary<string, string>>("/api/identity/roleslist");
        claimTypes = await Http.GetFromJsonAsync<Dictionary<string, string>>("/api/identity/claimslist");

        usersProvider = async req =>
        {
            var sortList = new List<string>();

            foreach (var item in req.GetSortByProperties())
                sortList.Add(item.PropertyName + " " +
                             (item.Direction == Microsoft.AspNetCore.Components.QuickGrid.SortDirection.Descending
                                 ? "DESC"
                                 : "ASC"));

            var url = NavManager.GetUriWithQueryParameters("/api/identity/userlist", new Dictionary<string, object?>
                {
                    { "skip", req.StartIndex },
                    { "limit", req.Count },
                    { "sort", String.Join(",", sortList) },
                    { "search", nameSearch },
                });

            var response = await Http.GetFromJsonAsync<UserList>(url, req.CancellationToken);
            return GridItemsProviderResult.From(items: response.data, totalItemCount: response.total);
        };
    }
}
