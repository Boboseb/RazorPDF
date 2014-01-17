// Copyright 2012 Al Nyveldt - http://nyveldt.com
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace RazorPDF
{
    public class PdfView : IView, IViewEngine
    {
        private readonly ViewEngineResult _result;

        public PdfView(ViewEngineResult result)
        {
            _result = result;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            // generate view into string
            var sb = new System.Text.StringBuilder();
            using (TextWriter tw = new System.IO.StringWriter(sb))
            {
                _result.View.Render(viewContext, tw);
            }

            using (TextReader reader = new StringReader(sb.ToString()))
            using (var document = new Document())
            {
                var hw = writer as HttpWriter;
                var sw = writer as StreamWriter;
                Stream outputStream = hw != null ? hw.OutputStream : sw != null ? sw.BaseStream : new MemoryStream();
                // associate output with response stream
                using (var pdfWriter = PdfWriter.GetInstance(document, outputStream))
                {
                    pdfWriter.CloseStream = false;

                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    document.Open();
                    var worker = XMLWorkerHelper.GetInstance();
                    worker.ParseXHtml(pdfWriter, document, reader);
                    document.Close();
                    pdfWriter.Close();
                }

                if (hw == null && sw == null)
                {
                    outputStream.Position = 0;
                    using (StreamReader sr = new StreamReader(outputStream, Encoding.Unicode))
                    {
                        writer.Write(sr.ReadToEnd());
                    }
                }
            }
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            throw new System.NotImplementedException();
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            throw new System.NotImplementedException();
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            _result.ViewEngine.ReleaseView(controllerContext, _result.View);
        }
    }
}
