using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CensusManagementSystem.DataAccess;
using CensusManagementSystem.Models;

namespace CensusManagementSystem.Services
{
    public class CitizenService
    {
        public List<Citizen> GetAllCitizens()
        {
            string query = @"SELECT c.CitizenId, c.CNIC, c.FullName, c.Gender, c.DateOfBirth, c.Age,
                             c.MaritalStatus, c.Education, c.Occupation, c.RelationshipWithHead,
                             c.HouseholdId, h.HouseNumber
                             FROM Citizens c
                             INNER JOIN Households h ON c.HouseholdId = h.HouseholdId
                             ORDER BY c.FullName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            List<Citizen> list = new List<Citizen>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Citizen
                {
                    CitizenId = Convert.ToInt32(row["CitizenId"]),
                    CNIC = row["CNIC"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    Age = Convert.ToInt32(row["Age"]),
                    MaritalStatus = row["MaritalStatus"].ToString(),
                    Education = row["Education"].ToString(),
                    Occupation = row["Occupation"].ToString(),
                    RelationshipWithHead = row["RelationshipWithHead"].ToString(),
                    HouseholdId = Convert.ToInt32(row["HouseholdId"]),
                    HouseNumber = row["HouseNumber"].ToString()
                });
            }
            return list;
        }

        public List<Citizen> GetCitizensByHousehold(int householdId)
        {
            string query = @"SELECT c.CitizenId, c.CNIC, c.FullName, c.Gender, c.DateOfBirth, c.Age,
                             c.MaritalStatus, c.Education, c.Occupation, c.RelationshipWithHead,
                             c.HouseholdId, h.HouseNumber
                             FROM Citizens c
                             INNER JOIN Households h ON c.HouseholdId = h.HouseholdId
                             WHERE c.HouseholdId = @HouseholdId ORDER BY c.FullName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@HouseholdId", householdId));
            List<Citizen> list = new List<Citizen>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Citizen
                {
                    CitizenId = Convert.ToInt32(row["CitizenId"]),
                    CNIC = row["CNIC"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    Age = Convert.ToInt32(row["Age"]),
                    MaritalStatus = row["MaritalStatus"].ToString(),
                    Education = row["Education"].ToString(),
                    Occupation = row["Occupation"].ToString(),
                    RelationshipWithHead = row["RelationshipWithHead"].ToString(),
                    HouseholdId = Convert.ToInt32(row["HouseholdId"]),
                    HouseNumber = row["HouseNumber"].ToString()
                });
            }
            return list;
        }

        public Citizen GetCitizenById(int id)
        {
            string query = @"SELECT c.CitizenId, c.CNIC, c.FullName, c.Gender, c.DateOfBirth, c.Age,
                             c.MaritalStatus, c.Education, c.Occupation, c.RelationshipWithHead,
                             c.HouseholdId, h.HouseNumber
                             FROM Citizens c
                             INNER JOIN Households h ON c.HouseholdId = h.HouseholdId
                             WHERE c.CitizenId = @Id";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@Id", id));
            if (dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];
            return new Citizen
            {
                CitizenId = Convert.ToInt32(row["CitizenId"]),
                CNIC = row["CNIC"].ToString(),
                FullName = row["FullName"].ToString(),
                Gender = row["Gender"].ToString(),
                DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                Age = Convert.ToInt32(row["Age"]),
                MaritalStatus = row["MaritalStatus"].ToString(),
                Education = row["Education"].ToString(),
                Occupation = row["Occupation"].ToString(),
                RelationshipWithHead = row["RelationshipWithHead"].ToString(),
                HouseholdId = Convert.ToInt32(row["HouseholdId"]),
                HouseNumber = row["HouseNumber"].ToString()
            };
        }

        public bool AddCitizen(Citizen citizen)
        {
            string query = @"INSERT INTO Citizens (CNIC, FullName, Gender, DateOfBirth, Age, MaritalStatus,
                             Education, Occupation, RelationshipWithHead, HouseholdId)
                             VALUES (@CNIC, @FullName, @Gender, @DOB, @Age, @MaritalStatus,
                             @Education, @Occupation, @RelHead, @HouseholdId)";
            SqlParameter[] parameters = {
                new SqlParameter("@CNIC", citizen.CNIC),
                new SqlParameter("@FullName", citizen.FullName),
                new SqlParameter("@Gender", citizen.Gender),
                new SqlParameter("@DOB", citizen.DateOfBirth),
                new SqlParameter("@Age", citizen.Age),
                new SqlParameter("@MaritalStatus", citizen.MaritalStatus),
                new SqlParameter("@Education", citizen.Education),
                new SqlParameter("@Occupation", citizen.Occupation),
                new SqlParameter("@RelHead", citizen.RelationshipWithHead),
                new SqlParameter("@HouseholdId", citizen.HouseholdId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateCitizen(Citizen citizen)
        {
            string query = @"UPDATE Citizens SET CNIC = @CNIC, FullName = @FullName, Gender = @Gender,
                             DateOfBirth = @DOB, Age = @Age, MaritalStatus = @MaritalStatus,
                             Education = @Education, Occupation = @Occupation,
                             RelationshipWithHead = @RelHead, HouseholdId = @HouseholdId
                             WHERE CitizenId = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@CNIC", citizen.CNIC),
                new SqlParameter("@FullName", citizen.FullName),
                new SqlParameter("@Gender", citizen.Gender),
                new SqlParameter("@DOB", citizen.DateOfBirth),
                new SqlParameter("@Age", citizen.Age),
                new SqlParameter("@MaritalStatus", citizen.MaritalStatus),
                new SqlParameter("@Education", citizen.Education),
                new SqlParameter("@Occupation", citizen.Occupation),
                new SqlParameter("@RelHead", citizen.RelationshipWithHead),
                new SqlParameter("@HouseholdId", citizen.HouseholdId),
                new SqlParameter("@Id", citizen.CitizenId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteCitizen(int id)
        {
            string query = "DELETE FROM Citizens WHERE CitizenId = @Id";
            return DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id)) > 0;
        }

        public bool IsCNICExists(string cnic, int? excludeId = null)
        {
            string query = "SELECT COUNT(*) FROM Citizens WHERE CNIC = @CNIC";
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@CNIC", cnic) };
            if (excludeId.HasValue)
            {
                query += " AND CitizenId != @ExcludeId";
                parameters.Add(new SqlParameter("@ExcludeId", excludeId.Value));
            }
            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters.ToArray()));
            return count > 0;
        }

        public List<Citizen> SearchCitizens(string keyword)
        {
            string query = @"SELECT c.CitizenId, c.CNIC, c.FullName, c.Gender, c.DateOfBirth, c.Age,
                             c.MaritalStatus, c.Education, c.Occupation, c.RelationshipWithHead,
                             c.HouseholdId, h.HouseNumber
                             FROM Citizens c
                             INNER JOIN Households h ON c.HouseholdId = h.HouseholdId
                             WHERE c.CNIC LIKE @Keyword OR c.FullName LIKE @Keyword
                             OR c.Occupation LIKE @Keyword OR c.Education LIKE @Keyword
                             ORDER BY c.FullName";
            SqlParameter[] parameters = {
                new SqlParameter("@Keyword", "%" + keyword + "%")
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            List<Citizen> list = new List<Citizen>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Citizen
                {
                    CitizenId = Convert.ToInt32(row["CitizenId"]),
                    CNIC = row["CNIC"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    Age = Convert.ToInt32(row["Age"]),
                    MaritalStatus = row["MaritalStatus"].ToString(),
                    Education = row["Education"].ToString(),
                    Occupation = row["Occupation"].ToString(),
                    RelationshipWithHead = row["RelationshipWithHead"].ToString(),
                    HouseholdId = Convert.ToInt32(row["HouseholdId"]),
                    HouseNumber = row["HouseNumber"].ToString()
                });
            }
            return list;
        }
    }
}
