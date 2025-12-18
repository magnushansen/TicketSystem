namespace AlbumRegister.Models
{
    public static class ViewModelFactory
    {
        public static TicketViewModel Details(Ticket t, IEnumerable<User> assignedUsers)
        {
            return new TicketViewModel
            {
                Ticket = t,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                AssignedUsers = assignedUsers,
                Users = Enumerable.Empty<User>()
            };
        }

        public static TicketViewModel Create(Ticket ticket, IEnumerable<User> users)
        {
            return new TicketViewModel
            {
                Ticket = ticket,
                Users = users,
                AssignedUsers = Enumerable.Empty<User>()
            };
        }

        public static TicketViewModel Edit(Ticket ticket, IEnumerable<User> users, IEnumerable<User> assignedUsers, List<int> selectedUserIds)
        {
            return new TicketViewModel
            {
                Ticket = ticket,
                Users = users,
                AssignedUsers = assignedUsers,
                SelectedUserIds = selectedUserIds,
                Theme = "warning",
                Action = "Edit"
            };
        }

        public static TicketViewModel Delete(Ticket t, IEnumerable<User> assignedUsers)
        {
            return new TicketViewModel
            {
                Ticket = t,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                AssignedUsers = assignedUsers,
                Users = Enumerable.Empty<User>()
            };
        }
    }
}