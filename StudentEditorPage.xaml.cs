namespace Lab03
{
    public partial class StudentEditorPage : ContentPage
    {
        private Student _student = new Student();
        public event Action<Student>? OnStudentSaved;

        public StudentEditorPage(Student? student = null)
        {
            InitializeComponent();

            if (student != null)
            {
                _student = student;

                EntLastName.Text = student.LastName;
                EntFirstName.Text = student.FirstName;
                EntPatronymic.Text = student.Patronymic;
                EntFaculty.Text = student.Faculty;
                EntSpecialty.Text = student.Specialty;
                EntChair.Text = student.Chair;
                EntCourse.Text = student.Course > 0 ? student.Course.ToString() : string.Empty;
                EntAddress.Text = student.Address;
                EntRoom.Text = student.Room;
                ChkScholarship.IsChecked = student.HasScholarship;

                switch (student.Status)
                {
                    case "Проживає": StatusPicker.SelectedIndex = 1; break;
                    case "Очікує": StatusPicker.SelectedIndex = 2; break;
                    case "Виселено": StatusPicker.SelectedIndex = 3; break;
                    default: StatusPicker.SelectedIndex = 0; break;
                }
            }
            else
            {
                _student = new Student();
                StatusPicker.SelectedIndex = 0;
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (StatusPicker.SelectedIndex <= 0)
                {
                    await DisplayAlertAsync("Помилка", "Будь ласка, оберіть коректний статус студента.", "OK");
                    return;
                }

                _student.Status = StatusPicker.SelectedItem.ToString() ?? "Очікує";

                _student.LastName = EntLastName.Text ?? string.Empty;
                _student.FirstName = EntFirstName.Text ?? string.Empty;
                _student.Patronymic = EntPatronymic.Text ?? string.Empty;
                _student.Faculty = EntFaculty.Text ?? string.Empty;
                _student.Specialty = EntSpecialty.Text ?? string.Empty;
                _student.Chair = EntChair.Text ?? string.Empty;
                _student.Room = EntRoom.Text;
                _student.Address = EntAddress.Text ?? string.Empty;

                if (int.TryParse(EntCourse.Text, out int courseVal))
                {
                    _student.Course = courseVal;
                }
                else
                {
                    _student.Course = 0;
                }

                _student.HasScholarship = ChkScholarship.IsChecked;

                OnStudentSaved?.Invoke(_student);

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка при збереженні", ex.Message, "OK");
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}