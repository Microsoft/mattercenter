// ***********************************************************************
// Assembly         : Microsoft.Legal.MatterCenter.Utility
// Author           : v-lapedd
// Created          : 04-07-2016
//
// ***********************************************************************
// <copyright file="MatterRepository.cs" company="Microsoft">
//     Copyright (c) . All rights reserved.
// </copyright>
// This class deals with all the matter related functions such as finding matter, pin, unpin, update matter etc
// ***********************************************************************

using Microsoft.Extensions.Options;
using Microsoft.Legal.MatterCenter.Models;
using Microsoft.Legal.MatterCenter.Utility;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.SharePoint.Client.WebParts;
using System.Text;
using System.IO;
using Microsoft.Legal.MatterCenter.Repository.Extensions;

namespace Microsoft.Legal.MatterCenter.Repository
{
    public class MatterRepository : IMatterRepository
    {
        private ISearch search;
        private ISPList spList;
        private MatterSettings matterSettings;
        private SearchSettings searchSettings;
        private CamlQueries camlQueries;
        private ListNames listNames;
        private IUsersDetails userdetails;
        private ISPOAuthorization spoAuthorization;
        private ISPPage spPage;
        private ErrorSettings errorSettings;
        private ISPContentTypes spContentTypes;
        private IExternalSharing extrnalSharing;
        private IUserRepository userRepositoy;
        private GeneralSettings generalSettings;
        //To get log error in spolog table.
        private ContentTypesConfig contentTypesSettings;
        private ICustomLogger customLogger;
        private LogTables logTables;

