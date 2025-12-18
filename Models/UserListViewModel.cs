namespace AlbumRegister.Models;

public class UserListViewModel
{
    public List<User>? Users { get; set; }
    public PagingInfo PagingInfo { get; set; } = new();
}
