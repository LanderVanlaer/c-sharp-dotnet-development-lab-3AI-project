﻿namespace desktopapplication.Models;

public class User
{
    public DateTime CreatedAt;
    public Guid Id;
    public ICollection<Group> UserGroups;

    public string Username;
}