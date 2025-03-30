using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

class MainMenuImage
{
    private static readonly List<string> ImagePaths = new List<string>();
    public static string Https = "https://www.loliapi.com/acg/pc/";
    public static int imgnum = 10;
   public static string FolderPath()
    {
        Process[] processes = Process.GetProcessesByName("Among Us");
        if (processes.Length == 0)
        {
            System.Console.WriteLine("未找到应用程序”Among Us");
            return null;
        }

        string exePath = processes[0].MainModule.FileName;
        string exeDir = Path.GetDirectoryName(exePath);
        string storageDir = Path.Combine(exeDir, $"ACG");
        return storageDir;
    }


    public static async Task Load()
    {

        try
        {
            
            Directory.CreateDirectory(FolderPath());
            System.Console.WriteLine($"创建了文件夹: {FolderPath()}");

            // 下载图片
            using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < imgnum; i++)
                {
                    try
                    {
                        byte[] imageData = await client.GetByteArrayAsync(Https);
                        string fileName = $"image_{(i + 1)}.jpg";
                        string filePath = Path.Combine(FolderPath(), fileName);

                        if (!System.IO.File.Exists(filePath))
                        {
                            // 使用FileStream异步写入
                            using (FileStream fs = new FileStream(
                                filePath,
                                FileMode.CreateNew,
                                FileAccess.Write,
                                FileShare.None,
                                bufferSize: 4096,
                                useAsync: true))
                            {
                                await fs.WriteAsync(imageData, 0, imageData.Length).ConfigureAwait(false);
                            }

                            ImagePaths.Add(filePath);
                            System.Console.WriteLine($"正在下载: {filePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"下载失败：{i + 1}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"错误信息: {ex.Message}");
        }
    }
}