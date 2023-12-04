using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace webshop.Models
{
    public class Base64
    {
        // browsers don´t seem to care for image/jpg or image/png
        private static readonly string Base64Prefix = "data:image/jpg;base64,";

        /// <summary>
        /// converts uploaded file to byte array
        /// </summary>
        /// <param name="image">request.files element</param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        /// <summary>
        /// convenient string preparation for use in img src attribute
        /// </summary>
        /// <param name="bin">image representation in byte array</param>
        /// <returns>string that can be passed to a View</returns>
        public static string ToBase64(byte[] bin)
        {
            // build a base64 string that can be used in <img src="" />
            return Base64Prefix + System.Convert.ToBase64String(bin);
        }
    }
}