﻿using Microsoft.Extensions.OptionsModel;
using Microsoft.Legal.MatterCenter.Models;
using Microsoft.Legal.MatterCenter.Utility;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.Legal.MatterCenter.Repository
{
    public class UserRepository:IUserRepository
    {
        private IUsersDetails userDetails;
        private ISPList spList;
        private CamlQueries camlQueries;
        private MatterSettings matterSettings;
        private ISearch search;
        private ListNames listNames;

        public UserRepository(IUsersDetails userDetails, ISPList spList, IOptions<ListNames> listNames, IOptions<CamlQueries> camlQueries,
            IOptions<MatterSettings> matterSettings, ISearch search)
        {
            this.userDetails = userDetails;
            this.spList = spList;
            this.matterSettings = matterSettings.Value;
            this.camlQueries = camlQueries.Value;
            this.search = search;
            this.listNames = listNames.Value; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public Users GetUserProfilePicture(Client client)
        {
            return userDetails.GetUserProfilePicture(client);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientContext"></param>
        /// <returns></returns>
        public Users GetLoggedInUserDetails(ClientContext clientContext)=> userDetails.GetLoggedInUserDetails(clientContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientContext"></param>
        /// <returns></returns>
        public Users GetLoggedInUserDetails(Client client) => userDetails.GetLoggedInUserDetails(client);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchRequestVM"></param>
        /// <returns></returns>
        public async Task<IList<Users>> GetUsersAsync(SearchRequestVM searchRequestVM)
        {
            IList<PeoplePickerUser> foundUsers = await Task.FromResult(search.SearchUsers(searchRequestVM));
            IList<Users> users = new List<Users>();
            if (foundUsers != null && foundUsers.Count != 0)
            {
                foreach (PeoplePickerUser item in foundUsers)
                {
                    Users tempUser = new Users();
                    tempUser.Name = Convert.ToString(item.DisplayText, CultureInfo.InvariantCulture);
                    tempUser.LogOnName = Convert.ToString(item.Key, CultureInfo.InvariantCulture);
                    tempUser.Email = string.Equals(item.EntityType, ServiceConstants.PEOPLE_PICKER_ENTITY_TYPE_USER, StringComparison.OrdinalIgnoreCase) ?
                        Convert.ToString(item.Description, CultureInfo.InvariantCulture) : Convert.ToString(item.EntityData.Email, CultureInfo.InvariantCulture);
                    tempUser.EntityType = Convert.ToString(item.EntityType, CultureInfo.InvariantCulture);
                    users.Add(tempUser);
                }
                return users;
            }
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<IList<Role>> GetRolesAsync(Client client)
        {
            IList<Role> roles = new List<Role>();
            ListItemCollection collListItem = await Task.FromResult(spList.GetData(client, listNames.DMSRoleListName, camlQueries.DMSRoleQuery));
            if (null != collListItem && 0 != collListItem.Count)
            {
                foreach (ListItem item in collListItem)
                {
                    Role tempRole = new Role();
                    tempRole.Id = Convert.ToString(item[matterSettings.ColumnNameGuid], CultureInfo.InvariantCulture);
                    tempRole.Name = Convert.ToString(item[matterSettings.RoleListColumnRoleName], CultureInfo.InvariantCulture);
                    tempRole.Mandatory = Convert.ToBoolean(item[matterSettings.RoleListColumnIsRoleMandatory], CultureInfo.InvariantCulture);
                    roles.Add(tempRole);
                }
            }
            return roles;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<IList<Role>> GetPermissionLevelsAsync(Client client)
        {
            IList<Role> roles = new List<Role>();
            List<RoleDefinition> roleDefinitions = await Task.FromResult(search.GetWebRoleDefinitions(client));
            if (roleDefinitions.Count != 0)
            {
                foreach (RoleDefinition role in roleDefinitions)
                {
                    Role tempRole = new Role();
                    tempRole.Name = role.Name;
                    tempRole.Id = Convert.ToString(role.Id, CultureInfo.InvariantCulture);
                    roles.Add(tempRole);
                }
            }
            return roles;
        }
    }
}