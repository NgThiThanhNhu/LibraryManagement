using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace DoAnCuoiKy.Helper
{
    public static class SlugHelper
    {
        public static string Slugify(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    if (char.IsLetterOrDigit(c))
                        sb.Append(char.ToLowerInvariant(c));
                    else if (char.IsWhiteSpace(c))
                        sb.Append('-');
                }
            }

            return Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');
        }
    }
}
