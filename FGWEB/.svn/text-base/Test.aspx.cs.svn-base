using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGWEB
{
    using BLL;
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //FileStorageService s = new FileStorageService();
           //var path= s.GetPath();
           //s.SaveFile(new Byte[]{ 12, 13 }, FileStorageService.C_Root + path);
           //Response.Write("xx");
           // var bytes= s.ReadFile(@"5\13\5\9085ba500254469ea925d6022ca2b363.d");
            TestList();
        }
        private void TestAddFile()
        {
            FileService fs = new FileService();
            var file=new Model.FG_AttFilePath();
            file.FG_Flag = "ttt";
            file.FileData = new byte[] { 13, 222, 23 };
            var it= fs.AddFile(file);
        }
        private void TestList()
        {
            SAPService s = new SAPService();
            var r= s.BomSanpshot("xxx", "1017021836");
        }
    }
}