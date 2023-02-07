using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Services;

public class MembershipService
{
    private readonly AuthDbContext _context;
    public MembershipService(AuthDbContext context)
    {
        _context = context;
    }
    public List<Register> GetAll()
    {
        return _context.Users.OrderBy(m => m.FName).ToList();
    }
    public Register? GetById(string id)
    {
        Register? user = _context.Users.FirstOrDefault(
        x => x.Email.Equals(id));
        return user;
    }
    public void AddUser(Register user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    public void UpdateUser(Register user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}
