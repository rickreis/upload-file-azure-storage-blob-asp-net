using System;
using System.Collections.Generic;
using System.Linq;

namespace UploadFile.Azure
{
    public class Result
    {
        public Result()
        {
            _errors = new HashSet<string>();
        }

        public bool HasErrors
        {
            get { return Errors.Count() > 0; }
        }

        HashSet<string> _errors;
        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }

        public void AddError(string error)
        {
            if (String.IsNullOrWhiteSpace(error))
            {
                return;
            }
            if (_errors.Contains(error) == false)
            {
                _errors.Add(error);
            }
        }
    }

    public class ResultUploadFile : Result
    {
        public string BlobName { get; set; }        

        public string Url
        {
            get { return String.Concat(FileSettings.UrlFile, this.BlobName); }
        }        
    }

    public class ResultDeleteFile : Result
    {
        public string FileName { get; set; }        
    }
}
