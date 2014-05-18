using Microsoft.AspNet.Identity.EntityFramework;

namespace Erbsenzaehler.Models
{
    public class User : IdentityUser
    {
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}