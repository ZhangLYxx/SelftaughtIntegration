using Google.Protobuf;
using Integration.Application.Contracts.SecondHandCar;
using Integration.Excel.ExcelSettings;
using Integration.Service.StartUp;
using Integration.ToolKits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spire.Xls;
using UCmember.Dto.Member;

namespace UCmember.Api.Controllers
{
    /// <summary>
    /// 会员
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly IMember _memberservice;

        public MemberController(IMember memberservice)
        {
            _memberservice = memberservice;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(PermissionConstantKey.ExaminePolicy)]
        public  async Task<string> Login()
        {
            await _memberservice.GetAsync();
            return "1";
        }

        #region 导出

        

        /// <summary>
        /// 生成导出文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="excelWriter"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ApiResult> Export([FromBody] OrderExportDto dto, [FromServices] IExcelWriter excelWriter, [FromServices] IWebHostEnvironment env)
        {
            var list = new List<OrderExportVo>();
            var sheetSettingsDataSources = list.ToArray();
            var ids = sheetSettingsDataSources.Select(x => x.OrderId).Distinct().ToArray();
            var guid = Guid.NewGuid();

            var record = new ExportRecord
            {
                Id = guid,
                Count = ids.Length,
                CreateTime = DateTime.Now,
                CreateBy = 1,
                CreateName = "1",
                FileName = $"{DateTimeOffset.Now.ToUnixTimeSeconds()}-{guid:N}.xlsx",
            };

            record.PhysicsPath = $"excels/{record.FileName}";

            byte[] bytes;
            var sheetSettings = GetOrderSettings();

            if (sheetSettingsDataSources.Length == 0)
            {
                //写入空文件
                sheetSettings.DataSources = new List<OrderExportVo>(1);
                excelWriter.Write(sheetSettings);
                bytes = excelWriter.Save();
            }
            else
            {
                //多sheet页
                var sn = sheetSettingsDataSources.Select(x => x.SupplierName).Distinct().ToArray();
                foreach (var name in sn)
                {
                    sheetSettings.SheetName = name;
                    sheetSettings.DataSources = sheetSettingsDataSources.Where(c => c.SupplierName == name).ToArray();
                    excelWriter.Write(sheetSettings);
                }
                bytes = excelWriter.Save();
            }

            var basePath = Path.Combine(env.ContentRootPath ?? env.WebRootPath, "excels");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            var path = Path.Combine(basePath, record.FileName);

            await using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                await stream.WriteAsync(bytes.AsMemory(0, bytes.Length));
            }
            return ApiResult.Ok(data: new { id = guid.ToString() });
        }

        /// <summary>
        /// 导出 订单设置
        /// </summary>
        /// <returns></returns>
        private static WriteSheetSettings<OrderExportVo> GetOrderSettings()
        {
            var s = new WriteSheetSettings<OrderExportVo>();
            s.Property(_ => _.ProductName).HasTitle("产品名称").HasConvert(_ => $"{_.ProductName} {_.ProductCode}".Trim());
            s.Property(_ => _.Sku).HasTitle("规格");
            s.Property(_ => _.Quantity).HasTitle("数量");
            s.Property(_ => _.MemberRemark).HasTitle("用户备注");
            s.Property(_ => _.ContactName).HasTitle("姓名");
            s.Property(_ => _.ContactMobile).HasTitle("电话");
            s.Property(_ => _.Address).HasTitle("地址");
            s.Property(_ => _.CardDescription).HasTitle("来源");
            s.Property(_ => _.DeliveryCode).HasTitle("快递单号");
            s.Property(_ => _.DeliveryCompany).HasTitle("快递公司");
            s.Property(_ => _.Id).HasTitle("子订单号").HasConvert(_ => _.Id.ToString());


            s.Property(_ => _.CreateTime).HasTitle("下单时间");


            s.HasOrdinalColumn = true;

            return s;
        }

        [HttpPost("[action]")]
        public async Task<ApiResult> ExportForFinance([FromBody] FianceQueryDto dto, [FromServices] IExcelWriter excelWriter, [FromServices] IWebHostEnvironment env)
        {
            var exportlist = new List<OrderFinanceDto>();
            var exports = exportlist.ToList();
            var paylist = new List<PayRecordExportDto>();
            var paySettingsDataSources = paylist.ToList();
            //合并单元格
            foreach (var item in paySettingsDataSources)
            {
                item.OrderFinances = exports.Where(c => c.Code == item.Code).ToList();
            }

            var ids = paySettingsDataSources.Select(x => x.Code).Distinct().ToArray();

            var guid = Guid.NewGuid();

            var record = new ExportRecord
            {
                Id = guid,
                Count = ids.Length,
                CreateTime = DateTime.Now,
                CreateBy = 1,
                CreateName = "",
                FileName = $"{DateTimeOffset.Now.ToUnixTimeSeconds()}-{guid:N}.xlsx",
            };

            record.PhysicsPath = $"excels/{record.FileName}";

            var sheetSettings = GetWriteFiSettings();
            sheetSettings.DataSources = paySettingsDataSources;
            var bytes = excelWriter.WriteAndSave(sheetSettings);

            var basePath = Path.Combine(env.ContentRootPath ?? env.WebRootPath, "excels");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            var path = Path.Combine(basePath, record.FileName);

            await using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                await stream.WriteAsync(bytes.AsMemory(0, bytes.Length));
            }
            return ApiResult.Ok(data: new { id = guid.ToString() });
        }

        private static WriteSheetSettings<PayRecordExportDto> GetWriteFiSettings()
        {
            var setting = new WriteSheetSettings<PayRecordExportDto>();

            var hb = setting.HasGroupColumn(c => c.OrderFinances);

            hb.Property(c => c.Code).HasTitle("订单编号");
            hb.Property(c => c.CreateTime).HasTitle("下单日期");
            hb.Property(c => c.Id).HasTitle("子单编号");
            hb.Property(c => c.ProductName).HasTitle("产品名称");
            hb.Property(c => c.Sku).HasTitle("规格");
            hb.Property(c => c.Quantity).HasTitle("数量");
            hb.Property(c => c.SalePrice).HasTitle("售价");
            hb.Property(c => c.PayAmount).HasTitle("支付金额");

            setting.Property(_ => _.JXPoints).HasTitle("嘉弦积分");
            setting.Property(_ => _.WxPrice).HasTitle("微信支付");
            setting.Property(_ => _.ExpressFee).HasTitle("运费");

            setting.HasOrdinalColumn = true;
            return setting;
        }
        #endregion

        #region 导入

        /// <summary>
        /// 发货信息导入
        /// </summary>
        /// <param name="fileCollection"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ApiResult<ExpressImportResult>> Express(IFormFileCollection fileCollection, [FromServices] IExcelReader reader)
        {
            //var session = HttpContext.GetManagerSession();
            if (fileCollection.Count == 0)
            {
                return ApiResult.Fail<ExpressImportResult>("未发现文件");
            }

            var file = fileCollection[0];
            if (!file.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) && !file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return ApiResult.Fail<ExpressImportResult>($"文件{file.FileName}，不是有效的excel格式");
            }
            int cnt;
            //支持老版xls
            if (file.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
            {
                var workbook = new Workbook();
                await using var fs = file.OpenReadStream();
                workbook.LoadFromStream(fs);
                using var ms = new MemoryStream();
                workbook.SaveToStream(ms, FileFormat.Version2016);
                reader.Load(ms);
                cnt = reader.GetSheetCount() - 1;
            }
            else
            {
                reader.Load(file.OpenReadStream());
                cnt = reader.GetSheetCount();
            }

            var setting = GetReadSheetSettings();
            var rows = new List<ExpressImportDto>();

            for (int i = 0; i < cnt; i++)
            {
                rows.AddRange(reader.Read(setting).ToArray());
            }

            var okRows = rows.Where(it => it.IsOk).ToArray();
            var orderIds = new List<long>();

      

            var oIds = orderIds.Distinct().ToArray();

            var errRows = rows.Where(it => !it.IsOk).ToList();

            var result = new ExpressImportResult
            {
                RowCount = rows.Count,
                ErrorCount = errRows.Count,
                ErrorRows = errRows
            };

            result.OkCount = result.RowCount - result.ErrorCount;

            return ApiResult.Ok(result);
        }


        /// <summary>
        /// 设置读取
        /// </summary>
        /// <returns></returns>
        private static ReadSheetSettings<ExpressImportDto> GetReadSheetSettings()
        {
            var setting = new ReadSheetSettings<ExpressImportDto>();

            setting.Property(_ => _.Id).HasTitle("子订单号");

            //setting.Property(_ => _.TrackingNumber).HasTitle("快递单号");

            //setting.Property(_ => _.Company).HasTitle("快递公司");

            return setting;
        }
        #endregion
    }
}
