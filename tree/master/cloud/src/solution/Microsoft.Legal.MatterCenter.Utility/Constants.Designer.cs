﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Legal.MatterCenter.Utility {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Constants {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Constants() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Legal.MatterCenter.Utility.Constants", typeof(Constants).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to perform content check.
        /// </summary>
        internal static string Content_Check_Failed {
            get {
                return ResourceManager.GetString("Content_Check_Failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Full Control.
        /// </summary>
        internal static string Edit_Matter_Allowed_Permission_Level {
            get {
                return ResourceManager.GetString("Edit_Matter_Allowed_Permission_Level", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to AS3FFCR7G2F8RC1F.
        /// </summary>
        internal static string Encryption_Vector {
            get {
                return ResourceManager.GetString("Encryption_Vector", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We can&apos;t upload folders or empty files.
        /// </summary>
        internal static string Error_Empty_File {
            get {
                return ResourceManager.GetString("Error_Empty_File", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;span&gt;The file &lt;a class=&quot;overWriteContent&quot; id=&quot;fileName&quot; href=&quot;{1}&quot; target=&quot;_blank&quot;&gt;{0}&lt;/a&gt; already exists in the folder. Do you want to overwrite it?&lt;/span&gt;.
        /// </summary>
        internal static string File_Already_Exist_Message {
            get {
                return ResourceManager.GetString("File_Already_Exist_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;span&gt;The file &lt;a class=&quot;overWriteContent&quot; id=&quot;fileName&quot; href=&quot;{1}&quot; target=&quot;_blank&quot;&gt; {0} &lt;/a&gt; might be a duplicate of a similar existing document in the folder. You can run a content check for a full text comparison, save the file without a comparison, or cancel the upload.&lt;br/&gt;
        ///Note: Depending on the file size, a content check may take some time and you can&apos;t navigate away from this screen until the check is complete.&lt;/span&gt;.
        /// </summary>
        internal static string File_Potential_Duplicate_Message {
            get {
                return ResourceManager.GetString("File_Potential_Duplicate_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The folder &lt;span class=&quot;overWriteContent&quot;&gt;{0}&lt;/span&gt; no longer exists in the Matter Library. Reload the pop up to refresh the folder structure..
        /// </summary>
        internal static string Folder_Structure_Modified {
            get {
                return ResourceManager.GetString("Folder_Structure_Modified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The document you are trying to upload has identical content and the same file name as an existing file..
        /// </summary>
        internal static string Found_Identical_Content_Message {
            get {
                return ResourceManager.GetString("Found_Identical_Content_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The two documents do not have identical content..
        /// </summary>
        internal static string Found_Non_Identical_Content_Message {
            get {
                return ResourceManager.GetString("Found_Non_Identical_Content_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;View Scope=&apos;RecursiveAll&apos;&gt;&lt;Query&gt;&lt;Where&gt;&lt;Eq&gt;&lt;FieldRef Name=&apos;FileRef&apos; /&gt;&lt;Value Type=&apos;File&apos;&gt;{0}&lt;/Value&gt;&lt;/Eq&gt;&lt;/Where&gt;&lt;/Query&gt;&lt;/View&gt;.
        /// </summary>
        internal static string Get_All_Files_In_Folder_Query {
            get {
                return ResourceManager.GetString("Get_All_Files_In_Folder_Query", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://MyTenant.sharepoint.com/sites/catalog/MatterCenter.
        /// </summary>
        internal static string Provision_Matter_App_URL {
            get {
                return ResourceManager.GetString("Provision_Matter_App_URL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0},ssl=true,password={1}.
        /// </summary>
        internal static string Redis_Cache_Connection_String {
            get {
                return ResourceManager.GetString("Redis_Cache_Connection_String", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DMS_Attachments.
        /// </summary>
        internal static string Search_Email_Attachments {
            get {
                return ResourceManager.GetString("Search_Email_Attachments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EmailCategories.
        /// </summary>
        internal static string Search_Email_Categories {
            get {
                return ResourceManager.GetString("Search_Email_Categories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DMS_Cc.
        /// </summary>
        internal static string Search_Email_CC {
            get {
                return ResourceManager.GetString("Search_Email_CC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ConversationId.
        /// </summary>
        internal static string Search_Email_ConversationId {
            get {
                return ResourceManager.GetString("Search_Email_ConversationId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ConversationTopic.
        /// </summary>
        internal static string Search_Email_ConversationTopic {
            get {
                return ResourceManager.GetString("Search_Email_ConversationTopic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File_x0020_Size.
        /// </summary>
        internal static string Search_Email_FileSize {
            get {
                return ResourceManager.GetString("Search_Email_FileSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DMS_From.
        /// </summary>
        internal static string Search_Email_From {
            get {
                return ResourceManager.GetString("Search_Email_From", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DMS_From_MailBox.
        /// </summary>
        internal static string Search_Email_From_Mailbox {
            get {
                return ResourceManager.GetString("Search_Email_From_Mailbox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HasAttachments.
        /// </summary>
        internal static string Search_Email_HasAttachments {
            get {
                return ResourceManager.GetString("Search_Email_HasAttachments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DMS_Mail_Importance.
        /// </summary>
        internal static string Search_Email_Importance {
            get {
                return ResourceManager.GetString("Search_Email_Importance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OriginalName.
        /// </summary>
        internal static string Search_Email_OriginalName {
            get {
                return ResourceManager.GetString("Search_Email_OriginalName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ReceivedDate.
        /// </summary>
        internal static string Search_Email_ReceivedDate {
            get {
                return ResourceManager.GetString("Search_Email_ReceivedDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sensitivity.
        /// </summary>
        internal static string Search_Email_Sensitivity {
            get {
                return ResourceManager.GetString("Search_Email_Sensitivity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SentDate.
        /// </summary>
        internal static string Search_Email_SentDate {
            get {
                return ResourceManager.GetString("Search_Email_SentDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DMS_Subject.
        /// </summary>
        internal static string Search_Email_Subject {
            get {
                return ResourceManager.GetString("Search_Email_Subject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DMS_To.
        /// </summary>
        internal static string Search_Email_To {
            get {
                return ResourceManager.GetString("Search_Email_To", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SendMail.
        /// </summary>
        internal static string Send_Mail_List_Name {
            get {
                return ResourceManager.GetString("Send_Mail_List_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 2.
        /// </summary>
        internal static string Sent_Date_Tolerance {
            get {
                return ResourceManager.GetString("Sent_Date_Tolerance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [@\\/:*?#%&lt;&gt;{}|~&amp;\&quot;].
        /// </summary>
        internal static string Special_Character_Expression_Content_Type {
            get {
                return ResourceManager.GetString("Special_Character_Expression_Content_Type", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [A-Za-z0-9_]+[-A-Za-z0-9_, .]*.
        /// </summary>
        internal static string Special_Character_Expression_Matter_Description {
            get {
                return ResourceManager.GetString("Special_Character_Expression_Matter_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [A-Za-z0-9_]+[-A-Za-z0-9_, ]*.
        /// </summary>
        internal static string Special_Character_Expression_Matter_Id {
            get {
                return ResourceManager.GetString("Special_Character_Expression_Matter_Id", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [A-Za-z0-9-_]+[A-Za-z0-9-_ ]*.
        /// </summary>
        internal static string Special_Character_Expression_Matter_Title {
            get {
                return ResourceManager.GetString("Special_Character_Expression_Matter_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -900000011.
        /// </summary>
        internal static string TokenRequestFailedErrorCode {
            get {
                return ResourceManager.GetString("TokenRequestFailedErrorCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Token request failed.
        /// </summary>
        internal static string TokenRequestFailedErrorMessage {
            get {
                return ResourceManager.GetString("TokenRequestFailedErrorMessage", resourceCulture);
            }
        }
    }
}
