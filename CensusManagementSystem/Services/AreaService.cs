using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CensusManagementSystem.DataAccess;
using CensusManagementSystem.Models;

namespace CensusManagementSystem.Services
{
    public class AreaService
    {
        // ==================== Province ====================
        public List<Province> GetAllProvinces()
        {
            string query = "SELECT ProvinceId, ProvinceName FROM Provinces ORDER BY ProvinceName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            List<Province> list = new List<Province>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Province
                {
                    ProvinceId = Convert.ToInt32(row["ProvinceId"]),
                    ProvinceName = row["ProvinceName"].ToString()
                });
            }
            return list;
        }

        public bool AddProvince(string name)
        {
            string query = "INSERT INTO Provinces (ProvinceName) VALUES (@Name)";
            return DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Name", name)) > 0;
        }

        public bool UpdateProvince(int id, string name)
        {
            string query = "UPDATE Provinces SET ProvinceName = @Name WHERE ProvinceId = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", name),
                new SqlParameter("@Id", id)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteProvince(int id)
        {
            string query = "DELETE FROM Provinces WHERE ProvinceId = @Id";
            return DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id)) > 0;
        }

        // ==================== District ====================
        public List<District> GetDistrictsByProvince(int provinceId)
        {
            string query = "SELECT d.DistrictId, d.DistrictName, d.ProvinceId, p.ProvinceName FROM Districts d INNER JOIN Provinces p ON d.ProvinceId = p.ProvinceId WHERE d.ProvinceId = @ProvinceId ORDER BY d.DistrictName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@ProvinceId", provinceId));
            List<District> list = new List<District>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new District
                {
                    DistrictId = Convert.ToInt32(row["DistrictId"]),
                    DistrictName = row["DistrictName"].ToString(),
                    ProvinceId = Convert.ToInt32(row["ProvinceId"]),
                    ProvinceName = row["ProvinceName"].ToString()
                });
            }
            return list;
        }

        public List<District> GetAllDistricts()
        {
            string query = "SELECT d.DistrictId, d.DistrictName, d.ProvinceId, p.ProvinceName FROM Districts d INNER JOIN Provinces p ON d.ProvinceId = p.ProvinceId ORDER BY d.DistrictName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            List<District> list = new List<District>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new District
                {
                    DistrictId = Convert.ToInt32(row["DistrictId"]),
                    DistrictName = row["DistrictName"].ToString(),
                    ProvinceId = Convert.ToInt32(row["ProvinceId"]),
                    ProvinceName = row["ProvinceName"].ToString()
                });
            }
            return list;
        }

        public bool AddDistrict(string name, int provinceId)
        {
            string query = "INSERT INTO Districts (DistrictName, ProvinceId) VALUES (@Name, @ProvinceId)";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", name),
                new SqlParameter("@ProvinceId", provinceId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateDistrict(int id, string name, int provinceId)
        {
            string query = "UPDATE Districts SET DistrictName = @Name, ProvinceId = @ProvinceId WHERE DistrictId = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", name),
                new SqlParameter("@ProvinceId", provinceId),
                new SqlParameter("@Id", id)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteDistrict(int id)
        {
            string query = "DELETE FROM Districts WHERE DistrictId = @Id";
            return DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id)) > 0;
        }

        // ==================== Tehsil ====================
        public List<Tehsil> GetTehsilsByDistrict(int districtId)
        {
            string query = "SELECT t.TehsilId, t.TehsilName, t.DistrictId, d.DistrictName FROM Tehsils t INNER JOIN Districts d ON t.DistrictId = d.DistrictId WHERE t.DistrictId = @DistrictId ORDER BY t.TehsilName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@DistrictId", districtId));
            List<Tehsil> list = new List<Tehsil>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Tehsil
                {
                    TehsilId = Convert.ToInt32(row["TehsilId"]),
                    TehsilName = row["TehsilName"].ToString(),
                    DistrictId = Convert.ToInt32(row["DistrictId"]),
                    DistrictName = row["DistrictName"].ToString()
                });
            }
            return list;
        }

        public bool AddTehsil(string name, int districtId)
        {
            string query = "INSERT INTO Tehsils (TehsilName, DistrictId) VALUES (@Name, @DistrictId)";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", name),
                new SqlParameter("@DistrictId", districtId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateTehsil(int id, string name, int districtId)
        {
            string query = "UPDATE Tehsils SET TehsilName = @Name, DistrictId = @DistrictId WHERE TehsilId = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", name),
                new SqlParameter("@DistrictId", districtId),
                new SqlParameter("@Id", id)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteTehsil(int id)
        {
            string query = "DELETE FROM Tehsils WHERE TehsilId = @Id";
            return DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id)) > 0;
        }

        // ==================== Union Council ====================
        public List<UnionCouncil> GetUnionCouncilsByTehsil(int tehsilId)
        {
            string query = "SELECT uc.UnionCouncilId, uc.UnionCouncilName, uc.TehsilId, t.TehsilName FROM UnionCouncils uc INNER JOIN Tehsils t ON uc.TehsilId = t.TehsilId WHERE uc.TehsilId = @TehsilId ORDER BY uc.UnionCouncilName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@TehsilId", tehsilId));
            List<UnionCouncil> list = new List<UnionCouncil>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new UnionCouncil
                {
                    UnionCouncilId = Convert.ToInt32(row["UnionCouncilId"]),
                    UnionCouncilName = row["UnionCouncilName"].ToString(),
                    TehsilId = Convert.ToInt32(row["TehsilId"]),
                    TehsilName = row["TehsilName"].ToString()
                });
            }
            return list;
        }

        public bool AddUnionCouncil(string name, int tehsilId)
        {
            string query = "INSERT INTO UnionCouncils (UnionCouncilName, TehsilId) VALUES (@Name, @TehsilId)";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", name),
                new SqlParameter("@TehsilId", tehsilId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateUnionCouncil(int id, string name, int tehsilId)
        {
            string query = "UPDATE UnionCouncils SET UnionCouncilName = @Name, TehsilId = @TehsilId WHERE UnionCouncilId = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", name),
                new SqlParameter("@TehsilId", tehsilId),
                new SqlParameter("@Id", id)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteUnionCouncil(int id)
        {
            string query = "DELETE FROM UnionCouncils WHERE UnionCouncilId = @Id";
            return DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id)) > 0;
        }

        // ==================== Census Block ====================
        public List<CensusBlock> GetCensusBlocksByUnionCouncil(int unionCouncilId)
        {
            string query = "SELECT cb.CensusBlockId, cb.BlockCode, cb.BlockName, cb.UnionCouncilId, uc.UnionCouncilName FROM CensusBlocks cb INNER JOIN UnionCouncils uc ON cb.UnionCouncilId = uc.UnionCouncilId WHERE cb.UnionCouncilId = @UCId ORDER BY cb.BlockName";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@UCId", unionCouncilId));
            List<CensusBlock> list = new List<CensusBlock>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CensusBlock
                {
                    CensusBlockId = Convert.ToInt32(row["CensusBlockId"]),
                    BlockCode = row["BlockCode"].ToString(),
                    BlockName = row["BlockName"].ToString(),
                    UnionCouncilId = Convert.ToInt32(row["UnionCouncilId"]),
                    UnionCouncilName = row["UnionCouncilName"].ToString()
                });
            }
            return list;
        }

        public bool AddCensusBlock(string code, string name, int unionCouncilId)
        {
            string query = "INSERT INTO CensusBlocks (BlockCode, BlockName, UnionCouncilId) VALUES (@Code, @Name, @UCId)";
            SqlParameter[] parameters = {
                new SqlParameter("@Code", code),
                new SqlParameter("@Name", name),
                new SqlParameter("@UCId", unionCouncilId)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateCensusBlock(int id, string code, string name, int unionCouncilId)
        {
            string query = "UPDATE CensusBlocks SET BlockCode = @Code, BlockName = @Name, UnionCouncilId = @UCId WHERE CensusBlockId = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Code", code),
                new SqlParameter("@Name", name),
                new SqlParameter("@UCId", unionCouncilId),
                new SqlParameter("@Id", id)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteCensusBlock(int id)
        {
            string query = "DELETE FROM CensusBlocks WHERE CensusBlockId = @Id";
            return DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", id)) > 0;
        }
    }
}
