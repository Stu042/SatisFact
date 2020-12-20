using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;

namespace SatisFact.Utilities
{
	public class FileUpload
	{
        public string ErrorString;
        public string FullFileName;
        public string ValidFilename;
        string ValidPath;
        IFormFile FFile;
        string[] AllowedContentTypes;


        // inits file/path name vars
        public FileUpload(IFormFile file, string destPath, string[] allowedContentTypes)
		{
            ErrorString = String.Empty;
            FFile = file;
            AllowedContentTypes = allowedContentTypes;
            ValidFilename = new string(file.FileName.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
            ValidPath = new string(destPath.Where(ch => !Path.GetInvalidPathChars().Contains(ch)).ToArray());
            FullFileName = Path.Combine(ValidPath, ValidFilename);
        }


        // return true if able to upload, sets error message if unable
        public bool AbleToUpload()
		{
            if (FFile == null)
            {
                ErrorString = "No file selected.";
                return false;
            }
            if (FFile.Length <= 0)
            {
                ErrorString = "File has zero length.";
                return false;
            }
            if (!AllowedContentTypes.Contains(FFile.ContentType))
            {
                ErrorString = "Wrong type of file.";
                return false;
            }
            if (File.Exists(FullFileName))
			{
                ErrorString = "File of same name already exists.";
                return false;
            }
            ErrorString = String.Empty;
            return true;
        }
        // "System.IO.DirectoryNotFoundException: Could not find a part of the path 
        // 'C:\\Users\\Stu\\source\\repos\\SatisFact\\SatisFact\\imgs\\Buildings\\Awesome Shop.png'.\r\n
        // at System.IO.FileStream.ValidateFileHandle(SafeFileHandle fileHandle)\r\n
        // at System.IO.FileStream.CreateFileOpenHandle(FileMode mode, FileShare share, FileOptions options)\r\n
        // at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)\r\n
        // at System.IO.FileStream..ctor(String path, FileMode mode)\r\n
        // at SatisFact.Utilities.FileUpload.Upload() in C:\\Users\\Stu\\source\\repos\\SatisFact\\SatisFact\\Utilities\\FileUpload.cs:line 65"

        // Copy file to valid destPath / destFileName, call AbleToUpload() before
        public bool Upload()
        {
            try
            {
				using var fileStream = FFile.OpenReadStream();
				using var fs = new FileStream(FullFileName, FileMode.CreateNew);
				fileStream.CopyTo(fs);
			}
            catch (IOException e)
            {
                ErrorString = e.ToString();
                return false;
            }
            return true;
        }
    }
}
