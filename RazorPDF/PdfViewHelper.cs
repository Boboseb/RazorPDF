using System.Web.Mvc;

namespace RazorPDF
{
    public static class PdfViewHelper
    {
        public static PdfResult Pdf(this Controller controller)
        {
            return controller.Pdf(null, null, null);
        }
        public static PdfResult Pdf(this Controller controller, object model)
        {
            return controller.Pdf(null, null, model);
        }
        public static PdfResult Pdf(this Controller controller, string viewName)
        {
            return controller.Pdf(viewName, null, null);
        }
        public static PdfResult Pdf(this Controller controller, string viewName, object model)
        {
            return controller.Pdf(viewName, null, model);
        }
        public static PdfResult Pdf(this Controller controller, string viewName, string masterName)
        {
            return controller.Pdf(viewName, masterName, null);
        }
        public static PdfResult Pdf(this Controller controller, string viewName, string masterName, object model)
        {
            if (model != null)
            {
                controller.ViewData.Model = model;
            }
            return new PdfResult { ViewName = viewName, MasterName = masterName, TempData = controller.TempData, ViewData = controller.ViewData };
        }
    }
}
