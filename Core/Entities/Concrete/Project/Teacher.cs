using System;
using Core.Entities;

namespace Core.Entities.Concrete.Project
{
    public class Teacher : IEntity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int PersonId { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsActive { get; set; }

        public Tenant Tenant { get; set; } = null!;

        public Person Person { get; set; } = null!;
    }
}
