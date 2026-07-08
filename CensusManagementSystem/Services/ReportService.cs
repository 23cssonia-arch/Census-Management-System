using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CensusManagementSystem.DataAccess;

namespace CensusManagementSystem.Services
{
    public class ReportService
    {
        public int GetTotalHouseholds()
        {
            string query = "SELECT COUNT(*) FROM Households";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        public int GetTotalPopulation()
        {
            string query = "SELECT COUNT(*) FROM Citizens";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        public int GetMalePopulation()
        {
            string query = "SELECT COUNT(*) FROM Citizens WHERE Gender = 'Male'";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        public int GetFemalePopulation()
        {
            string query = "SELECT COUNT(*) FROM Citizens WHERE Gender = 'Female'";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        public DataTable GetPopulationByArea()
        {
            string query = "SELECT * FROM [vw populationByArea]";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable GetGenderDistribution()
        {
            string query = @"SELECT Gender, COUNT(*) AS Count FROM Citizens GROUP BY Gender";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable GetLiteracyStatistics()
        {
            string query = "SELECT * FROM vw_LiteracyStatistics";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable GetEmploymentStatistics()
        {
            string query = "SELECT * FROM vw_EmploymentStatistics";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable GetPopulationByProvince()
        {
            string query = @"SELECT p.ProvinceName, COUNT(c.CitizenId) AS TotalPopulation,
                             SUM(CASE WHEN c.Gender = 'Male' THEN 1 ELSE 0 END) AS MaleCount,
                             SUM(CASE WHEN c.Gender = 'Female' THEN 1 ELSE 0 END) AS FemaleCount
                             FROM Provinces p
                             LEFT JOIN Districts d ON p.ProvinceId = d.ProvinceId
                             LEFT JOIN Tehsils t ON d.DistrictId = t.DistrictId
                             LEFT JOIN UnionCouncils uc ON t.TehsilId = uc.TehsilId
                             LEFT JOIN CensusBlocks cb ON uc.UnionCouncilId = cb.UnionCouncilId
                             LEFT JOIN Households h ON cb.CensusBlockId = h.CensusBlockId
                             LEFT JOIN Citizens c ON h.HouseholdId = c.HouseholdId
                             GROUP BY p.ProvinceName
                             ORDER BY TotalPopulation DESC";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable GetMaritalStatusDistribution()
        {
            string query = @"SELECT MaritalStatus, COUNT(*) AS Count,
                             SUM(CASE WHEN Gender = 'Male' THEN 1 ELSE 0 END) AS MaleCount,
                             SUM(CASE WHEN Gender = 'Female' THEN 1 ELSE 0 END) AS FemaleCount
                             FROM Citizens GROUP BY MaritalStatus";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable GetAgeGroupDistribution()
        {
            string query = @"SELECT
                             CASE
                                 WHEN Age < 18 THEN '0-17 (Children)'
                                 WHEN Age BETWEEN 18 AND 35 THEN '18-35 (Young Adults)'
                                 WHEN Age BETWEEN 36 AND 55 THEN '36-55 (Adults)'
                                 WHEN Age BETWEEN 56 AND 70 THEN '56-70 (Senior)'
                                 ELSE '70+ (Elderly)'
                             END AS AgeGroup,
                             COUNT(*) AS Count,
                             SUM(CASE WHEN Gender = 'Male' THEN 1 ELSE 0 END) AS MaleCount,
                             SUM(CASE WHEN Gender = 'Female' THEN 1 ELSE 0 END) AS FemaleCount
                             FROM Citizens
                             GROUP BY
                             CASE
                                 WHEN Age < 18 THEN '0-17 (Children)'
                                 WHEN Age BETWEEN 18 AND 35 THEN '18-35 (Young Adults)'
                                 WHEN Age BETWEEN 36 AND 55 THEN '36-55 (Adults)'
                                 WHEN Age BETWEEN 56 AND 70 THEN '56-70 (Senior)'
                                 ELSE '70+ (Elderly)'
                             END
                             ORDER BY MIN(Age)";
            return DatabaseHelper.ExecuteQuery(query);
        }
    }
}
