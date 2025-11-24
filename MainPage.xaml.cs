namespace Lab03
{
    public partial class MainPage : ContentPage
    {
        private readonly StudentService _service;

        public MainPage()
        {
            InitializeComponent();
            _service = new StudentService();

            StudentsList.ItemsSource = _service.Students;
        }

        private async void LoadBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var customJson = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
                    { DevicePlatform.WinUI, new[] { ".json" } },
                    { DevicePlatform.Android, new[] { "application/json" } },
                });

                var result = await FilePicker.PickAsync(new PickOptions { FileTypes = customJson, PickerTitle = "Оберіть JSON" });

                if (result != null)
                {
                    await _service.LoadFromFileAsync(result.FullPath);
                    StudentsList.ItemsSource = _service.Students;
                }
            }
            catch (Exception ex) { await DisplayAlertAsync("Помилка", ex.Message, "OK"); }
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
#if WINDOWS
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                savePicker.FileTypeChoices.Add("JSON File", new List<string>() { ".json" });
                savePicker.SuggestedFileName = "students";

                var window = (Application.Current.Windows.FirstOrDefault()?.Handler?.PlatformView as Microsoft.UI.Xaml.Window);
                if (window == null) throw new Exception("Не вдалося отримати вікно програми.");

                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

                var file = await savePicker.PickSaveFileAsync();

                if (file != null)
                {
                    await _service.SaveToFileAsync(file.Path);
                    await DisplayAlertAsync("Успіх", $"Файл збережено:\n{file.Path}", "OK");
                }
#else
                string path = Path.Combine(FileSystem.AppDataDirectory, "students.json");
                await _service.SaveToFileAsync(path);
                await DisplayAlertAsync("Успіх", $"Збережено: {path}", "OK");
#endif
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка", ex.Message, "OK");
            }
        }

        private async void AddBtn_Clicked(object sender, EventArgs e)
        {
            var editor = new StudentEditorPage(null);
            editor.OnStudentSaved += (newStudent) =>
            {
                _service.AddStudent(newStudent);
                ApplyFilters();
            };
            await Navigation.PushAsync(editor);
        }

        private async void EditBtn_Clicked(object sender, EventArgs e)
        {
            if (StudentsList.SelectedItem is Student selected)
            {
                var editor = new StudentEditorPage(selected);
                editor.OnStudentSaved += (updatedStudent) =>
                {
                    _service.UpdateStudent(updatedStudent);
                    ApplyFilters();
                };
                await Navigation.PushAsync(editor);
            }
            else
            {
                await DisplayAlertAsync("Увага", "Оберіть студента зі списку", "OK");
            }
        }

        private void DeleteBtn_Clicked(object sender, EventArgs e)
        {
            if (StudentsList.SelectedItem is Student selected)
            {
                _service.DeleteStudent(selected);
                ApplyFilters();
            }
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = _service.Search(FilterName.Text, FilterFaculty.Text, FilterCourse.Text);
            StudentsList.ItemsSource = filtered;
        }

        private async void AboutBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AboutPage());
        }
    }
}