using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.Entities.Concrete.Project
{
    public class Student : IEntity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int PersonId { get; set; }

        public string? StudentNumber { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public bool IsActive { get; set; }

        public Tenant Tenant { get; set; } = null!;

        public Person Person { get; set; } = null!;

        public ICollection<StudentParent> Parents { get; set; } = new List<StudentParent>();
    }
}
