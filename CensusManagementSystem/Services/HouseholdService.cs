using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CensusManagementSystem.DataAccess;
using CensusManagementSystem.Models;

namespace CensusManagementSystem.Services
{
    public class HouseholdService
    {
        public List<Household> GetAllHouseholds()
        {
            string query = @"SELECT h.HouseholdId, h.HouseNumber, h.FamilyNumber, h.Address, h.HeadOfFamily,
                             h.NumberOfFamilyMembers, h.CensusBlockId, cb.BlockName
                             FROM Households h
                             INNER JOIN CensusBlocks cb ON h.CensusBlockId = cb.CensusBlockId
                             ORDER BY h.HouseNumber";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            List<Household> list = new List<Household>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Household
                {
                    HouseholdId = Convert.ToInt32(row["HouseholdId"]),
                    HouseNumber = row["HouseNumber"].ToString(),
                    FamilyNumber = row["FamilyNumber"].ToString(),
                    Address = row["Address"].ToString(),
                    HeadOfFamily = row["HeadOfFamily"].ToString(),
                    NumberOfFamilyMembers = Convert.ToInt32(row["NumberOfFamilyMembers"]),
                    CensusBlockId = Convert.ToInt32(row["CensusBlockId"]),
                    BlockName = row["BlockName"].ToString()
                });
            }
            return list;
        }

        public List<Household> GetHouseholdsByBlock(int blockId)
        {
            string query = @"SELECT h.HouseholdId, h.HouseNumber, h.FamilyNumber, h.Address, h.HeadOfFamily,
                             h.NumberOfFamilyMembers, h.CensusBlockId, cb.BlockName
                             FROM Households h
                             INNER JOIN CensusBlocks cb ON h.CensusBlockId = cb.CensusBlockId
                             WHERE h.CensusBlockId = @BlockId ORDER BY h.HouseNumber";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@BlockId", blockId));
            List<Household> list = new List<Household>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Household
                {
                    HouseholdId = Convert.ToInt32(row["HouseholdId"]),
                    HouseNumber = row["HouseNumber"].ToString(),
                    FamilyNumber = row["FamilyNumber"].ToString(),
                    Address = row["Address"].ToString(),
                    HeadOfFamily = row["HeadOfFamily"].ToString(),
                    NumberOfFamilyMembers = Convert.ToInt32(row["NumberOfFamilyMembers"]),
                    CensusBlockId = Convert.ToInt32(row["CensusBlockId"]),
                    BlockName = row["BlockName"].ToString()
                });
            }
            return list;
        }

        public Household GetHouseholdById(int id)
        {
            string query = @"SELECT h.HouseholdId, h.HouseNumber, h.FamilyNumber, h.Address, h.HeadOfFamily,
                             h.NumberOfFamilyMembers, h.CensusBlockId, cb.BlockName
                             FROM Households h
                             INNER JOIN CensusBlocks cb ON h.CensusBlockId = cb.CensusBlockId
                             WHERE h.HouseholdId = @Id";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Id", id));
            if (dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];
            return new Household
            {
                HouseholdId = Convert.ToInt32(row["HouseholdId"]),
                HouseNumber = row["HouseNumber"].ToString(),
                FamilyNumber = row["FamilyNumber"].ToString(),
                Address = row["Address"].ToString(),
                HeadOfFamily = row["HeadOfFamily"].ToString(),
                NumberOfFamilyMembers = Convert.ToInt32(row["NumberOfFamilyMembers"]),
                CensusBlockId = Convert.ToInt32(row["CensusBlockId"]),
                BlockName = row["BlockName"].ToString()
            };
        }

        public bool AddHousehold(Household household)
        {
            string query = @"INSERT INTO Households (HouseNumber, FamilyNumber, Address, HeadOfFamily, NumberOfFamilyMembers, CensusBlockId)
                             VALUES (@HouseNumber, @FamilyNumber, @Address, @HeadOfFamily, @NumMembers, @BlockId)";
            SqlParameter[] parameters = {
                new SqlParameter("@HouseNumber", household.HouseNumber),
                new SqlParameter("@FamilyNumber", household.FamilyNumber),
                new SqlParameter("@Address", household.Address),
                new SqlParameter("@HeadOfFamily", household.HeadOfFamily),
                new SqlParameter("@NumMembers", household.NumberOfFamilyMembers),
                new SqlParameter("@BlockId", household.CensusBlockId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateHousehold(Household household)
        {
            string query = @"UPDATE Households SET HouseNumber = @HouseNumber, FamilyNumber = @FamilyNumber,
                             Address = @Address, HeadOfFamily = @HeadOfFamily, NumberOfFamilyMembers = @NumMembers,
                             CensusBlockId = @BlockId WHERE HouseholdId = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@HouseNumber", household.HouseNumber),
                new SqlParameter("@FamilyNumber", household.FamilyNumber),
                new SqlParameter("@Address", household.Address),
                new SqlParameter("@HeadOfFamily", household.HeadOfFamily),
                new SqlParameter("@NumMembers", household.NumberOfFamilyMembers),
                new SqlParameter("@BlockId", household.CensusBlockId),
                new SqlParameter("@Id", household.HouseholdId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteHousehold(int id)
        {
            string query = "DELETE FROM Households WHERE HouseholdId = @Id";
            return DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id)) > 0;
        }

        public List<Household> SearchHouseholds(string keyword)
        {
            string query = @"SELECT h.HouseholdId, h.HouseNumber, h.FamilyNumber, h.Address, h.HeadOfFamily,
                             h.NumberOfFamilyMembers, h.CensusBlockId, cb.BlockName
                             FROM Households h
                             INNER JOIN CensusBlocks cb ON h.CensusBlockId = cb.CensusBlockId
                             WHERE h.HouseNumber LIKE @Keyword OR h.FamilyNumber LIKE @Keyword
                             OR h.Address LIKE @Keyword OR h.HeadOfFamily LIKE @Keyword
                             ORDER BY h.HouseNumber";
            SqlParameter[] parameters = {
                new SqlParameter("@Keyword", "%" + keyword + "%")
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            List<Household> list = new List<Household>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Household
                {
                    HouseholdId = Convert.ToInt32(row["HouseholdId"]),
                    HouseNumber = row["HouseNumber"].ToString(),
                    FamilyNumber = row["FamilyNumber"].ToString(),
                    Address = row["Address"].ToString(),
                    HeadOfFamily = row["HeadOfFamily"].ToString(),
                    NumberOfFamilyMembers = Convert.ToInt32(row["NumberOfFamilyMembers"]),
                    CensusBlockId = Convert.ToInt32(row["CensusBlockId"]),
                    BlockName = row["BlockName"].ToString()
                });
            }
            return list;
        }
    }
}
