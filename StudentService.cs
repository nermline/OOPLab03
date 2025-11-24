using System.Collections.ObjectModel;
using System.Text.Json;

namespace Lab03
{
    public class StudentService
    {
        private string _currentFilePath;
        public ObservableCollection<Student> Students { get; private set; } = new ObservableCollection<Student>();

        public async Task SaveToFileAsync(string filePath)
        {
            _currentFilePath = filePath;

            using var stream = File.Create(filePath);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            await JsonSerializer.SerializeAsync(stream, Students, options);
        }

        public async Task LoadFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) return;

            _currentFilePath = filePath;
            using var stream = File.OpenRead(filePath);
            var list = await JsonSerializer.DeserializeAsync<List<Student>>(stream);

            Students.Clear();
            foreach (var student in list)
            {
                Students.Add(student);
            }
        }

        public void AddStudent(Student student) => Students.Add(student);

        public void UpdateStudent(Student updatedStudent)
        {
            var existing = Students.FirstOrDefault(s => s.Id == updatedStudent.Id);
            if (existing != null)
            {
                int index = Students.IndexOf(existing);
                Students[index] = updatedStudent;
            }
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

            if (!string.IsNullOrWhiteSpace(course))
                query = query.Where(s => s.Course == course);

            return query.ToList();
        }
    }
}