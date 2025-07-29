using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem
{
    public class UsersInfoDTO
    {
        public List<string> AdminAccounts { get; set; }
        public List<string> StandardAccounts { get; set; }

        private static string[] DefaultAccounts = { "Administrator", "Guest", "DefaultAccount" };

        public UsersInfoDTO() 
        {
            AdminAccounts = new List<string>();
            StandardAccounts = new List<string>();
        }

        public static UsersInfoDTO ExtractWMIUsersInfo(Win32_GroupUser[] groupUser)
        {

            var usersInfo = groupUser.Aggregate(new UsersInfoDTO(), (acc, groupUser) =>
            {
                var isUserAccount = groupUser.PartComponent.Contains("Win32_UserAccount");

                if (!isUserAccount) return acc;

                // Given the structure of the stringified value we will split the string it is assumed that the 2nd element would have user account name 
                // example string "Win32_UserAccount (Name = \"Administrator\", Domain = \"DESKTOP-VCO4722\")"
                var userAccountName = groupUser.PartComponent.Split("\"")[1];

                var isDefaultAccount = DefaultAccounts.Contains(userAccountName);

                if (isDefaultAccount) return acc;

                var isAdminAccount = groupUser.GroupComponent.Contains("Administrators");

                if (isAdminAccount)
                {
                    acc.AdminAccounts.Add(userAccountName);
                }
                else
                {
                    acc.StandardAccounts.Add(userAccountName);
                }

                return acc;
            });

            return usersInfo;
        }
    }

    public class Win32_GroupUser
    {
        public string GroupComponent { get; set; }
        public string PartComponent { get; set; }
        
    }
}
