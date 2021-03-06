﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace XCLCMS.Lib.SysWebSetting
{
    /// <summary>
    /// 获取站点配置
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// 获取所有配置
        /// </summary>
        private static List<XCLNetTools.Entity.KeyValue> GetAllSettings()
        {
            XCLCMS.Data.BLL.SysWebSetting bll = new Data.BLL.SysWebSetting();
            var settingList = bll.GetModelList(string.Format("RecordState='{0}'", XCLCMS.Data.CommonHelper.EnumType.RecordStateEnum.N.ToString()));

            var sysEnv = XCLCMS.Lib.Common.Comm.GetCurrentEnvironment();
            var lst = new List<XCLNetTools.Entity.KeyValue>();
            settingList.ForEach(m =>
            {
                var kv = new XCLNetTools.Entity.KeyValue();
                kv.Key = m.KeyName;
                switch (sysEnv)
                {
                    case Common.Comm.SysEnvironmentEnum.FAT:
                        kv.Value = m.TestKeyValue;
                        break;

                    case Common.Comm.SysEnvironmentEnum.PRD:
                        kv.Value = m.PrdKeyValue;
                        break;

                    case Common.Comm.SysEnvironmentEnum.UAT:
                        kv.Value = m.UATKeyValue;
                        break;

                    case Common.Comm.SysEnvironmentEnum.DEV:
                    default:
                        kv.Value = m.KeyValue;
                        break;
                }
                lst.Add(kv);
            });

            return lst;
        }

        /// <summary>
        /// model形式的配置
        /// </summary>
        public static XCLCMS.Lib.Model.SettingModel SettingModel
        {
            get
            {
                XCLCMS.Lib.Model.SettingModel model = null;

                //先从缓存读取
                if (XCLNetTools.Cache.CacheClass.Exists(Lib.Common.Comm.SettingCacheName))
                {
                    model = XCLNetTools.Cache.CacheClass.GetCache(Lib.Common.Comm.SettingCacheName) as XCLCMS.Lib.Model.SettingModel;
                    if (null != model)
                    {
                        return model;
                    }
                }

                //若缓存中没有，从数据库中读取
                var all = Setting.GetAllSettings();
                if (null == all || all.Count == 0)
                {
                    return null;
                }

                model = new XCLCMS.Lib.Model.SettingModel();
                string propsName = string.Empty;
                XCLNetTools.Entity.KeyValue tempKeyModel = null;
                var props = model.GetType().GetProperties();
                if (null != props && props.Length > 0)
                {
                    for (int i = 0; i < props.Length; i++)
                    {
                        propsName = props[i].Name;
                        tempKeyModel = all.Where(k => string.Equals(k.Key, propsName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (null == tempKeyModel)
                        {
                            throw new Exception(string.Format("配置{0}在数据库中不存在！", propsName));
                        }
                        props[i].SetValue(model, tempKeyModel.Value);
                    }
                }
                XCLNetTools.Cache.CacheClass.SetCache(Lib.Common.Comm.SettingCacheName, model);

                return model;
            }
        }
    }
}