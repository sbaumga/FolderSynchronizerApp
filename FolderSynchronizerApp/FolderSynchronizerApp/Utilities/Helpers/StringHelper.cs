using System;
using System.Linq;

namespace FolderSynchronizerApp.Utilities.Helpers
{
    public static class StringHelper
    {
        public static bool AnyAreNullOrEmpty(params string[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (!input.Any())
            {
                throw new NotImplementedException($"{nameof(StringHelper)}.{nameof(AnyAreNullOrEmpty)} does not support an empty array as input.");
            }

            var result = input.Any(string.IsNullOrEmpty);
            return result;
        }
    }
}