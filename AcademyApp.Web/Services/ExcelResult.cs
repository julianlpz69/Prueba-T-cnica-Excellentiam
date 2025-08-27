using Microsoft.AspNetCore.Mvc;

namespace AcademyApp.Web.Services
{
    public class ExcelResult : FileContentResult
    {
        public ExcelResult(byte[] fileBytes, string fileName)
            : base(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = fileName;
        }
    }
}
