namespace desktopapplication.Views;

public partial class GroupsPage : ContentPage
{
    public GroupsPage(Guid groupId)
    {
        InitializeComponent();

        Label.Text = groupId.ToString();
    }
}