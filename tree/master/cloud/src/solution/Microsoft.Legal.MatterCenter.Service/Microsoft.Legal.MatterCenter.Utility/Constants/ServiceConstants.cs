// ***********************************************************************
// Assembly         : Microsoft.Legal.MatterCenter.Utility
// Author           : v-lapedd
// Created          : 04-07-2016
//
// ***********************************************************************
// <copyright file="ServiceConstants.cs" company="Microsoft">
//     Copyright (c) . All rights reserved.
// </copyright>

// ***********************************************************************

namespace Microsoft.Legal.MatterCenter.Utility
{

    public enum SPORoleIdMapping
    {
        FullControl=1073741829,
        Contribute= 1073741827,
        Read= 1073741826
    }

    /// <summary>
    /// All the constants that are used in matter center will be defined here
    /// </summary>
    public class ServiceConstants
    {
        /// <summary>
        /// Base64 string format for the image
        /// </summary>
        public const string IS_FOLDER_STRUCTURE_PRESENT_FALSE = "FALSE";
        public const string IS_FOLDER_STRUCTURE_PRESENT_TRUE = "TRUE";
        public const string BASE64_IMAGE_FORMAT = "data:image/png;base64, "; //ends with a space
        public const int Matter_ONE_NOTE_LENGTH = 120;
        public const string CLIENT_TERM_PATH = "/";
        public const string CACHE_PERMISSIONS = "Permissions";
        public const string DOUBLE_INVERTED_COMMA = "\"";

        public const string SUCCESS = "200";
        /// <summary>
        /// The underscore constant
        /// </summary>
        public const string UNDER_SCORE = "_";

        /// <summary>
        /// The invalid char regex
        /// </summary>
        public const string INVALID_CHARREGEX = @"[\*\?\|\\\t/:""'<>#{}%~&]";

        /// <summary>
        /// The start end regex
        /// </summary>
        public const string START_END_REGEX = @"^[\. ]|[\. ]$";

        /// <summary>
        /// The invalid rule regex
        /// </summary>
        public const string INVALID_RULE_REGEX = @"\.{2,}";

        /// <summary>
        /// The extra space regex
        /// </summary>
        public const string EXTRA_SPACE_REGEX = " {2,}";

        /// <summary>
        /// The mail address field regular expression
        /// </summary>
        public const string MAIL_ADDRESS_FIELD_REGEX = @"""[^""]*""";

        /// <summary>
        /// The invalid file name regex
        /// </summary>
        public const string INVALID_FILENAME_REGEX = "_fajlovi|.files|-Dateien|_fichiers|_bestanden|_file|_archivos|-filer|_tiedostot|_pliki|_soubory|_elemei|_ficheiros|_arquivos|_dosyalar|_datoteke|_fitxers|_failid|_fails|_bylos|_fajlovi|_fitxategiak$";

        

        #region Azure Cache keys
        /// <summary>
        /// Clients cache key
        /// </summary>
        public const string CACHE_CLIENTS = "Clients";

        /// <summary>
        /// Matter type cache key
        /// </summary>
        public const string CACHE_MATTER_TYPE = "MatterType";

        /// <summary>
        /// Roles cache key
        /// </summary>
        public const string CACHE_ROLES = "Roles";

        /// <summary>
        /// The encoded double quotes
        /// </summary>
        public const string ENCODED_DOUBLE_QUOTES = "&quot;";

        /// <summary>
        /// The Double quote constant
        /// </summary>
        public const string DOUBLE_QUOTE = "\"";

        /// <summary>
        /// The (*) asterisk constant
        /// </summary>
        public const string ASTERISK = "*";

        /// <summary>
        /// The comma  constant
        /// </summary>
        public const string COMMA = ",";

        /// <summary>
        /// The semi colon constant
        /// </summary>
        public const string SEMICOLON = ";";

        /// <summary>
        /// The space constant
        /// </summary>
        public const string SPACE = " ";

        /// <summary>
        /// The Period constant
        /// </summary>
        public const string PERIOD = ".";

        /// <summary>
        /// Horizontal tab constant
        /// </summary>
        public const string HORIZONTAL_TAB = "\t";       

        /// <summary>
        /// The opening bracket constant
        /// </summary>
        public const string OPENING_BRACKET = "(";

        /// <summary>
        /// The backward slash constant
        /// </summary>
        public const string BACKWARD_SLASH = "\\";

        /// <summary>
        /// The forward slash constant
        /// </summary>
        public const string FORWARD_SLASH = "/";

        /// <summary>
        /// The lists constant
        /// </summary>
        public const string LISTS = "Lists";

        /// <summary>
        /// The pipe constant
        /// </summary>
        public const string PIPE = "|";

        /// <summary>
        /// Hash Symbol
        /// </summary>
        public const string SYMBOL_HASH = "#";

        /// <summary>
        /// The @ symbol constant
        /// </summary>
        public const string SYMBOL_AT = "@";

        /// <summary>
        /// The closing bracket constant
        /// </summary>
        public const string CLOSING_BRACKET = ")";

        /// <summary>
        /// The opening angular bracket
        /// </summary>
        public const string OPENING_ANGULAR_BRACKET = "<";

        /// <summary>
        /// The closing angular bracket
        /// </summary>
        public const string CLOSING_ANGULAR_BRACKET = ">";

        /// <summary>
        /// The dollar constant
        /// </summary>
        public const string DOLLAR = "$";

        /// <summary>
        /// The colon constant
        /// </summary>
        public const string COLON = ":";

        /// <summary>
        /// The operator ampersand constant
        /// </summary>
        public const string OPERATOR_AMPERSAND = "&";

        /// <summary>
        /// The ?(question) constant
        /// </summary>
        public const string QUESTION = "?";

        /// <summary>
        /// The OR operator
        /// </summary>
        public const string OPERATOR_OR = "OR";

