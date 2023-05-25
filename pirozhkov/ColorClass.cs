using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace pirozhkov

{
    public partial class Product
    {
        public Brush BackColor
        {
            get
            {
                if (ProductQuantityInStock > 0)
                    return Brushes.LightBlue;
                else
                    return Brushes.Gray;
            }
        }
    }
}
