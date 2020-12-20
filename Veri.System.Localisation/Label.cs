using System;
using System.Collections.Generic;
using System.Text;
using Veri.System.Data;

namespace Veri.System.Localisation
{
    public class Label : BaseTable
    {
        public Label(ICacheContext cache) : base(cache)
        {
        }

        public string Id { get; set; }
        public List<LabelLocal> Labels { get; set; }
    }

    public class LabelLocal
    {
        public string Iso { get; set; }
        public string Label { get; set; }
    }
}
