using System;
using Core.Entities;

namespace Core.Entities.Concrete.Project
{
    public class TenantUser : IEntity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int UserId { get; set; }

        public int? BranchId { get; set; }

        public bool IsActive { get; set; }

        public Tenant Tenant { get; set; } = null!;

        public Branch? Branch { get; set; }
    }
}
