using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.IO.Compression;
using System.Runtime.ExceptionServices;
using System.Threading;


/*
 * 1.	Вывести информацию в консоль о логических дисках, именах, метке тома, размере типе файловой системы. 
2.	Работа с файлами ( класс File, FileInfo, FileStream и другие) 
•	Создать файл 
•	Записать в файл строку 
•	Прочитать файл в консоль 
•	Удалить файл 
3.	Работа с форматом JSON 
•	Создать файл формате JSON из редактора в 
•	Создать новый объект. Выполнить сериализацию объекта в формате JSON и записать в файл. 
•	Прочитать файл в консоль 
•	Удалить файл 
4.	Работа с форматом XML 
•	Создать файл формате XML из редактора 
•	Записать в файл новые данные из консоли . 
•	Прочитать файл в консоль. 
•	Удалить файл. 
5.	Создание zip архива, добавление туда файла, определение размера архива 
•	Создать архив в форматер zip 
•	Добавить файл в архив 
•	Разархивировать файл и вывести данные о нем 
•	Удалить файл и архив 
*/

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string company { get; set; }
}

class Program
{

    static void FirstTask()
    {
        DriveInfo[] drives = DriveInfo.GetDrives();
        foreach (DriveInfo drive in drives)
        {
            Console.WriteLine($"Название: {drive.Name}");
            Console.WriteLine($"Тип: {drive.DriveType}");
            if (drive.IsReady)
            {
                Console.WriteLine($"Объем диска: {drive.TotalSize}");
                Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                Console.WriteLine($"Метка: {drive.VolumeLabel}");
            }
            Console.WriteLine();

        }
    }

    static void SecondTask()
    {

        string path = @"C:\Test";

        if (Directory.Exists(path))
        {
            Console.Write("Choose file name: ");
            string fi = Console.ReadLine();
            string file = @"\" + fi + ".txt";
            if (File.Exists(path + file)) File.Delete(path + file);

            StreamWriter fileW = new StreamWriter(new FileStream(path + file, FileMode.OpenOrCreate));
            Console.Write("Enter text: ");
            string text = Console.ReadLine();
            fileW.Write(text);
            fileW.Close();


            StreamReader fileR = new StreamReader(new FileStream(path + file, FileMode.OpenOrCreate));
            Console.Write("Your text from file: {0}", fileR.ReadToEnd());
            Console.WriteLine();

            fileR.Close();

            while (true)
            {
                Console.Write("Do you wanna delete file?[y\\n]: ");
                string answer = Console.ReadLine();
                if (answer.ToLower() == "y")
                {
                    File.Delete(path + file);
                    break;
                }
                else if (answer.ToLower() == "n") break;
                Console.WriteLine("Error: Incorrect answer. Try again.");
            }

        }

        Console.WriteLine();

    }

    static async Task ThirdTask()
    {

        string file = @"\user.json";
        string path = @"C:\Test";



        if (Directory.Exists(path))
        {

            using (FileStream fs = new FileStream(path + file, FileMode.OpenOrCreate))
            {
                Console.Write("Enter the name: ");
                string name = Console.ReadLine();
                int age = 0;

                while (true)
                {
                    Console.Write("Enter age: ");
                    try
                    {
                        age = Int32.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Error. Incorrect input. Try again.");
                    }
                }
                Person tom = new Person() { Name = name, Age = age };
                await JsonSerializer.SerializeAsync<Person>(fs, tom);
                Thread.Sleep(1000);
                Console.WriteLine("Data has been saved to file");
                fs.Close();
            }


            using (FileStream fs = new FileStream(path + file, FileMode.OpenOrCreate))
            {
                Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
                Thread.Sleep(1000);
                Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
                fs.Close();
            }
        }

        while (true)
        {
            Console.Write("Do you wanna delete file?[y\\n]: ");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                File.Delete(path + file);
                break;
            }
            else if (answer.ToLower() == "n") break;
            Console.WriteLine("Error: Incorrect answer. Try again.");
        }

        Console.WriteLine();


    }

    static void FourthTask()
    {

        string file = @"\note.xml";
        string path = @"C:\Test";

        if (Directory.Exists(path))
        {

            int choice = 0;
            string name, text;

            XDocument xdoc = new XDocument();
            XElement users = new XElement("users");

            while (true)
            {

                Console.Write("1. Add new Element.\n2. End adding.\nYour answer [1/2]: ");

                try
                {
                    choice = Int32.Parse(Console.ReadLine());
                    if (choice < 1 || choice > 2) throw new Exception();
                }
                catch
                {
                    Console.WriteLine("Error. Incorrect input. Try again.");
                }

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter the name of node: ");
                        name = Console.ReadLine();
                        Console.Write("Enter the text in node: ");
                        text = Console.ReadLine();
                        XElement xElement = new XElement(name, text);
                        users.Add(xElement);
                        break;
                    case 2:
                        xdoc.Add(users);
                        xdoc.Save(path + file);
                        StreamReader fr = new StreamReader(new FileStream(path + file, FileMode.OpenOrCreate));
                        Console.WriteLine(fr.ReadToEnd());
                        fr.Close();
                        break;
                }
                if (choice == 2) break;
            }

            while (true)
            {
                Console.Write("Do you wanna delete file?[y\\n]: ");
                string answer = Console.ReadLine();
                if (answer.ToLower() == "y")
                {
                    File.Delete(path + file);
                    break;
                }
                else if (answer.ToLower() == "n") break;
                Console.WriteLine("Error: Incorrect answer. Try again.");
            }
        }

        Console.WriteLine();

    }

    static void FifthTask()
    {
        string path = @"C:\Test";
        string zipFile = @"\test.zip";

        if (File.Exists(path + zipFile)) File.Delete(path + @"\test.zip");
        if (File.Exists(path + @"\new_jaj.txt")) File.Delete(path + @"\new_jaj.txt");
        FileStream f = new FileStream(path + @"\jaj.txt", FileMode.Create);
        f.Close();

        using (ZipArchive zipArchive = ZipFile.Open(path + zipFile, ZipArchiveMode.Create))
        {

            const string pathFileToAdd = @"C:\Test\jaj.txt";
            const string nameFileToAdd = "jaj.txt";
            zipArchive.CreateEntryFromFile(pathFileToAdd, nameFileToAdd);

        }

        using (ZipArchive zipArchive = ZipFile.OpenRead(path + zipFile))
        {
            const string nameExtractFile = "jaj.txt";
            const string pathExtractFile = @"C:\Test\new_jaj.txt";

            zipArchive.Entries.FirstOrDefault(x => x.Name == nameExtractFile)?.
                ExtractToFile(pathExtractFile);
        }

        while (true)
        {
            Console.Write("Do you wanna delete file?[y\\n]: ");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                File.Delete(path + @"\test.zip");
                File.Delete(path + @"\new_jaj.txt");
                break;
            }
            else if (answer.ToLower() == "n") break;
            Console.WriteLine("Error: Incorrect answer. Try again.");
        }

    }



    static void Main()
    {

        FirstTask();
        SecondTask();
        ThirdTask();
        FourthTask();
        FifthTask();

    }
}