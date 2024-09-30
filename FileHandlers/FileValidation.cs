using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NewsWebSite.FileHandlers
{
    public class FileValidation
    {
        public bool checkIfFileEmpty(HttpPostedFileBase file)
        {
            return file == null || file.ContentLength == 0;
          
        }

        public bool CheckFileValidaity(HttpPostedFileBase file ,string[] allowedExtensions)
        {
            
          //  var allowedExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" };
            string fileExtension = Path.GetExtension(file.FileName);
            bool FileAllowed = allowedExtensions.Contains(fileExtension);
            return FileAllowed;
        }
        public string GetUniqueFileName(string FilePathWithoutName ,string fileName)
        {
            if (string.IsNullOrEmpty(FilePathWithoutName)|| string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }
            string encdoingFileName = HttpUtility.HtmlEncode(fileName);
            string filePath =string.Format("{0}{1}", FilePathWithoutName,encdoingFileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(encdoingFileName);
            string fileExtension = Path.GetExtension(encdoingFileName);
            string UniquefileName = encdoingFileName;
            while (System.IO.File.Exists(filePath))
            {
                UniquefileName = $"{fileNameWithoutExtension}{DateTime.Now.Ticks}{fileExtension}";
                filePath = Path.Combine(FilePathWithoutName, UniquefileName);
            }
            return UniquefileName;
        }
    }
}