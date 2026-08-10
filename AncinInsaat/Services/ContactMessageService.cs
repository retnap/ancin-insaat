using AncinInsaat.Data;
using AncinInsaat.Data.Entities;

namespace AncinInsaat.Services;

public class ContactMessageService : IContactMessageService
{
    private readonly AppDbContext _context;

    public ContactMessageService(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(ContactMessage message, CancellationToken cancellationToken = default)
    {
        message.CreatedAt = DateTime.UtcNow;
        message.IsRead = false;

        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
