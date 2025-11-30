namespace Lab03
{
    public class Student
    {
        public string Status { get; set; } = string.Empty;
        public string? Room { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string? Chair { get; set; }
        public int Course { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public bool HasScholarship { get; set; }

        public string FullName => $"{LastName} {FirstName} {Patronymic}";

    }
}
