using System.Collections.Generic;
using System.Text;
using Shoc.ModelCore;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// Base class for containerizers
    /// </summary>
    public class ContainerizeBase
    {
        /// <summary>
        /// Adds copy to dockerfile
        /// </summary>
        /// <returns></returns>
        protected virtual string CopyStatement(List<FileCopySpec> fileList)
        {
            // make sure file list not empty
            if (fileList.Count < 1)
            {
                return null;
            }

            // create output string
            var output = new StringBuilder();

            // append each copy file
            foreach (var file in fileList)
            {
                output.AppendLine($"COPY {file.From} {file.To}");
            }

            return output.ToString();
        }

        /// <summary>
        /// Adds quotation marks to value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string MakeParameter(string value)
        {
            // makre sure value not empty
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            // make sure value is not already parameterized
            if (value[0] == '"' && value[^1] == '"')
            {
                return value;
            }

            // return parameterized value
            return $"\"{value}\"";
        }
    }
}