        /// <summary>
        /// The AND operator
        /// </summary>
        public const string OPERATOR_AND = "AND";

        /// <summary>
        /// Constant required for range operator
        /// </summary>
        public const string OPERATOR_RANGE = "range";


        /// <summary>
        /// Placeholder for minimum value of date for date filtering in range operator
        /// </summary>
        public const string MIN_DATE = "min";

        /// <summary>
        /// Placeholder for maximum value of date for date filtering in range operator
        /// </summary>
        public const string MAX_DATE = "max";

        /// <summary>
        /// Constant required for document library filter condition
        /// </summary>
        public const string DOCUMENT_LIBRARY_FILTER_CONDITION = "or(contentclass:STS_ListItem_DocumentLibrary,contentclass:STS_View)";

        /// <summary>
        /// Constant required for document item filter condition
        /// </summary>
        public const string DOCUMENT_ITEM_FILTER_CONDITION = "or(contentclass:STS_ListItem_DocumentLibrary,contentclass:STS_View)";

        /// <summary>
        /// Open square brace
        /// </summary>
        public const string OPEN_SQUARE_BRACE = "[";

        /// <summary>
        /// The (=)operator equal constant
        /// </summary>
        public const string OPERATOR_EQUAL = "=";

        /// <summary>
        /// Close square brace
        /// </summary>
        public const string CLOSE_SQUARE_BRACE = "]";

        /// <summary>
        /// Opening curly brace
        /// </summary>
        public const string OPENING_CURLY_BRACE = "{";

        /// <summary>
        /// Closing curly brace
        /// </summary>
        public const string CLOSING_CURLY_BRACE = "}";

        /// <summary>
        /// Name of the path field in results returned from SharePoint search
        /// </summary>
        public const string PATH_FIELD_NAME = "Path";

        /// <summary>
        /// Event type information
        /// </summary>
        public const string EVENT_INFORMATION = "INFO";

        /// <summary>
        /// Event type error
        /// </summaryClassName
        public const string EVENT_ERROR = "ERROR";

        /// <summary>
        /// Event type warning
        /// </summary>
        public const string EVENT_WARNING = "WARNING";

        /// <summary>
        /// Exception Message
        /// </summary>
        public const string EXCEPTION_MESSAGE = "EXCEPTION MESSAGE:";

        /// <summary>
        /// Class Name
        /// </summary>
        public const string CLASS_NAME = "CLASS NAME:";

        /// <summary>
        /// Class Name
        /// </summary>
        public const string METHOD_NAME = "METHOD NAME:";

        /// <summary>
        /// Class Name
        /// </summary>
        public const string LINE_NUMBER = "LINE NUMBER:";

        /// <summary>
        /// Default Event ID
        /// </summary>
        public const int DEFAULT_EVENT_ID = 1000;

        /// <summary>
        /// Default Failed to logging code
        /// </summary>
        public const int LOGGING_FAILED_CODE = 2000;

        /// <summary>
        /// Default Failed to logging code
        /// </summary>
        public const string LOGGING_FAILED_MESSAGE = "Failed to Logging";

        /// <summary>
        /// The error code constant
        /// </summary>
        public const string ERROR_CODE = "{0}" + SYMBOL_HASH + PIPE + SYMBOL_HASH + "{1}";

        /// <summary>
        /// The Escape character pattern
        /// </summary>
        public const string ESCAPE_CHARACTER_PATTERN = @"[\a|\b|\f|\n|\r|\t|\v]";

        /// <summary>
        /// Constant required for no pinned matters message
        /// </summary>
        public const string NO_PINNED_MESSAGE = "You do not have any pinned matters/documents";

        /// <summary>
        /// The column name for document GUID
        /// </summary>
        public const string DOCUMENT_GUID_COLUMN_NAME = "UniqueId";

        /// <summary>
        /// Title column for list item
        /// </summary>
        public const string TITLE = "Title";

        /// <summary>
        /// Default layout
        /// </summary>
        public const string DefaultLayout = "ReportCenterLayout";

        /// <summary>
        /// Master Page Gallery
        /// </summary>
        public const string MasterPageGallery = "Master Page Gallery";

        /// <summary>
        /// Top Zone
        /// </summary>
        public const string TOP_ZONE = "TopLeftZone";

        /// <summary>
        /// Header Zone
        /// </summary>
        public const string HEADER_ZONE = "HeaderZone";

        /// <summary>
        /// Footer Zone
        /// </summary>
        public const string FOOTER_ZONE = "FooterZone";

        /// <summary>
        /// Middle left zone
        /// </summary>
        public const string MIDDLE_LEFT_ZONE = "MiddleLeftZone";

        /// <summary>
        /// Right Zone
        /// </summary>
        public const string RIGHT_ZONE = "RightZone";

        /// <summary>
        /// Bottom Left Zone
        /// </summary>
        public const string BOTTOM_ZONE = "BottomLeftZone";

        /// <summary>
        /// Middle Center Zone
        /// </summary>
        public const string MIDDLE_CENTER_ZONE = "MiddleMiddleZone";

        /// <summary>
        /// Links static string
        /// </summary>
        public const string LINKS_STATIC_STRING = "Links";

        /// <summary>
        /// Represents the entity type user in people picker control
        /// </summary>
        public const string PEOPLE_PICKER_ENTITY_TYPE_USER = "User";

        /// <summary>
        /// Calendar Id
        /// </summary>
        public const string CALENDARID = "106";

        /// <summary>
        /// Calendar Name
        /// </summary>
        public const string CALENDAR_NAME = "Calendar";

        /// <summary>
        /// Alternate name for Calendar list
        /// </summary>
        public const string CALENDAR_NAME_ALTERNATE = "Calendar List";

        /// <summary>
        /// Provision Matter CreateMatter validation value
        /// </summary>
        public const string PROVISION_MATTER_CREATEMATTER = "2";

