using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTaking
{
    [JsonObject(MemberSerialization.OptIn)]
    class Note
    {
        [JsonProperty("Title")]
        public string Title { get; private set; }
        [JsonProperty("Message")]
        public string Message { get; private set; }

        public Note(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
