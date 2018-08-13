using System.IO;
using System.Threading.Tasks;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public static class Extensions
    {
        public static async Task<string> ReadContentAsync(this FileInfo fileInfo)
        {
            using (var reader = fileInfo.OpenText())
                return await reader.ReadToEndAsync();
        }
    }
}