using MudBlazor;
using MudBlazor.Utilities;

namespace DuQ.Components.Layout;

public partial class MainLayout
{
    private static readonly MudColor FullertonCollegeColor = new("#0f406b");

    public readonly MudTheme FullertonCollegeTheme = new MudTheme()
                                                     {
                                                         PaletteLight = new PaletteLight()
                                                                        {
                                                                            Primary = FullertonCollegeColor,
                                                                            AppbarBackground = FullertonCollegeColor,
                                                                            Secondary = Colors.Yellow.Darken1,
                                                                            Tertiary = Colors.Gray.Lighten5
                                                                        }
                                                     };
}
