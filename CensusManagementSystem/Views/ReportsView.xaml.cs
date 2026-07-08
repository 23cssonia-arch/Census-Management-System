using System;
using System.Data;
using System.Windows.Controls;
using CensusManagementSystem.Services;

namespace CensusManagementSystem.Views
{
    public partial class ReportsView : Page
    {
        private readonly ReportService _reportService = new ReportService();

        public ReportsView()
        {
            InitializeComponent();
            LoadReports();
        }

        private void LoadReports()
        {
            try
            {
                DgProvincePopulation.ItemsSource = _reportService.GetPopulationByProvince().DefaultView;
                DgAreaPopulation.ItemsSource = _reportService.GetPopulationByArea().DefaultView;
                DgGender.ItemsSource = _reportService.GetGenderDistribution().DefaultView;
                DgLiteracy.ItemsSource = _reportService.GetLiteracyStatistics().DefaultView;
                DgEmployment.ItemsSource = _reportService.GetEmploymentStatistics().DefaultView;
                DgAgeGroups.ItemsSource = _reportService.GetAgeGroupDistribution().DefaultView;
                DgMaritalStatus.ItemsSource = _reportService.GetMaritalStatusDistribution().DefaultView;

                LoadGenderSummary();
            }
            catch (Exception)
            {
                // Database not connected
            }
        }

        private void LoadGenderSummary()
        {
            try
            {
                DataTable genderData = _reportService.GetGenderDistribution();
                int male = 0, female = 0, other = 0;
                foreach (DataRow row in genderData.Rows)
                {
                    string gender = row["Gender"].ToString();
                    int count = Convert.ToInt32(row["Count"]);
                    if (gender == "Male") male = count;
                    else if (gender == "Female") female = count;
                    else other = count;
                }
                LblMaleCount.Text = male.ToString("N0");
                LblFemaleCount.Text = female.ToString("N0");
                LblOtherCount.Text = other.ToString("N0");
            }
            catch { }
        }
    }
}