        /// <summary>
        ///Provision Matter common validation value
        /// </summary>
        public const string PROVISION_MATTER_COMMON_VALIDATION = "0";

        /// <summary>
        /// Provision Matter CheckMatterExists validation value
        /// </summary>
        public const string PROVISION_MATTER_CHECK_MATTER_EXISTS = "1";

        

        /// <summary>
        /// Provision Matter AssignUserPermissions value
        /// </summary>
        public const string PROVISION_MATTER_ASSIGN_USER_PERMISSIONS = "3";

        /// <summary>
        /// Provision Matter MatterLandingPage validation value
        /// </summary>
        public const string PROVISIONMATTER_MATTER_LANDING_PAGE = "4";

        /// <summary>
        /// Provision Matter UpdateMetadataForList validation value
        /// </summary>
        public const string ProvisionMatterUpdateMetadataForList = "6";

        /// <summary>
        /// Provision Matter ShareMatter validation value
        /// </summary>
        public const string ProvisionMatterShareMatter = "7";

        /// <summary>
        /// Edit Matter permission validation value
        /// </summary>
        public const string EditMatterPermission = "8";

        /// <summary>
        /// Provision Matter AssignContentType validation value
        /// </summary>
        public const string ProvisionMatterAssignContentType = "5";

        

        /// <summary>
        /// The false constant
        /// </summary>
        public const string FALSE = "false";

        /// <summary>
        /// The false constant
        /// </summary>
        public const string TRUE = "true";

        /// <summary>
        /// The false constant
        /// </summary>
        public const string YES = "Yes";

        /// <summary>
        /// The false constant
        /// </summary>
        public const string NO = "No";

        /// <summary>
        /// Page extension
        /// </summary>
        public const string ASPX_EXTENSION = ".aspx";
        /// <summary>
        /// The service response HTML chunk for role information
        /// </summary>
        public const string RoleInfoHtmlChunk = "<div>{0}: {1}</div>";

        /// <summary>
        /// The HTTPS header constant
        /// </summary>
        public const string HTTPS = "https";

        /// <summary>
        /// The HTTP header constant
        /// </summary>
        public const string HTTP = "http";

        /// <summary>
        /// The string for matter
        /// </summary>
        public const string MATTER = "Matter";

        /// <summary>
        /// Modified date column
        /// </summary>
        public const string MODIFIED_DATE_COLUMN = "Modified";


        /// <summary>
        /// String to be used for creating default value for metadata. This string is in following format: WSSID;#VAL|GUID
        /// </summary>
        public const string MetadataDefaultValue = "{0};#{1}|{2}";
        #endregion

        #region Mail

        /// <summary>
        /// The SOAP envelop URI
        /// </summary>
        public const string SOAP_ENVELOP_URI = "http://schemas.xmlsoap.org/soap/envelope/";

        /// <summary>
        /// The exchange service message
        /// </summary>
        public const string EXCHANGE_SERVICE_MESSAGE = "http://schemas.microsoft.com/exchange/services/2006/messages";

        /// <summary>
        /// Constant required for email file extension
        /// </summary>
        public const string EMAIL_FILE_EXTENSION = ".eml";

        /// <summary>
        /// The mail file extension key
        /// </summary>
        public const string MAIL_FILE_EXTENSION_KEY = "fileExtension";

        /// <summary>
        /// The mail sender key
        /// </summary>
        public const string MAIL_SENDER_KEY = "mailSender";

        /// <summary>
        /// The mail search email subject
        /// </summary>
        public const string MAIL_SEARCH_EMAIL_SUBJECT = "searchEmailSubject";

        /// <summary>
        /// The mail received date key
        /// </summary>
        public const string MAIL_RECEIVED_DATEKEY = "receivedDate";

        /// <summary>
        /// The mail search email from mailbox key
        /// </summary>
        public const string MAIL_SEARCH_EMAIL_FROM_MAIL_BOX_KEY = "searchEmailFromMailbox";

        /// <summary>
        /// The mail receiver key
        /// </summary>
        public const string MAIL_RECEIVER_KEY = "mailReceiver";

        /// <summary>
        /// The mail CC field identifier
        /// </summary>
        public const string MAIL_CC_ADDRESS_KEY = "ccMailAddress";
        
        /// <summary>
        /// The mail categories identifier
        /// </summary>
        public const string MAIL_CATEGORIES_KEY = "categories";

        /// <summary>
        /// The mail sensitivity identifier
        /// </summary>
        public const string MAIL_SENSITIVITY_KEY = "sensitivity";

        /// <summary>
        /// The mail sent date identifier
        /// </summary>
        public const string MAIL_SENT_DATE_KEY = "sentDate";

        /// <summary>
        /// Conversation Id identifier
        /// </summary>
        public const string MAIL_CONVERSATIONID_KEY = "conversationId";

        /// <summary>
        /// Conversation Topic identifier
        /// </summary>
        public const string MAIL_CONVERSATION_TOPIC_KEY = "conversationTopic";

        /// <summary>
        /// Has Attachments identifier
        /// </summary>
        public const string MAIL_HAS_ATTACHMENTS_KEY = "hasAttachments";

        /// <summary>
        /// The mail attachment key
        /// </summary>
        public const string MAIL_ATTACHMENT_KEY = "attachment";

        /// <summary>
        /// The mail importance key
        /// </summary>
        public const string MAIL_IMPORTANCE_KEY = "importance";

        /// <summary>
        /// The mail default importance
        /// </summary>
        public const string MAIL_DEFAULT_IMPORTANCE = "normal";

        /// <summary>
        /// The original name of uploaded mail
        /// </summary>
        public const string MAIL_ORIGINAL_NAME = "originalName";

        /// <summary>
        /// Categories text
        /// </summary>
        public const string CATEGORIES = "category";

       

