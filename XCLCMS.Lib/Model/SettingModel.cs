﻿namespace XCLCMS.Lib.Model
{
    /// <summary>
    /// 站点配置model
    /// </summary>
    public class SettingModel
    {
        public string Common_DESKey { get; set; }
        public string Common_PwdKey { get; set; }
        public string Common_UserDefaultPwd { get; set; }
        public string Common_UserLoginFlagName { get; set; }
        public string Common_WebAPIServiceURL { get; set; }

        public string Admin_LogOutURL { get; set; }
        public string Admin_LogOnURL { get; set; }

        public string FileManager_UploadPath { get; set; }
        public string FileManager_UploadPathTemp { get; set; }
        public string FileManager_RootURL { get; set; }
        public string FileManager_FileListURL { get; set; }
        public string FileManager_FileUploadURL { get; set; }
        public string FileManager_LogicFileListURL { get; set; }
        public string FileManager_OpenAPI_ShowAttachmentURL { get; set; }
    }
}