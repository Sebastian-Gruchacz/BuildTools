namespace ObscureWare.BuildCmdlets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Runtime.InteropServices;

    public class MsiHelper
    {
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern Int32 MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref Int32 len);

        [DllImport("msi.dll", EntryPoint = "MsiEnumProductsExW", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern uint MsiEnumProductsEx(
            string szProductCode,
            string szUserSid,
            uint dwContext,
            uint dwIndex,
            string szInstalledProductCode,
            out object pdwInstalledProductContext,
            string szSid,
            ref uint pccSid);

        public enum MSIINSTALLCONTEXT
        {
            MSIINSTALLCONTEXT_NONE = 0,
            MSIINSTALLCONTEXT_USERMANAGED = 1,
            MSIINSTALLCONTEXT_USERUNMANAGED = 2,
            MSIINSTALLCONTEXT_MACHINE = 4,
            MSIINSTALLCONTEXT_ALL = (MSIINSTALLCONTEXT_USERMANAGED | MSIINSTALLCONTEXT_USERUNMANAGED | MSIINSTALLCONTEXT_MACHINE),
        }


        public static string GetProductInfo(string productId)
        {
            Int32 len = 512;
            System.Text.StringBuilder builder = new System.Text.StringBuilder(len);
            var result = MsiGetProductInfo(productId, "InstallLocation", builder, ref len);
            if (result == 0)
            {
                return builder.ToString();
            }

            return null;
        }

        //public static bool FindByProductCode(Guid productCode, ref string productName)
        //{
        //    try
        //    {
        //        string codeToFind = productCode.ToString("B").ToUpper();

        //        var code = new string(new char[40]);
        //        object junk = null;
        //        string szSid = new string(new char[300]);
        //        uint pccSid = 300;

        //        uint res = MsiEnumProductsEx(codeToFind, null, (uint)MSIINSTALLCONTEXT.MSIINSTALLCONTEXT_USERUNMANAGED, 0, code, out junk, szSid, ref pccSid);

        //        if (res == 0)
        //        {
        //            uint valueSize = 1024;
        //            var valueProductName = new string(new char[valueSize]);

        //            MsiGetProductInfo(code, MsiInstallerProperty.ProductName, valueProductName, ref valueSize);

        //            productName = valueProductName;

        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //    }

        //    return false;
        //}
    }
}

