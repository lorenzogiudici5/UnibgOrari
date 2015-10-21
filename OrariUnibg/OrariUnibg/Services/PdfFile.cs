using System;
using Syncfusion.Pdf;
using System.IO;
using Syncfusion.Pdf.Graphics;
using OrariUnibg.Helpers;
using Syncfusion.Drawing;
using System.Threading.Tasks;

namespace OrariUnibg
{
	public class PdfFile
	{
		#region Constructor
		public PdfFile ()
		{
		}
		#endregion

		#region Private Fields
		private IFile _file;
		public string _filename;
		private PdfDocument _document;
		#endregion

		#region Public Methods
		public void Create()
		{
			_document = new PdfDocument();


			GeneratePdf ();
		}

		public async Task Save()
		{
			MemoryStream stream = new MemoryStream();
			_document.Save(stream);
			_document.Close(true);

			_file = Xamarin.Forms.DependencyService.Get<IFile>();
			DateTime now = DateTime.Now;

			_filename = _file.Combine(await _file.GetABBInternalFolder(), string.Format("sysUpdateReport_{0}{1}{2}{3}{4}{5}.pdf", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second));

			await _file.WriteAllBytes(_filename, stream.ToArray());
		}
		public async Task Display()
		{
			await _file.Show(_filename);
		}

		public async Task Send()
		{
			await _file.SendEmail(_filename);
		}

		public void GeneratePdf()
		{
			//Create a new PDF document.

			PdfDocument document = new PdfDocument();

			//Add a page

			PdfPage page = document.Pages.Add();

			//Creates Pdf graphics for the page

			PdfGraphics graphics = page.Graphics;

			//Creates a solid brush.

			PdfBrush brush = new PdfSolidBrush (Color.Black);

			//Sets the font.

			PdfFont font = new PdfStandardFont (PdfFontFamily.Helvetica, 15);

			//Draws the text.

			graphics.DrawString("Lorem Ipsum is simply dummy text of the" +
				"printing and typesetting industry. Lorem Ipsum has been the" +
				"standard dummy text ever since the 1500s, when an unknown printer" +
				"took a galley of type and scrambled it to make a type specimen" +
				"book.", font, brush, new RectangleF(0,0, page.GetClientSize().
					Width,200));


			//Saves the document.

			MemoryStream stream = new MemoryStream();

			document.Save(stream);

			document.Close(true);
		}

		#endregion
	}
}

