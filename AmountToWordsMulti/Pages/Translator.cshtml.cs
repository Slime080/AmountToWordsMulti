using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;

namespace AmountToWordsMulti
{
    public class TranslatorModel : PageModel
    {
        [BindProperty]
        public decimal Amount { get; set; }  // Changed to decimal to support cents

        [BindProperty]
        public string Language { get; set; }

        public string Result { get; set; }
        public string FormattedNumber { get; set; } // To store the formatted number
        public string LanguageDisplay { get; set; } // To store the full name of the selected language

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var wholePart = (int)Math.Floor(Amount);  // Get the integer part
            var fractionalPart = (int)((Amount - wholePart) * 100);  // Get the fractional part (cents)

            // Convert the number to words
            string wholePartWords = ConvertNumberToWords(wholePart, Language);
            string fractionalPartWords = ConvertNumberToWords(fractionalPart, Language);

            // Format the number according to the chosen language
            string formattedNumber = FormatNumber(Amount, Language);
            FormattedNumber = formattedNumber;

            // Add the appropriate currency terms
            string currency = GetCurrencyTerm(Language);
            string fractionalCurrency = GetFractionalCurrencyTerm(Language);

            // Get full language display name
            LanguageDisplay = GetLanguageDisplayName(Language);

            Result = $"{wholePartWords} {currency} and {fractionalPartWords} {fractionalCurrency}";

            return Page();
        }

        private string ConvertNumberToWords(int number, string language)
        {
            try
            {
                CultureInfo culture = new CultureInfo(language);

                if (language.ToLower() == "ko")
                {
                    return ConvertNumberToWordsKorean(number);
                }

                return number.ToWords(culture);
            }
            catch
            {
                return number.ToWords(CultureInfo.InvariantCulture);
            }
        }

        private string ConvertNumberToWordsKorean(int number)
        {
            string[] ones = { "", "일", "이", "삼", "사", "오", "육", "칠", "팔", "구" };
            string[] tens = { "", "십", "이십", "삼십", "사십", "오십", "육십", "칠십", "팔십", "구십" };
            string[] thousands = { "", "천", "만", "억", "조" };

            string result = "";
            int group = 0;

            while (number > 0)
            {
                int currentGroup = number % 10000;
                if (currentGroup > 0)
                {
                    result = ConvertGroupToKorean(currentGroup, ones, tens) + thousands[group] + result;
                }
                number /= 10000;
                group++;
            }

            return result.Trim();
        }

        private string ConvertGroupToKorean(int group, string[] ones, string[] tens)
        {
            string groupWords = "";
            int tensPlace = group / 10;
            int onesPlace = group % 10;

            if (tensPlace > 0)
            {
                groupWords += tens[tensPlace];
            }
            if (onesPlace > 0)
            {
                groupWords += ones[onesPlace];
            }

            return groupWords;
        }

        private string FormatNumber(decimal number, string language)
        {
            try
            {
                CultureInfo culture = new CultureInfo(language);
                return string.Format(culture, "{0:N}", number);
            }
            catch
            {
                return number.ToString("N", CultureInfo.InvariantCulture);
            }
        }

        private string GetCurrencyTerm(string language)
        {
            switch (language.ToLower())
            {
                case "en":
                    return "dollars";
                case "fr":
                    return "dollars";
                case "es":
                    return "dólares";
                case "de":
                    return "Dollar";
                case "it":
                    return "dollari";
                case "pt":
                    return "dólares";
                case "zh-hans":
                    return "元";
                case "zh-hant":
                    return "元";
                case "ja":
                    return "ドル";
                case "ko":
                    return "달러";
                default:
                    return "currency";
            }
        }

        private string GetFractionalCurrencyTerm(string language)
        {
            switch (language.ToLower())
            {
                case "en":
                    return "cents";
                case "fr":
                    return "centimes";
                case "es":
                    return "centavos";
                case "de":
                    return "Cent";
                case "it":
                    return "centesimi";
                case "pt":
                    return "centavos";
                case "zh-hans":
                    return "分";
                case "zh-hant":
                    return "分";
                case "ja":
                    return "セント";
                case "ko":
                    return "센트";
                default:
                    return "cents";
            }
        }

        private string GetLanguageDisplayName(string languageCode)
        {
            return languageCode.ToLower() switch
            {
                "en" => "English",
                "fr" => "French",
                "es" => "Spanish",
                "de" => "German",
                "it" => "Italian",
                "pt" => "Portuguese",
                "zh-hans" => "Chinese (Simplified)",
                "zh-hant" => "Chinese (Traditional)",
                "ja" => "Japanese",
                "ko" => "Korean",
                _ => "Unknown Language"
            };
        }
    }
}
