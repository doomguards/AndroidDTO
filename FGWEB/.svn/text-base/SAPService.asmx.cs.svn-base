using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PDMAPI.Domain;
using PDMAPI;
namespace FGWEB
{

    using Model;
    using BLL;
    /// <summary>
    /// SAPService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class SAPService : System.Web.Services.WebService
    {

        [WebMethod(Description="下载并保存当前BOM未一个快照")]
        public RetriveBomSanpshotResponse BomSanpshot(string version,string mno)
        {
            var response = new RetriveBomSanpshotResponse();
            try
            {
                var wrap = new SAPWrap();
                var bom= wrap.SAP_Craft_GetMBOM(mno);
                if (bom.Items.Count <= 0) throw new Exception("改物料未建立BOM!");

                using (var ctx = DBCtx.Ctx())
                {
                    var it= ctx.Sys_BOM.Where(ent => ent.FGpartno == mno).FirstOrDefault();
                    //BOM存在，需要进行保存到历史表中
                    if (it != null)
                    {
                        if (string.Compare(it.VerNo.Trim(), version) == 0) throw new Exception("该版本号已经存在!");

                        #region SQL
                        string cmdText = @"INSERT INTO [FDG].[dbo].[SingleBOM_History]
                                           ([ID]
                                           ,[FGpartno]
                                           ,[p_yw]
                                           ,[p_FGName]
                                           ,[partno]
                                           ,[p_partnoName]
                                           ,[p_oldpartno]
                                           ,[p_type]
                                           ,[stock]
                                           ,[p_czy]
                                           ,[parentno]
                                           ,[partnoqty]
                                           ,[UM]
                                           ,[p_supplier]
                                           ,[p_brand]
                                           ,[used]
                                           ,[activedate]
                                           ,[Lock]
                                           ,[LockUser]
                                           ,[default_partno]
                                           ,[default_partnoName]
                                           ,[default_supplier]
                                           ,[remark]
                                           ,[VerNo]
                                           ,[CreateTime]
                                           ,[VerFlag])
                                Select 

                                            [ID]
                                           ,[FGpartno]
                                           ,[p_yw]
                                           ,[p_FGName]
                                           ,[partno]
                                           ,[p_partnoName]
                                           ,[p_oldpartno]
                                           ,[p_type]
                                           ,[stock]
                                           ,[p_czy]
                                           ,[parentno]
                                           ,[partnoqty]
                                           ,[UM]
                                           ,[p_supplier]
                                           ,[p_brand]
                                           ,[used]
                                           ,[activedate]
                                           ,[Lock]
                                           ,[LockUser]
                                           ,[default_partno]
                                           ,[default_partnoName]
                                           ,[default_supplier]
                                           ,[remark]
                                           ,[VerNo]
                                           ,getdate()
                                           ,'$Guid'
                                from sys_BOM
                                where FGPartno='$Mno'";
                        #endregion

                        var guid = Guid.NewGuid().ToString();
                        var sql = cmdText.Replace("$Guid", guid).Replace("$Mno", mno.Trim());
                        ctx.ExecuteStoreCommand(sql);

                        ctx.ExecuteStoreCommand("Delete From Sys_Bom Where FGpartno='" + mno.Trim() + "'");

                        response.VerFlag = Guid.Parse(guid);
                    }
            


                    foreach (var bomItem in bom.Items)
                    {

                      var bomEnt = new Sys_BOM();
                      var idStr = "0000000000" + DBCtx.GetValue(2);
                      bomEnt.FGpartno = mno.Trim();
                      bomEnt.ID = idStr.Substring(idStr.Length - 10);
                      bomEnt.p_brand=bomItem.Brand;
                      bomEnt.p_partnoName=bomItem.MName;
                      bomEnt.partno=bomItem.MNo;
                      bomEnt.UM= bomItem.MU;
                      bomEnt.partnoqty=bomItem.Qty;
                      bomEnt.p_supplier=bomItem.Supplier;
                      bomEnt.p_yw = "??";
                      bomEnt.VerNo = version;
                      ctx.AddToSys_BOM(bomEnt);
                    }

                    ctx.SaveChanges();
                    response.Code = 0;
                }
               

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Msg = ex.Message;
            }
            return response;
        }
    }
}
