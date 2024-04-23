using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StudentTestPicker.Models
{
    class AllColections
    {
        public ObservableCollection<Colection> Students { get; set; } = new ObservableCollection<Colection>();
        public string NazwaKolekcji;

        public AllColections(string klasa)
        {
            NazwaKolekcji = klasa;
            LoadStudents(klasa);

        }

        public string getClassNumber()
        {
            return NazwaKolekcji;
        }

        public void LoadStudents(string klasa)
        {
            Students.Clear();

            string path = FileSystem.AppDataDirectory;
            List<Colection> students = new List<Colection>();

            if (File.Exists(Path.Combine(path, $"{klasa}.txt")))
            {
                string text = File.ReadAllText(Path.Combine(path, $"{klasa}.txt"));
                string[] studentsData = text.Split('\n');
                foreach (string s in studentsData)
                {
                    string[] Data = s.Split("\t");
                    if (Data.Length > 3)
                    {
                        students.Add(new Colection()
                        {
                            Number = int.Parse(Data[0]),
                            Nazwa = Data[1],
                            Opis = Data[2],
                            NazwaKolekcji = Data[3]
                        });
                    }
                }

                foreach (Colection student in students)
                {
                    Students.Add(student);
                }
            }
        }


        public Colection DrawStudent(string classNumber)
        {
            List<Colection> classStudents = new List<Colection>();
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{classNumber}.txt");
            foreach (Colection student in Students)
            {
                if (student.NazwaKolekcji == classNumber)
                {
                    classStudents.Add(student);
                }
            }
            if (classStudents.Count == 0)
            {
                return new Colection();
            }

            Random rnd = new Random();
            int randomIndex = rnd.Next(classStudents.Count);
            return classStudents[randomIndex];
        }


        public int AddStudent(string klasa, string name, string surname)
        {
            string path = FileSystem.AppDataDirectory;
            Colection student = new Colection()
            {
                Number  = Students.Count + 1,
                Nazwa = name,
                Opis = surname,
                NazwaKolekcji=klasa

            };

            if (!File.Exists(Path.Combine(path, $"{klasa}.txt")))
                return 1;
            string text = File.ReadAllText(Path.Combine(path, $"{klasa}.txt"));
            string studentData = text + $"\n{student.Number}\t{student.Nazwa}\t{student.Opis}\t{student.NazwaKolekcji}";

            File.WriteAllText(Path.Combine(path, $"{klasa}.txt"), studentData);
            Students.Add(student);

            return 0;
        }

        public int DeleteStudent(string klasa, string name, string surname)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{klasa}.txt");
            string new_text = "";
            string text = File.ReadAllText(path);
            string[] studentData = text.Split("\n");
            int counter = 1;
            for (int i = 0; i < studentData.Length; i++)
            {
                Colection s;
                string[] stD = studentData[i].Split("\t");
                if (stD.Length == 1)
                {
                    new_text += $"{stD[0]}";
                }
                else if (stD.Length > 2 && (stD[1] != name || stD[2] != surname))
                {
                    new_text += $"\n{counter}\t{stD[1]}\t{stD[2]}\t{stD[3]}";
                    counter++;
                }
            }

            File.WriteAllText(path, new_text);

            return 1;
        }
    }
}