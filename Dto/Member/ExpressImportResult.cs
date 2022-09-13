using System.Collections.Generic;

namespace UCmember.Dto.Member
{
    public class ExpressImportResult
    {
        public int RowCount { get; set; }

        public int OkCount { get; set; }

        public int ErrorCount { get; set; }

        public List<ExpressImportDto> ErrorRows { get; set; }
    }
}
