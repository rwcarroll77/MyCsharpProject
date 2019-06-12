using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RoleAccessor
    {
        public static List<String> SelectAllRoles()
        {
            List<String> roles = new List<string>();

            var cmdText = @"sp_select_all_roles";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        roles.Add(r.GetString(0));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return roles;
        }
    }
}
