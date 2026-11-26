using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BankAtmSystem.DTO;

namespace BankAtmSystem.DAL
{
    public class AtmStationDAL
    {
        public List<AtmStationDTO> GetAll(string? keyword = null)
        {
            var list = new List<AtmStationDTO>();
            string sql = @"SELECT MaTram, DiaChi, TonQuy, TrangThai, AnhHienTruong, NgayCapNhat
                           FROM AtmStations";
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " WHERE MaTram LIKE @Kw OR DiaChi LIKE @Kw OR TrangThai LIKE @Kw";
                parameters.Add(new SqlParameter("@Kw", SqlDbType.NVarChar, 250) { Value = "%" + keyword.Trim() + "%" });
            }

            sql += " ORDER BY MaTram ASC";

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new AtmStationDTO
                {
                    MaTram = row["MaTram"].ToString() ?? string.Empty,
                    DiaChi = row["DiaChi"].ToString() ?? string.Empty,
                    TonQuy = Convert.ToDecimal(row["TonQuy"]),
                    TrangThai = row["TrangThai"].ToString() ?? "Hoạt động",
                    AnhHienTruong = row["AnhHienTruong"] == DBNull.Value ? null : row["AnhHienTruong"].ToString(),
                    NgayCapNhat = Convert.ToDateTime(row["NgayCapNhat"])
                });
            }

            return list;
        }

        public bool ExistsById(string maTram)
        {
            string sql = "SELECT COUNT(1) FROM AtmStations WHERE MaTram = @MaTram";
            var param = new[] { new SqlParameter("@MaTram", SqlDbType.VarChar, 20) { Value = maTram } };
            object? result = DatabaseHelper.ExecuteScalar(sql, param);
            return Convert.ToInt32(result) > 0;
        }

        public bool Insert(AtmStationDTO station)
        {
            string sql = @"INSERT INTO AtmStations (MaTram, DiaChi, TonQuy, TrangThai, AnhHienTruong, NgayCapNhat)
                           VALUES (@MaTram, @DiaChi, @TonQuy, @TrangThai, @AnhHienTruong, GETDATE())";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaTram", SqlDbType.VarChar, 20) { Value = station.MaTram },
                new SqlParameter("@DiaChi", SqlDbType.NVarChar, 250) { Value = station.DiaChi },
                new SqlParameter("@TonQuy", SqlDbType.Decimal) { Value = station.TonQuy },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = station.TrangThai },
                new SqlParameter("@AnhHienTruong", SqlDbType.NVarChar, 500)
                {
                    Value = string.IsNullOrWhiteSpace(station.AnhHienTruong) ? DBNull.Value : station.AnhHienTruong
                }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(AtmStationDTO station)
        {
            string sql = @"UPDATE AtmStations
                           SET DiaChi = @DiaChi,
                               TonQuy = @TonQuy,
                               TrangThai = @TrangThai,
                               AnhHienTruong = @AnhHienTruong,
                               NgayCapNhat = GETDATE()
                           WHERE MaTram = @MaTram";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaTram", SqlDbType.VarChar, 20) { Value = station.MaTram },
                new SqlParameter("@DiaChi", SqlDbType.NVarChar, 250) { Value = station.DiaChi },
                new SqlParameter("@TonQuy", SqlDbType.Decimal) { Value = station.TonQuy },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = station.TrangThai },
                new SqlParameter("@AnhHienTruong", SqlDbType.NVarChar, 500)
                {
                    Value = string.IsNullOrWhiteSpace(station.AnhHienTruong) ? DBNull.Value : station.AnhHienTruong
                }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(string maTram)
        {
            string sql = "DELETE FROM AtmStations WHERE MaTram = @MaTram";
            var param = new[] { new SqlParameter("@MaTram", SqlDbType.VarChar, 20) { Value = maTram } };
            return DatabaseHelper.ExecuteNonQuery(sql, param) > 0;
        }
    }
}
