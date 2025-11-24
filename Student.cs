using System;
using System.Collections.Generic;
using System.Text;

namespace Lab03
{
    public class Student
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public string Course { get; set; }
        public string City { get; set; }
        public bool HasScholarship { get; set; }

        public string FullName => $"{LastName} {FirstName} {Patronymic}";
    }
}
