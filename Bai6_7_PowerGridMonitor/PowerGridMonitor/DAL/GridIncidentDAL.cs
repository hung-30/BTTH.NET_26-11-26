using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PowerGridMonitor.DTO;

namespace PowerGridMonitor.DAL
{
    public class GridIncidentDAL
    {
        public List<GridIncidentLogDTO> GetRecentLogs(int top = 50)
        {
            var list = new List<GridIncidentLogDTO>();
            string sql = $@"SELECT TOP ({top}) MaSuCo, MaTram, TenTram, ThoiGian, DienApKV, MucTaiPhanTram, TrangThai, MoTa
                            FROM GridIncidentLogs
                            ORDER BY MaSuCo DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new GridIncidentLogDTO
                {
                    MaSuCo = Convert.ToInt32(row["MaSuCo"]),
                    MaTram = row["MaTram"].ToString() ?? string.Empty,
                    TenTram = row["TenTram"].ToString() ?? string.Empty,
                    ThoiGian = Convert.ToDateTime(row["ThoiGian"]),
                    DienApKV = Convert.ToDouble(row["DienApKV"]),
                    MucTaiPhanTram = Convert.ToInt32(row["MucTaiPhanTram"]),
                    TrangThai = row["TrangThai"].ToString() ?? string.Empty,
                    MoTa = row["MoTa"].ToString() ?? string.Empty
                });
            }
            return list;
        }

        public bool Insert(GridIncidentLogDTO log)
        {
            string sql = @"INSERT INTO GridIncidentLogs (MaTram, TenTram, ThoiGian, DienApKV, MucTaiPhanTram, TrangThai, MoTa)
                           VALUES (@MaTram, @TenTram, @ThoiGian, @DienApKV, @MucTaiPhanTram, @TrangThai, @MoTa)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@MaTram", SqlDbType.VarChar, 20) { Value = log.MaTram },
                new SqlParameter("@TenTram", SqlDbType.NVarChar, 150) { Value = log.TenTram },
                new SqlParameter("@ThoiGian", SqlDbType.DateTime) { Value = log.ThoiGian },
                new SqlParameter("@DienApKV", SqlDbType.Float) { Value = log.DienApKV },
                new SqlParameter("@MucTaiPhanTram", SqlDbType.Int) { Value = log.MucTaiPhanTram },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = log.TrangThai },
                new SqlParameter("@MoTa", SqlDbType.NVarChar, 300) { Value = log.MoTa }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
