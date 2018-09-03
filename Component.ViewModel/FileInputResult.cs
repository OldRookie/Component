using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.ViewModel
{
    public class FileInputResult
    {
        public string error { get; set; }

        public List<string> initialPreview { get; set; }

        public List<InitialPreviewConfig> initialPreviewConfig { get; set; }

        public bool append { get; set; }

        public string initialPreviewDownloadUrl { get; set; }
    }
    public class InitialPreviewConfig {
        public string type { get; set; }

        public string filetype { get; set; }

        public float size { get; set; }

        public string previewAsData { get; set; }

        public string caption { get; set; }

        public string width { get; set; }

        public string url { get; set; }

        public string key { get; set; }

        public string frameClass { get; set; }

        public string frameAttr { get; set; }

        public string extra { get; set; }

        public string downloadUrl { get; set; }
    }
}
