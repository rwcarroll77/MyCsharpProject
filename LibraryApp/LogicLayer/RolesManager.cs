using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class RolesManager
    {
        public List<String> RetrieveAllRoles()
        {
            List<String> roles = new List<string>();

            try
            {
                roles = RoleAccessor.SelectAllRoles();
            }
            catch (Exception)
            {

                throw;
            }

            return roles;
        }
    }
}
