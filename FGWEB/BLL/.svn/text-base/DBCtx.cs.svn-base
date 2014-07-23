using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGWEB.BLL
{
    using Model;
    public class DBCtx
    {
        public static FDGEntities Ctx()
        {
            return new FDGEntities();
        }

        public static long GetValue(int id)
        {
            using (var ctx = Ctx())
            {
               var it= ctx.KB_Identity.FirstOrDefault(ent => ent.Id == id);
               if (it == null) throw new Exception("主键编号不存在!");
                it.KeyValue = (it.KeyValue.HasValue? it.KeyValue.Value : 0) + 1;
               ctx.SaveChanges();

               return it.KeyValue.Value;
            }
        }
    }
}