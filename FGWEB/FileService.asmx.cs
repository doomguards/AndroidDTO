using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace FGWEB
{
    using Model;
    using BLL;
    /// <summary>
    /// FileService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class FileService : System.Web.Services.WebService
    {
        

        [WebMethod(Description="新增文件,如果没有指定NewVerId,系统将生成NewVerId")]
        public AddFileResponse AddFile(FG_AttFilePath file)
        {
            var  response = new AddFileResponse();
            try
            {
                if (file.FileData.Length <= 0) throw new Exception("文件长度为空!");
                using (var ctx = DBCtx.Ctx())
                {
                    if (!file.NewVerID.HasValue || file.NewVerID <= 0)
                    {
                        file.NewVerID = DBCtx.GetValue(1);
                    }

                    file.AddTime = DateTime.Now;
                    file.AttachFilePath = new FileStorageService().SaveFile(file.FileData);

                    ctx.AddToFG_AttFilePath(file);
                    ctx.SaveChanges();
                }
                response.Code = 0;
                response.FileId = file.NewVerID;
                response.Id = file.ID;


            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Msg = ex.Message;
            }
            return response;

        }
        [WebMethod(Description="更新文件内容 ")]
        public UpdateFileResponse UpdateFileData(long id,byte[] fileData)
        {
            var response = new UpdateFileResponse();
            try
            {
                if (fileData.Length <= 0) throw new Exception("文件长度为空!");
                using (var ctx = DBCtx.Ctx())
                {
                    var it = ctx.FG_AttFilePath.FirstOrDefault(ent => ent.ID == id);
                    if (it == null) throw new Exception("该Id的记录不存在!");
                    var path = new FileStorageService().SaveFile(fileData);
                    it.AttachFilePath = path;
                    ctx.SaveChanges();
                }

                response.Code = 0;


            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Msg = ex.Message;
            }
            return response;
        }
        [WebMethod(Description="必需提供NewVerId选择提供FG_Ver,FG_SubVer")]
        public RetriveResponse GetFile(FG_AttFilePath file)
        {
            var response = new RetriveResponse();
            try
            {
                if (file.NewVerID == null || file.NewVerID <= 0)
                {
                    throw new Exception("必须提供文件NewVerID!");

                }
                

                using (var ctx = DBCtx.Ctx())
                {
                    var q = ctx.FG_AttFilePath.Where(ent => ent.NewVerID == file.NewVerID);
                    if (!string.IsNullOrWhiteSpace(file.FG_Ver))
                    {
                        q = q.Where(ent => ent.FG_Ver == file.FG_Ver);
                        if (!string.IsNullOrWhiteSpace(file.FG_subVer))
                        {
                            q = q.Where(ent => ent.FG_subVer == file.FG_subVer);
                        }
                    }
      
                    response.Files= q.ToList();
                }
                response.Code = 0;
            


            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Msg = ex.Message;
            }
            return response;
        }
        [WebMethod(Description="Id为记录号")]
        public RetriveDataResponse GetData(long id)
        {
            var response = new RetriveDataResponse();
            try
            {
    
                using (var ctx = DBCtx.Ctx())
                {
                    var it=ctx.FG_AttFilePath.FirstOrDefault(ent => ent.ID == id);
                    if (it == null) throw new Exception("该Id的记录不存在!");
                    it.FileData = new FileStorageService().ReadFile(it.AttachFilePath);
                    response.FileData = it.FileData;
                }

                response.Code = 0;

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Msg = ex.Message;
            }
            return response;
        }
        [WebMethod(Description="分页获取表记录")]
        public RetriveResponse GetList(string fg_flag,int startIndex, int maxRecordSize)
        {
            var response = new RetriveResponse();
            try
            {
               
                using (var ctx = DBCtx.Ctx())
                {
                   var list= ctx.FG_AttFilePath.Where(ent=>ent.FG_Flag.Contains(fg_flag)).OrderBy(ent=>ent.ID).Skip(startIndex).Take(maxRecordSize).ToList();
                   response.Files = list;
                }
                response.Code = 0;

            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Msg = ex.Message;
            }
            return response;
        }
        [WebMethod(Description="删除指定Id的文件")]
        public DeleteFileResponse DeleteFile(List<long> ids)
        {
          
            var response = new DeleteFileResponse();
            try
            {
                if (ids.Count <= 0) throw new Exception("集合为空!");
                using (var ctx = DBCtx.Ctx())
                {
                    var list = ctx.FG_AttFilePath.Where(ent=>ids.Contains(ent.ID)).ToList();
                    list.ForEach(ent => {
                        ctx.FG_AttFilePath.DeleteObject(ent);
                    });
                    ctx.SaveChanges();
                }

                response.Code = 0;

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
