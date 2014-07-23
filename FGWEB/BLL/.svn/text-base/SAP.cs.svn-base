using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SAP.Middleware.Connector;
using System.IO;

namespace PDMAPI
{
    using Domain;
    using System.ComponentModel;

    //登陆SAP前的准备工作
    public class MyBackendConfig : IDestinationConfiguration
    {
        public RfcConfigParameters GetParameters(String destinationName)
        {
            if ("PRD_000".Equals(destinationName))
            {
                RfcConfigParameters parms = new RfcConfigParameters();
                parms.Add(RfcConfigParameters.AppServerHost, "10.86.87.185");   //SAP主机IP
                parms.Add(RfcConfigParameters.SAPRouter, "/H/218.75.72.116/H/");   //SAP主机IP
                parms.Add(RfcConfigParameters.SystemNumber, "00");  //SAP实例
                parms.Add(RfcConfigParameters.User, "MESRFC");  //用户名
                parms.Add(RfcConfigParameters.Password, "Evun123");  //密码
                parms.Add(RfcConfigParameters.Client, "100");  // Client
                parms.Add(RfcConfigParameters.Language, "ZH");  //登陆语言
                return parms;

                //RfcConfigParameters parms = new RfcConfigParameters();
                //parms.Add(RfcConfigParameters.AppServerHost, "10.86.87.181");   //SAP主机IP
                //parms.Add(RfcConfigParameters.SAPRouter, "/H/218.75.72.116/H/");   //SAP主机IP
                //parms.Add(RfcConfigParameters.SystemNumber, "00");  //SAP实例
                //parms.Add(RfcConfigParameters.User, "Liangxj");  //用户名
                //parms.Add(RfcConfigParameters.Password, "lxj123");  //密码
                //parms.Add(RfcConfigParameters.Client, "101");  // Client
                //parms.Add(RfcConfigParameters.Language, "ZH");  //登陆语言
                //return parms;
            }
            else return null;
        }
        public bool ChangeEventsSupported()
        {
            return false;
        }
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;


        public  static bool IsAlive = false;

        private static RfcDestination _RfcDest = null;
        /// <summary>
        /// 获取验证票
        /// </summary>
        /// <returns></returns>
        public static  RfcDestination GetRfcDest()
        {
            if (_RfcDest == null || IsAlive==false)
            {
                lock (typeof(string))
                {
                    if (_RfcDest == null || IsAlive == false)
                    {
                        IDestinationConfiguration ID = new MyBackendConfig();
                        RfcDestinationManager.RegisterDestinationConfiguration(ID);
                        RfcDestination prd = RfcDestinationManager.GetDestination("PRD_000");
                        RfcDestinationManager.UnregisterDestinationConfiguration(ID);
                        _RfcDest = prd;
                        IsAlive = true;
                    }
                }
            }


            return _RfcDest;
        }

        


    }

