using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {   
        public static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "Resources\\";
        public static readonly string FilesDirectory = BaseDirectory + "Files\\";
        public static readonly string TestDirectory = BaseDirectory + "Test\\";
        public static readonly string StudentDirectory = BaseDirectory + "Students\\";

        public static readonly string ImagesDirectory = BaseDirectory + "Images\\";
        int quection_count;
        int correct_answers;
        int wrong_answers;

        string student;
        string group;

        string imgCreated = null;
        string currFile = null;
        string trueAnswer = "1";
        string confimedQComplex;

        string[] array;

        int correct_answers_number;
        int selected_response;


        StreamReader Read;
        Bitmap PreMainImage;


        public Form1()
        {
            InitializeComponent();

            string path = BaseDirectory;
            var dir = new DirectoryInfo(path);
            var files = new List<string>();

            foreach (FileInfo file in dir.GetFiles("*.txt"))
            {
                files.Add(Path.GetFileName(file.Name));
            }
            foreach (string str in files)
            {
                listOfTest.Items.Add(str.Replace(".txt", ""));
            }


            string path3 = StudentDirectory + "\\";
            var dir3 = new DirectoryInfo(path3);
            var files3 = new List<string>();

            foreach (FileInfo file3 in dir3.GetFiles("*.txt"))
            {
                files3.Add(Path.GetFileName(file3.Name));
            }
            foreach (string str3 in files3)
            {
                listOfGroup.Items.Add(str3.Replace(".txt", ""));
            }
        }


        void start()
        {
            if (currFile != null)
            {
                var Encoding = System.Text.Encoding.GetEncoding(65001);
                try
                {

                    Read = new StreamReader(BaseDirectory + currFile, Encoding);
                    this.Text = Read.ReadLine();

                    quection_count = 0;
                    correct_answers = 0;
                    wrong_answers = 0;

                    array = new String[10];
                }
                catch (Exception)
                {
                    MessageBox.Show("ошибка 1");
                }
                вопрос();
            }
        }


        void вопрос()
        {
            label1.Text = Read.ReadLine();

            string infoLine = Read.ReadLine();
            string nextLine;
            FileChecker currentImage = new FileChecker();
            currentImage.FileCheck(infoLine, FilesDirectory);
            if (currentImage.FileCheck(infoLine, FilesDirectory))
            {
                imgBox.Visible = true;
                imgBox.Image = new Bitmap(currentImage.checkedFile);
                nextLine = Read.ReadLine();
                label1.Location = new Point(30, 265);
                button1.Location = new Point(180, 355);
            }

            else
            {
                nextLine = Read.ReadLine();
                imgBox.Visible = false;
                label1.Location = new Point(30, 170);
                button1.Location = new Point(180, 305);

            }

            radioButton1.Text = nextLine;
            radioButton2.Text = Read.ReadLine();
            radioButton3.Text = Read.ReadLine();

            correct_answers_number = int.Parse(Read.ReadLine());


            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
       
            button1.Enabled = false;
            quection_count = quection_count + 1;
            button1.Text = "Выберите ответ";

            if (Read.EndOfStream == true) button1.Text = "Завершить";

        }

        void conditionChanged(object sender, EventArgs e)
        {
          
            button1.Enabled = true; button1.Focus();
            RadioButton Переключатель = (RadioButton)sender;
            var tmp = Переключатель.Name;
           
            selected_response = int.Parse(tmp.Substring(11));
        }







        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (button1.Text != "Завершить")
                button1.Text = "Следующий вопрос";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (button1.Text != "Завершить")
                button1.Text = "Следующий вопрос";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (button1.Text != "Завершить")
                button1.Text = "Следующий вопрос";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            imgBox.Image = null;
            if (selected_response == correct_answers_number) 
                correct_answers = correct_answers + 1;
            if (selected_response != correct_answers_number)
            {
               
                wrong_answers = wrong_answers + 1;
                
                array[wrong_answers] = label1.Text;
            }
            if (button1.Text == "Начать тестирование сначала")
            {
                button1.Text = "Следующий вопрос";
              
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
              
                start(); return;
            }
            if (button1.Text == "Завершить")
            {
                listOfGroup.Enabled = true;
                listOfStudents.Enabled = true;
                Directory.CreateDirectory(TestDirectory + listOfTest.Text);
                File.WriteAllText(TestDirectory + "\\" + listOfTest.Text + "\\" + student  + " " + group + ".txt", txtBoxFIO.Text + "\n" + "Группа: " + txtBoxGroup.Text + "\n" + "Правильных ответов: " + correct_answers + " из " + quection_count + "\n" + "Баллов: " + (correct_answers * 5.0F) / quection_count);
                Read.Close();
                
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
               
                label1.Text = String.Format("Результаты теста:\n" +
                    "Правильных ответов: {0} из {1}.\n" +
                    "Набранные баллы: {2:F2}.", correct_answers,
                    quection_count, (correct_answers * 5.0F) / quection_count);
              
                button1.Text = "Начать тестирование сначала";
                bttStart.Visible = true;
                bttCreate.Enabled = true;

                var Str = "Вы ошиблись в следующих вопросах" +
                          ":\n\n";
                for (int i = 1; i <= wrong_answers; i++)
                    Str = Str + array[i] + "\n";

             
                if (wrong_answers != 0) MessageBox.Show(
                                          Str, "Тестирование завершено");
            } 
            if (button1.Text == "Следующий вопрос") вопрос();

        }

        private void button2_Click(object sender, EventArgs e)
        {


            this.Close();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Следующий вопрос";
            button2.Text = "Выход";
          
            radioButton1.CheckedChanged += new EventHandler(conditionChanged);
            radioButton2.CheckedChanged += new EventHandler(conditionChanged);
            radioButton3.CheckedChanged += new EventHandler(conditionChanged);
            start();

        }

        private void bttStart_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            listOfGroup.Enabled = false;
            listOfStudents.Enabled = false;
            Width = 790;
            Height = 490;
            bttCreate.Enabled = false;
            bttStart.Visible = false;
            radioButton1.Visible = true;
            radioButton2.Visible = true;
            radioButton3.Visible = true;
            button1.Visible = true;

            start();
        }

        private void listOfTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            bttStart.Enabled = true;
            currFile = @"\" + listOfTest.Text + ".txt";
            button1.Text = "Следующий вопрос";
        }

        private void bttQuestionEnd_Click(object sender, EventArgs e)
        {
            /*
            string question = txtboxQuestion.Text;
            string image = imgCreated; 
            string answer1 = txtboxAnswer1.Text;
            string answer2 = txtboxAnswer2.Text;
            string answer3 = txtboxAnswer3.Text;
            */
            string qComplex = txtboxQuestion.Text + "\n" + imgCreated + "\n" + txtboxAnswer1.Text + "\n" + txtboxAnswer2.Text + "\n" + txtboxAnswer3.Text + "\n" + trueAnswer + "\n";
            confimedQComplex = confimedQComplex + qComplex;

            /*
            Write = new StreamWriter(BaseDirectory + txtboxName.Text + ".txt");
            Write.WriteLine(qComplex);
            Write.Close();
            */
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            trueAnswer = "1";
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            trueAnswer = "2";
        }

        private void rb3_CheckedChanged(object sender, EventArgs e)
        {
            trueAnswer = "3";
        }

        private void bttCreateTest_Click(object sender, EventArgs e)
        {
        }

        private void bttTestEnd_Click(object sender, EventArgs e)
        {
            File.WriteAllText(BaseDirectory + txtboxName.Text + ".txt", txtboxName.Text + "\n" + confimedQComplex);
            listOfTest.Items.Clear();
            string path = BaseDirectory;
            var dir = new DirectoryInfo(path);
            var files = new List<string>();

            foreach (FileInfo file in dir.GetFiles("*.txt"))
            {
                files.Add(Path.GetFileName(file.Name));
            }
            foreach (string str in files)
            {
                listOfTest.Items.Add(str.Replace(".txt", ""));
            }
        }

        private void bttAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenMainImg = new OpenFileDialog();
            OpenMainImg.Filter = "Image Files(*.BPM;*.JPG;*.PNG;*.GIF)| *.BPM; *.JPG; *.PNG; *.GIF|All files (*.*)|*.*";

            if (OpenMainImg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PreMainImage = null;
                    picBoxLoad.Image = null;
                    picBoxLoad.BackgroundImage = null;
                    FileInfo img = new FileInfo(OpenMainImg.FileName);
                    string imgg = OpenMainImg.FileName;
                    string myImage = OpenMainImg.SafeFileName;
                    File.Copy(imgg, FilesDirectory + myImage, true);
                    Bitmap G = new Bitmap(FilesDirectory + myImage);
                    PreMainImage = new Bitmap(G);
                    picBoxLoad.Image = G;
                    imgCreated = myImage;
                }
                catch
                {
                    MessageBox.Show("Изображение уже загружено)", "Ошибочка вышла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bttNull_Click(object sender, EventArgs e)
        {
            picBoxLoad.BackgroundImage = null;
            picBoxLoad.Image = null;
            imgCreated = null;
        }

        private void bttCreate_Click(object sender, EventArgs e)
        {
            pnlCreate.Enabled = !pnlCreate.Enabled;
            pnlCreate.Visible = !pnlCreate.Visible;
            pnlRegistration.Enabled = !pnlRegistration.Enabled;
            pnlRegistration.Visible = !pnlRegistration.Visible;
            Width = 790;
            Height = 520;
            button1.Visible = false;
        }

        private void bttCreateStudent_Click(object sender, EventArgs e)
        {
            listOfStudents.Items.Clear();
            listOfGroup.Items.Clear();
            Directory.CreateDirectory(StudentDirectory + txtBoxGroup.Text);
            File.Create(StudentDirectory + txtBoxGroup.Text + ".txt");
            File.WriteAllText(StudentDirectory + "\\" + txtBoxGroup.Text + "\\" + txtBoxFIO.Text + ".txt", txtBoxFIO.Text + " " + txtBoxGroup.Text);
            string path = StudentDirectory + txtBoxGroup.Text;
            var dir = new DirectoryInfo(path);
            var files = new List<string>();

            string path2 = StudentDirectory + "\\" + group + "\\";
            var dir2 = new DirectoryInfo(path2);
            var files2 = new List<string>();

            foreach (FileInfo file2 in dir2.GetFiles("*.txt"))
            {
                files2.Add(Path.GetFileName(file2.Name));
            }
            foreach (string str2 in files2)
            {
                listOfStudents.Items.Add(str2.Replace(".txt", ""));
            }

            string path3 = StudentDirectory + "\\";
            var dir3 = new DirectoryInfo(path3);
            var files3 = new List<string>();

            foreach (FileInfo file3 in dir3.GetFiles("*.txt"))
            {
                files3.Add(Path.GetFileName(file3.Name));
            }
            foreach (string str3 in files3)
            {
                listOfGroup.Items.Add(str3.Replace(".txt", ""));
            }
        }

        private void bttAdmin_Click(object sender, EventArgs e)
        {
            pnlRegistration.Enabled = !pnlRegistration.Enabled;
            pnlRegistration.Visible = !pnlRegistration.Visible;
            bttCreate.Enabled = !bttCreate.Enabled;
            bttCreate.Visible = !bttCreate.Visible;
            Width = 790;
            Height = 520;
            button1.Visible = false;
        }

        private void listOfStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            student = listOfStudents.Text;
        }

        private void listOfGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            group = listOfGroup.Text;

            listOfStudents.Items.Clear();

            string path2 = StudentDirectory + "\\" + group + "\\";
            var dir2 = new DirectoryInfo(path2);
            var files2 = new List<string>();

            foreach (FileInfo file2 in dir2.GetFiles("*.txt"))
            {
                files2.Add(Path.GetFileName(file2.Name));
            }
            foreach (string str2 in files2)
            {
                listOfStudents.Items.Add(str2.Replace(".txt", ""));
            }
        }

        private void bttViewResult_Click(object sender, EventArgs e)
        {
            txtBoxResult.Text = null;

            if (File.Exists(TestDirectory + listOfTest.Text + "\\" + student + " " + group + ".txt"))
            {
                StreamReader resulter = new StreamReader(TestDirectory + listOfTest.Text + "\\" + student + " " + group + ".txt");
                txtBoxResult.Text = (resulter.ReadLine() + "\n " + resulter.ReadLine() + "\n " + resulter.ReadLine() + "\n " + resulter.ReadLine());
            }
            else
                txtBoxResult.Text = "Данный студент не проходил этот тест";
        }

        private void bttDelete_Click(object sender, EventArgs e)
        {
            txtBoxResult.Text = null;

            if (File.Exists(StudentDirectory + group + "\\" + student + ".txt"))
            {
                File.Delete(StudentDirectory + group + "\\" + student + ".txt");
                txtBoxResult.Text = (student + " был удалён");
            }
            else
                txtBoxResult.Text = "Данный студент не существует";
        }
    }
}
