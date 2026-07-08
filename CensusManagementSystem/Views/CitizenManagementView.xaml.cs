using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CensusManagementSystem.Helpers;
using CensusManagementSystem.Models;
using CensusManagementSystem.Services;

namespace CensusManagementSystem.Views
{
    public partial class CitizenManagementView : Page
    {
        private readonly CitizenService _citizenService = new CitizenService();
        private readonly HouseholdService _householdService = new HouseholdService();
        private Citizen _selectedCitizen;

        public CitizenManagementView()
        {
            InitializeComponent();
            Loaded += CitizenManagementView_Loaded;
        }

        private void CitizenManagementView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCitizens();
            LoadHouseholds();
        }

        private void LoadCitizens()
        {
            if (DgCitizens != null)
                DgCitizens.ItemsSource = _citizenService.GetAllCitizens();
        }

        private void LoadHouseholds()
        {
            if (CmbHousehold != null)
                CmbHousehold.ItemsSource = _householdService.GetAllHouseholds();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidationHelper.ValidateRequiredFields(
                (TxtCNIC.Text, "CNIC"),
                (TxtFullName.Text, "Full Name"),
                (TxtAge.Text, "Age"),
                (TxtOccupation.Text, "Occupation")))
                return;

            if (!ValidationHelper.ValidateCNIC(TxtCNIC.Text.Trim()))
                return;

            string cnic = TxtCNIC.Text.Trim();

            if (_citizenService.IsCNICExists(cnic))
            {
                ValidationHelper.ShowValidationError("A citizen with this CNIC already exists.");
                return;
            }

            int age;
            if (!int.TryParse(TxtAge.Text.Trim(), out age))
            {
                ValidationHelper.ShowValidationError("Age must be a valid number.");
                return;
            }

            if (!ValidationHelper.ValidateAge(age))
                return;

            ComboBoxItem gender = CmbGender.SelectedItem as ComboBoxItem;
            ComboBoxItem marital = CmbMaritalStatus.SelectedItem as ComboBoxItem;
            ComboBoxItem education = CmbEducation.SelectedItem as ComboBoxItem;
            ComboBoxItem relation = CmbRelationship.SelectedItem as ComboBoxItem;
            Household household = CmbHousehold.SelectedItem as Household;

            if (gender == null || marital == null || education == null || relation == null || household == null)
            {
                ValidationHelper.ShowValidationError("Please fill all dropdown fields.");
                return;
            }

            Citizen citizen = new Citizen();
            citizen.CNIC = cnic;
            citizen.FullName = TxtFullName.Text.Trim();
            citizen.Gender = gender.Content.ToString();
            citizen.DateOfBirth = DpDOB != null && DpDOB.SelectedDate.HasValue
                ? DpDOB.SelectedDate.Value
                : DateTime.Now.AddYears(-age);

            citizen.Age = age;
            citizen.MaritalStatus = marital.Content.ToString();
            citizen.Education = education.Content.ToString();
            citizen.Occupation = TxtOccupation.Text.Trim();
            citizen.RelationshipWithHead = relation.Content.ToString();
            citizen.HouseholdId = household.HouseholdId;

