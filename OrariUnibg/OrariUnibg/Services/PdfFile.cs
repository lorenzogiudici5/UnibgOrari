﻿using System;
using Syncfusion.Pdf;
using System.IO;
using Syncfusion.Pdf.Graphics;
using OrariUnibg.Helpers;
using Syncfusion.Drawing;
using System.Threading.Tasks;
using System.Reflection;

namespace OrariUnibg.Services
{
	public class PdfFile
	{
		#region Constructor
		public PdfFile ()
		{
		}
		#endregion

		#region Constants
		private PdfSolidBrush brushBlack = new PdfSolidBrush(Syncfusion.Drawing.Color.FromArgb(255, 0, 0, 0));
		private PdfSolidBrush brushGray = new PdfSolidBrush(Syncfusion.Drawing.Color.FromArgb(235, 235, 235));
		private PdfPen blackPen = new PdfPen(Syncfusion.Drawing.Color.FromArgb(255, 0, 0, 0));
		private PdfPen transparentPen = new PdfPen(Syncfusion.Drawing.Color.FromArgb(0, 0, 0, 0), .3f);
		private PdfStandardFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 24);
        private PdfStandardFont subTitleFontBold = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold);
        private PdfStandardFont subTitleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 18);
//		private PdfStandardFont groupTitleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
		private PdfStandardFont textFontBold = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
		private PdfStandardFont textFontInfo = new PdfStandardFont(PdfFontFamily.Helvetica, 14);
		private PdfStandardFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
		private PdfStandardFont smallFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
		private PdfStandardFont smallFontBold = new PdfStandardFont(PdfFontFamily.Helvetica, 9, PdfFontStyle.Bold);

		private float marginLeft = 45;
		private float marginRight = 45;
		private float marginTop = 20;
//		private float marginBottom = 50;
		private float heightHeader = 60; //different from the default one
		private float heightFooter = 60; //different from the default one
		#endregion

		#region Private Fields
		private IFile _file;
		public string _filename;
		private PdfDocument _document;
		private float currentY;
		private float spaceDefault = 30; //spazio per stampare la riga del titolo di pagina
		private float spaceSubtitle = 20;
		private float spaceLine = 10;
