using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Central_EDI.Model
{
    public class EDI_Model
    {
        public EDI_ST ST { get; set; }
        public EDI_SE SE { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<EDI_Content> Content { get; set; }
        public List<object> ValidationErrors { get; set; }
    }

    public class EDI_Content
    {
        public string E { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<EDI_Content> Content { get; set; }
    }

    public class EDI_ST
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public List<EDI_Content> Content { get; set; }
    }

    public class EDI_SE
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public List<EDI_Content> Content { get; set; }
    }
}
