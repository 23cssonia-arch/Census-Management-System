using System;
using System.Windows.Controls;
using CensusManagementSystem.Services;

namespace CensusManagementSystem.Views
{
    public partial class DashboardView : Page
    {
        private readonly ReportService _reportService = new ReportService();
        private readonly AreaService _areaService = new AreaService();
        private readonly CitizenService _citizenService = new CitizenService();

        public DashboardView()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                int totalHH = _reportService.GetTotalHouseholds();
                int totalPop = _reportService.GetTotalPopulation();
                int male = _reportService.GetMalePopulation();
                int female = _reportService.GetFemalePopulation();

                LblHouseholds.Text = totalHH.ToString("N0");
                LblPopulation.Text = totalPop.ToString("N0");
                LblMale.Text = male.ToString("N0");
                LblFemale.Text = female.ToString("N0");

                if (totalPop > 0)
                {
                    LblMalePercent.Text = ((male * 100.0) / totalPop).ToString("F1") + "% of total";
                    LblFemalePercent.Text = ((female * 100.0) / totalPop).ToString("F1") + "% of total";
                }

                LblProvinces.Text = _areaService.GetAllProvinces().Count.ToString();
                LblDistricts.Text = _areaService.GetAllDistricts().Count.ToString();
                LblTehsils.Text = "0";
                LblUCs.Text = "0";
                LblBlocks.Text = "0";

                var citizens = _citizenService.GetAllCitizens();
                int displayCount = Math.Min(citizens.Count, 10);
                DgRecentCitizens.ItemsSource = citizens.GetRange(0, displayCount);
            }
            catch (Exception)
            {
                // Database not connected yet - show zeros
            }
        }
    }
}
