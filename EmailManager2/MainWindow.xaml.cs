using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;
using System.IO;
using Microsoft.Win32;

namespace EmailManager2 {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    string login = @"..\..\Login.txt";
    string pass = @"..\..\password.txt";
    string ContactFile = @"..\..\contact.txt";
    bool attachent = false;
    int id = 0;

    List<Models.Contact> contacts = new List<Models.Contact>();
    Models.Contact SelectedContact;
    public MainWindow() {
      InitializeComponent();
      string[] res = GetText(ContactFile).Split('|');// инцализация контактов
      for (int i = 0; i < res.Length; i++, id++) {
        Models.Contact contact = new Models.Contact() {
          Id = id,
          Email = res[i].Split(':')[0],
          About = res[i].Split(':')[1]
        };
        contacts.Add(contact);
        ContactsList2.Items.Add(res[i].Split(':')[1]);
      }
    }

    private void SendButton_Click(object sender, RoutedEventArgs e) {//отправка сообщения
      try {
        MailAddress from = new MailAddress(GetText(login));
        MailAddress to = new MailAddress(ToField.Text);
        MailMessage mess = new MailMessage(from, to);

        mess.Subject = SubjectField.Text;
        mess.Body = BodyField.Text;
        if (attachent) {
          for (int i = 0; i < FilesList2.Items.Count; i++) {
            Attachment att = new Attachment(FilesList2.Items[i].ToString());
            mess.Attachments.Add(att);
          }
        }
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        client.Credentials = new NetworkCredential(GetText(login), GetText(pass));
        client.EnableSsl = true;
        client.Send(mess);
        MessageBox.Show("Сообщение отправлено!");
      } catch (Exception ex) {

        MessageBox.Show("Ошибка: " + ex.Message);
      }
    }
    static string GetText(string path) {
      string passw = "";
      using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
        using (StreamReader sr = new StreamReader(fs)) {
          passw = sr.ReadLine();
        }
      }
      return passw;
    }

    private void SelectButton_Click(object sender, RoutedEventArgs e) {
      try {
        SelectedContact = contacts[ContactsList2.SelectedIndex];
        ToField.Text = SelectedContact.Email;
      } catch (Exception ex) {
        MessageBox.Show("Ошибка: " + ex.Message);
      }
    }

    private void AddFilesButton_Click(object sender, RoutedEventArgs e) {//добавление файлов
      try {
        OpenFileDialog openFile = new OpenFileDialog();
        openFile.ShowDialog();
        string path = openFile.FileName;
        FilesList1.Items.Add(path);
      } catch (Exception err) {
        MessageBox.Show("Ошибка: " + err.Message);
      }
    }

    private void SaveFilesButton_Click(object sender, RoutedEventArgs e) {//сохранение файлов
      try {
        FilesList2.Items.Add(FilesList1.SelectedItem);
        attachent = true;
      } catch (Exception err) {
        MessageBox.Show("Ошибка: " + err.Message);
      }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e) {//чистка файлов
      try {
        FilesList2.Items.Clear();
        if (FilesList2.Items.Count == 0) {
          attachent = false;
        }
      } catch (Exception err) {
        MessageBox.Show("Ошибка: " + err.Message);
      }
    }
  }
}
