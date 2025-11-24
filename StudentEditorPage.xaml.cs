namespace Lab03
{
    public partial class StudentEditorPage : ContentPage
    {
        private Student _student;
        public event Action<Student> OnStudentSaved;

        public StudentEditorPage(Student student = null)
        {
            InitializeComponent();

            if (student != null)
            {
                _student = student;
                EntLastName.Text = student.LastName;
                EntFirstName.Text = student.FirstName;
                EntPatronymic.Text = student.Patronymic;
                EntFaculty.Text = student.Faculty;
                EntDepartment.Text = student.Department;
                EntCourse.Text = student.Course;
                EntCity.Text = student.City;
                ChkScholarship.IsChecked = student.HasScholarship;
            }
            else
            {
                _student = new Student();
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            _student.LastName = EntLastName.Text;
            _student.FirstName = EntFirstName.Text;
            _student.Patronymic = EntPatronymic.Text;
            _student.Faculty = EntFaculty.Text;
            _student.Department = EntDepartment.Text;
            _student.Course = EntCourse.Text;
            _student.City = EntCity.Text;
            _student.HasScholarship = ChkScholarship.IsChecked;

            OnStudentSaved?.Invoke(_student);
            await Navigation.PopAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}