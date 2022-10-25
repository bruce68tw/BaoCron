using BaoLib.Enums;
using Base.Services;
using System;
using System.Threading.Tasks;

namespace BaoCron.Services
{
    public class MyService
    {
        public async Task RunAsync()
        {
            const string preLog = "BaoCron: ";
            await _Log.InfoA(preLog + "Start.");

            #region 1.清除 Redis Cache
            var info = "";
            await _Redis.FlushDbA();
            #endregion

            #region 2.尋寶遊戲上下架
            await using (var db = new Db())
            {
                //下架
                var today = DateTime.Today;
                var tmr = today.AddDays(1);
                var sql = $@"
update dbo.Bao set 
    LaunchStatus='{LaunchStatusEstr.Over}',
    Revised=getdate()
where LaunchStatus='{LaunchStatusEstr.Yes}'
and EndTime < getdate()
and Status=1
";
                var count = await db.ExecSqlA(sql);
                await _Log.InfoA("下架 Bao 筆數: " + count);

                //上架
                sql = $@"
update dbo.Bao set 
    LaunchStatus='{LaunchStatusEstr.Yes}',
    Revised=getdate()
where LaunchStatus='{LaunchStatusEstr.Doing}'
and StartTime < getdate()
and Status=1
";
                count = await db.ExecSqlA(sql);
                await _Log.InfoA("上架 Bao 筆數: " + count);

            }
            #endregion

            #region close db & log
            //lab_exit:
            //if (db != null)
            //    await db.DisposeAsync();
            if (info != "")
                await _Log.InfoA(preLog + info);

            await _Log.InfoA(preLog + "End.");
            #endregion
        }

    }//class
}
