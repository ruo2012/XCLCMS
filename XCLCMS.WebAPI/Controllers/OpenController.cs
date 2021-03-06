﻿using System.Threading.Tasks;
using System.Web.Http;
using XCLCMS.Data.WebAPIEntity;

namespace XCLCMS.WebAPI.Controllers
{
    /// <summary>
    /// 开放的API
    /// </summary>
    public class OpenController : BaseAPIController
    {
        private XCLCMS.Data.BLL.UserInfo userInfoBLL = new XCLCMS.Data.BLL.UserInfo();

        /// <summary>
        /// 登录检查
        /// </summary>
        [HttpPost]
        public async Task<APIResponseEntity<XCLCMS.Data.Model.UserInfo>> LogonCheck([FromBody] APIRequestEntity<XCLCMS.Data.WebAPIEntity.RequestEntity.Open.LogonCheckEntity> request)
        {
            return await Task.Run(() =>
            {
                var response = new APIResponseEntity<XCLCMS.Data.Model.UserInfo>();

                var userModel = userInfoBLL.GetModel(request.Body.UserName, XCLCMS.Lib.Encrypt.EncryptHelper.EncryptStringMD5(request.Body.Pwd));
                if (null == userModel)
                {
                    response.Message = string.Format("用户名或密码不正确！", request.Body.UserName);
                    response.IsSuccess = false;
                }
                else if (!string.Equals(userModel.UserState, XCLCMS.Data.CommonHelper.EnumType.UserStateEnum.N.ToString()))
                {
                    response.Message = string.Format("用户名{0}已被禁用！", request.Body.UserName);
                    response.IsSuccess = false;
                }
                else
                {
                    response.Body = userModel;
                    response.IsSuccess = true;
                }

                //写入日志
                XCLNetLogger.Log.WriteLog(new XCLNetLogger.Model.LogModel()
                {
                    LogType = XCLCMS.Data.CommonHelper.EnumType.LogTypeEnum.LOGIN.ToString(),
                    LogLevel = XCLNetLogger.Config.LogConfig.LogLevel.INFO,
                    Title = string.Format("用户{0}，尝试登录系统{1}", request.Body.UserName, response.IsSuccess ? "成功" : "失败")
                });

                return response;
            });
        }
    }
}