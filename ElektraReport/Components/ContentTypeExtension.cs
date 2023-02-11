using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Components
{
    public class ContentTypeExtension
    {
        public static string GetContent(string fileName)
        {
            string res = "";
            var _f = fileName.Split(".");
            switch (_f.LastOrDefault()?.Trim().ToLower())
            {
                case "jpg":
                    res = "image/jpeg";
                    break;
                case "png":
                    res = "image/png";
                    break;
                case "pdf":
                    res = "application/pdf";
                    break;
                case "xls":
                    res = "application/vnd.ms-excel";
                    break;
                case "xlsx":
                    res = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                default:
                    res = "";
                    break;
            }

            return res;
        }

        public static FileType GetContentFileTypeEnum(string fileName)
        {
            FileType res = FileType.Other;
            var _f = fileName.Split(".");
            switch (_f.LastOrDefault()?.Trim().ToLower())
            {
                case "jpg":
                    res = FileType.Image;
                    break;
                case "png":
                    res = FileType.Image;
                    break;
                case "pdf":
                    res = FileType.Pdf;
                    break;
                case "xls":
                    res = FileType.Excel;
                    break;
                case "xlsx":
                    res = FileType.Excel;
                    break;
                default:
                    res = FileType.Other;
                    break;
            }

            return res;
        }

        public static bool CheckInline(string fileName)
        {
            bool res = false;
            var _f = fileName.Split(".");
            switch (_f.LastOrDefault())
            {
                case "jpg":
                    res = true;
                    break;
                case "png":
                    res = true;
                    break;
                case "pdf":
                    res = true;
                    break;
                case "xls":
                    res = false;
                    break;
                case "xlsx":
                    res = false;
                    break;
                default:
                    res = false;
                    break;
            }

            return res;
        }
    }


    public enum FileType
    {
        Other = 0,
        Image = 1,
        Pdf = 2,
        Excel = 3,
        Video = 4,
        Word = 5,
        Sound = 6
    }
}
