using System;
using System.Collections.Generic;
using System.Data;
using PowerGridMonitor.DTO;

namespace PowerGridMonitor.DAL
{
    public class SubstationDAL
    {
        public List<SubstationDTO> GetAll()
        {
            var list = new List<SubstationDTO>();
            string sql = @"SELECT MaTram, TenTram, CapDienAp, KhuVuc, NguongDienApMin, NguongDienApMax, NguongQuaTai
                           FROM Substations
                           ORDER BY MaTram ASC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SubstationDTO
                {
                    MaTram = row["MaTram"].ToString() ?? string.Empty,
                    TenTram = row["TenTram"].ToString() ?? string.Empty,
                    CapDienAp = row["CapDienAp"].ToString() ?? string.Empty,
                    KhuVuc = row["KhuVuc"].ToString() ?? string.Empty,
                    NguongDienApMin = Convert.ToDouble(row["NguongDienApMin"]),
                    NguongDienApMax = Convert.ToDouble(row["NguongDienApMax"]),
                    NguongQuaTai = Convert.ToInt32(row["NguongQuaTai"])
                });
            }
            return list;
        }
    }
}