        /// <summary>
        /// Constructory which will inject all the related dependencies related to matter
        /// </summary>
        /// <param name="search"></param>
        public MatterRepository(ISearch search,
            IOptions<MatterSettings> matterSettings,
            IOptions<SearchSettings> searchSettings,
            IOptions<ListNames> listNames,
            ISPOAuthorization spoAuthorization,
            ISPContentTypes spContentTypes,
            IExternalSharing extrnalSharing,
            IUserRepository userRepositoy,
            ISPList spList,
            IOptions<CamlQueries> camlQueries,
            IOptions<GeneralSettings> generalSettings,
            IUsersDetails userdetails,
            IOptions<ErrorSettings> errorSettings,
            IOptions<ContentTypesConfig> contentTypesSettings,
            ICustomLogger customLogger,
            IOptions<LogTables> logTables,
            ISPPage spPage)
        {
            this.search = search;
            this.matterSettings = matterSettings.Value;
            this.searchSettings = searchSettings.Value;
            this.listNames = listNames.Value;
            this.spList = spList;
            this.camlQueries = camlQueries.Value;
            this.userdetails = userdetails;
            this.spoAuthorization = spoAuthorization;
            this.spPage = spPage;
            this.errorSettings = errorSettings.Value;
            this.customLogger = customLogger;
            this.generalSettings = generalSettings.Value;
            this.spContentTypes = spContentTypes;
            this.extrnalSharing = extrnalSharing;
            this.logTables = logTables.Value;
            this.userRepositoy = userRepositoy;
            //Contenttypesettings require to get the sitecolumn group like _MatterCenter to filter columns.
            this.contentTypesSettings = contentTypesSettings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="matterConfigurations"></param>
        /// <returns></returns>
        public GenericResponseVM SaveConfigurations(ClientContext clientContext, MatterConfigurations matterConfigurations)
        {
            GenericResponseVM genericResponse = null;
            try
            {
                string listQuery = string.Format(CultureInfo.InvariantCulture, camlQueries.MatterConfigurationsListQuery,
                    searchSettings.ManagedPropertyTitle, searchSettings.MatterConfigurationTitleValue);
                ListItemCollection collection = spList.GetData(clientContext, listNames.MatterConfigurationsList, listQuery);
                // Set the default value for conflict check flag
                matterConfigurations.IsContentCheck = matterSettings.IsContentCheck;
                if (0 == collection.Count)
                {
                    List<string> columnNames = new List<string>() { searchSettings.MatterConfigurationColumn,
                        searchSettings.ManagedPropertyTitle };
                    List<object> columnValues = new List<object>() { WebUtility.HtmlEncode(JsonConvert.SerializeObject(matterConfigurations)),
                        searchSettings.MatterConfigurationTitleValue };
                    Web web = clientContext.Web;
                    List list = web.Lists.GetByTitle(listNames.MatterConfigurationsList);
                    spList.AddItem(clientContext, list, columnNames, columnValues);
                }
                else
                {
                    bool response = spList.CheckItemModified(collection, matterConfigurations.CachedItemModifiedDate);
                    if (response)
                    {
                        foreach (ListItem item in collection)
                        {
                            item[searchSettings.MatterConfigurationColumn] = WebUtility.HtmlEncode(JsonConvert.SerializeObject(matterConfigurations));
                            item.Update();
                            break;
                        }
                    }
                    else
                    {
                        genericResponse = new GenericResponseVM()
                        {
                            Code = errorSettings.IncorrectTeamMembersCode,
                            Value = errorSettings.IncorrectTeamMembersMessage
                        };
                    }
                }
                if (genericResponse == null)
                {
                    clientContext.ExecuteQuery();
                }

                listQuery = string.Format(CultureInfo.InvariantCulture, camlQueries.MatterConfigurationsListQuery,
                    searchSettings.ManagedPropertyTitle, searchSettings.MatterConfigurationTitleValue);
                ListItem settingsItem = spList.GetData(clientContext, listNames.MatterConfigurationsList, listQuery).FirstOrDefault();
                if (null != settingsItem)
                {
                    string cachedItemModifiedDate = Convert.ToString(settingsItem[searchSettings.ColumnNameModifiedDate], CultureInfo.InvariantCulture);
                    genericResponse = new GenericResponseVM()
                    {
                        Code = "",
                        Value = cachedItemModifiedDate,
                        IsError = false
                    };
                }
            }
            catch (Exception exception)
            {
                //result = Logger.LogError(exception, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ServiceConstantStrings.LogTableName);
            }
            return genericResponse;
        }

        /// <summary>
        /// This method will check whether login user can create matter or not
        /// </summary>
        /// <param name="client">The sharepoint site collection in which we need to check whether the login user is present in the sharepoint group or not</param>
        /// <returns></returns>
        public bool CanCreateMatter(Client client)
        {
            return spList.CheckPermissionOnList(client, matterSettings.SendMailListName, PermissionKind.EditListItems);
        }


        /// <summary>
        /// Method to delete user along with his permissions from the matter and its associated lists and page
        /// </summary>
        /// <param name="matterInformation"></param>
        /// <returns></returns>
        public GenericResponseVM DeleteUserFromMatter(MatterInformationVM matterInformation)
        {
            var matter = matterInformation.Matter;
            var matterDetails = matterInformation.MatterDetails;
            var client = matterInformation.Client;

            int listItemId = -1;
            string loggedInUserName = "";
            bool isEditMode = matterInformation.EditMode;
            ClientContext clientContext = null;
            IEnumerable<RoleAssignment> userPermissionOnLibrary = null;
            GenericResponseVM genericResponse = null;
            try
            {
                clientContext = spoAuthorization.GetClientContext(matterInformation.Client.Url);
                PropertyValues matterStampedProperties = GetStampedProperties(clientContext, matter.Name);
                loggedInUserName = userRepositoy.GetLoggedInUserDetails(clientContext).Name;
                // Get matter library current permissions
                userPermissionOnLibrary = FetchUserPermissionForLibrary(clientContext, matter.Name);
                string originalMatterName = GetMatterName(clientContext, matter.Name);
                string matterAssosicatedListName = "";
                List<string> usersToRemove = new List<string>();
                foreach (IList<string> removedUsers in matterInformation.UsersNamesToRemove)
                {
                    foreach (var removedUser in removedUsers)
                    {
                        usersToRemove.Add(removedUser);
                    }
                }
                if (matterInformation.MatterAssociatedInfo.ToLower() == UpdateMatterOperation.MatterLibrary.ToString().ToLower())
                {
                    RemoveSpecificUsers(clientContext, usersToRemove, loggedInUserName, false, matter.Name, -1);
                }
                else
                {


                    if (matterInformation.MatterAssociatedInfo.ToLower() == UpdateMatterOperation.MatterPage.ToString().ToLower())
                    {
                        listItemId = RetrieveItemId(clientContext, matterSettings.MatterLandingPageRepositoryName, originalMatterName);
                        if (listItemId <= 0)
                        {
                            RemoveSpecificUsers(clientContext, usersToRemove, loggedInUserName, true, matterSettings.MatterLandingPageRepositoryName, listItemId);
                        }
                    }
                    else if (matterInformation.MatterAssociatedInfo.ToLower() == UpdateMatterOperation.StampedProperties.ToString().ToLower())
                    {
                        isEditMode = false;
                        UpdateMatterStampedProperties(clientContext, matterDetails, matter, matterStampedProperties, isEditMode, usersToRemove);
                    }
                    else
                    {

                        RemoveSpecificUsers(clientContext, usersToRemove, loggedInUserName, false, matter.Name + matterInformation.MatterAssociatedInfo, -1);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }




        /// <summary>
        /// Method to update matter if any user is added or updated to the matter
        /// </summary>
        /// <param name="matterInformation"></param>
        /// <returns></returns>
        public GenericResponseVM UpdateMatter(MatterInformationVM matterInformation)
        {
            var matter = matterInformation.Matter;
            var matterDetails = matterInformation.MatterDetails;
            var client = matterInformation.Client;
            int listItemId = -1;
            string loggedInUserName = "";
            bool isEditMode = matterInformation.EditMode;
            ClientContext clientContext = null;
            IEnumerable<RoleAssignment> userPermissionOnLibrary = null;
            GenericResponseVM genericResponse = null;
            try
            {
                clientContext = spoAuthorization.GetClientContext(matterInformation.Client.Url);

                loggedInUserName = userRepositoy.GetLoggedInUserDetails(clientContext).Name;
                // Get matter library current permissions
                userPermissionOnLibrary = FetchUserPermissionForLibrary(clientContext, matter.Name);
                string originalMatterName = GetMatterName(clientContext, matter.Name);
                listItemId = RetrieveItemId(clientContext, matterSettings.MatterLandingPageRepositoryName, originalMatterName);
                List<string> usersToRemove = new List<string>();
                List<string> existingUsers = RetrieveMatterUsers(userPermissionOnLibrary);
                foreach (IList<string> removedUsers in matterInformation.UsersNamesToRemove)
                {
                    foreach (var removedUser in removedUsers)
                    {
                        usersToRemove.Add(removedUser);
                    }
                }

                foreach (IList<string> removedUsers in matterInformation.Matter.AssignUserEmails)
                {
                    foreach (var removedUser in removedUsers)
                    {
                        usersToRemove.Add(removedUser);
                    }
                }

                bool hasFullPermission = CheckFullPermissionInAssignList(matter.AssignUserNames, matter.Permissions, loggedInUserName);
                List<string> listExists = new List<string>();
                bool result = false;
                List<string> listNumbers = new List<string>();
                if (matterInformation.MatterAssociatedInfo == string.Empty)
                {
                    listExists = MatterAssociatedLists(clientContext, matter.Name);

                    AssignRemoveFullControl(clientContext, matter, loggedInUserName, listItemId, listExists, true, hasFullPermission);
                    if (listExists.Contains(matter.Name))
                    {
                        listNumbers.Add(UpdateMatterOperation.MatterLibrary.ToString());
                    }
                    if (listExists.Contains(matter.Name + matterSettings.OneNoteLibrarySuffix))
                    {
                        listNumbers.Add(matterSettings.OneNoteLibrarySuffix);
                    }
                    if (listExists.Contains(matter.Name + matterSettings.CalendarNameSuffix))
                    {
                        listNumbers.Add(matterSettings.CalendarNameSuffix);
                    }
                    if (listExists.Contains(matter.Name + matterSettings.TaskNameSuffix))
                    {
                        listNumbers.Add(matterSettings.TaskNameSuffix);
                    }
                    if (0 <= listItemId)
                    {
                        listNumbers.Add(UpdateMatterOperation.MatterPage.ToString());
                    }
                    result = true;
                }


                if (matterInformation.MatterAssociatedInfo.ToLower() == UpdateMatterOperation.MatterLibrary.ToString().ToLower())
                {
                    //updating the users and their permissions to the matter.
                    result = UpdatePermission(clientContext, matter, usersToRemove, loggedInUserName, false, matter.Name, -1, isEditMode);

                }
                else
                {
                    if (matterInformation.MatterAssociatedInfo.ToLower() == UpdateMatterOperation.MatterPage.ToString().ToLower())
                    {
                        //updating the users and their permissions to the matter specified list item.
                        if (0 <= listItemId)
                        {
                            result = UpdatePermission(clientContext, matter, usersToRemove, loggedInUserName, true, matterSettings.MatterLandingPageRepositoryName, listItemId, isEditMode);
                            result = result ? false : true;
                        }
                    }
                    else if (matterInformation.MatterAssociatedInfo.ToLower() == UpdateMatterOperation.StampedProperties.ToString().ToLower())
                    {
                        // Update matter metadata
                        isEditMode = false;
                        PropertyValues matterStampedProperties = GetStampedProperties(clientContext, matter.Name);
                        result = UpdateMatterStampedProperties(clientContext, matterDetails, matter, matterStampedProperties, isEditMode, usersToRemove);
                    }
                    else
                    {
                        result = UpdatePermission(clientContext, matter, usersToRemove, loggedInUserName, false, matter.Name + matterInformation.MatterAssociatedInfo, -1, isEditMode);
                    }
                }
                if (result)
                {
                    if (matterInformation.MatterAssociatedInfo != UpdateMatterOperation.StampedProperties.ToString())
                    {
                        matterInformation.Matter = null;
                    }
                    if (matterInformation.MatterAssociatedInfo == string.Empty)
                    {
                        genericResponse = new GenericResponseVM()
                        {

                            IsError = false,
                            Description = string.Join(";", listNumbers)
                        };
                    }
                    return genericResponse;
                }
            }
            catch (Exception ex)
            {
                MatterRevertList matterRevertListObject = new MatterRevertList()
                {
                    MatterLibrary = matter.Name,
                    MatterOneNoteLibrary = matter.Name + matterSettings.OneNoteLibrarySuffix,
                    MatterCalendar = matter.Name + matterSettings.CalendarNameSuffix,
                    MatterTask = matter.Name + matterSettings.TaskNameSuffix,
                    MatterSitePages = matterSettings.MatterLandingPageRepositoryName
                };
                RevertMatterUpdates(client, matter, matterRevertListObject, isEditMode, userPermissionOnLibrary);
            }
            return ServiceUtility.GenericResponse("9999999", "Error in updating matter information");
        }

        /// <summary>
        /// Gets the display name of users having permission on library.
        /// </summary>
        /// <param name="userPermissionOnLibrary">Users having permission on library</param>
        /// <returns></returns>
        internal List<string> RetrieveMatterUsers(IEnumerable<RoleAssignment> userPermissionOnLibrary)
        {
            List<string> users = new List<string>();
            try
            {
                if (null != userPermissionOnLibrary && 0 < userPermissionOnLibrary.Count())
                {
                    foreach (RoleAssignment roles in userPermissionOnLibrary)
                    {
                        users.Add(roles.Member.Title);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return users;
        }

        /// <summary>
        /// Check Full Permission for logged in User.
        /// </summary>
        /// <param name="AssignUserNames">List of Assigned UserNames</param>
        /// <param name="Permissions">List of Permission</param>
        /// <param name="loggedInUserName">Name of logged in User</param>
        /// <returns>Status of Full Permission</returns>
        internal bool CheckFullPermissionInAssignList(IList<IList<string>> AssignUserNames, IList<string> Permissions, string loggedInUserName)
        {
            bool result = false;
            if (null != Permissions && null != AssignUserNames && Permissions.Count == AssignUserNames.Count)
            {
                int position = 0;
                foreach (string roleName in Permissions)
                {
                    IList<string> assignUserNames = AssignUserNames[position];
                    if (!string.IsNullOrWhiteSpace(roleName) && null != assignUserNames)
                    {
                        foreach (string user in assignUserNames)
                        {
                            if (!string.IsNullOrWhiteSpace(user) && user.Trim().Equals(loggedInUserName.Trim()))
                            {
                                if (roleName == matterSettings.EditMatterAllowedPermissionLevel)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    position++;
                }
                return result;
            }
            return result;
        }

        public bool CreateList(ClientContext clientContext, ListInformation listInformation)
        {
            return spList.CreateList(clientContext, listInformation);
        }

        public bool BreakItemPermission(ClientContext clientContext, string listName, int listItemId, bool isCopyRoleAssignment)
        {
            return spList.BreakItemPermission(clientContext, listName, listItemId, isCopyRoleAssignment);
        }

        public bool BreakPermission(ClientContext clientContext, string libraryName, bool isCopyRoleAssignment)
        {
            return spList.BreakPermission(clientContext, libraryName, isCopyRoleAssignment);
        }

        public bool CheckPermissionOnList(ClientContext clientContext, string listName, PermissionKind permission)
        {
            return spList.CheckPermissionOnList(clientContext, listName, permission);
        }

        public GenericResponseVM ValidateTeamMembers(ClientContext clientContext, Matter matter, IList<string> userId)
        {
            bool isInvalidUser = false;
            int iCounter = 0, teamMembersRowCount = matter.AssignUserEmails.Count(), iCount = 0;
            List<Principal> teamMemberPrincipalCollection = new List<Principal>();
            GenericResponseVM genericResponse = null;
            try
            {
                for (iCounter = 0; iCounter < teamMembersRowCount; iCounter++)
                {
                    IList<string> userList = matter.AssignUserEmails[iCounter].Where(user => !string.IsNullOrWhiteSpace(user.Trim())).ToList();
                    IList<string> userNameList = matter.AssignUserNames[iCounter].Where(user => !string.IsNullOrWhiteSpace(user.Trim())).ToList();
                    int userAtLocation = 0;
                    List<int> itemsToRemoveUsers = new List<int>();
                    foreach (string userName in userList)
                    {
                        //Check has been made to check whether the user is present in the system as part of external sharing implementation
                        if (!string.IsNullOrWhiteSpace(userName) && userdetails.CheckUserPresentInMatterCenter(clientContext, userName))
                        {
                            Principal teamMemberPrincipal = clientContext.Web.EnsureUser(userName.Trim());
                            clientContext.Load(teamMemberPrincipal, teamMemberPrincipalProperties => teamMemberPrincipalProperties.Title, teamMemberPrincipalProperties => teamMemberPrincipalProperties.LoginName);
                            teamMemberPrincipalCollection.Add(teamMemberPrincipal);
                        }
                        else
                        {
                            itemsToRemoveUsers.Add(userAtLocation);
                        }
                        userAtLocation++;
                    }

                    if (itemsToRemoveUsers.Count > 0)
                    {
                        for (int k = 0; k < itemsToRemoveUsers.Count; k++)
                        {
                            userNameList[itemsToRemoveUsers[k]] = string.Empty; ;
                        }
                    }
                    userNameList = userNameList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    //Check has been made to check whether the user is present in the system as part of external sharing implementation
                    if (teamMemberPrincipalCollection.Count > 0)
                    {
                        iCount = 0;
                        clientContext.ExecuteQuery();
                        //// Check whether the name entered by the user and the name resolved by SharePoint is same.
                        foreach (string teamMember in userNameList)
                        {
                            if (!string.Equals(teamMember.Trim(), teamMemberPrincipalCollection[iCount].Title.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                !teamMemberPrincipalCollection[iCount].LoginName.ToString().ToLower().Trim().Contains(teamMember.ToLower().Replace("@", "_").Trim()))
                            {
                                genericResponse = new GenericResponseVM();
                                //result = string.Format(CultureInfo.InvariantCulture, ConstantStrings.ServiceResponse, ServiceConstantStrings.IncorrectTeamMembersCode, ServiceConstantStrings.IncorrectTeamMembersMessage + ConstantStrings.DOLLAR + ConstantStrings.Pipe + ConstantStrings.DOLLAR + userId[iCounter]);
                                genericResponse.Code = errorSettings.IncorrectTeamMembersCode;
                                genericResponse.Code = errorSettings.IncorrectTeamMembersMessage + ServiceConstants.DOLLAR + ServiceConstants.PIPE + ServiceConstants.DOLLAR + userId[iCounter];
                                isInvalidUser = true;
                                break;
                            }
                            iCount++;
                        }
                    }
                    if (isInvalidUser)
                    {
                        break; // To break the outer loop as there is an invalid user
                    }
                    teamMemberPrincipalCollection = new List<Principal>();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return genericResponse;
        }

        /// <summary>
        /// Checks if the lists exist
        /// </summary>
        /// <param name="client"></param>
        /// <param name="matterName"></param>
        /// <param name="matterConfigurations"></param>
        /// <returns></returns>
        public List<string> Exists(Client client, ReadOnlyCollection<string> lists)
        {
            List<string> listExists = spList.Exists(client, lists);
            return listExists;
        }

        public GenericResponseVM SaveConfigurationToList(MatterConfigurations matterConfigurations, ClientContext clientContext, string cachedItemModifiedDate)
        {
            string result = string.Empty;
            GenericResponseVM genericResponse = null;
            try
            {
                string listQuery = string.Format(CultureInfo.InvariantCulture, camlQueries.MatterConfigurationsListQuery, searchSettings.ManagedPropertyTitle, searchSettings.MatterConfigurationTitleValue);
                ListItemCollection collection = spList.GetData(clientContext, listNames.MatterConfigurationsList, listQuery);
                // Set the default value for conflict check flag
                matterConfigurations.IsContentCheck = matterSettings.IsContentCheck;
                if (0 == collection.Count)
                {
                    List<string> columnNames = new List<string>() { matterSettings.MatterConfigurationColumn, searchSettings.ManagedPropertyTitle };
                    List<object> columnValues = new List<object>() { WebUtility.HtmlEncode(JsonConvert.SerializeObject(matterConfigurations)), searchSettings.MatterConfigurationTitleValue };
                    Web web = clientContext.Web;
                    List list = web.Lists.GetByTitle(listNames.MatterConfigurationsList);
                    spList.AddItem(clientContext, list, columnNames, columnValues);
                }
                else
                {
                    bool response = spList.CheckItemModified(collection, cachedItemModifiedDate);
                    if (response)
                    {
                        foreach (ListItem item in collection)
                        {
                            item[matterSettings.MatterConfigurationColumn] = WebUtility.HtmlEncode(JsonConvert.SerializeObject(matterConfigurations));
                            item.Update();
                            break;
                        }
                    }
                    else
                    {
                        //result = string.Format(CultureInfo.InvariantCulture, ConstantStrings.ServiceResponse, ServiceConstantStrings.IncorrectTeamMembersCode, ServiceConstantStrings.IncorrectTeamMembersMessage + ConstantStrings.DOLLAR + ConstantStrings.Pipe + ConstantStrings.DOLLAR);
                        genericResponse = new GenericResponseVM()
                        {
                            Code = errorSettings.IncorrectTeamMembersCode,
                            Value = errorSettings.IncorrectTeamMembersMessage
                        };
                    }
                }
                if (genericResponse == null)
                {
                    clientContext.ExecuteQuery();
                    genericResponse = new GenericResponseVM()
                    {
                        Code = "200",
                        Value = true.ToString()
                    };
                };
            }
            catch (Exception exception)
            {
                //result = Logger.LogError(exception, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ServiceConstantStrings.LogTableName);
                throw;
            }
            return genericResponse;
        }

        public bool IsPageExists(ClientContext clientContext, string pageUrl)
        {
            return spPage.PageExists(pageUrl, clientContext);
        }

        public ListItem GetItem(ClientContext clientContext, string listName, string listQuery)
        {
            ListItem settingsItem = spList.GetData(clientContext, listName, listQuery).FirstOrDefault();
            return settingsItem;
        }

        public IList<FieldUserValue> ResolveUserNames(Client client, IList<string> userNames)
        {
            return userdetails.ResolveUserNames(client, userNames);
        }

        public IList<FieldUserValue> ResolveUserNames(ClientContext clientContext, IList<string> userNames)
        {
            return userdetails.ResolveUserNames(clientContext, userNames);
        }

        /// <summary>
        /// This method will try to fetch all the matters that are provisioned by the user
        /// </summary>
        /// <param name="searchRequestVM"></param>
        /// <returns></returns>
        public async Task<SearchResponseVM> GetMattersAsync(SearchRequestVM searchRequestVM, ClientContext clientContext)
        {
            return await Task.FromResult(search.GetMatters(searchRequestVM, clientContext));
        }

        public bool AddItem(ClientContext clientContext, List list, IList<string> columns, IList<object> values)
        {
            return spList.AddItem(clientContext, list, columns, values);
        }

        /// <summary>
        /// This method will try to fetch all the matters that are provisioned by the user
        /// </summary>
        /// <param name="searchRequestVM"></param>
        /// <returns></returns>
        public async Task<SearchResponseVM> GetPinnedRecordsAsync(SearchRequestVM searchRequestVM, ClientContext clientContext)
        {
            return await Task.FromResult(search.GetPinnedData(searchRequestVM, listNames.UserPinnedMatterListName,
                searchSettings.PinnedListColumnMatterDetails, false, clientContext));
        }

        public int CreateWebPartPage(ClientContext clientContext, string pageName, string layout, string masterpagelistName, string listName, string pageTitle)
        {
            return spPage.CreateWebPartPage(clientContext, pageName, layout, masterpagelistName, listName, pageTitle);
        }

        public bool SaveMatter(Client client, Matter matter, string matterListName, MatterConfigurations matterConfigurations, string matterSiteURL)
        {
            bool returnFlag = false;
            try
            {
                using (ClientContext clientContext = spoAuthorization.GetClientContext(matterSiteURL))
                {
                    if (!string.IsNullOrWhiteSpace(matterListName) && null != matter && null != client)
                    {
                        FieldUserValue tempUser = null;
                        List<FieldUserValue> blockUserList = null;
                        List<List<FieldUserValue>> assignUserList = null;

                        List<string> columnNames = new List<string>()
                        {
                            matterSettings.MattersListColumnTitle,
                            matterSettings.MattersListColumnClientName,
                            matterSettings.MattersListColumnClientID,
                            matterSettings.MattersListColumnMatterName,
                            matterSettings.MattersListColumnMatterID
                        };
                        List<object> columnValues = new List<object>()
                        {
                            string.Concat(client.Name, ServiceConstants.UNDER_SCORE, matter.Name),
                            client.Name,
                            client.Id,
                            matter.Name,
                            matter.Id
                        };

                        if (matterConfigurations.IsConflictCheck)
                        {

                            if (null != matter.Conflict && !string.IsNullOrWhiteSpace(matter.Conflict.CheckBy))
                            {
                                tempUser = ResolveUserNames(clientContext, new List<string>() { matter.Conflict.CheckBy }).FirstOrDefault();
                                columnNames.Add(matterSettings.MattersListColumnConflictCheckBy);
                                columnValues.Add(tempUser);
                                if (!string.IsNullOrWhiteSpace(matter.Conflict.CheckOn))
                                {
                                    columnNames.Add(matterSettings.MattersListColumnConflictCheckOn);
                                    columnValues.Add(Convert.ToDateTime(matter.Conflict.CheckOn, CultureInfo.InvariantCulture));
                                }

                                columnNames.Add(matterSettings.MattersListColumnConflictIdentified);
                                columnValues.Add(Convert.ToBoolean(matter.Conflict.Identified, CultureInfo.InvariantCulture));
                            }

                            if (null != matter.BlockUserNames)
                            {
                                blockUserList = new List<FieldUserValue>();
                                blockUserList = ResolveUserNames(clientContext, matter.BlockUserNames).ToList();
                                columnNames.Add(matterSettings.MattersListColumnBlockUsers);
                                columnValues.Add(blockUserList);
                            }
                        }

                        if (null != matter.AssignUserEmails)
                        {
                            assignUserList = new List<List<FieldUserValue>>();
                            foreach (IList<string> assignUsers in matter.AssignUserEmails)
                            {
                                List<FieldUserValue> tempAssignUserList = ResolveUserNames(clientContext, assignUsers).ToList();
                                assignUserList.Add(tempAssignUserList);
                            }

                            if (0 != assignUserList.Count && null != matter.Roles && 0 != matter.Roles.Count)
                            {
                                int assignPosition = 0;
                                List<FieldUserValue> managingAttorneyList = new List<FieldUserValue>();
                                List<FieldUserValue> teamMemberList = new List<FieldUserValue>();
                                foreach (string role in matter.Roles)
                                {
                                    switch (role)
                                    {
                                        case ServiceConstants.MANAGING_ATTORNEY_VALUE:
                                            managingAttorneyList.AddRange(assignUserList[assignPosition]);
                                            break;
                                        default:
                                            teamMemberList.AddRange(assignUserList[assignPosition]);
                                            break;
                                    }

                                    assignPosition++;
                                }

                                columnNames.Add(matterSettings.MattersListColumnManagingAttorney);
                                columnValues.Add(managingAttorneyList);
                                columnNames.Add(matterSettings.MattersListColumnSupport);
                                columnValues.Add(teamMemberList);
                            }
                        }

                        Web web = clientContext.Web;
                        List matterList = web.Lists.GetByTitle(matterListName);
                        // To avoid the invalid symbol error while parsing the JSON, return the response in lower case
                        returnFlag = spList.AddItem(clientContext, matterList, columnNames, columnValues);
                    }
                }
            }
            catch (Exception exception)
            {
                //customLogger.LogError(exception, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, logTables.SPOLogTable);
                throw;
            }

            return returnFlag;
        }



        public IList<string> RoleCheck(string url, string listName, string columnName)
        {
            ListItemCollection collListItem = spList.GetData(generalSettings.CentralRepositoryUrl,
                listNames.DMSRoleListName,
                camlQueries.DMSRoleQuery);
            IList<string> roles = new List<string>();
            roles = collListItem.AsEnumerable().Select(roleList => Convert.ToString(roleList[matterSettings.RoleListColumnRoleName],
                CultureInfo.InvariantCulture)).ToList();
            //if (matter.Roles.Except(roles).Count() > 0)
            //{
            //    returnValue = string.Format(CultureInfo.InvariantCulture, ConstantStrings.ServiceResponse, TextConstants.IncorrectInputUserRolesCode, TextConstants.IncorrectInputUserRolesMessage);
            //}
            return roles;
        }

        public GenericResponseVM DeleteMatter(Client client, Matter matter)
        {
            GenericResponseVM genericResponse = null;
            using (ClientContext clientContext = spoAuthorization.GetClientContext(client.Url))
            {
                string stampResult = spList.GetPropertyValueForList(clientContext, matter.Name, matterSettings.StampedPropertySuccess);
                if (0 != string.Compare(stampResult, ServiceConstants.TRUE, StringComparison.OrdinalIgnoreCase))
                {
                    IList<string> lists = new List<string>();
                    lists.Add(matter.Name);
                    lists.Add(string.Concat(matter.Name, matterSettings.CalendarNameSuffix));
                    lists.Add(string.Concat(matter.Name, matterSettings.OneNoteLibrarySuffix));
                    lists.Add(string.Concat(matter.Name, matterSettings.TaskNameSuffix));
                    bool bListDeleted = spList.Delete(clientContext, lists);
                    if (bListDeleted)
                    {
                        //result = string.Format(CultureInfo.InvariantCulture, ConstantStrings.ServiceResponse, ServiceConstantStrings.DeleteMatterCode, TextConstants.MatterDeletedSuccessfully);
                        genericResponse = ServiceUtility.GenericResponse(matterSettings.DeleteMatterCode, matterSettings.MatterDeletedSuccessfully);
                    }
                    else
                    {
                        //result = string.Format(CultureInfo.InvariantCulture, ConstantStrings.ServiceResponse, ServiceConstantStrings.DeleteMatterCode, ServiceConstantStrings.MatterNotPresent);
                        genericResponse = ServiceUtility.GenericResponse(matterSettings.DeleteMatterCode, matterSettings.MatterNotPresent);
                    }
                    Uri clientUri = new Uri(client.Url);
                    string matterLandingPageUrl = string.Concat(clientUri.AbsolutePath, ServiceConstants.FORWARD_SLASH,
                        matterSettings.MatterLandingPageRepositoryName.Replace(ServiceConstants.SPACE, string.Empty),
                        ServiceConstants.FORWARD_SLASH, matter.MatterGuid, ServiceConstants.ASPX_EXTENSION);
                    spPage.Delete(clientContext, matterLandingPageUrl);
                    return ServiceUtility.GenericResponse("", "Matter deleted successfully");
                }
                else
                {
                    return ServiceUtility.GenericResponse(errorSettings.MatterLibraryExistsCode,
                         errorSettings.ErrorDuplicateMatter + ServiceConstants.MATTER + ServiceConstants.DOLLAR +
                         MatterPrerequisiteCheck.LibraryExists);
                }
            }
        }

        /// <summary>
        /// Create a new pin for the information that has been passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pinData"></param>
        /// <returns></returns>
        public async Task<bool> PinRecordAsync<T>(T pinRequestData)
        {
            PinRequestMatterVM pinRequestMatterVM = (PinRequestMatterVM)Convert.ChangeType(pinRequestData, typeof(PinRequestMatterVM));
            var matterData = pinRequestMatterVM.MatterData;
            matterData.MatterName = ServiceUtility.EncodeValues(matterData.MatterName);
            matterData.MatterDescription = ServiceUtility.EncodeValues(matterData.MatterDescription);
            matterData.MatterCreatedDate = ServiceUtility.EncodeValues(matterData.MatterCreatedDate);
            matterData.MatterUrl = ServiceUtility.EncodeValues(matterData.MatterUrl);
            matterData.MatterPracticeGroup = ServiceUtility.EncodeValues(matterData.MatterPracticeGroup);
            matterData.MatterAreaOfLaw = ServiceUtility.EncodeValues(matterData.MatterAreaOfLaw);
            matterData.MatterSubAreaOfLaw = ServiceUtility.EncodeValues(matterData.MatterSubAreaOfLaw);
            matterData.MatterClientUrl = ServiceUtility.EncodeValues(matterData.MatterClientUrl);
            matterData.MatterClient = ServiceUtility.EncodeValues(matterData.MatterClient);
            matterData.MatterClientId = ServiceUtility.EncodeValues(matterData.MatterClientId);
            matterData.HideUpload = ServiceUtility.EncodeValues(matterData.HideUpload);
            matterData.MatterID = ServiceUtility.EncodeValues(matterData.MatterID);
            matterData.MatterResponsibleAttorney = ServiceUtility.EncodeValues(matterData.MatterResponsibleAttorney);
            matterData.MatterModifiedDate = ServiceUtility.EncodeValues(matterData.MatterModifiedDate);
            matterData.MatterGuid = ServiceUtility.EncodeValues(matterData.MatterGuid);
            matterData.PinType = ServiceUtility.EncodeValues(matterData.PinType);
            pinRequestMatterVM.MatterData = matterData;
            return await Task.FromResult(search.PinMatter(pinRequestMatterVM));
        }

        /// <summary>
        /// Unpin the matter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pinRequestData"></param>
        /// <returns></returns>
        public async Task<bool> UnPinRecordAsync<T>(T pinRequestData)
        {
            PinRequestMatterVM pinRequestMatterVM = (PinRequestMatterVM)Convert.ChangeType(pinRequestData, typeof(PinRequestMatterVM));
            return await Task.FromResult(search.UnPinMatter(pinRequestMatterVM));
        }

        /// <summary>
        /// Get the folder hierarchy
        /// </summary>
        /// <param name="matterData"></param>
        /// <returns></returns>
        public async Task<List<FolderData>> GetFolderHierarchyAsync(MatterData matterData)
        {
            return await Task.FromResult(spList.GetFolderHierarchy(matterData));
        }

        public string AddOneNote(ClientContext clientContext, string clientAddressPath, string oneNoteLocation, string listName, string oneNoteTitle)
        {
            return spList.AddOneNote(clientContext, clientAddressPath, oneNoteLocation, listName, oneNoteTitle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteCollectionUrl"></param>
        /// <returns></returns>
        public async Task<GenericResponseVM> GetConfigurationsAsync(string siteCollectionUrl)
        {
            return await Task.FromResult(search.GetConfigurations(siteCollectionUrl, listNames.MatterConfigurationsList));
        }

        public List<Tuple<int, Principal>> CheckUserSecurity(Client client, Matter matter, IList<string> userIds)
        {
            List<Tuple<int, Principal>> teamMemberPrincipalCollection = userdetails.GetUserPrincipal(client, matter, userIds);
            return teamMemberPrincipalCollection;

        }

        public PropertyValues GetStampedProperties(ClientContext clientContext, string libraryname)
        {
            return spList.GetListProperties(clientContext, libraryname);
        }



        public IEnumerable<RoleAssignment> FetchUserPermissionForLibrary(ClientContext clientContext, string libraryname)
        {
            return spList.FetchUserPermissionForLibrary(clientContext, libraryname);
        }

        public string GetMatterName(ClientContext clientContext, string matterName)
        {
            PropertyValues propertyValues = spList.GetListProperties(clientContext, matterName);
            return propertyValues.FieldValues.ContainsKey(matterSettings.StampedPropertyMatterGUID) ?
                WebUtility.HtmlDecode(Convert.ToString(propertyValues.FieldValues[matterSettings.StampedPropertyMatterGUID], CultureInfo.InvariantCulture)) : matterName;
        }

        public int RetrieveItemId(ClientContext clientContext, string libraryName, string originalMatterName)
        {
            int listItemId = -1;
            ListItemCollection listItemCollection = spList.GetData(clientContext, libraryName);
            clientContext.Load(listItemCollection, listItemCollectionProperties =>
                                listItemCollectionProperties.Include(
                                    listItemProperties => listItemProperties.Id,
                                    listItemProperties => listItemProperties.DisplayName));
            clientContext.ExecuteQuery();

            ListItem listItem = listItemCollection.Cast<ListItem>().FirstOrDefault(listItemProperties => listItemProperties.DisplayName.ToUpper(CultureInfo.InvariantCulture).Equals(originalMatterName.ToUpper(CultureInfo.InvariantCulture)));

            if (null != listItem)
            {
                listItemId = listItem.Id;
            }
            return listItemId;
        }

        public List<string> MatterAssociatedLists(ClientContext clientContext, string matterName, MatterConfigurations matterConfigurations = null)
        {
            List<string> lists = new List<string>();
            lists.Add(matterName);
            lists.Add(matterName + matterSettings.OneNoteLibrarySuffix);
            if (null == matterConfigurations || matterConfigurations.IsCalendarSelected)
            {
                lists.Add(matterName + matterSettings.CalendarNameSuffix);
            }
            if (null == matterConfigurations || matterConfigurations.IsTaskSelected)
            {
                lists.Add(matterName + matterSettings.TaskNameSuffix);
            }
            List<string> listExists = spList.MatterAssociatedLists(clientContext, new ReadOnlyCollection<string>(lists));
            return listExists;
        }

        public bool SetItemPermission(ClientContext clientContext, IList<IList<string>> assignUserName, string listName, int listItemId, IList<string> permissions)
        {
            return spList.SetItemPermission(clientContext, assignUserName, listName, listItemId, permissions);
        }

        public string[] ConfigureXMLCodeOfWebParts(Client client, Matter matter, ClientContext clientContext, string pageName, Uri uri,
            Web web, MatterConfigurations matterConfigurations)
        {
            return spPage.ConfigureXMLCodeOfWebParts(client, matter, clientContext, pageName, uri, web, matterConfigurations);
        }

        public bool AddWebPart(ClientContext clientContext, LimitedWebPartManager limitedWebPartManager, WebPartDefinition webPartDefinition,
            string[] webParts, string[] zones)
        {
            return spPage.AddWebPart(clientContext, limitedWebPartManager, webPartDefinition, webParts, zones);
        }

        /// <summary>
        /// Remove old users and assign permissions to new users.
        /// </summary>
        /// <param name="clientContext">ClientContext object</param>
        /// <param name="requestObject">RequestObject</param>
        /// <param name="client">Client object</param>
        /// <param name="matter">Matter object</param>
        /// <param name="users">List of users to remove</param>
        /// <param name="isListItem">ListItem or list</param>
        /// <param name="list">List object</param>
        /// <param name="matterLandingPageId">List item id</param>
        /// <param name="isEditMode">Add/ Edit mode</param>
        /// <returns></returns>
        public bool UpdatePermission(ClientContext clientContext, Matter matter, List<string> users,
            string loggedInUserTitle, bool isListItem, string listName, int matterLandingPageId, bool isEditMode)
        {
            bool result = false;
            try
            {
                if (null != clientContext && !string.IsNullOrWhiteSpace(listName))
                {
                    if (isEditMode)
                    {
                        RemoveSpecificUsers(clientContext, users, loggedInUserTitle, isListItem, listName, matterLandingPageId);
                    }
                    // Add permission
                    if (!isListItem)
                    {
                        result = spList.SetPermission(clientContext, matter.AssignUserEmails, matter.Permissions, listName);
                    }
                    else
                    {
                        result = spList.SetItemPermission(clientContext, matter.AssignUserEmails, matterSettings.MatterLandingPageRepositoryName,
                            matterLandingPageId, matter.Permissions);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            // To avoid the invalid symbol error while parsing the JSON, return the response in lower case 
            return result;
        }


        /// <summary>
        /// Removes the users' permission from list or list item.
        /// </summary>
        /// <param name="clientContext">ClientContext object</param>
        /// <param name="usersToRemove">List of users</param>
        /// <param name="isListItem">ListItem or list</param>
        /// <param name="list">List object</param>
        /// <param name="matterLandingPageId">List item id</param>
        private void RemoveSpecificUsers(ClientContext clientContext, List<string> usersToRemove, string loggedInUserTitle,
            bool isListItem, string listName, int matterLandingPageId)
        {
            try
            {
                ListItem listItem = null;
                RoleAssignmentCollection roleCollection = null;
                Microsoft.SharePoint.Client.Web web = clientContext.Web;
                List list = web.Lists.GetByTitle(listName);
                clientContext.Load(list);
                clientContext.ExecuteQuery();
                if (0 < usersToRemove.Count)
                {
                    if (isListItem)
                    {
                        // Fetch the list item
                        if (0 <= matterLandingPageId)
                        {
                            listItem = list.GetItemById(matterLandingPageId);
                            clientContext.Load(listItem, listItemProperties =>
                            listItemProperties.RoleAssignments.Include(roleAssignmentProperties => roleAssignmentProperties.Member,
                            roleAssignmentProperties => roleAssignmentProperties.Member.Title,
                            roleAssignmentProperties => roleAssignmentProperties.RoleDefinitionBindings.Include(roleDef => roleDef.Name,
                                                                                                                roleDef => roleDef.BasePermissions)));
                            clientContext.ExecuteQuery();
                            roleCollection = listItem.RoleAssignments;
                        }
                    }
                    else
                    {
                        clientContext.Load(list, listProperties =>
                        listProperties.RoleAssignments.Include(roleAssignmentProperties => roleAssignmentProperties.Member,
                        roleAssignmentProperties => roleAssignmentProperties.Member.Title,
                        roleAssignmentProperties => roleAssignmentProperties.RoleDefinitionBindings.Include(roleDef => roleDef.Name,
                                                                                                            roleDef => roleDef.BasePermissions)));
                        clientContext.ExecuteQuery();
                        roleCollection = list.RoleAssignments;
                    }

                    if (null != roleCollection && 0 < roleCollection.Count && 0 < usersToRemove.Count)
                    {                       
                        foreach (string user in usersToRemove)
                        {
                            foreach (RoleAssignment role in roleCollection)
                            {
                                List<RoleDefinition> roleDefinationList = new List<RoleDefinition>();
                                foreach (RoleDefinition roleDef in role.RoleDefinitionBindings)
                                {
                                    // Removing permission for all the user except current user with full control
                                    // Add those users in list, then traverse the list and removing all users from that list

                                    //Get email from the user instead of name for comparison
                                    Principal principal = role.Member;
                                    //Check has been made to see whether the principal is of type group
                                    if (principal.GetType().Name != "Group")
                                    {
                                        User currentListUser = (User)principal;
                                        if (currentListUser.Email.ToLower() == user.ToLower() && !((role.Member.Title.ToLower() == loggedInUserTitle.ToLower()) && (roleDef.Name ==
                                            matterSettings.EditMatterAllowedPermissionLevel)))
                                        {
                                            roleDefinationList.Add(roleDef);
                                        }
                                    }

                                }
                                if (roleDefinationList.Count > 0)
                                {
                                    foreach (RoleDefinition roleDef in roleDefinationList)
                                    {
                                        role.RoleDefinitionBindings.Remove(roleDef);
                                    }
                                    role.Update();
                                }

                            }
                        }
                    }
                    clientContext.ExecuteQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }



        public bool UpdateMatterStampedProperties(ClientContext clientContext, MatterDetails matterDetails,
            Matter matter, PropertyValues matterStampedProperties, bool isEditMode, List<string> usersToRemove)
        {

            try
            {
                if (null != clientContext && null != matter && null != matterDetails && (0 < matterStampedProperties.FieldValues.Count))
                {
                    var index = 0;
                    List<string> arrRoles = new List<string>();
                    List<string> arrPermissions = new List<string>();
                    string stampedUsers = GetStampPropertyValue(matterStampedProperties.FieldValues, matterSettings.StampedPropertyMatterCenterUsers);
                    var stampedUserList = stampedUsers.Replace("$|$", "$").Split('$').ToList();
                    string stampedUserEmails = GetStampPropertyValue(matterStampedProperties.FieldValues, matterSettings.StampedPropertyMatterCenterUserEmails);
                    var stampedUserEmailsList = stampedUserEmails.Replace("$|$", "$").Split('$').ToList();
                    string stampedPermissions = GetStampPropertyValue(matterStampedProperties.FieldValues, matterSettings.StampedPropertyMatterCenterPermissions);
                    var stampedPermissionsList = stampedPermissions.Replace("$|$", "$").Split('$').ToList();
                    string stampedRoles = GetStampPropertyValue(matterStampedProperties.FieldValues, matterSettings.StampedPropertyMatterCenterRoles);
                    var stampedRolesList = stampedRoles.Replace("$|$", "$").Split('$').ToList();
                    stampedRolesList = stampedRolesList.Where(rolesList => rolesList != string.Empty).ToList();

                    string stampedTeamMembers = GetStampPropertyValue(matterStampedProperties.FieldValues, matterSettings.StampedPropertyTeamMembers);
                    var stampedTeamMembersList = stampedTeamMembers.Replace(";", "$").Split('$').ToList();
                    stampedTeamMembersList = stampedTeamMembersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    
                    List<int> itemsToRemoveStampedUserProp = new List<int>();
                    foreach (IList<string> userNames in matter.AssignUserEmails)
                    {
                        IList<string> userNamesListTemp = userNames.Where(user => !string.IsNullOrWhiteSpace(user)).ToList();
                        foreach (var userTem in userNamesListTemp)
                        {
                            arrRoles.Add(matter.Roles[index]);
                            arrPermissions.Add(matter.Permissions[index]);
                            if (stampedUserEmailsList.Contains(userTem))
                            {
                                itemsToRemoveStampedUserProp.Add(stampedUserEmailsList.IndexOf(userTem));
                            }
                        }

                        index++;
                    }

                    matter.Roles = arrRoles;
                    matter.Permissions = arrPermissions;
                    Dictionary<string, string> propertyList = new Dictionary<string, string>();
                    usersToRemove = usersToRemove.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                    // Get existing stamped properties
                    foreach (var userNameToRemove in usersToRemove)
                    {
                        if (stampedUserEmailsList.Contains(userNameToRemove))
                        {
                            itemsToRemoveStampedUserProp.Add(stampedUserEmailsList.IndexOf(userNameToRemove));
                        }

                    }

                    if (itemsToRemoveStampedUserProp.Count > 0)
                    {
                        for (int i = 0; i < itemsToRemoveStampedUserProp.Count; i++)
                        {
                            stampedUserList[itemsToRemoveStampedUserProp[i]] = string.Empty;
                            stampedUserEmailsList[itemsToRemoveStampedUserProp[i]] = string.Empty;

                            if (stampedRolesList != null && stampedRolesList.Count > 0)
                            {
                                stampedRolesList[itemsToRemoveStampedUserProp[i]] = string.Empty;
                            }
                            stampedPermissionsList[itemsToRemoveStampedUserProp[i]] = string.Empty;
                            stampedTeamMembersList[itemsToRemoveStampedUserProp[i]] = string.Empty;
                        }
                    }


                    stampedUserList = stampedUserList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    stampedUsers = string.Join("$|$", stampedUserList);
                    stampedUserEmailsList = stampedUserEmailsList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    stampedUserEmails = string.Join("$|$", stampedUserEmailsList);
                    stampedRolesList = stampedRolesList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    stampedRoles = string.Join("$|$", stampedRolesList);
                    stampedPermissionsList = stampedPermissionsList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    stampedPermissions = string.Join("$|$", stampedPermissionsList);
                    stampedTeamMembersList = stampedTeamMembersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    stampedTeamMembers = string.Join(";", stampedTeamMembersList);

                    string stampedResponsibleAttorneys = GetStampPropertyValue(matterStampedProperties.FieldValues, matterSettings.StampedPropertyResponsibleAttorney);
                    var stampedResponsibleAttorneysList = stampedResponsibleAttorneys.Split(';').ToList();
                    stampedResponsibleAttorneysList = stampedResponsibleAttorneysList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    string stampedResponsibleAttorneysEmail = GetStampPropertyValue(matterStampedProperties.FieldValues, matterSettings.StampedPropertyResponsibleAttorneyEmail);
                    var stampedResponsibleAttorneysEmailList = stampedResponsibleAttorneysEmail.Split(';').ToList();
                    stampedResponsibleAttorneysEmailList = stampedResponsibleAttorneysEmailList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                    string currentPermissions = string.Join(ServiceConstants.DOLLAR + ServiceConstants.PIPE + ServiceConstants.DOLLAR, matter.Permissions.Where(user => !string.IsNullOrWhiteSpace(user)));
                    string currentRoles = string.Join(ServiceConstants.DOLLAR + ServiceConstants.PIPE + ServiceConstants.DOLLAR, matter.Roles.Where(user => !string.IsNullOrWhiteSpace(user)));
                    string currentBlockedUploadUsers = string.Join(ServiceConstants.SEMICOLON, matterDetails.UploadBlockedUsers.Where(user => !string.IsNullOrWhiteSpace(user)));
                    string currentUsers = GetMatterAssignedUsers(matter);
                    currentUsers = currentUsers.Replace(";", "$|$").ToString();
                    string currentUserEmails = spList.GetMatterAssignedUsersEmail(clientContext, matter);
                    currentUserEmails = currentUserEmails.Replace(";", "$|$").ToString();

                    string currentResponsibleAttorneys = matterDetails.ResponsibleAttorney;
                    var currentResponsibleAttorneysList = currentResponsibleAttorneys.Split(';').ToList();

                    string currentResponsibleAttorneysEmail = matterDetails.ResponsibleAttorneyEmail;
                    var currentResponsibleAttorneysEmailList = currentResponsibleAttorneysEmail.Split(';').ToList();

                    List<int> itemsToRemoveStampedResponsibleAttroneyProp = new List<int>();
                    foreach (var respAttroneyUser in currentResponsibleAttorneysList)
                    {
                        if (respAttroneyUser != "")
                        {
                            if (stampedResponsibleAttorneysList.Contains(respAttroneyUser))
                            {
                                itemsToRemoveStampedResponsibleAttroneyProp.Add(stampedResponsibleAttorneysList.IndexOf(respAttroneyUser));
                            }
                        }
                    }
                    foreach (var userNameToRemove in usersToRemove)
                    {
                        if (stampedResponsibleAttorneysEmailList.Contains(userNameToRemove))
                        {
                            itemsToRemoveStampedResponsibleAttroneyProp.Add(stampedResponsibleAttorneysEmailList.IndexOf(userNameToRemove));
                        }

                    }

                    if (itemsToRemoveStampedResponsibleAttroneyProp.Count > 0)
                    {
                        for (int i = 0; i < itemsToRemoveStampedResponsibleAttroneyProp.Count; i++)
                        {
                            stampedResponsibleAttorneysList[itemsToRemoveStampedResponsibleAttroneyProp[i]] = string.Empty;
                            stampedResponsibleAttorneysEmailList[itemsToRemoveStampedResponsibleAttroneyProp[i]] = string.Empty;
                        }
                    }

                    stampedResponsibleAttorneysList = stampedResponsibleAttorneysList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    stampedResponsibleAttorneys = string.Join(";", stampedResponsibleAttorneysList);
                    stampedResponsibleAttorneysEmailList = stampedResponsibleAttorneysEmailList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    stampedResponsibleAttorneysEmail = string.Join(";", stampedResponsibleAttorneysEmailList);




                    string finalBlockedUploadUsers = string.Empty;
                    string finalMatterPermissions = string.IsNullOrWhiteSpace(stampedPermissions) || isEditMode ? currentPermissions : string.Concat(stampedPermissions, ServiceConstants.DOLLAR + ServiceConstants.PIPE + ServiceConstants.DOLLAR, currentPermissions);
                    string finalMatterRoles = string.IsNullOrWhiteSpace(stampedRoles) || isEditMode ? currentRoles : string.Concat(stampedRoles, ServiceConstants.DOLLAR + ServiceConstants.PIPE + ServiceConstants.DOLLAR, currentRoles);
                    string finalResponsibleAttorneys = string.IsNullOrWhiteSpace(stampedResponsibleAttorneys) || isEditMode ? matterDetails.ResponsibleAttorney : string.Concat(stampedResponsibleAttorneys, ServiceConstants.SEMICOLON, matterDetails.ResponsibleAttorney);
                    string finalTeamMembers = string.IsNullOrWhiteSpace(stampedTeamMembers) || isEditMode ? matterDetails.TeamMembers : string.Concat(stampedTeamMembers, ServiceConstants.SEMICOLON, matterDetails.TeamMembers);
                    string finalMatterCenterUsers = string.IsNullOrWhiteSpace(stampedUsers) || isEditMode ? currentUsers : string.Concat(stampedUsers, ServiceConstants.DOLLAR + ServiceConstants.PIPE + ServiceConstants.DOLLAR, currentUsers);
                    
                    string finalMatterCenterUserEmails = string.IsNullOrWhiteSpace(stampedUserEmails) || isEditMode ? currentUserEmails : string.Concat(stampedUserEmails, ServiceConstants.DOLLAR + ServiceConstants.PIPE + ServiceConstants.DOLLAR, currentUserEmails);
                    string finalResponsibleAttorneysEmail = string.IsNullOrWhiteSpace(stampedResponsibleAttorneysEmail) || isEditMode ? matterDetails.ResponsibleAttorneyEmail : string.Concat(stampedResponsibleAttorneysEmail, ServiceConstants.SEMICOLON, matterDetails.ResponsibleAttorneyEmail);
                    var finalMatterUsers = finalMatterCenterUsers.Replace("$|$", ";").ToString();
                    var finalMatterUsersList = finalMatterUsers.Replace(";", "$").Split('$').ToList();
                    finalMatterUsersList = finalMatterUsersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();


                    var finalTeamMembersList = finalTeamMembers.Replace(";", "$").Split('$').ToList();
                    finalTeamMembersList = finalTeamMembersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                    var finalMatterPermissionsList = finalMatterPermissions.Replace("$|$", "$").Split('$').ToList();
                    finalMatterPermissionsList = finalMatterPermissionsList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    var finalMatterRolesList = finalMatterRoles.Replace("$|$", "$").Split('$').ToList();
                    finalMatterRolesList = finalMatterRolesList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                    var finalMatterCenterUserEmailsString = finalMatterCenterUserEmails.Replace("$|$", ";");
                    var finalMatterCenterUserEmailsList = finalMatterCenterUserEmailsString.Replace(";", "$").Split('$').ToList();
                    finalMatterCenterUserEmailsList = finalMatterCenterUserEmailsList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                    var finalResponsibleAttorneysEmailListString = finalResponsibleAttorneysEmail.Replace("$|$", ";");
                    var finalResponsibleAttorneysEmailList = finalResponsibleAttorneysEmailListString.Replace(";", "$").Split('$').ToList();
                    finalResponsibleAttorneysEmailList = finalResponsibleAttorneysEmailList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                    var finalResponsibleAttorneysUsersListString = finalResponsibleAttorneys.Replace(";", "$");
                    var finalResponsibleAttorneysUsersList = finalResponsibleAttorneysUsersListString.Split('$').ToList();
                    finalResponsibleAttorneysUsersList = finalResponsibleAttorneysUsersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();


                    #region Remove all the external users from the request object so that only internal users are added to the matter
                    //Once the external users accepted the invitation, those external users will be added to the matter by azure web app job
                    List<int> itemsToRemove = new List<int>();
                    ///Below changes are required for the matters where no roles are available in the stamped properties. By default we are adding Responsible Attorney
                    if (generalSettings.IsBackwardCompatible == true)
                    {
                        if (finalMatterRolesList != null && finalMatterRolesList.Count == 0)
                        {
                            for (int i = 0; i < finalMatterPermissionsList.Count; i++)
                            {
                                finalMatterRolesList.Add("Responsible Attorney");
                            }
                        }
                    } 

                    //If the user permissions has been changed to read, need to add that user to blocked users list, so that, read only users can't upload 
                    //documents to matters
                    var finalBlockedUploadUsersList = new List<string>();
                    for (int i = 0; i < finalMatterPermissionsList.Count; i++)
                    {
                        if (finalMatterPermissionsList[i].ToLower() == "read")
                        {
                            finalBlockedUploadUsersList.Add(finalMatterCenterUserEmailsList[i]);
                        }
                    }
                    finalBlockedUploadUsers = string.Join(";", finalBlockedUploadUsersList);


                    for (int i = 0; i < finalMatterCenterUserEmailsList.Count; i++)
                    {
                        if (userdetails.CheckUserPresentInMatterCenter(clientContext, finalMatterCenterUserEmailsList[i]) == false)
                        {
                            itemsToRemove.Add(i);
                        }
                    }

                    if (itemsToRemove.Count > 0)
                    {
                        for (int i = 0; i < itemsToRemove.Count; i++)
                        {
                            finalMatterUsersList[itemsToRemove[i]] = string.Empty;
                            finalTeamMembersList[itemsToRemove[i]] = string.Empty;
                            finalMatterPermissionsList[itemsToRemove[i]] = string.Empty;
                            finalMatterRolesList[itemsToRemove[i]] = string.Empty;
                            finalMatterCenterUserEmailsList[itemsToRemove[i]] = string.Empty;
                            if (finalResponsibleAttorneysEmailList.Count > itemsToRemove[i] && finalResponsibleAttorneysEmailList[itemsToRemove[i]] != null)
                            {
                                finalResponsibleAttorneysEmailList[itemsToRemove[i]] = string.Empty;
                                finalResponsibleAttorneysUsersList[itemsToRemove[i]] = string.Empty;
                            }
                        }
                        finalMatterUsersList = finalMatterUsersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        finalTeamMembersList = finalTeamMembersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        finalMatterPermissionsList = finalMatterPermissionsList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        finalMatterRolesList = finalMatterRolesList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        finalResponsibleAttorneysEmailList = finalResponsibleAttorneysEmailList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        finalResponsibleAttorneysUsersList = finalResponsibleAttorneysUsersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        finalMatterCenterUserEmailsList = finalMatterCenterUserEmailsList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        var finalMatterUsersArray = finalMatterUsersList.ToArray();
                        var finalTeamMembersArray = finalTeamMembersList.ToArray();
                        var finalMatterPermissionsArray = finalMatterPermissionsList.ToArray();
                        var finalMatterRolesArray = finalMatterRolesList.ToArray();
                        var finalMatterCenterUserEmailsArray = finalMatterCenterUserEmailsList.ToArray();
                        var finalResponsibleAttorneysEmailArray = finalResponsibleAttorneysEmailList.ToArray();
                        var finalResponsibleAttorneysUsersArray = finalResponsibleAttorneysUsersList.ToArray();

                        finalMatterCenterUsers = string.Join("$|$", finalMatterUsersArray);
                        finalTeamMembers = string.Join(";;", finalTeamMembersArray);
                        finalMatterCenterUserEmails = string.Join("$|$", finalMatterCenterUserEmailsArray);
                        finalMatterPermissions = string.Join("$|$", finalMatterPermissionsArray);
                        finalMatterRoles = string.Join("$|$", finalMatterRolesArray);
                        finalResponsibleAttorneysEmail = string.Join(";", finalResponsibleAttorneysEmailArray);
                        finalResponsibleAttorneys = string.Join(";", finalResponsibleAttorneysUsersArray);
                    }
                    #endregion

                    propertyList.Add(matterSettings.StampedPropertyResponsibleAttorney, WebUtility.HtmlEncode(finalResponsibleAttorneys));
                    propertyList.Add(matterSettings.StampedPropertyResponsibleAttorneyEmail, WebUtility.HtmlEncode(finalResponsibleAttorneysEmail));
                    propertyList.Add(matterSettings.StampedPropertyTeamMembers, WebUtility.HtmlEncode(finalTeamMembers));
                    propertyList.Add(matterSettings.StampedPropertyBlockedUploadUsers, WebUtility.HtmlEncode(finalBlockedUploadUsers));
                    propertyList.Add(matterSettings.StampedPropertyMatterCenterRoles, WebUtility.HtmlEncode(finalMatterRoles));
                    propertyList.Add(matterSettings.StampedPropertyMatterCenterPermissions, WebUtility.HtmlEncode(finalMatterPermissions));
                    propertyList.Add(matterSettings.StampedPropertyMatterCenterUsers, WebUtility.HtmlEncode(finalMatterCenterUsers));
                    propertyList.Add(matterSettings.StampedPropertyMatterCenterUserEmails, WebUtility.HtmlEncode(finalMatterCenterUserEmails));
                    spList.SetPropertBagValuesForList(clientContext, matterStampedProperties, matter.Name, propertyList);

                    #region Update matter and matterdetails object to have only external users so that notification can be send to those external users
                    //For each external user, an entry will be created in the azure table storage and that entry will contain only the information related to the
                    //external user, such as his role, his permission etc
                    itemsToRemove.Clear();
                    List<int> itemsToRemoveAttorneys = new List<int>();

                    int l = 0;
                    finalResponsibleAttorneysEmailList = matterDetails.ResponsibleAttorneyEmail.Split(';').ToList();
                    finalResponsibleAttorneysUsersList = matterDetails.ResponsibleAttorney.Split(';').ToList();
                    foreach (string userName in finalResponsibleAttorneysUsersList)
                    {
                        if (!string.IsNullOrWhiteSpace(userName) && userdetails.CheckUserPresentInMatterCenter(clientContext, userName) == true)
                        {
                            itemsToRemoveAttorneys.Add(l);
                        }
                        l = l + 1;
                    }
                    if (itemsToRemoveAttorneys.Count > 0)
                    {
                        for (int k = 0; k < itemsToRemoveAttorneys.Count; k++)
                        {
                            if (finalResponsibleAttorneysEmailList.Count > itemsToRemoveAttorneys[k] &&
                                finalResponsibleAttorneysEmailList[itemsToRemoveAttorneys[k]] != null)
                            {
                                finalResponsibleAttorneysUsersList[itemsToRemoveAttorneys[k]] = string.Empty;
                                finalResponsibleAttorneysEmailList[itemsToRemoveAttorneys[k]] = string.Empty;
                            }
                        }
                        finalResponsibleAttorneysUsersList = finalResponsibleAttorneysUsersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        finalResponsibleAttorneysEmailList = finalResponsibleAttorneysEmailList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    }
                    l = 0;

                    //Check if any of the assigned team member is not an external user?



                    //setting the internal users to null and adding the index to remove the roles and permissions in matter.Permissions and matter.roles
                    for (int z = 0; z < matter.AssignUserNames.Count; z++)
                    {
                        for (int y = 0; y < matter.AssignUserNames[z].Count; y++)
                        {
                            string userName = matter.AssignUserNames[z][y].ToString();
                            if (userName != "")
                            {
                                if (userdetails.CheckUserPresentInMatterCenter(clientContext, userName) == true)
                                {
                                    itemsToRemove.Add(l);
                                    matter.AssignUserNames[z][y] = null;
                                    matter.AssignUserEmails[z][y] = null;
                                }
                                l = l + 1;
                            }
                        }
                    }



                    for (int x = 0; x < matter.AssignUserNames.Count; x++)
                    {
                        var matterAssignedNames = matter.AssignUserNames[x].Where(user => !string.IsNullOrWhiteSpace(user));
                        var matterAssignedUserEmails = matter.AssignUserEmails[x].Where(user => !string.IsNullOrWhiteSpace(user));
                        if (matterAssignedNames.Count() > 0)
                        {
                            matter.AssignUserNames[x] = matterAssignedNames.ToList();
                            matter.AssignUserEmails[x] = matterAssignedUserEmails.ToList();
                        }
                        else
                        {
                            matter.AssignUserNames[x] = null;
                            matter.AssignUserEmails[x] = null;
                        }
                    }



                    //  finalTeamMembersList = matterDetails.TeamMembers.Replace(";", "$").Split('$').ToList();
                    finalTeamMembersList = matterDetails.TeamMembers.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    if (itemsToRemove.Count > 0)
                    {
                        for (int k = 0; k < itemsToRemove.Count; k++)
                        {
                            matter.Permissions[itemsToRemove[k]] = string.Empty;
                            matter.Roles[itemsToRemove[k]] = string.Empty;
                            //matter.AssignUserEmails[itemsToRemove[k]] = null;
                            //matter.AssignUserNames[itemsToRemove[k]] = null;
                            finalTeamMembersList[itemsToRemove[k]] = string.Empty;
                        }
                    }

                    matter.Permissions = matter.Permissions.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    matter.Roles = matter.Roles.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    finalTeamMembersList = finalTeamMembersList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    matter.AssignUserEmails = matter.AssignUserEmails.Where(s => s != null).ToList();
                    matter.AssignUserNames = matter.AssignUserNames.Where(s => s != null).ToList();


                    matterDetails.ResponsibleAttorneyEmail = string.Empty;
                    matterDetails.ResponsibleAttorney = string.Empty;
                    matterDetails.TeamMembers = string.Join(";", finalTeamMembersList.ToArray());
                    matterDetails.ResponsibleAttorneyEmail = string.Join(";", finalResponsibleAttorneysEmailList.ToArray());
                    matterDetails.ResponsibleAttorney = string.Join(";", finalResponsibleAttorneysUsersList.ToArray());
                    #endregion

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw; //// This will transfer control to catch block of parent function.
            }
            return false;
        }

        public void SetPropertBagValuesForList(ClientContext clientContext, PropertyValues props, string matterName, Dictionary<string, string> propertyList)
        {
            spList.SetPropertBagValuesForList(clientContext, props, matterName, propertyList);
        }

        /// <summary>
        /// Assign or Remove Full Control base on parameter given.
        /// </summary>
        /// <param name="clientContext">Client context object</param>
        /// <param name="matter">Matter object</param>
        /// <param name="loggedInUser">Name of logged in user</param>
        /// <param name="listExists">List of existed list</param>
        /// <param name="listItemId">ID of the list</param>
        /// <param name="assignFullControl">Flag to determine Assign or Remove Permission</param>
        public void AssignRemoveFullControl(ClientContext clientContext, Matter matter, string loggedInUser,
            int listItemId, List<string> listExists, bool assignFullControl, bool hasFullPermission)
        {
            IList<IList<string>> currentUser = new List<IList<string>>();
            IList<string> currentLoggedInUser = new List<string>() { loggedInUser };
            currentUser.Add(currentLoggedInUser);

            IList<string> permission = new List<string>() { matterSettings.EditMatterAllowedPermissionLevel };

            if (assignFullControl)
            {
                //Assign full control to Matter
                if (listExists.Contains(matter.Name))
                {
                    spList.SetPermission(clientContext, currentUser, permission, matter.Name);
                }
                //Assign full control to OneNote
                if (listExists.Contains(matter.Name + matterSettings.OneNoteLibrarySuffix))
                {
                    spList.SetPermission(clientContext, currentUser, permission, matter.Name + matterSettings.OneNoteLibrarySuffix);
                }
                // Assign full control to Task list 
                if (listExists.Contains(matter.Name + matterSettings.TaskNameSuffix))
                {
                    spList.SetPermission(clientContext, currentUser, permission, matter.Name + matterSettings.TaskNameSuffix);
                }
                //Assign full control to calendar 
                if (listExists.Contains(matter.Name + matterSettings.CalendarNameSuffix))
                {
                    spList.SetPermission(clientContext, currentUser, permission, matter.Name + matterSettings.CalendarNameSuffix);
                }
                // Assign full control to Matter Landing page
                if (0 <= listItemId)
                {
                    spList.SetItemPermission(clientContext, currentUser, matterSettings.MatterLandingPageRepositoryName, listItemId, permission);
                }
            }
            else
            {
                if (!hasFullPermission)
                {
                    //Remove full control to Matter
                    if (listExists.Contains(matter.Name))
                    {
                        RemoveFullControl(clientContext, matter.Name, loggedInUser, false, -1);
                    }
                    //Remove full control to OneNote
                    if (listExists.Contains(matter.Name + matterSettings.OneNoteLibrarySuffix))
                    {
                        RemoveFullControl(clientContext, matter.Name + matterSettings.OneNoteLibrarySuffix, loggedInUser, false, -1);
                    }
                    // Remove full control to Task list 
                    if (listExists.Contains(matter.Name + matterSettings.TaskNameSuffix))
                    {
                        RemoveFullControl(clientContext, matter.Name + matterSettings.TaskNameSuffix, loggedInUser, false, -1);
                    }
                    //Remove full control to calendar 
                    if (listExists.Contains(matter.Name + matterSettings.CalendarNameSuffix))
                    {
                        RemoveFullControl(clientContext, matter.Name + matterSettings.CalendarNameSuffix, loggedInUser, false, -1);
                    }
                    if (0 <= listItemId)
                    {
                        RemoveFullControl(clientContext, matterSettings.MatterLandingPageRepositoryName, loggedInUser, true, listItemId);
                    }
                }
            }
        }

        /// <summary>
        /// Reverts the permission of users from matter, OneNote, Calendar libraries and matter landing page
        /// </summary>
        /// <param name="requestObject">Request object</param>
        /// <param name="client">Client object</param>
        /// <param name="matter">Matter object</param>
        /// <param name="clientContext">ClientContext object</param>
        /// <param name="matterRevertListObject">MatterRevertObjectList object</param>
        /// <param name="loggedInUserTitle">Logged-in user title</param>
        /// <param name="oldUserPermissions">Old library users</param>
        /// <param name="matterLandingPageId">List item id</param>
        /// <param name="isEditMode">Add/ Edit mode</param>
        /// <returns>Status of operation</returns>
        public bool RevertMatterUpdates(Client client, Matter matter, MatterRevertList matterRevertListObject, bool isEditMode, IEnumerable<RoleAssignment> oldUserPermissionOnLibrary
            )
        {
            bool result = false;
            try
            {
                var clientContext = spoAuthorization.GetClientContext(client.Url);
                string loggedInUserTitle = userRepositoy.GetLoggedInUserDetails(clientContext).Email;

                string originalMatterName = GetMatterName(clientContext, matter.Name);
                int listItemId = RetrieveItemId(clientContext, matterSettings.MatterLandingPageRepositoryName, originalMatterName);



                if (null != client && null != matter && null != clientContext && null != matterRevertListObject)
                {
                    List<string> users = new List<string>();
                    users = matter.AssignUserNames.SelectMany(user => user).Distinct().ToList();

                    // Remove recently added users
                    if (null != matterRevertListObject.MatterLibrary)
                    {
                        RemoveSpecificUsers(clientContext, users, loggedInUserTitle, false, matterRevertListObject.MatterLibrary, -1);
                    }
                    if (null != matterRevertListObject.MatterCalendar)
                    {
                        RemoveSpecificUsers(clientContext, users, loggedInUserTitle, false, matterRevertListObject.MatterCalendar, -1);
                    }
                    if (null != matterRevertListObject.MatterOneNoteLibrary)
                    {
                        RemoveSpecificUsers(clientContext, users, loggedInUserTitle, false, matterRevertListObject.MatterOneNoteLibrary, -1);
                    }
                    if (null != matterRevertListObject.MatterTask)
                    {
                        RemoveSpecificUsers(clientContext, users, loggedInUserTitle, false, matterRevertListObject.MatterTask, -1);
                    }
                    if (null != matterRevertListObject.MatterSitePages)
                    {
                        RemoveSpecificUsers(clientContext, users, loggedInUserTitle, true, matterRevertListObject.MatterSitePages, listItemId);
                    }

                    if (isEditMode)
                    {
                        Matter matterRevertUserPermission = PrepareUserPermission(oldUserPermissionOnLibrary);
                        if (null != matterRevertListObject.MatterLibrary)
                        {
                            result = spList.SetPermission(clientContext, matterRevertUserPermission.AssignUserNames, matterRevertUserPermission.Permissions, matterRevertListObject.MatterLibrary);
                        }
                        if (null != matterRevertListObject.MatterOneNoteLibrary)
                        {
                            result = spList.SetPermission(clientContext, matterRevertUserPermission.AssignUserNames, matterRevertUserPermission.Permissions, matterRevertListObject.MatterOneNoteLibrary);
                        }
                        if (null != matterRevertListObject.MatterCalendar)
                        {
                            result = spList.SetPermission(clientContext, matterRevertUserPermission.AssignUserNames, matterRevertUserPermission.Permissions, matterRevertListObject.MatterCalendar);
                        }
                        if (null != matterRevertListObject.MatterTask)
                        {
                            result = spList.SetPermission(clientContext, matterRevertUserPermission.AssignUserNames, matterRevertUserPermission.Permissions, matterRevertListObject.MatterTask);
                        }
                        if (null != matterRevertListObject.MatterSitePages && 0 <= listItemId)
                        {
                            result = spList.SetItemPermission(clientContext, matterRevertUserPermission.AssignUserNames, matterSettings.MatterLandingPageRepositoryName, listItemId, matterRevertUserPermission.Permissions);
                        }
                    }
                }
                return result;
            }
            catch (Exception exception)
            {
                //Logger.LogError(exception, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ServiceConstants.LogTableName);
            }
            // To avoid the invalid symbol error while parsing the JSON, return the response in lower case
            return result;
        }

        public IList<ContentType> GetContentTypeData(ClientContext clientContext, IList<string> contentTypes, Client client, Matter matter)
        {
            return spContentTypes.GetContentTypeData(clientContext, contentTypes, client, matter);
        }

        public GenericResponseVM AssignContentTypeHelper(MatterMetadata matterMetadata, ClientContext clientContext, IList<ContentType> contentTypeCollection, Client client, Matter matter)
        {
            return spContentTypes.AssignContentTypeHelper(matterMetadata, clientContext, contentTypeCollection, client, matter);
        }


        /// <summary>
        /// Fetches the users to remove permission.
        /// </summary>
        /// <param name="userPermissions">Users having permission on library</param>
        /// <returns>Matter object containing user name and permissions</returns>
        internal Matter PrepareUserPermission(IEnumerable<RoleAssignment> userPermissions)
        {
            Matter matterUserPermission = new Matter();
            matterUserPermission.AssignUserNames = new List<IList<string>>();
            matterUserPermission.Permissions = new List<string>();

            if (null != userPermissions && 0 < userPermissions.Count())
            {
                foreach (RoleAssignment userPermission in userPermissions)
                {
                    foreach (RoleDefinition roleDefinition in userPermission.RoleDefinitionBindings)
                    {
                        matterUserPermission.AssignUserNames.Add(new List<string> { userPermission.Member.Title });
                        matterUserPermission.Permissions.Add(roleDefinition.Name);
                    }
                }
            }
            return matterUserPermission;
        }

        /// <summary>
        /// Remove Full Permission.
        /// </summary>
        /// <param name="clientContext">Client context object</param>
        /// <param name="listName">Name of the list</param>
        /// <param name="currentLoggedInUser">Name of logged in User</param>
        internal void RemoveFullControl(ClientContext clientContext, string listName, string currentLoggedInUser, bool isListItem, int matterLandingPageId)
        {
            ListItem listItem = null;
            RoleAssignmentCollection roleCollection = null;
            List list = clientContext.Web.Lists.GetByTitle(listName);
            clientContext.Load(list);
            clientContext.ExecuteQuery();
            if (isListItem)
            {
                // Fetch the list item
                if (0 <= matterLandingPageId)
                {
                    listItem = list.GetItemById(matterLandingPageId);
                    clientContext.Load(listItem, listProperties => listProperties.RoleAssignments.Include(roleAssignmentProperties =>
                        roleAssignmentProperties.Member,
                        roleAssignmentProperties => roleAssignmentProperties.Member.Title,
                        roleAssignmentProperties => roleAssignmentProperties.RoleDefinitionBindings.Include(roleDef => roleDef.Name, roleDef => roleDef.BasePermissions)));
                    clientContext.ExecuteQuery();
                    roleCollection = listItem.RoleAssignments;
                }
            }
            else
            {
                clientContext.Load(list, listProperties => listProperties.RoleAssignments.Include(roleAssignmentProperties =>
                    roleAssignmentProperties.Member,
                    roleAssignmentProperties => roleAssignmentProperties.Member.Title,
                    roleAssignmentProperties => roleAssignmentProperties.RoleDefinitionBindings.Include(roleDef => roleDef.Name, roleDef => roleDef.BasePermissions)));
                clientContext.ExecuteQuery();
                roleCollection = list.RoleAssignments;
            }


            if (null != roleCollection && 0 < roleCollection.Count)
            {
                foreach (RoleAssignment role in roleCollection)
                {
                    if (role.Member.Title == currentLoggedInUser)
                    {
                        IList<RoleDefinition> roleDefinationList = new List<RoleDefinition>();
                        foreach (RoleDefinition roleDef in role.RoleDefinitionBindings)
                        {
                            if (roleDef.Name == matterSettings.EditMatterAllowedPermissionLevel)
                            {
                                roleDefinationList.Add(roleDef);
                            }
                        }
                        foreach (RoleDefinition roleDef in roleDefinationList)
                        {
                            role.RoleDefinitionBindings.Remove(roleDef);
                        }
                    }
                    role.Update();
                }
            }
            clientContext.ExecuteQuery();

        }

        /// <summary>
        /// Checks if the property exists in property bag. Returns the value for the property from property bag.
        /// </summary>
        /// <param name="stampedPropertyValues">Dictionary object containing matter property bag key/value pairs</param>
        /// <param name="key">Key to check in dictionary</param>
        /// <returns>Property bag value for </returns>
        internal string GetStampPropertyValue(Dictionary<string, object> stampedPropertyValues, string key)
        {
            string result = string.Empty;
            if (stampedPropertyValues.ContainsKey(key))
            {
                result = WebUtility.HtmlDecode(Convert.ToString(stampedPropertyValues[key], CultureInfo.InvariantCulture));
            }

            // This is just to check for null value in key, if exists
            return (!string.IsNullOrWhiteSpace(result)) ? result : string.Empty;
        }

        /// <summary>
        /// Converts the matter users in a form that can be stamped to library.
        /// </summary>
        /// <param name="matter">Matter object</param>
        /// <returns>Users that can be stamped</returns>
        private string GetMatterAssignedUsers(Matter matter)
        {
            try
            {
                string currentUsers = string.Empty;
                string separator = string.Empty;
                if (null != matter && 0 < matter.AssignUserNames.Count)
                {
                    foreach (IList<string> userNames in matter.AssignUserNames)
                    {
                        currentUsers += separator + string.Join(ServiceConstants.SEMICOLON, userNames.Where(user => !string.IsNullOrWhiteSpace(user)));
                        separator = ServiceConstants.DOLLAR + ServiceConstants.PIPE + ServiceConstants.DOLLAR;
                    }
                }
                return currentUsers;
            }
            catch (Exception ex)
            {
                customLogger.LogError(ex, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, logTables.SPOLogTable);
                throw;
            }
        }

        public bool SetPermission(ClientContext clientContext, IList<IList<string>> assignUserName, IList<string> permissions, string listName) => spList.SetPermission(clientContext, assignUserName, permissions, listName);

        public GenericResponseVM ShareMatterToExternalUser(MatterInformationVM matterInformation) => extrnalSharing.ShareMatter(matterInformation);

        public bool OneNoteUrlExists(MatterInformationVM matterInformation) => search.PageExists(matterInformation.Client, matterInformation.RequestedUrl);

        /// <summary>
        /// This method will get all the Field columns of the content type as the JSON object which is used to render the dynamic UI.
        /// </summary>
        /// <param name="contentTypeName"></param>
        /// <param name="client"></param>
        /// <returns>Json</returns>
        public string GetMatterProvisionExtraProperties(string contentTypeName, Client client)
        {
            try
            {
                var clientContext = spoAuthorization.GetClientContext(client.Url);
                FieldCollection fieldCollection = contentTypeName.GetFieldsInContentType(clientContext);

                //To get configuration settings...
                var listQuery = string.Format(CultureInfo.InvariantCulture, camlQueries.MatterConfigurationsListQuery,
                        searchSettings.ManagedPropertyTitle, searchSettings.MatterConfigurationTitleValue);
                ListItem settingsItem = spList.GetData(clientContext,
                    listNames.MatterConfigurationsList, listQuery).FirstOrDefault();

                IList<MatterExtraFields> addFields = new List<MatterExtraFields>();

                if (settingsItem != null) 
                {
                    Newtonsoft.Json.Linq.JObject settingConfig = (Newtonsoft.Json.Linq.JObject)
                        JsonConvert.DeserializeObject(WebUtility.HtmlDecode(Convert.ToString(
                            settingsItem.FieldValues["ConfigurationValue"])));

                    if (settingConfig != null && settingConfig.GetValue("AdditionalFieldValues") != null)
                    {
                        foreach (var objField in settingConfig.GetValue("AdditionalFieldValues"))
                        {
                            addFields.Add(new MatterExtraFields
                            {
                                FieldName = objField["FieldName"].ToString(),
                                IsDisplayInUI = String.IsNullOrWhiteSpace(objField["IsDisplayInUI"].ToString()) ? "false"
                                                 : objField["IsDisplayInUI"].ToString(),
                                IsMandatory = String.IsNullOrWhiteSpace(objField["IsMandatory"].ToString()) ? "false"
                                                 : objField["IsMandatory"].ToString()
                            });
                        }
                    }
                }
                //When the document is getting uploaded we have to send the default values of Additional Matter
                //content type site columns so that the user can override/update those values for the document 
                //that is getting uploaded.
                //Get all site columns that are present in 'Additional Matter Properties' content type.
               
                Dictionary<string, string> matterExtraPropertiesDefaultValues = new Dictionary<string, string>();              
                CamlQuery query = new CamlQuery();
                if (client.Name != null)
                {
                    Web web = clientContext.Web;
                    ListCollection lists = web.Lists;
                    List selectedList = lists.GetByTitle(client.Name);
                    spContentTypes.AssignContentType(clientContext, contentTypeName, selectedList);
                    FieldCollection contentTypeFields = contentTypeName.GetFieldsInContentType(clientContext);
                    FieldCollection fields = spList.GetMatterExtraDefaultSiteColumns(clientContext, selectedList);
                    if (fields != null && contentTypeFields != null && contentTypeFields.Count > 0)
                    {
                        foreach (var field in fields)
                        {
                            foreach (var contentTypeField in contentTypeFields)
                            {
                                //If document library field name is part of content type field name 
                                //then update default value of tht column name to the value 
                                //of that column name.
                                if (field.InternalName == contentTypeField.InternalName)
                                {
                                    if (field.Group == contentTypesSettings.OneDriveContentTypeGroup)
                                    {
                                        string fieldValue = string.Empty;
                                        if (field.TypeAsString.ToLower() == "datetime")
                                        {
                                            if (!string.IsNullOrEmpty(field.DefaultValue))
                                            {
                                                fieldValue = DateTime.Parse(field.DefaultValue).ToString("MM/dd/yyyy");
                                            }
                                        }
                                        else
                                        {
                                            fieldValue = field.DefaultValue;
                                        }
                                        matterExtraPropertiesDefaultValues.Add(field.InternalName, fieldValue);
                                    }
                                }
                            }
                        }
                    }
                } 
                StringBuilder sb = new StringBuilder();
                JsonWriter jw = new JsonTextWriter(new StringWriter(sb));
                jw.Formatting = Formatting.Indented;
                jw.WriteStartObject();

                jw.WritePropertyName("Fields");
                jw.WriteStartArray();
                foreach (var field in fieldCollection)
                {
                    if (field.Group == this.contentTypesSettings.OneDriveContentTypeGroup)
                    {
                        jw.WriteStartObject();
                        jw.WritePropertyName("name");
                        jw.WriteValue(field.Title);

                        jw.WritePropertyName("fieldInternalName");
                        jw.WriteValue(field.InternalName);

                        jw.WritePropertyName("required");
                        string isRequired= "false";
                        string required = "false";
                        string isDisplayInUI = "false";
                       
                        foreach (var item in addFields)
                        {
                        
                            if(item.FieldName== field.InternalName)
                            {
                                 isRequired = addFields.Count > 0 ? addFields.Where(x => x.FieldName == field.InternalName).SingleOrDefault().IsMandatory : field.Required.ToString();
                                 required = string.IsNullOrWhiteSpace(isRequired) ? false.ToString() : isRequired.ToLower();
                                 isDisplayInUI = addFields.Count > 0 ? addFields.Where(x => x.FieldName == field.InternalName).SingleOrDefault().IsDisplayInUI : "false";
                                break;
                            }
                           
                        }
                       
                        jw.WriteValue(required);

                        jw.WritePropertyName("displayInUI");
                       
                        isDisplayInUI = string.IsNullOrWhiteSpace(isDisplayInUI) ? "false" : isDisplayInUI;
                        jw.WriteValue(isDisplayInUI);

                        jw.WritePropertyName("originalType");
                        jw.WriteValue(field.TypeAsString);
                        jw.WritePropertyName("defaultValue");
                        if (client.Name!=null && matterExtraPropertiesDefaultValues!=null && matterExtraPropertiesDefaultValues.Count>0 && matterExtraPropertiesDefaultValues.ContainsKey(field.InternalName)) {
                            jw.WriteValue(matterExtraPropertiesDefaultValues[field.InternalName]);
                        }
                        else {
                            jw.WriteValue(field.DefaultValue);
                        }
                        jw.WritePropertyName("description");
                        jw.WriteValue(field.Description);

                        if (field.TypeAsString == "Choice")
                        {
                            jw.WritePropertyName("type");
                            jw.WriteValue(Convert.ToString(((Microsoft.SharePoint.Client.FieldChoice)field).EditFormat));
                            List<string> options = GetChoiceFieldValues(clientContext, field);
                            jw.WritePropertyName("values");
                            jw.WriteStartArray();
                            int optionCounter = 1;

                            foreach (string option in options)
                            {
                                jw.WriteStartObject();
                                jw.WritePropertyName("choiceId");
                                jw.WriteValue(optionCounter);
                                jw.WritePropertyName("choiceValue");
                                jw.WriteValue(option);
                                optionCounter++;
                                jw.WriteEndObject();
                            }
                            jw.WriteEndArray();
                        }
                        else if (field.TypeAsString == "MultiChoice")
                        {
                            jw.WritePropertyName("type");
                            jw.WriteValue(Convert.ToString(((Microsoft.SharePoint.Client.FieldMultiChoice)field).TypeAsString));
                            List<string> options = GetChoiceFieldValues(clientContext, field);
                            jw.WritePropertyName("values");
                            jw.WriteStartArray();
                            int optionCounter = 1;

                            foreach (string option in options)
                            {
                                jw.WriteStartObject();
                                jw.WritePropertyName("choiceId");
                                jw.WriteValue(optionCounter);
                                jw.WritePropertyName("choiceValue");
                                jw.WriteValue(option);
                                optionCounter++;
                                jw.WriteEndObject();
                            }

                            jw.WriteEndArray();
                        }
                        else
                        {
                            jw.WritePropertyName("type");
                            jw.WriteValue(Convert.ToString(field.TypeAsString));
                        }
                        jw.WriteEndObject();
                    }
                }
                jw.WriteEndArray();
                jw.WriteEndObject();
                return sb.ToString();
            }
            catch (Exception ex)
            {
                customLogger.LogError(ex, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, logTables.SPOLogTable);
                throw;
            }
        }
        /// <summary>
        /// This method is to get the choice field values of choice data type
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static List<string> GetChoiceFieldValues(ClientContext clientContext, SharePoint.Client.Field fieldName)
        {
            List<string> fieldList = new List<string>();
            try
            {
                FieldChoice fieldChoice = clientContext.CastTo<FieldChoice>(fieldName);
                clientContext.Load(fieldChoice, f => f.Choices);
                clientContext.ExecuteQuery();
                foreach (string item in fieldChoice.Choices)
                {
                    fieldList.Add(item.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return fieldList;
        }
    }
}