    public class SAPWrap
    {
        /// <summary>
        /// 获取物料BOM
        /// </summary>
        /// <param name="mno"></param>
        public BomModel SAP_Craft_GetMBOM(string mno)
        {
            var prd = MyBackendConfig.GetRfcDest();
            RfcRepository repo = prd.Repository;
            IRfcFunction companyBapi = repo.CreateFunction("ZIPP002_BOM");   //调用函数名
            companyBapi.SetValue("I_MATNR", mno);
            companyBapi.Invoke(prd);

            var model = new BomModel();
            //获取返输出参数
           model.BOMType= companyBapi.GetString("BOMTYPE");
           model.AddTime =TryParser<DateTime?>( companyBapi.GetValue("AddTime"),null);
           model.LastEditTime = TryParser<DateTime?>(companyBapi.GetValue("LastEditTime"), null);
           model.MCost=TryParser<decimal?>(companyBapi.GetValue("MCost"),0.0m);
           model.Purpose = companyBapi.GetString("Purpose");
           model.Version = companyBapi.GetString("Version");
            //获取返回的内表
            IRfcTable rfcTable = companyBapi.GetTable("ITAB");
            for (int i = 0; i < rfcTable.RowCount; i++)
            {

                rfcTable.CurrentIndex = i;
                var bomItem = new BomItem();
                bomItem.Brand = rfcTable.GetString("Brand");
                bomItem.HasBOM = string.Compare(rfcTable.GetString("HasBOM"), "X", true) == 0;
                bomItem.MName = rfcTable.GetString("MName");
                bomItem.MNo = rfcTable.GetString("MNo").TrimStart("0".ToArray());
                bomItem.MU = rfcTable.GetString("MU");
                bomItem.Price = rfcTable.GetDecimal("Price");
                bomItem.Qty = rfcTable.GetDecimal("Qty");
                bomItem.Supplier = rfcTable.GetString("Supplier");
                model.Items.Add(bomItem);
            }

            return model;
        }
        /*
         public static List<EmpModel> GetEmpList()
         {
             var list = new List<EmpModel>();

             var prd = MyBackendConfig.GetRfcDest();
             RfcRepository repo = prd.Repository;
             IRfcFunction companyBapi = repo.CreateFunction("ZHR_GET_LIST");   //调用函数名
             companyBapi.Invoke(prd);
             IRfcTable rfcTable = companyBapi.GetTable("GT_EMP_OUT");

             for (int i = 0; i < rfcTable.RowCount; i++)
             {
                
                  rfcTable.CurrentIndex = i;
                 var emp=new EmpModel();
                 emp.EmpNo=  rfcTable.GetString("YGNO");
                 emp.Name= rfcTable.GetString("ZName");
                 emp.MachineNO= rfcTable.GetString("JITAI");
                 list.Add(emp);
             }
             return list;
         }

         public static void SyncEmp()
         {
             var list = new List<EmpModel>();
             using (var ctx = DBFac.NewCtx())
             {
                 var cmdText=@"select employee_id EmpNo,name,machineNO from dbo.Employee_basic_infomation
                     where now_instance<>'离职'";
                 list= ctx.ExecuteStoreQuery<EmpModel>(cmdText).ToList();
             }
            
             var prd = MyBackendConfig.GetRfcDest();
             RfcRepository repo = prd.Repository;
             IRfcFunction companyBapi = repo.CreateFunction("ZHR_SYNC");   //调用函数名
             IRfcTable rfcTable = companyBapi.GetTable("gt_emps_in");

             // companyBapi.SetValue("EX_WERKS", "3003");   //设置Import的参数 ，即：输入参数

             //多行
             foreach (var emp in list)
             {
                 rfcTable.Insert();
                 rfcTable.CurrentRow.SetValue("YGNO", emp.EmpNo);
                 rfcTable.CurrentRow.SetValue("ZNAME", emp.Name);
                 rfcTable.CurrentRow.SetValue("JITAI", emp.MachineNO);
                 rfcTable.CurrentRow.SetValue("FLAG", "C");
             }



             companyBapi.Invoke(prd);   //执行函数     
            
             var status = companyBapi.GetString("EXEC_STATUS");
             if (status!= "完成!")
             {
                 throw new Exception("同步出错!");
             }
         }
         */

        #region Helper
        protected T TryParser<T>(object v, T dValue)
        {

            if (v == null)
            {
                return dValue;
            }
            else
            {
                T t = default(T);
                try
                {
                    if (t == null)//可空类型
                    {
                        Type type = typeof(T);

                        TypeConverter conv = TypeDescriptor.GetConverter(type);
                        t = (T)conv.ConvertFrom(v);
                    }
                    else
                    {

                        t = (T)Convert.ChangeType(v, typeof(T));
                    }
                }
                catch
                {

                    t = dValue;
                }
                return t;
            }
        }
        protected T TryParser<T>(string v, T dValue)
        {
            if (string.IsNullOrEmpty(v))
            {
                return dValue;
            }
            else
            {
                T t = default(T);
                try
                {
                    if (t == null)//可空类型
                    {
                        Type type = typeof(T);

                        TypeConverter conv = TypeDescriptor.GetConverter(type);
                        t = (T)conv.ConvertFrom(v);
                    }
                    else
                    {

                        t = (T)Convert.ChangeType(v, typeof(T));
                    }
                }
                catch
                {

                    t = dValue;
                }
                return t;
            }
        }
        #endregion
    }
 
    #region Model
    public class EmpModel
    {
        public string EmpNo { get; set; }
        public string Name { get; set; }
        public string MachineNO { get; set; }
        public string Flag { get; set; }
    }
#endregion


}
