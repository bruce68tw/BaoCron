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
            _Log.Info(preLog + "Start.");

            #region 1.清除 Redis Cache
            var info = "";
            await _Cache.ResetDbA();
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
                _Log.Info("下架 Bao 筆數: " + count);

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
                _Log.Info("上架 Bao 筆數: " + count);

            }
            #endregion

            #region close db & log
            //lab_exit:
            //if (db != null)
            //    await db.DisposeAsync();
            if (info != "")
                _Log.Info(preLog + info);

            _Log.Info(preLog + "End.");
            #endregion
        }

    }//class
}
