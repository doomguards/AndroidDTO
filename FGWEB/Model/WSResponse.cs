using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGWEB.Model
{
    public class WSResponse
    {
        public int Code { get; set; }
        public string Msg { get; set; }

        
    }


    public class AddFileResponse : WSResponse
    {
        public long? FileId { get; set; }
        public long? Id { get; set; }
    }
    public class UpdateFileResponse : WSResponse
    {

    }
    public class DeleteFileResponse : WSResponse
    {

    }
    public class RetriveResponse : WSResponse
    {
        public List<FG_AttFilePath> Files { get; set; }
        public RetriveResponse()
        {
            Files = new List<FG_AttFilePath>();
        }
    }
    public class RetriveDataResponse : WSResponse
    {
        public byte[] FileData { get; set; }
    }

    public class RetriveBomSanpshotResponse : WSResponse
    {
        public Guid VerFlag { get; set; }
    }



}