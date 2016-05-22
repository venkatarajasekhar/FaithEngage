using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Scripture.Data
{
    internal class BookInfo
    {
        internal int Number { get; set; }
        internal string Name { get; set; }
        internal string ShortAbbr { get; set; }
        internal string LongAbbr { get; set; }
        internal int Ordinal { get; set; }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return
                this.Name.GetHashCode() ^ this.Number;
        }
    }
}