        /// <summary>
        /// The upload succeeded
        /// </summary>
        public const bool UPLOAD_SUCCEEDED = true;

        /// <summary>
        /// The upload failed
        /// </summary>
        public const bool UPLOAD_FAILED = false;
        /// <summary>
        /// Mail content type
        /// </summary>
        public const string MAIL_CONTENT_TYPE = "text/plain; charset=us-ascii";

        /// <summary>
        /// The mail media sub type
        /// </summary>
        public const string MAIL_MEDIA_SUBTYPE = "plain";

        /// <summary>
        /// The text media main type
        /// </summary>
        public const string TEXT_MEDIA_MAIN_TYPE = "text";

        /// <summary>
        /// The HTML media main type
        /// </summary>
        public const string HtmlMediaMainType = "html";

        /// <summary>
        /// The image media main type
        /// </summary>
        public const string IMAGE_MEDIA_MAIN_TYPE = "image";

        /// <summary>
        /// The application media main type
        /// </summary>
        public const string APPLICATION_MEDIA_MAIN_TYPE = "application";

        /// <summary>
        /// The message media main type
        /// </summary>
        public const string MESSAGE_MEDIA_MAIN_TYPE = "message";

        /// <summary>
        /// The multipart media type
        /// </summary>
        public const string MULTI_PART_MEDIA_TYPE = "MULTIPART/ALTERNATIVE";

        /// <summary>
        /// The string hyphen
        /// </summary>
        public const string HYPHEN = "-";

        /// <summary>
        /// The mail media type
        /// </summary>
        public const string MailMediaType = "text/plain";

        /// <summary>
        /// Constant required for extension for OneNote table of content
        /// </summary>
        public const string EXTENSION_ONENOTE_TABLE_OF_CONTENT = ".onetoc2";

        /// <summary>
        /// Constant required for extension for OneNote table of content
        /// </summary>
        public const string ONE_NOTE_RELATIVE_FILE_PATH = "Open Notebook.onetoc2";

        /// <summary>
        /// The managing attorney value constant
        /// </summary>
        public const string MANAGING_ATTORNEY_VALUE = "Responsible Attorney";

        /// <summary>
        /// Task list template type
        /// </summary>
        public const string TASK_LIST_TEMPLATE_TYPE = "Tasks";

        /// <summary>
        /// Web Query string
        /// </summary>
        public const string WEB_STRING = "?Web=1";

        /// <summary>
        /// Constant required for HTML file type constant
        /// </summary>
        public const string HtmlFileType = "HTML_x0020_File_x0020_Type";

        /// <summary>
        /// Constant required for impersonation claim type trust
        /// </summary>
        public const string TRUSTED_FOR_IMPERSONATION_CLAIMTYPE = "trustedfordelegation";

        /// <summary>
        /// The mail attachment media main type
        /// </summary>
        public const string MAIL_ATTACHMENT_MEDIA_MAINT_YPE = "message";

        /// <summary>
        /// The mail search email from mailbox key
        /// </summary>
        public const string MAIL_SEARCH_EMAIL_FROM_MAILBOX_KEY = "searchEmailFromMailbox";

        /// <summary>
        /// The exchange service types
        /// </summary>
        public const string EXCHANGE_SERVICE_TYPES = "http://schemas.microsoft.com/exchange/services/2006/types";

        /// <summary>
        /// The hexadecimal decoder regular expression
        /// </summary>
        public const string HEX_DECODER_REGEX = "(\\=([0-9A-F][0-9A-F]))";


        /// <summary>
        /// The Document content type in user's OneDrive.
        /// </summary>
        public const string ONEDRIVE_DOCUMENT_CONTENTTYPE = "Document";

        /// <summary>
        /// The Folder content type in user's OneDrive.
        /// </summary>
        public const string ONEDRIVE_FOLDER_CONTENTTYPE = "Folder";

       
        /// <summary>
        /// The property associated with content type
        /// </summary>
        public const string OneDrive_ContentType_Property = "ContentTypeId";

        /// <summary>
        /// The mail message receiver header
        /// </summary>
        public const string Mail_Message_Receiver_Header = "x-receiver";

        /// <summary>
        /// The mail message sender header
        /// </summary>
        public const string Mail_Message_Sender_Header = "x-sender";

        /// <summary>
        /// Event type information
        /// </summary>
        public const string Event_Information = "INFO";

        /// <summary>
        /// Event type error
        /// </summary>
        public const string Event_Error = "ERROR";

        /// <summary>
        /// Event type warning
        /// </summary>
        public const string EventWarning = "WARNING";

        /// <summary>
        /// Exception Message
        /// </summary>
        public const string ExceptionMessage = "EXCEPTION MESSAGE:";

        /// <summary>
        /// Class Name
        /// </summary>
        public const string ClassName = "CLASS NAME:";

        /// <summary>
        /// Class Name
        /// </summary>
        public const string MethodName = "METHOD NAME:";

        /// <summary>
        /// Class Name
        /// </summary>
        public const string LineNumber = "LINE NUMBER:";

        /// <summary>
        /// Default Event ID
        /// </summary>
        public const int DefaultEventId = 1000;

        /// <summary>
        /// Default Failed to logging code
        /// </summary>
        public const int LoggingFailedCode = 2000;

        /// <summary>
        /// Default Failed to logging code
        /// </summary>
        public const string LoggingFailedMessage = "Failed to Logging";


