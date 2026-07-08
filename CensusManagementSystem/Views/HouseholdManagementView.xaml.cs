using CensusManagementSystem.Helpers;
using CensusManagementSystem.Models;
using CensusManagementSystem.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CensusManagementSystem.Views
{
    public partial class HouseholdManagementView : Page
    {
        private readonly HouseholdService _householdService = new HouseholdService();
        private readonly AreaService _areaService = new AreaService();
        private Household _selectedHousehold;

        public HouseholdManagementView()
        {
            InitializeComponent();
            LoadHouseholds();
        }

        private void LoadHouseholds() => DgHouseholds.ItemsSource = _householdService.GetAllHouseholds();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Explicitly declared as an array to resolve the CS0029 compilation error
            if (!ValidationHelper.ValidateRequiredFields(new[] {
                (TxtHouseNumber.Text, "House Number"),
                (TxtFamilyNumber.Text, "Family Number"),
                (TxtAddress.Text, "Address"),
                (TxtHeadOfFamily.Text, "Head of Family"),
                (TxtCensusBlock.Text, "Census Block ID"),
                (TxtNumMembers.Text, "Number of Family Members")
            }))
                return;

            if (!ValidationHelper.IsNumeric(TxtNumMembers.Text.Trim()))
            {
                ValidationHelper.ShowValidationError("Number of Family Members must be a valid number.");
                return;
            }

            if (!ValidationHelper.IsNumeric(TxtCensusBlock.Text.Trim()))
            {
                ValidationHelper.ShowValidationError("Census Block ID must be a valid numeric ID.");
                return;
            }

            if (!ValidationHelper.IsNumeric(TxtCensusBlock.Text.Trim()))
            {
                ValidationHelper.ShowValidationError("Census Block ID must be a valid numeric ID.");
                return;
            }

            // ---- ADD THIS FOREIGN KEY CHECK HERE ----
            int enteredBlockId = int.Parse(TxtCensusBlock.Text.Trim());
            string checkQuery = "SELECT COUNT(1) FROM dbo.CensusBlocks WHERE CensusBlockId = @BlockId";
            object result = CensusManagementSystem.DataAccess.DatabaseHelper.ExecuteScalar(
                checkQuery,
                new Microsoft.Data.SqlClient.SqlParameter("@BlockId", enteredBlockId)
            );

            if (result == null || Convert.ToInt32(result) == 0)
            {
                MessageBox.Show($"Census Block ID '{enteredBlockId}' does not exist in the system. Please enter a valid existing ID.",
                                "Invalid ID Reference", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var household = new Household
            {
                HouseNumber = TxtHouseNumber.Text.Trim(),
                FamilyNumber = TxtFamilyNumber.Text.Trim(),
                Address = TxtAddress.Text.Trim(),
                HeadOfFamily = TxtHeadOfFamily.Text.Trim(),
                NumberOfFamilyMembers = int.Parse(TxtNumMembers.Text.Trim()),
                CensusBlockId = int.Parse(TxtCensusBlock.Text.Trim())
            };

            if (_householdService.AddHousehold(household))
            {
                ClearForm();
                LoadHouseholds();
                MessageBox.Show("Household added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add household.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgHouseholds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgHouseholds.SelectedItem is Household h)
            {
                _selectedHousehold = h;
                TxtHouseNumber.Text = h.HouseNumber;
                TxtFamilyNumber.Text = h.FamilyNumber;
                TxtAddress.Text = h.Address;
                TxtHeadOfFamily.Text = h.HeadOfFamily;
                TxtNumMembers.Text = h.NumberOfFamilyMembers.ToString();
                TxtCensusBlock.Text = h.CensusBlockId.ToString();

                BtnAdd.Visibility = Visibility.Collapsed;
                BtnUpdate.Visibility = Visibility.Visible;
                BtnCancel.Visibility = Visibility.Visible;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedHousehold == null) return;

            if (!ValidationHelper.IsNumeric(TxtCensusBlock.Text.Trim()))
            {
                ValidationHelper.ShowValidationError("Census Block ID must be a valid numeric ID.");
                return;
            }

            _selectedHousehold.HouseNumber = TxtHouseNumber.Text.Trim();
            _selectedHousehold.FamilyNumber = TxtFamilyNumber.Text.Trim();
            _selectedHousehold.Address = TxtAddress.Text.Trim();
            _selectedHousehold.HeadOfFamily = TxtHeadOfFamily.Text.Trim();
            _selectedHousehold.NumberOfFamilyMembers = int.Parse(TxtNumMembers.Text.Trim());
            _selectedHousehold.CensusBlockId = int.Parse(TxtCensusBlock.Text.Trim());

            if (_householdService.UpdateHousehold(_selectedHousehold))
            {
                ClearForm();
                LoadHouseholds();
                MessageBox.Show("Household updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClearForm();

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Household h)
            {
                var result = MessageBox.Show($"Delete household '{h.HouseNumber}'?", "Confirm",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes && _householdService.DeleteHousehold(h.HouseholdId))
                {
                    LoadHouseholds();
                }
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = TxtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                DgHouseholds.ItemsSource = _householdService.SearchHouseholds(keyword);
            }
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            TxtSearch.Clear();
            LoadHouseholds();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) BtnSearch_Click(sender, null);
        }

        private void ClearForm()
        {
            _selectedHousehold = null;
            TxtHouseNumber.Clear();
            TxtFamilyNumber.Clear();
            TxtAddress.Clear();
            TxtHeadOfFamily.Clear();
            TxtNumMembers.Clear();
            TxtCensusBlock.Clear();
            BtnAdd.Visibility = Visibility.Visible;
            BtnUpdate.Visibility = Visibility.Collapsed;
            BtnCancel.Visibility = Visibility.Collapsed;
        }
    }
}