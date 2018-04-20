using System;
using System.Globalization;
using System.Net.Http;
using Spire.Doc.Documents;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Reports
{
    public class FiniquitoReport
    {
        private readonly ITextDocumentServices _textDoucmentServices;
        private readonly IStudentRepository _studentRepository;
        private readonly IDownloadbleFile _downloadbleFile;

        public FiniquitoReport(ITextDocumentServices textDocumentServices, IStudentRepository studentRepository,IDownloadbleFile downloadbleFile)
        {
            _textDoucmentServices = textDocumentServices;
            _studentRepository = studentRepository;
            _downloadbleFile = downloadbleFile;
        }

        public HttpResponseMessage GenerateFiniquitoReport(string accountId)
        {
            var student = _studentRepository.GetByAccountNumber(accountId);
            var totalHours = _studentRepository.GetStudentHours(accountId);

            student.Finiquiteado = true;
            _studentRepository.Update(student);
            _studentRepository.Save();
            var doc = _textDoucmentServices.CreaDocument();
            var page1 = _textDoucmentServices.CreatePage(doc);
            _textDoucmentServices.SetPageMArgins(page1,35f,71f,85f,85f);
            var header = _textDoucmentServices.CreateHeader(doc);
            var headerParagraph = _textDoucmentServices.CreateHeaderParagraph(header);
            _textDoucmentServices.AppendPictureToHeader(headerParagraph,Properties.Resources.LaurateLogo,39f,122f,15f,-2f);
            _textDoucmentServices.AppendPictureToHeader(headerParagraph,Properties.Resources.UnitecFullLogo, 51f, 151f, 430f, -12f);

            var footer = _textDoucmentServices.CreaFooter(doc);
            var footerParagraph = _textDoucmentServices.CreateFooterParagraph(footer);
            var footerBody ="Elaborado y revisado por:\r\n" +
                            "Andrea Cecilia Orellana Zelaya\r\n" +
                            "Jefe de Vinculación y Emprendimiento\r\n\r\n";
           _textDoucmentServices.AppendTextToFooter(footerParagraph,footerBody, "Segoe UI", 10f);

            var p0 = _textDoucmentServices.CreateParagraph(page1);
            var p0Style = _textDoucmentServices.CreateParagraphStyle(doc, "FiniquitoTitle", "Segoe UI", 14f, true);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n\r\nCARTA DE FINIQUITO DEFINITIVO",p0,p0Style,doc,HorizontalAlignment.Center, 13.8f);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n\r\nPROGRAMA DE SERVICIO SOCIAL",p0,p0Style,doc,HorizontalAlignment.Center, 13.8f);

            var p1 = _textDoucmentServices.CreateParagraph(page1);
            var month = DateTime.Now.ToString("MMMM", new CultureInfo("es-ES"));
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n\r\nFecha: "+DateTime.Now.Day+" de "+ char.ToUpper(month[0])+month.Substring(1) +" del " +
                                                     +DateTime.Now.Year,p1,p0Style,doc, HorizontalAlignment.Left, 13.8f);

            var p2 = _textDoucmentServices.CreateParagraph(page1);
            var p2Style = _textDoucmentServices.CreateParagraphStyle(doc, "FiniquitoBody", "Segoe UI", 12f, false);
            var body1 = "Yo, Rafael Antonio Delgado Elvir, Director de Desarrollo Institucional de UNITEC\r\n" +
                        "Campus SPS, hago constar que en los registros figura que el estudiante: ";
            var studentName = student.Name.ToUpper();
            var body2 = ", con número de cuenta N° " + accountId + ", estudiante de la carrera de " +
                            "\"" + student.Major.Name + "\", realizó un total de " + totalHours + " horas" +
                            " de vinculacion, cumpliendo asi con el requerimiento de horas que estipula el Reglamento General del Programa de Servicio Social.";

            _textDoucmentServices.AddTextToParagraph("\r\n"+ body1, p2,p2Style,doc,HorizontalAlignment.Justify, 13.8f);
            var text = _textDoucmentServices.AddTextToParagraph(studentName, p2, p2Style, doc,
                HorizontalAlignment.Justify, 13.8f);
            text.CharacterFormat.Bold = true;
            _textDoucmentServices.AddTextToParagraph(body2, p2, p2Style, doc, HorizontalAlignment.Justify, 13.8f);

            var p4 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\nSe extiende la presente constancia para los fines que al interesado convengan el ",p4,p2Style,doc,HorizontalAlignment.Justify, 13.8f);
            var fechaText = _textDoucmentServices.AddTextToParagraph("" + DateTime.Now.Day + " de "+ char.ToUpper(month[0])+month.Substring(1) + " del " + DateTime.Now.Year + ".",p4,p2Style,doc,HorizontalAlignment.Justify, 13.8f);
            fechaText.CharacterFormat.Bold = true;

            var ending = " ______________________________________\r\n"+
                         "Director de Desarrollo Institucional\r\n"+
                         "UNITEC San Pedro Sula";

            var p5 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n\r\n"+ending, p5, p2Style, doc, HorizontalAlignment.Justify, 13.8f);
            return _downloadbleFile.ToHttpResponseMessage(doc, "Finiquito.docx");
        }
    }
}