        /// <summary>
        /// The mail SOAP request
        /// </summary>
        public const string MAIL_SOAP_REQUEST =
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                    xmlns:m=""http://schemas.microsoft.com/exchange/services/2006/messages""
                    xmlns:t=""http://schemas.microsoft.com/exchange/services/2006/types"" 
                    xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Header>
                    <t:RequestServerVersion Version=""Exchange2013""/>
                    </soap:Header>
                    <soap:Body>
                    <m:GetItem>
                    <m:ItemShape>
                        <t:BaseShape>Default</t:BaseShape>
                        <t:IncludeMimeContent>true</t:IncludeMimeContent>
                        <t:AdditionalProperties>
		                    <t:FieldURI FieldURI=""item:ConversationId""/>		          
			                <t:FieldURI FieldURI=""message:ConversationTopic""/>			  
        		        </t:AdditionalProperties>
                    </m:ItemShape>
                    <m:ItemIds>
                    <t:ItemId Id=""{0}""/>
                    </m:ItemIds>
                    </m:GetItem>
                    </soap:Body>
                    </soap:Envelope>";

        /// <summary>
        /// The attachment SOAP request
        /// </summary>
        public const string ATTACHMENT_SOAP_REQUEST =
                     @"<?xml version=""1.0"" encoding=""utf-8""?>
                     <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                     xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                     xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
                     xmlns:t=""http://schemas.microsoft.com/exchange/services/2006/types"">
                     <soap:Header>
                     <t:RequestServerVersion Version=""Exchange2013"" />
                     </soap:Header>
                     <soap:Body>
                     <GetAttachment xmlns=""http://schemas.microsoft.com/exchange/services/2006/messages""
                      xmlns:t=""http://schemas.microsoft.com/exchange/services/2006/types"">
                     <AttachmentShape>
                     <t:IncludeMimeContent>true</t:IncludeMimeContent>
                        <t:AdditionalProperties>
		                    <t:FieldURI FieldURI=""item:ConversationId""/>		                    
			                <t:FieldURI FieldURI=""message:ConversationTopic""/>			                
        		        </t:AdditionalProperties>
                     </AttachmentShape>
                     <AttachmentIds>
                     <t:AttachmentId Id=""{0}""/>
                     </AttachmentIds>
                     </GetAttachment>
                     </soap:Body>
                    </soap:Envelope>";


        #endregion
        

        /// <summary>
        /// The Attachments tag in Exchange response
        /// </summary>
        public const string ATTACHMENTS_TAG = "Attachments";

        /// <summary>
        /// The FileAttachment tag in Exchange response
        /// </summary>
        public const string FILE_ATTACHMENT_TAG = "FileAttachment";

        /// <summary>
        /// The ItemAttachment tag in Exchange response
        /// </summary>
        public const string ITEM_ATTACHMENT_TAG = "ItemAttachment";

        /// <summary>
        /// The Name tag in Exchange response
        /// </summary>
        public const string FILE_NAME_TAG = "Name";

        /// <summary>
        /// The AttachmentID tag in Exchange response
        /// </summary>
        public const string ATTACHMENT_ID_TAG = "AttachmentId";

        /// <summary>
        /// The Id attribute in Exchange response
        /// </summary>
        public const string ID_ATTRIBUTE = "Id";

        /// <summary>
        /// The name of the HTTP header for web request
        /// </summary>
        public const string REQUEST_HEADER_NAME = "Authorization";

        /// <summary>
        /// The value of the HTTP header for web request
        /// </summary>
        public const string REQUEST_HEADER_VALUE = "Bearer {0}";

        /// <summary>
        /// The Method for web request
        /// </summary>
        public const string REQUEST_METHOD = "POST";

        /// <summary>
        /// The value of the Content-type HTTP header for web request
        /// </summary>
        public const string REQUEST_CONTENT_TYPE = "text/xml; charset=utf-8";
        /// <summary>
        /// The mail attributes
        /// </summary>
        public static class MailAttributes
        {
            /// <summary>
            /// The BCC mail attribute
            /// </summary>
            public const string BCC = "BCC";

            /// <summary>
            /// The cc mail attribute
            /// </summary>
            public const string CC = "CC";

            /// <summary>
            /// The attachment mail attribute
            /// </summary>
            public const string ATTACHMENT = "ATTACHMENT";

            /// <summary>
            /// The content description
            /// </summary>
            public const string CONTENT_DESCRIPTION = "CONTENT-DESCRIPTION";

            /// <summary>
            /// The content disposition
            /// </summary>
            public const string CONTENT_DISPOSITION = "CONTENT-DISPOSITION";

            /// <summary>
            /// The content identifier
            /// </summary>
            public const string CONTENT_ID = "CONTENT-ID";

            /// <summary>
            /// The content transfer encoding
            /// </summary>
            public const string CONTENT_TRANSFER_ENCODING = "CONTENT-TRANSFER-ENCODING";

            /// <summary>
            /// The content type mail attribute
            /// </summary>
            public const string CONTENT_TYPE = "CONTENT-TYPE";

            /// <summary>
            /// The date mail attribute
            /// </summary>
            public const string DATE = "DATE";

            /// <summary>
            /// From mail attribute
            /// </summary>
            public const string FROM = "FROM";

            /// <summary>
            /// The sender mail attribute
            /// </summary>
            public const string SENDER = "SENDER";

            /// <summary>
            /// The subject mail attribute
            /// </summary>
            public const string SUBJECT = "SUBJECT";

            /// <summary>
            /// To mail attribute
            /// </summary>
            public const string TO = "TO";

            /// <summary>
            /// The filename mail attribute
            /// </summary>
            public const string FILENAME = "FILENAME";

            /// <summary>
            /// The size mail attribute
            /// </summary>
            public const string SIZE = "SIZE";

            /// <summary>
            /// The creation date mail attribute
            /// </summary>
            public const string CREATION_DATE = "CREATION-DATE";

            /// <summary>
            /// The modification date mail attribute
            /// </summary>
            public const string MODIFICATION_DATE = "MODIFICATION-DATE";

            /// <summary>
            /// The read date mail attribute
            /// </summary>
            public const string READ_DATE = "READ-DATE";

