using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;

namespace Lista
{
    public partial class MainWindow : Window
    {
        private void firstNameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            firstNameTextBox.Text = "";
        }
        private void lastNameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            lastNameTextBox.Text = ""; 
        }
        private void ageTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ageTextBox.Text = ""; 
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string json = JsonConvert.SerializeObject(People);
                File.WriteAllText(filePath, json);

                MessageBox.Show("Dane zapisanie poprawnie.");
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    string json = File.ReadAllText(filePath);
                    People = JsonConvert.DeserializeObject<ObservableCollection<Person>>(json);
                    peopleListBox.ItemsSource = People;

                    MessageBox.Show("Dane załadowane poprawnie.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd przy ładowniu danych: {ex.Message}");
                }
            }
        }
        public ObservableCollection<Person> People { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            People = new ObservableCollection<Person>();
            peopleListBox.ItemsSource = People;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            int age;

            if (int.TryParse(ageTextBox.Text, out age) && genderComboBox.SelectedItem != null)
            {
                string gender = genderComboBox.Text;

                Person newPerson = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Gender = gender
                };

                People.Add(newPerson);

         
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Podaj poprawny wiek i płeć.");
            }
        }

        private void ClearInputFields()
        {
            firstNameTextBox.Clear();
            lastNameTextBox.Clear();
            ageTextBox.Clear();
            genderComboBox.SelectedIndex = -1;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (peopleListBox.SelectedItem != null)
            {
                Person selectedPerson = (Person)peopleListBox.SelectedItem;

                firstNameTextBox.Text = selectedPerson.FirstName;
                lastNameTextBox.Text = selectedPerson.LastName;
                ageTextBox.Text = selectedPerson.Age.ToString();
                genderComboBox.SelectedItem = selectedPerson.Gender;

                People.Remove(selectedPerson);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (peopleListBox.SelectedItem != null)
            {
                People.Remove((Person)peopleListBox.SelectedItem);
            }
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }

    enum Plec 
    {
        Mezczyzna = 0,
        Kobieta = 1,
        Inna = 2
    }
}