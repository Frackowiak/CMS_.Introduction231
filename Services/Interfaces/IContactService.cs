using CMS_.Introduction.Models;

namespace CMS_.Introduction.Services.Interfaces;

public interface IContactService
{
    IEnumerable<ContactModel> Get();
    ContactModel Get(Guid id);
    void Add(ContactModel contact);
    void Update(ContactModel contact);
    void Delete(Guid id);
}