//		private float spaceSmall = 5;
		#endregion

		#region Properties
		public string Title {get; set;}
		public string TitleFacolta {get;set;}
		public string TitleInfo {get; set;}
		public string Text { get; set; }
		public string PdfType { get; set;}
		#endregion

		#region Public Methods
		public void CreateGiornaliero()
		{
			_document = new PdfDocument();
			PdfType = "Giornaliero";

			generatePdf ();
		}

		public void CreateCompleto()
		{
			_document = new PdfDocument();
			PdfType = "Completo";

			generatePdf ();
		}

		public void CreateFavourite()
		{
			_document = new PdfDocument();
			PdfType = "Preferiti";

			generatePdf ();
		}

		public async Task Save()
		{
			MemoryStream stream = new MemoryStream();
			_document.Save(stream);
			_document.Close(true);

			_file = Xamarin.Forms.DependencyService.Get<IFile>();
			DateTime now = DateTime.Now;

			_filename = _file.Combine(await _file.GetInternalFolder(), string.Format("{0}{1}{2}{3}{4}{5}_{6}.pdf", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, PdfType));

			await _file.WriteAllBytes(_filename, stream.ToArray());
		}

		public async Task Display()
		{
			await _file.Show (_filename);
		}
		public async Task Send()
		{
			await _file.SendEmail(_filename);
		}
			
		#endregion

		#region Private Methods
		private void generatePdf()
		{
			//Add a page
			_document.PageSettings.Margins.All = 0;
			_document.Pages.Add();

			createHeader();
			createFooter();

			currentY = marginTop;
			addTitles (); //title & subtitle

			currentY += spaceLine;

			//testo
			PdfStringFormat stringFormat = new PdfStringFormat() { Alignment = PdfTextAlignment.Left };
			PdfTextElement element = new PdfTextElement (Text, textFont, brushBlack);
			PdfLayoutFormat format = new PdfLayoutFormat();
			format.Layout = PdfLayoutType.Paginate;
			format.PaginateBounds = new RectangleF(marginLeft, heightHeader + spaceDefault + spaceSubtitle, _document.PageSettings.Size.Width - marginRight - marginLeft, _document.PageSettings.Size.Height - marginTop * 3);

			element.Draw (_document.Pages [0], new RectangleF (marginLeft, currentY, _document.PageSettings.Width - marginRight - marginLeft, _document.PageSettings.Height - marginTop * 3));

		}

		private void createHeader()
		{
			Stream logoStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("OrariUnibg.Resources.Image.Logo_flat.jpg");
			// header
			PdfPageTemplateElement header = new PdfPageTemplateElement(new RectangleF(new PointF(0, 0), new SizeF(_document.PageSettings.Width, heightHeader)));
            string tramite = string.Format("Condiviso tramite: {0}", Settings.AppName);
            if (Settings.IsLoggedIn)
                tramite = string.Format("{0} tramite: {1}", Settings.Name, Settings.AppName);
            
            header.Graphics.DrawString(tramite, smallFont, brushBlack, new PointF(marginLeft, 40 ));
			header.Graphics.DrawImage(PdfImage.FromStream(logoStream), new PointF(_document.PageSettings.Width + 5 - marginRight * 2, 15), new SizeF(40, 40));
			_document.Template.Top = header;
		}

		private void createFooter()
		{
			//footer
			PdfPageTemplateElement footer = new PdfPageTemplateElement(new RectangleF(new PointF(0, _document.PageSettings.Height - heightFooter), new SizeF(_document.PageSettings.Width, heightFooter)));
			footer.Graphics.DrawString(System.DateTime.Now.ToString(), smallFont, brushBlack, new PointF(marginLeft, 15));

			//Creates page number field.
			PdfPageNumberField pageNumber = new PdfPageNumberField(smallFont, brushBlack);
			//Creates page count field.
			PdfPageCountField count = new PdfPageCountField(smallFont, brushBlack);
			//Adds the fields in composite fields.
			PdfCompositeField compositeField = new PdfCompositeField(smallFont, brushBlack, "Pagina {0} di {1}", pageNumber, count);
			compositeField.Bounds = footer.Bounds;
			//Draws the composite field in footer.
			compositeField.Draw(footer.Graphics, new PointF(_document.PageSettings.Width-marginRight*2, 15));

			_document.Template.Bottom = footer;
		}

		private void addTitles()
		{
			PdfStringFormat stringFormat = new PdfStringFormat() { Alignment = PdfTextAlignment.Center };
			RectangleF rectangleCenterPage = new Syncfusion.Drawing.RectangleF(0, currentY, _document.PageSettings.Size.Width, titleFont.Height);
            
            //title
            _document.Pages[0].Graphics.DrawString(Title, titleFont, brushBlack, rectangleCenterPage, stringFormat);

            currentY += spaceDefault;
            rectangleCenterPage.Y = currentY;

            //Laurea
            _document.Pages[0].Graphics.DrawString(TitleFacolta, subTitleFontBold, brushBlack, rectangleCenterPage, stringFormat);

			currentY += spaceDefault;
			rectangleCenterPage.Y = currentY;

			//Info = anno semestre
			_document.Pages[0].Graphics.DrawString(TitleInfo, subTitleFont, brushBlack, rectangleCenterPage, stringFormat);
			currentY += spaceDefault;

//			rectangleCenterPage.Y = currentY;

			//subTitleInfo
			//_document.Pages[0].Graphics.DrawString(SubtitleInfo, subTitleFont, brushBlack, rectangleCenterPage, stringFormat);
			//currentY += spaceDefault;

		}
		#endregion
	}
}

