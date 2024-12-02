using Microsoft.Maui.Storage;
using System.IO;
using CommunityToolkit.Maui.Storage;
using System.Xml.Xsl;
using XMLParsing;
using System.Text;

namespace Xmlinator
{
    public partial class MainPage : ContentPage
    {
        private FileResult currentFile;
        private IFilePicker filePicker;
        private SearchParameters searchParameters;
        private IList<Book> books;
        private IParser parser;
        private bool isParsed = false;
        private FileResult outputFile;
        private IFileSaver Saver;


        public MainPage()
        {
            InitializeComponent();
            filePicker = FilePicker.Default;
            searchParameters = new SearchParameters();
            books = new List<Book>();
            ParserPicker.SelectedIndex = 1;
            Saver = FileSaver.Default;
        }


        private async void OpenFileButton_Clicked(object sender, EventArgs e)
        {
            var fileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                    { DevicePlatform.WinUI, new[] { ".xml" } },
                    });
            var options = new PickOptions() { PickerTitle = "Оберіть xml файл із датабазою бібліотеки", FileTypes = fileType };
            currentFile = await filePicker.PickAsync(options);

            if (currentFile == null)
            {
                await DisplayAlert("Помилка завантаження", "Файл не було завантажено", "OK");
                return;
            }
            StatusLabel.Text = "Поточний файл: " + currentFile.FileName;
        }

        private void CreateLabel(int row, int col, string text)
        {
            var freshLabel = new Label
            {
                Text = text,

                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };


            Grid.SetRow(freshLabel, row);
            Grid.SetColumn(freshLabel, col);
            ResultsTable.Children.Add(freshLabel);
        }

        private void Display(int row, Book book)
        {
                ResultsTable.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                CreateLabel(row, 0, book.Title);
                CreateLabel(row, 1, book.AuthorName);
                CreateLabel(row, 2, book.AU_ID);
                CreateLabel(row, 3, book.Year);
                CreateLabel(row, 4, book.Format);
        }

        private void ManageFilters()
        {
            searchParameters.Clear();
            if (TitleCheckbox.IsChecked)
            {
                searchParameters.Title = TitleEntry.Text ?? "";
            }

            if (FormatCheckbox.IsChecked)
            {
                searchParameters.Format = FormatEntry.Text ?? "";
            }

            if (AuthorCheckbox.IsChecked)
            {
                searchParameters.AuthorName = AuthorEntry.Text ?? "";
            }

            if (AU_IDCheckbox.IsChecked)
            {
                searchParameters.AU_ID = IDEntry.Text ?? "";
            }

            if (YearCheckbox.IsChecked)
            {
                searchParameters.Year = YearEntry.Text ?? "";
            }
            int row = 1;
            foreach (var book in books)
            {
                if (searchParameters.BookFits(book))
                {
                    Display(row, book);
                    row++;
                }
            }
        }

        private void DisplayGrid()
        {
            if (ResultsTable.RowDefinitions.Count > 1) DeleteTable();
            ManageFilters();
            int row = 1;
            foreach (var book in books)
            {
                if (searchParameters.BookFits(book))
                {
                    Display(row, book);
                    row++;
                }
            }
        }

        private async void SearchButton_Clicked(object sender, EventArgs e) 
        {
            if (currentFile == null)
            {
                await DisplayAlert("Помилка", "Спершу обреіть файл", "Ok");
                ClearCheckboxes();
                return;
            }
            string filePath = currentFile.FullPath;
            if (!isParsed) 
                try { books = parser.Parse(filePath); }
            catch (Exception)
            {
                await DisplayAlert("Помилка", "Спроба обробки невалідного файлу", "Ok");
                return;
            }
            isParsed = true;
            DisplayGrid();
        }

        private void ClearCheckboxes()
        {
            TitleEntry.Text = "";
            TitleCheckbox.IsChecked = false;

            AuthorEntry.Text = "";
            AuthorCheckbox.IsChecked = false;

            FormatEntry.Text = "";
            FormatCheckbox.IsChecked = false;

            YearEntry.Text = "";
            YearCheckbox.IsChecked = false;

            IDEntry.Text = "";
            AU_IDCheckbox.IsChecked = false;
        }

        private void ClearButton_Clicked(object sender, EventArgs e) 
        {
            ClearCheckboxes();
            DisplayGrid();

        }

        private async void ExporAstButton_Clicked(object sender, EventArgs e) 
        {
            if (currentFile == null)
            {
                await DisplayAlert("Помилка експорту", "Спершу оберіть файл-джерело", "Ок");
                return;
            }
            XslCompiledTransform xslt = new XslCompiledTransform();

            string f1 = "D:\\proha\\Xmlinator\\XSLTTemplate.xslt"; // xsl path
            xslt.Load(f1);


            // Виконання перетворення і виведення результатів у файл.
            string f2 = currentFile.FullPath;
            using var stream = new MemoryStream(Encoding.Default.GetBytes(""));
            try { 
                var result = await Saver.SaveAsync(currentFile.FileName.Split(".")[0] + ".html", stream, new CancellationTokenSource().Token);
                xslt.Transform(f2, result.FilePath);
            }
            catch (Exception)
            {
                await DisplayAlert("Помилка експорту", "Файл не було створено", "Ок");
                return;
            }
        }

        private async void ExitButton_Clicked(object sender, EventArgs e) 
        {
            bool answer2 = await DisplayAlert("Підтвердження", "Ви дійсно хочете вийти?",
                                                "Так", "Ні");
            if (answer2)
            {
                System.Environment.Exit(0);
            }
        }

        public void DeleteTable()
        {
            var childrenToRemove = ResultsTable.Children
                .Where(child =>
                    child is Label &&
                    Grid.GetRow((View)child) > 0) 
                .ToList();  

            foreach (var child in childrenToRemove)
            {
                ResultsTable.Children.Remove(child);
            }

            for (int row = 1; row < ResultsTable.RowDefinitions.Count; row++)
            {
                ResultsTable.RowDefinitions.RemoveAt(row);
            }
        }


        private void Parser_Selected(object sender, EventArgs e) 
        {
            if (isParsed) isParsed = false;
            books?.Clear();
            switch (ParserPicker.SelectedIndex)
            {
                case 0:
                    parser = new SAXParser();
                    break;
                case 1:
                    parser = new DOMParser();
                    break;
                case 2:
                    parser = new LINQParser();
                    break;
            }
        }
    }
}
