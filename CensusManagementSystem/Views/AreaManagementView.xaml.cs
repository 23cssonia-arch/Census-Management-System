using System.Windows;
using System.Windows.Controls;
using CensusManagementSystem.Models;
using CensusManagementSystem.Services;

namespace CensusManagementSystem.Views
{
    public partial class AreaManagementView : Page
    {
        private readonly AreaService _areaService = new AreaService();
        private Province _selectedProvince;

        public AreaManagementView()
        {
            InitializeComponent();
            LoadProvinces();
            LoadDistrictsCombo();
        }

        // ==================== Province ====================
        private void LoadProvinces() => DgProvinces.ItemsSource = _areaService.GetAllProvinces();

        private void BtnAddProvince_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtProvinceName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Province name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_areaService.AddProvince(name))
            {
                TxtProvinceName.Clear();
                LoadProvinces();
                MessageBox.Show("Province added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add province.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgProvinces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgProvinces.SelectedItem is Province p)
            {
                _selectedProvince = p;
                TxtProvinceName.Text = p.ProvinceName;
                BtnAddProvince.Visibility = Visibility.Collapsed;
                BtnUpdateProvince.Visibility = Visibility.Visible;
                BtnCancelProvince.Visibility = Visibility.Visible;
            }
        }

        private void BtnUpdateProvince_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProvince == null) return;
            string name = TxtProvinceName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name)) return;
            if (_areaService.UpdateProvince(_selectedProvince.ProvinceId, name))
            {
                BtnCancelProvince_Click(null, null);
                LoadProvinces();
                MessageBox.Show("Province updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCancelProvince_Click(object sender, RoutedEventArgs e)
        {
            _selectedProvince = null;
            TxtProvinceName.Clear();
            BtnAddProvince.Visibility = Visibility.Visible;
            BtnUpdateProvince.Visibility = Visibility.Collapsed;
            BtnCancelProvince.Visibility = Visibility.Collapsed;
        }

        private void BtnDeleteProvince_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Province p)
            {
                var result = MessageBox.Show($"Delete province '{p.ProvinceName}'?", "Confirm",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes && _areaService.DeleteProvince(p.ProvinceId))
                {
                    LoadProvinces();
                }
            }
        }

        // ==================== District ====================
        private void LoadDistrictsCombo() => CmbDistrictProvince.ItemsSource = _areaService.GetAllProvinces();

        private void CmbDistrictProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbDistrictProvince.SelectedItem is Province p)
            {
                CmbTehsilDistrict.ItemsSource = _areaService.GetDistrictsByProvince(p.ProvinceId);
                LoadDistrictsByProvince(p.ProvinceId);
            }
        }

        private void LoadDistrictsByProvince(int provinceId) => DgDistricts.ItemsSource = _areaService.GetDistrictsByProvince(provinceId);

        private void BtnAddDistrict_Click(object sender, RoutedEventArgs e)
        {
            if (CmbDistrictProvince.SelectedItem is Province p && !string.IsNullOrWhiteSpace(TxtDistrictName.Text.Trim()))
            {
                if (_areaService.AddDistrict(TxtDistrictName.Text.Trim(), p.ProvinceId))
                {
                    TxtDistrictName.Clear();
                    LoadDistrictsByProvince(p.ProvinceId);
                    CmbTehsilDistrict.ItemsSource = _areaService.GetDistrictsByProvince(p.ProvinceId);
                }
            }
        }

        private void BtnDeleteDistrict_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is District d)
            {
                var result = MessageBox.Show($"Delete district '{d.DistrictName}'?", "Confirm",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes && _areaService.DeleteDistrict(d.DistrictId))
                {
                    CmbDistrictProvince_SelectionChanged(null, null);
                }
            }
        }

        // ==================== Tehsil ====================
        private void CmbTehsilDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbTehsilDistrict.SelectedItem is District d)
            {
                CmbUCTehsil.ItemsSource = _areaService.GetTehsilsByDistrict(d.DistrictId);
                DgTehsils.ItemsSource = _areaService.GetTehsilsByDistrict(d.DistrictId);
            }
        }

        private void BtnAddTehsil_Click(object sender, RoutedEventArgs e)
        {
            if (CmbTehsilDistrict.SelectedItem is District d && !string.IsNullOrWhiteSpace(TxtTehsilName.Text.Trim()))
            {
                if (_areaService.AddTehsil(TxtTehsilName.Text.Trim(), d.DistrictId))
                {
                    TxtTehsilName.Clear();
                    CmbTehsilDistrict_SelectionChanged(null, null);
                }
            }
        }

        // ==================== Union Council ====================
        private void CmbUCTehsil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbUCTehsil.SelectedItem is Tehsil t)
            {
                CmbBlockUC.ItemsSource = _areaService.GetUnionCouncilsByTehsil(t.TehsilId);
                DgUnionCouncils.ItemsSource = _areaService.GetUnionCouncilsByTehsil(t.TehsilId);
            }
        }

        private void BtnAddUC_Click(object sender, RoutedEventArgs e)
        {
            if (CmbUCTehsil.SelectedItem is Tehsil t && !string.IsNullOrWhiteSpace(TxtUCName.Text.Trim()))
            {
                if (_areaService.AddUnionCouncil(TxtUCName.Text.Trim(), t.TehsilId))
                {
                    TxtUCName.Clear();
                    CmbUCTehsil_SelectionChanged(null, null);
                }
            }
        }

        // ==================== Census Block ====================
        private void CmbBlockUC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbBlockUC.SelectedItem is UnionCouncil uc)
            {
                DgCensusBlocks.ItemsSource = _areaService.GetCensusBlocksByUnionCouncil(uc.UnionCouncilId);
            }
        }

        private void BtnAddBlock_Click(object sender, RoutedEventArgs e)
        {
            if (CmbBlockUC.SelectedItem is UnionCouncil uc
                && !string.IsNullOrWhiteSpace(TxtBlockCode.Text.Trim())
                && !string.IsNullOrWhiteSpace(TxtBlockName.Text.Trim()))
            {
                if (_areaService.AddCensusBlock(TxtBlockCode.Text.Trim(), TxtBlockName.Text.Trim(), uc.UnionCouncilId))
                {
                    TxtBlockCode.Clear();
                    TxtBlockName.Clear();
                    CmbBlockUC_SelectionChanged(null, null);
                }
            }
        }
    }
}
