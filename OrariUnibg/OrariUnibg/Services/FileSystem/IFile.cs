using System;
using System.Threading.Tasks;

namespace OrariUnibg
{
	public interface IFile
	{
		Task<string> ReadAllText(string filename);
		Task WriteAllText(string filename, string text);
		Task<byte[]> ReadAllBytes(string filename);
		Task WriteAllBytes(string filename, byte[] bytes);
		string[] GetFiles(string path);
		string GetPersonalFolderPath();
		Task<string> GetInternalFolder();
		Task Delete(string filename);
		Task Show(string filename);
		Task SendEmail(String fileName);
		void Share (string fileName);
		string Combine(string filename1, string filename2);
	}
}

