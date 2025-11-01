using DAL.RFN_2;
using BE.RFN_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.RFN_2
{
    public class ProductoC_BLL
    {
        ProductoC_DAL dao;
        public ProductoC_BLL()
        {
            dao = new ProductoC_DAL();
        }
        public List<ProductoC> RetonarProductoC()
        {
            return dao.RetonarProductoC();
        }
        public void ActivarProductoC(ProductoC c)
        {
            dao.ActivarProductoC(c);
        }
    }
}
