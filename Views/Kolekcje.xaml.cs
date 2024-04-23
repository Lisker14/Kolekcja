using System;
using System.IO;
using StudentTestPicker.Models;

namespace StudentTestPicker.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class ClassStudents : ContentPage
    {
        public string ItemId
        {
            set { Load(value); }
        }

        public ClassStudents()
        {
            InitializeComponent();
        }

        public void Load(string classNumber)
        {
            BindingContext = new Models.AllColections(classNumber);
        }

        private void Add(object sender, EventArgs e)
        {
            ((Models.AllColections)BindingContext).AddStudent(((Models.AllColections)BindingContext).getClassNumber(), StudentName.Text, StudentSurname.Text);
            ((Models.AllColections)BindingContext).LoadStudents(((Models.AllColections)BindingContext).getClassNumber());
        }

        private async void ReturnButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        private async void Draw(object sender, EventArgs e)
        {
            Models.AllColections allColection = (Models.AllColections)BindingContext;

            if (allColection.Students.Count == 0)
            {
                await DisplayAlert("Brak przedmiotow", "Nie ma dostêpnych przedmiotow do losowania.", "OK");
            }
            else
            {
                Random random = new Random();
                int randomIndex = random.Next(0, allColection.Students.Count);

                Models.Colection s = allColection.Students[randomIndex];
                if (string.IsNullOrEmpty(s.Nazwa))
                {
                    await DisplayAlert("Brak przedmiotow", "Nie ma dostêpnych przedmiotow do losowania.", "OK");
                }
                else
                {

                    await DisplayAlert("Wylosowany przedmiot", $"{s.Nazwa} {s.Opis}", "OK");
                }
            }
        }
        private void Remove(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var student = (Models.Colection)button.CommandParameter;

            ((Models.AllColections)BindingContext).DeleteStudent(student.NazwaKolekcji, student.Nazwa, student.Opis);
            ((Models.AllColections)BindingContext).LoadStudents(student.NazwaKolekcji);
        }
    }
}
