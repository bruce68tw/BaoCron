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
            await _Log.InfoAsync(preLog + "Start.");

            #region 1.清除 Redis Cache
            var info = "";
            _Redis.DeleteAll();
            #endregion

            #region 2.遊戲上下架
            await using (var db = new Db())
            {
                //下架
                var today = DateTime.Today;
                var tmr = today.AddDays(1);
                var sql = $@"
update dbo.Bao
    set LaunchStatus=3
where LaunchStatus=2
and EndTime < getdate()
and Status=1
";
                await db.ExecSqlAsync(sql);

                //上架
                sql = $@"
update dbo.Bao
    set LaunchStatus=2
where LaunchStatus=1
and StartTime < getdate()
and Status=1
";
                await db.ExecSqlAsync(sql);

            }
            #endregion

        #region close db & log
        lab_exit:
            //if (db != null)
            //    await db.DisposeAsync();
            if (info != "")
                await _Log.InfoAsync(preLog + info);

            await _Log.InfoAsync(preLog + "End.");
            #endregion
        }

    }//class
}
