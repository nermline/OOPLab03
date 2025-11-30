using System.Text.Json;

namespace Lab03
{
    public class StudentService
    {
        private string _currentFilePath = string.Empty;
        public List<Student> Students { get; set; } = new List<Student>();

        public async Task SaveToFileAsync(string filePath)
        {
            _currentFilePath = filePath;

            using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, Students);
        }

        public async Task LoadFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) return;

            _currentFilePath = filePath;
            using var stream = File.OpenRead(filePath);
            var list = await JsonSerializer.DeserializeAsync<List<Student>>(stream);

            Students.Clear();

            if (list != null)
            {
                foreach (var student in list)
                {
                    Students.Add(student);
                }
            }
        }

        public void AddStudent(Student student) => Students.Add(student);

        public void UpdateStudent(Student selected, Student updatedStudent)
        {
            selected = updatedStudent;
        }

        public void DeleteStudent(Student student)
        {
            if (Students.Contains(student)) Students.Remove(student);
        }

        public List<Student> Search(string text, string faculty, string course)
        {
            var query = Students.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(text))
                query = query.Where(s => s.FullName.Contains(text, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(faculty))
                query = query.Where(s => s.Faculty.Contains(faculty, StringComparison.OrdinalIgnoreCase));

            int filterCourse = int.Parse(course);
            if (filterCourse > 0)
                query = query.Where(s => s.Course == filterCourse);

            return query.ToList();
        }
    }
}