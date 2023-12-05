using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desktopapplication.Views;

public partial class GroupsPage : ContentPage
{
    public GroupsPage(Guid groupId)
    {
        InitializeComponent();

        Label.Text = groupId.ToString();
    }
}