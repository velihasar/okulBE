using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.Entities.Concrete.Project
{
    public class Branch : IEntity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public bool IsActive { get; set; }

        public Tenant Tenant { get; set; } = null!;

        public ICollection<TenantUser> Users { get; set; } = new List<TenantUser>();
    }
}
