

using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;
using System.IO;

namespace SUIFW {
    public static class CommonFunc {

        public static string GenerateMD5Str(string value) {
            value = value.Trim();
            byte[] valueBytes=  System.Text.Encoding.UTF8.GetBytes(value);
            MD5 strMd5 = new MD5CryptoServiceProvider();
            byte[] ret= strMd5.ComputeHash(valueBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < ret.Length; i++) {
                sb.Append(ret[i].ToString("x2"));
            }                                   
            return sb.ToString();
        }

        public static string GenerateMD5StrByStream(string filePath) {
            if (string.IsNullOrEmpty(filePath)) {
                Debug.LogError("CommonFunc.cs/GenerateMD5StrByStream()/参数不能为空！！！");
                return null;
            }
            FileStream fs = new FileStream(filePath,FileMode.Open);
            MD5 strMd5 = new MD5CryptoServiceProvider();
            byte[] ret = strMd5.ComputeHash(fs);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < ret.Length; i++) {
                sb.Append(ret[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