            /// <summary>
            /// The 7biT encoding
            /// </summary>
            public const string BIT_7 = "7BIT";

            /// <summary>
            /// The 8biT encoding
            /// </summary>
            public const string BIT_8 = "8BIT";

            /// <summary>
            /// The quoted printable
            /// </summary>
            public const string QUOTED_PRINTABLE = "QUOTED-PRINTABLE";

            /// <summary>
            /// The base 64 encoding
            /// </summary>
            public const string BASE64 = "BASE64";

            /// <summary>
            /// The importance mail attribute
            /// </summary>
            public const string IMPORTANCE = "IMPORTANCE";

            /// <summary>
            /// The mail categories attribute
            /// </summary>
            public const string CATEGORIES = "KEYWORDS";

            /// <summary>
            /// The mail received attribute
            /// </summary>
            public const string RECEIVED = "RECEIVED";

            
        }

        /// <summary>
        /// The original name of uploaded mail
        /// </summary>
        public const string MailOriginalName = "originalName";

        #region Matter Landing Page Constants

        /// <summary>
        /// Zone index to be used while creating matter page
        /// </summary>
        public const int ZONE_INDEX = 1;

        #region Web Part Constants
        /// <summary>
        /// XML definition of the left web part
        /// </summary>
        public const string CONTENT_EDITOR_WEB_PART = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <WebPart xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.microsoft.com/WebPart/v2"">
                      <Title>Content Editor</Title>
                      <FrameType>None</FrameType>
                      <Description>Allows authors to enter rich text content.</Description>
                      <IsIncluded>true</IsIncluded>
                      <ZoneID>MiddleLeftZone</ZoneID>
                      <PartOrder>0</PartOrder>
                      <FrameState>Normal</FrameState>
                      <Height />
                      <Width />
                      <AllowRemove>true</AllowRemove>
                      <AllowZoneChange>true</AllowZoneChange>
                      <AllowMinimize>true</AllowMinimize>
                      <AllowConnect>true</AllowConnect>
                      <AllowEdit>true</AllowEdit>
                      <AllowHide>true</AllowHide>
                      <IsVisible>true</IsVisible>
                      <DetailLink />
                      <HelpLink />
                      <HelpMode>Modeless</HelpMode>
                      <Dir>Default</Dir>
                      <PartImageSmall />
                      <MissingAssembly>Cannot import this Web Part.</MissingAssembly>
                      <PartImageLarge>/_layouts/15/images/mscontl.gif</PartImageLarge>
                      <IsIncludedFilter />
                      <Assembly>Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c</Assembly>
                      <TypeName>Microsoft.SharePoint.WebPartPages.ContentEditorWebPart</TypeName>
                      <ContentLink xmlns=""http://schemas.microsoft.com/WebPart/v2/ContentEditor"" />
                      <Content xmlns=""http://schemas.microsoft.com/WebPart/v2/ContentEditor"">
                        <![CDATA[
                            {0}
                          ]]>
                      </Content>
                      <PartStorage xmlns=""http://schemas.microsoft.com/WebPart/v2/ContentEditor"" />
                    </WebPart>";

