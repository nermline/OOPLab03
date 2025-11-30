using System.Collections.ObjectModel;
using System.Text.Json;

namespace Lab03
{
    public class StudentService
    {
        private string _currentFilePath = string.Empty;
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        public async Task SaveToFileAsync(string filePath)
        {
            _currentFilePath = filePath;

            using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, Students, _options);
        }

        public async Task LoadFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) return;

            _currentFilePath = filePath;
            using var stream = File.OpenRead(filePath);
            var list = await JsonSerializer.DeserializeAsync<List<Student>>(stream, _options);

            Students.Clear();

            if (list != null)
            {
                foreach (var item in list) Students.Add(item);
            }
        }

        public void AddStudent(Student student) => Students.Add(student);

        public void UpdateStudent(Student selected, Student updatedStudent)
        {
            var index = Students.IndexOf(selected);

            if (index != -1)
            {
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

            if (int.TryParse(course, out int filterCourse) && filterCourse > 0)
            {
                query = query.Where(s => s.Course == filterCourse);
            }

            return query.ToList();
        }
    }
}