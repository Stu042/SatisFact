using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Linq;



namespace SatisFact.Utilities {
	public class FileUpload {
		public string ErrorString;
		public string RelURL;
		public string ValidFilename;
		public string PhysicalPath;
		string _validPath;
		IFormFile FFile;
		string[] _allowedContentTypes;


		// inits file/path name vars
		public FileUpload(IFormFile file, string destPath, string[] allowedContentTypes) {
			ErrorString = String.Empty;
			FFile = file;
			_allowedContentTypes = allowedContentTypes;
			ValidFilename = new string(file.FileName.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
			_validPath = new string(destPath.Where(ch => !Path.GetInvalidPathChars().Contains(ch)).ToArray());
			RelURL = Path.Combine(_validPath, ValidFilename);
			var contentRootPath = (string)AppDomain.CurrentDomain.GetData("ContentRootPath");
			var webRootPath = (string)AppDomain.CurrentDomain.GetData("WebRootPath");
			var relpath = RelURL.Replace(@"/", @"\");
			PhysicalPath = Path.Combine(webRootPath, relpath.TrimStart('\\'));
		}


		// return true if able to upload, sets error message if unable
		public bool AbleToUpload() {
			if (FFile == null) {
				ErrorString = "No file selected.";
				return false;
			}
			if (FFile.Length <= 0) {
				ErrorString = "File has zero length.";
				return false;
			}
			if (!_allowedContentTypes.Contains(FFile.ContentType)) {
				ErrorString = "Wrong type of file.";
				return false;
			}
			if (File.Exists(PhysicalPath)) {
				ErrorString = "File of same name already exists.";
				return false;
			}
			ErrorString = String.Empty;
			return true;
		}


		// Copy file to valid destPath / destFileName, call AbleToUpload() before
		public bool Upload() {
			try {
				using var fileStream = FFile.OpenReadStream();
				using var fs = new FileStream(PhysicalPath, FileMode.CreateNew);
				fileStream.CopyTo(fs);
			} catch (IOException e) {
				ErrorString = e.ToString();
				return false;
			}
			return true;
		}


		public bool Delete(string UrlPath, string ImageName) {
			try {
				File.Delete(Path.Combine(PhysicalPath, UrlPath, ImageName));
			} catch (System.Exception e) {
				ErrorString = e.ToString();
				return false;
			}
			return true;
		}
	}
}
