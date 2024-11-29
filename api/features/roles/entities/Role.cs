using api.features.users.entities;

namespace api.features.roles.entities
{
    public class Role
    {
        public int Id { get; set; }
        public String Name { get; set; } // Enum como nombre del rol
        public ICollection<User> Users { get; set; } // Relación con usuarios
    }
}
