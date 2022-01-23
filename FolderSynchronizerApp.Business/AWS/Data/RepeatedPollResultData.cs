using System.Collections.Generic;

namespace FolderSynchronizerApp.Business.AWS.Data
{
    public class RepeatedPollResultData
    {
        public IEnumerable<string> PollMessages { get; set; }

        public string ErrorMessage { get; set; }
    }
}