            if (_citizenService.AddCitizen(citizen))
            {
                ClearForm();
                LoadCitizens();

                MessageBox.Show("Citizen added successfully.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add citizen.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgCitizens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgCitizens == null || DgCitizens.SelectedItem == null)
                return;

            Citizen c = DgCitizens.SelectedItem as Citizen;
            if (c == null) return;

            _selectedCitizen = c;

            TxtCNIC.Text = c.CNIC;
            TxtFullName.Text = c.FullName;
            TxtAge.Text = c.Age.ToString();
            TxtOccupation.Text = c.Occupation;

            if (DpDOB != null)
                DpDOB.SelectedDate = c.DateOfBirth;

            SetCombo(CmbGender, c.Gender);
            SetCombo(CmbMaritalStatus, c.MaritalStatus);
            SetCombo(CmbEducation, c.Education);
            SetCombo(CmbRelationship, c.RelationshipWithHead);

            var households = CmbHousehold.ItemsSource as System.Collections.IEnumerable;
            if (households != null)
            {
                foreach (var item in households)
                {
                    if (item is Household h && h.HouseholdId == c.HouseholdId)
                    {
                        CmbHousehold.SelectedItem = h;
                        break;
                    }
                }
            }

            BtnAdd.Visibility = Visibility.Collapsed;
            BtnUpdate.Visibility = Visibility.Visible;
            BtnCancel.Visibility = Visibility.Visible;
        }

        private void SetCombo(ComboBox combo, string value)
        {
            if (combo == null || value == null) return;

            foreach (ComboBoxItem item in combo.Items)
            {
                if (item.Content != null && item.Content.ToString() == value)
                {
                    combo.SelectedItem = item;
                    break;
                }
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCitizen == null)
                return;

            int age;
            if (!int.TryParse(TxtAge.Text.Trim(), out age))
            {
                ValidationHelper.ShowValidationError("Invalid age.");
                return;
            }

            ComboBoxItem gender = CmbGender.SelectedItem as ComboBoxItem;
            ComboBoxItem marital = CmbMaritalStatus.SelectedItem as ComboBoxItem;
            ComboBoxItem education = CmbEducation.SelectedItem as ComboBoxItem;
            ComboBoxItem relation = CmbRelationship.SelectedItem as ComboBoxItem;
            Household household = CmbHousehold.SelectedItem as Household;

            if (gender == null || marital == null || education == null || relation == null || household == null)
            {
                ValidationHelper.ShowValidationError("Please fill all dropdown fields.");
                return;
            }

            _selectedCitizen.CNIC = TxtCNIC.Text.Trim();
            _selectedCitizen.FullName = TxtFullName.Text.Trim();
            _selectedCitizen.Gender = gender.Content.ToString();
            _selectedCitizen.DateOfBirth = DpDOB != null && DpDOB.SelectedDate.HasValue
                ? DpDOB.SelectedDate.Value
                : DateTime.Now;

            _selectedCitizen.Age = age;
            _selectedCitizen.MaritalStatus = marital.Content.ToString();
            _selectedCitizen.Education = education.Content.ToString();
            _selectedCitizen.Occupation = TxtOccupation.Text.Trim();
            _selectedCitizen.RelationshipWithHead = relation.Content.ToString();
            _selectedCitizen.HouseholdId = household.HouseholdId;

            if (_citizenService.UpdateCitizen(_selectedCitizen))
            {
                ClearForm();
                LoadCitizens();

                MessageBox.Show("Citizen updated successfully.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedCitizen = null;
            TxtCNIC.Clear();
            TxtFullName.Clear();
            TxtAge.Clear();
            TxtOccupation.Clear();
            if (DpDOB != null) DpDOB.SelectedDate = null;
            CmbGender.SelectedIndex = -1;
            CmbMaritalStatus.SelectedIndex = -1;
            CmbEducation.SelectedIndex = -1;
            CmbRelationship.SelectedIndex = -1;
            CmbHousehold.SelectedIndex = -1;

            BtnAdd.Visibility = Visibility.Visible;
            BtnUpdate.Visibility = Visibility.Collapsed;
            BtnCancel.Visibility = Visibility.Collapsed;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Citizen c = btn.DataContext as Citizen;
            if (c == null) return;

            MessageBoxResult result = MessageBox.Show(
                "Delete citizen '" + c.FullName + "'?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _citizenService.DeleteCitizen(c.CitizenId);
                LoadCitizens();
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchCitizens();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchCitizens();
            }
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            TxtSearch.Clear();
            LoadCitizens();
        }

        private void CmbGenderFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void SearchCitizens()
        {
            string keyword = TxtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
                LoadCitizens();
            else
                DgCitizens.ItemsSource = _citizenService.SearchCitizens(keyword);
        }
    }
}