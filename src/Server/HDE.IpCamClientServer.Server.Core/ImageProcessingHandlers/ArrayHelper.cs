using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public static class ArrayHelper
    {
        public static double GetStandardDeviation(double[] data)
        {
            if (data.Length == 0)
            {
                return 0;
            }

            var average = data.Average();
            return data.Sum(item => Math.Abs(item - average))/data.Length;
        }

        public static byte GetMedian(byte[] bytes)
        {
            Array.Sort(bytes);
            return bytes[bytes.Length/2];
        }

        public static bool Compare(byte[,] a, byte[,] b, int thethhold)
        {
            if (a.GetLength(0) != b.GetLength(0) ||
                a.GetLength(1) != b.GetLength(1))
            {
                return false;
            }

            for (int dim1 = 0; dim1 < a.GetLength(0); dim1++)
                for (int dim2 = 0; dim2 < a.GetLength(1); dim2++)
                {
                    if (Math.Abs(a[dim1, dim2] - b[dim1, dim2]) > thethhold)
                    {
                        return false;
                    }
                }
            return true;
        }

        public static byte[,] FromFile(string file)
        {
            var data = File.ReadAllLines(file);
            var width = data[0].Split('\t').Length;
            byte[,] dataWidthHeight = new byte[width, data.Length];
            for (int heightI = 0; heightI < data.Length; heightI++)
            {
                var splited = data[heightI].Split('\t');
                for (int widthI = 0; widthI < width; widthI++)
                {
                    dataWidthHeight[widthI, heightI] = byte.Parse(splited[widthI], CultureInfo.InvariantCulture);
                }
            }
            return dataWidthHeight;
        }

        public static void ToFile(byte[,] dataWidthHeight, string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            var builder = new StringBuilder();
            var width = dataWidthHeight.GetLength(0);
            var height = dataWidthHeight.GetLength(1);

            for (int heightI = 0; heightI < height; heightI++)
            {
                for (int widthI = 0; widthI < width; widthI++)
                {
                    builder.Append(dataWidthHeight[widthI, heightI].ToString(CultureInfo.InvariantCulture));
                    if (widthI != width - 1)
                    {
                        builder.Append("\t");
                    }
                }

                if (heightI != height -1)
                {
                    builder.AppendLine();
                }
            }
            File.WriteAllText(file, builder.ToString());
        }

        public static void SetToAll<TData>(TData[] energyBackground, TData defaultValue)
        {
            for (int i = 0; i < energyBackground.Length; i++)
            {
                energyBackground[i] = defaultValue;
            }
        }
    }
}