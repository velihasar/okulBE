using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.Entities.Concrete.Project
{
    public class Parent : IEntity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int PersonId { get; set; }

        public bool IsActive { get; set; }

        public Tenant Tenant { get; set; } = null!;

        public Person Person { get; set; } = null!;

        public ICollection<StudentParent> Students { get; set; } = new List<StudentParent>();
    }
}