        /// <summary>
        /// XML definition of the RSS feed web part
        /// </summary>
        public const string RSS_FEED_WEB_PART = @"<webParts>
                  <webPart xmlns=""http://schemas.microsoft.com/WebPart/v3"">
                    <metaData>
                      <type name=""Microsoft.SharePoint.Portal.WebControls.RSSAggregatorWebPart, Microsoft.SharePoint.Portal, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"" />
                      <importErrorMessage>Cannot import this web part.</importErrorMessage>
                    </metaData>
                    <data>
                      <properties>
                        <property name=""InitialAsyncDataFetch"" type=""bool"">False</property>
                        <property name=""ChromeType"" type=""chrometype"">None</property>
                        <property name=""ListName"" type=""string"" null=""true"" />
                        <property name=""Height"" type=""string"" />
                        <property name=""CacheXslStorage"" type=""bool"">True</property>
                        <property name=""Default"" type=""string"" />
                        <property name=""ParameterBindings"" type=""string"">&lt;ParameterBinding Name=""RequestUrl"" Location=""WPProperty[FeedUrl]""/&gt;</property>
                        <property name=""AllowZoneChange"" type=""bool"">True</property>
                        <property name=""AutoRefresh"" type=""bool"">False</property>
                        <property name=""XmlDefinitionLink"" type=""string"" />
                        <property name=""DataFields"" type=""string"" />
                        <property name=""FeedLimit"" type=""int"">5</property>
                        <property name=""Hidden"" type=""bool"">False</property>
                        <property name=""NoDefaultStyle"" type=""string"" />
                        <property name=""XslLink"" type=""string"" null=""true"" />
                        <property name=""ViewFlag"" type=""string"">0</property>
                        <property name=""CatalogIconImageUrl"" type=""string"" />
                        <property name=""CacheXslTimeOut"" type=""int"">600</property>
                        <property name=""AutoRefreshInterval"" type=""int"">60</property>
                        <property name=""AllowConnect"" type=""bool"">True</property>
                        <property name=""FeedUrl"" type=""string"">http://www.bing.com/search?q={0}&amp;format=rss</property>
                        <property name=""AllowClose"" type=""bool"">True</property>
                        <property name=""ShowWithSampleData"" type=""bool"">False</property>       
                        <property name=""EnableOriginalValue"" type=""bool"">False</property>
                        <property name=""ExpandFeed"" type=""bool"">False</property>
                        <property name=""ListUrl"" type=""string"" null=""true"" />
                        <property name=""DataSourceID"" type=""string"" />
                        <property name=""FireInitialRow"" type=""bool"">True</property>
                        <property name=""ManualRefresh"" type=""bool"">False</property>
                        <property name=""ViewFlags"" type=""Microsoft.SharePoint.SPViewFlags, Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"">None</property>
                        <property name=""ChromeState"" type=""chromestate"">Normal</property>
                        <property name=""AllowHide"" type=""bool"">True</property>
                        <property name=""ListDisplayName"" type=""string"" null=""true"" />
                        <property name=""SampleData"" type=""string"" null=""true"" />
                        <property name=""AsyncRefresh"" type=""bool"">False</property>
                        <property name=""Direction"" type=""direction"">NotSet</property>
                        <property name=""Title"" type=""string"">RSS Viewer</property>
                        <property name=""ListId"" type=""System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">00000000-0000-0000-0000-000000000000</property>
                        <property name=""Description"" type=""string"">Displays an RSS feed.</property>
                        <property name=""AllowMinimize"" type=""bool"">True</property>
                        <property name=""TitleUrl"" type=""string"" />
                        <property name=""DataSourcesString"" type=""string"">
                        &lt;%@ Register TagPrefix=""WebControls"" Namespace=""Microsoft.SharePoint.WebControls"" Assembly=""Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"" %&gt;
                        &lt;%@ Register TagPrefix=""WebPartPages"" Namespace=""Microsoft.SharePoint.WebPartPages"" Assembly=""Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"" %&gt;
                        &lt;WebControls:XmlUrlDataSource runat=""server"" AuthType=""None"" HttpMethod=""GET""&gt;
                        &lt;DataFileParameters&gt;
                        &lt;WebPartPages:DataFormParameter Name=""RequestUrl"" ParameterKey=""RequestUrl"" PropertyName=""ParameterValues""/&gt;
                        &lt;/DataFileParameters&gt;
                        &lt;/WebControls:XmlUrlDataSource&gt;</property>
                        <property name=""DisplayName"" type=""string"" />
                        <property name=""PageType"" type=""Microsoft.SharePoint.PAGETYPE, Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"">PAGE_NORMALVIEW</property>
                        <property name=""Width"" type=""string"" />
                        <property name=""AllowEdit"" type=""bool"">True</property>
                        <property name=""ExportMode"" type=""exportmode"">All</property>
                        <property name=""CacheRefreshTimeInMins"" type=""int"">120</property>
                        <property name=""PageSize"" type=""int"">-1</property>
                        <property name=""ViewContentTypeId"" type=""string"" />
                        <property name=""HelpUrl"" type=""string"" />
                        <property name=""XmlDefinition"" type=""string"" />
                        <property name=""UseSQLDataSourcePaging"" type=""bool"">True</property>
                        <property name=""TitleIconImageUrl"" type=""string"" />
                        <property name=""MissingAssembly"" type=""string"">Cannot import this web part.</property>
                        <property name=""HelpMode"" type=""helpmode"">Modeless</property>
                      </properties>
                    </data>
                  </webPart>
                </webParts>";
        /// <summary>
        /// XML definition of the list view web part
        /// </summary>
        public const string LIST_VIEW_WEBPART = @"
                <webParts>
                    <webPart xmlns=""http://schemas.microsoft.com/WebPart/v3"">
                        <metaData>
                            <type name=""Microsoft.SharePoint.WebPartPages.XsltListViewWebPart, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"" />
                            <importErrorMessage>Cannot import this Web Part.</importErrorMessage>
                        </metaData>
                        <data>
                            <properties>
                                <property name=""ShowWithSampleData"" type=""bool"">False</property>
                                <property name=""Default"" type=""string"" />
                                <property name=""NoDefaultStyle"" type=""string"" null=""true"" />
                                <property name=""CacheXslStorage"" type=""bool"">True</property>
                                <property name=""ViewContentTypeId"" type=""string"" />
                                <property name=""XmlDefinitionLink"" type=""string"" />
                                <property name=""ManualRefresh"" type=""bool"">False</property>
                                <property name=""ListUrl"" type=""string"" />
                                <property name=""ListId"" type=""System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">{0}</property>
                                <property name=""TitleUrl"" type=""string"">{1}</property>
                                <property name=""EnableOriginalValue"" type=""bool"">False</property>
                                <property name=""Direction"" type=""direction"">NotSet</property>
                                <property name=""ServerRender"" type=""bool"">False</property>
                                <property name=""ViewFlags"" type=""Microsoft.SharePoint.SPViewFlags, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"">Html, TabularView, Hidden, Mobile</property>
                                <property name=""AllowConnect"" type=""bool"">True</property>
                                <property name=""ListName"" type=""string"">{2}</property>
                                <property name=""ListDisplayName"" type=""string"" />
                                <property name=""AllowZoneChange"" type=""bool"">True</property>
                                <property name=""ChromeState"" type=""chromestate"">Normal</property>
                                <property name=""DisableSaveAsNewViewButton"" type=""bool"">False</property>
                                <property name=""ViewFlag"" type=""string"" />
                                <property name=""DataSourceID"" type=""string"" />
                                <property name=""ExportMode"" type=""exportmode"">All</property>
                                <property name=""AutoRefresh"" type=""bool"">False</property>
                                <property name=""FireInitialRow"" type=""bool"">True</property>
                                <property name=""AllowEdit"" type=""bool"">True</property>
                                <property name=""Description"" type=""string"" />
                                <property name=""HelpMode"" type=""helpmode"">Modeless</property>
                                <property name=""BaseXsltHashKey"" type=""string"" null=""true"" />
                                <property name=""AllowMinimize"" type=""bool"">True</property>
                                <property name=""CacheXslTimeOut"" type=""int"">86400</property>
                                <property name=""ChromeType"" type=""chrometype"">None</property>
                                <property name=""Xsl"" type=""string"" null=""true"" />
                                <property name=""JSLink"" type=""string"" null=""true"" />
                                <property name=""CatalogIconImageUrl"" type=""string"">/_layouts/15/images/itdl.png?rev=33</property>
                                <property name=""SampleData"" type=""string"" null=""true"" />
                                <property name=""UseSQLDataSourcePaging"" type=""bool"">True</property>
                                <property name=""TitleIconImageUrl"" type=""string"" />
                                <property name=""PageSize"" type=""int"">-1</property>
                                <property name=""ShowTimelineIfAvailable"" type=""bool"">True</property>
                                <property name=""Width"" type=""string"" >750px</property>
                                <property name=""DataFields"" type=""string"" />
                                <property name=""Hidden"" type=""bool"">False</property>
                                <property name=""Title"" type=""string"" />
                                <property name=""PageType"" type=""Microsoft.SharePoint.PAGETYPE, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"">PAGE_NORMALVIEW</property>
                                <property name=""DataSourcesString"" type=""string"" />
                                <property name=""AllowClose"" type=""bool"">True</property>
                                <property name=""InplaceSearchEnabled"" type=""bool"">True</property>
                                <property name=""WebId"" type=""System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">00000000-0000-0000-0000-000000000000</property>
                                <property name=""Height"" type=""string"" />
                                <property name=""GhostedXslLink"" type=""string"">main.xsl</property>
                                <property name=""DisableViewSelectorMenu"" type=""bool"">False</property>
                                <property name=""DisplayName"" type=""string"" />
                                <property name=""IsClientRender"" type=""bool"">False</property>
                                <property name=""XmlDefinition"" type=""string"">&lt;View Name=""{3}"" MobileView=""TRUE"" Type=""HTML"" Hidden=""TRUE"" DisplayName="" "" Url=""{4}"" Level=""1"" BaseViewID=""1"" ContentTypeID=""0x"" ImageUrl=""/_layouts/15/images/dlicon.png?rev=37"" &gt;&lt;Query&gt;&lt;OrderBy&gt;&lt;FieldRef Name=""FileLeafRef""/&gt;&lt;/OrderBy&gt;&lt;/Query&gt;&lt;ViewFields&gt;&lt;FieldRef Name=""DocIcon""/&gt;&lt;FieldRef Name=""LinkFilename""/&gt;&lt;FieldRef Name=""Modified""/&gt;&lt;FieldRef Name=""Editor""/&gt;&lt;/ViewFields&gt;&lt;RowLimit Paged=""TRUE""&gt;30&lt;/RowLimit&gt;&lt;JSLink&gt;clienttemplates.js&lt;/JSLink&gt;&lt;XslLink Default=""TRUE""&gt;main.xsl&lt;/XslLink&gt;&lt;Toolbar Type=""Standard""/&gt;&lt;/View&gt;</property>
                                <property name=""InitialAsyncDataFetch"" type=""bool"">False</property>
                                <property name=""AllowHide"" type=""bool"">True</property>
                                <property name=""ParameterBindings"" type=""string"">
                                    &lt;ParameterBinding Name=""dvt_sortdir"" Location=""Postback;Connection""/&gt;
                                    &lt;ParameterBinding Name=""dvt_sortfield"" Location=""Postback;Connection""/&gt;
                                    &lt;ParameterBinding Name=""dvt_startposition"" Location=""Postback"" DefaultValue="" ""/&gt;
                                    &lt;ParameterBinding Name=""dvt_firstrow"" Location=""Postback;Connection""/&gt;
                                    &lt;ParameterBinding Name=""OpenMenuKeyAccessible"" Location=""Resource(wss,OpenMenuKeyAccessible)"" /&gt;
                                    &lt;ParameterBinding Name=""open_menu"" Location=""Resource(wss,open_menu)"" /&gt;
                                    &lt;ParameterBinding Name=""select_deselect_all"" Location=""Resource(wss,select_deselect_all)"" /&gt;
                                    &lt;ParameterBinding Name=""idPresEnabled"" Location=""Resource(wss,idPresEnabled)"" /&gt;&lt;ParameterBinding Name=""NoAnnouncements"" Location=""Resource(wss,noitemsinview_doclibrary)"" /&gt;&lt;ParameterBinding Name=""NoAnnouncementsHowTo"" Location=""Resource(wss,noitemsinview_doclibrary_howto2)"" /&gt;</property>
                                <property name=""DataSourceMode"" type=""Microsoft.SharePoint.WebControls.SPDataSourceMode, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"">List</property>
                                <property name=""AutoRefreshInterval"" type=""int"">60</property>
                                <property name=""AsyncRefresh"" type=""bool"">False</property>
                                <property name=""HelpUrl"" type=""string"" />
                                <property name=""MissingAssembly"" type=""string"">Cannot import this Web Part.</property>
                                <property name=""XslLink"" type=""string"" null=""true"" />
                                <property name=""SelectParameters"" type=""string"" />
                                <property name=""HasClientDataSource"" type=""bool"">False</property>
                            </properties>
                        </data>
                    </webPart>
                </webParts>";


        

        /// <summary>
        /// Section to be appended on the page
        /// </summary>
        public const string MATTER_LANDING_SECTION_CONTENT = "<div id=\"{0}\"></div>";

        /// <summary>
        /// Style tag
        /// </summary>
        public const string STYLE_TAG = "<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" />";

        /// <summary>
        /// Script tag
        /// </summary>
        public const string SCRIPT_TAG = "<script src=\"{0}\" type=\"text/javascript\"></script>";

        /// <summary>
        /// Script tag with contents
        /// </summary>
        public const string SCRIPT_TAG_WITH_CONTENTS = "<script type=\"text/javascript\">{0}</script>";

        /// <summary>
        /// Matter landing stamp properties
        /// </summary>
        public const string MATTER_LANDING_STAMP_PROPERTIES = "var documentLibraryName = \"{0}\", isNewMatterLandingPage = true, documentLibraryGUID=\"{1}\", matterExtraProperties=\"{2}\";";
        #endregion



        #endregion
    }
}
