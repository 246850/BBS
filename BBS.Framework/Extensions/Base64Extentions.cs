using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BBS.Framework.Extensions
{
    public static class Base64Extentions
    {
        /// <summary>
        /// Base64上传
        /// </summary>
        /// <param name="base64">图片base</param>
        /// <param name="webRootPath">静态决定路径</param>
        /// <param name="folder">上传文件夹</param>
        /// <returns></returns>
        public static string Upload(this string base64, string webRootPath, string folder)
        {
            var m = Regex.Match(base64, ".+image/(.+?);base64,");
            if (!m.Success) throw new Exception("不支持的图片上传格式");

            string fileName = string.Format("{0}/{1}.{2}", DateTime.Now.ToString("yyyyMMdd"),Guid.NewGuid(), m.Groups[1].Value);
            string vituralPath = Path.Combine("upload", folder, fileName);
            string absolutePath = Path.Combine(webRootPath, vituralPath);
            byte[] bytes = Convert.FromBase64String(Regex.Replace(base64, ".+image/.+?;base64,", string.Empty));

            string path = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(absolutePath, bytes);
            return "/" + vituralPath;

        }
    }
}
