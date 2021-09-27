using FileScanner.Commands;
using FileScanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace FileScanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string fileImagePath = "/Views/file.png";
        private string folderImagePath = "/Views/folder.jpg";
        private string selectedFolder;
        private ObservableCollection<FolderItem> folderItems = new ObservableCollection<FolderItem>();

        public DelegateCommand<string> OpenFolderCommand { get; private set; }
        public DelegateCommand<string> ScanFolderCommand { get; private set; }

        public ObservableCollection<FolderItem> FolderItems
        {
            get => folderItems;
            set
            {
                folderItems = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFolder
        {
            get => selectedFolder;
            set
            {
                selectedFolder = value;
                OnPropertyChanged();
                ScanFolderCommand.RaiseCanExecuteChanged();
            }
        }

        public MainViewModel()
        {
            OpenFolderCommand = new DelegateCommand<string>(OpenFolder);
            ScanFolderCommand = new DelegateCommand<string>(ScanFolder, CanExecuteScanFolder);

            //SelectedFolder = "C:\\Users\\douce\\Desktop";
        }

        private bool CanExecuteScanFolder(string obj)
        {
            return !string.IsNullOrEmpty(SelectedFolder);
        }

        private void OpenFolder(string obj)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SelectedFolder = fbd.SelectedPath;
                }
            }
        }

        private async void ScanFolder(string dir)
        {
            FolderItems = new ObservableCollection<FolderItem>();
            DirectoryInfo myRoot = new DirectoryInfo(dir);
            string[] myDir = Directory.GetDirectories(dir);

            await recursiveLoop(myDir);
            addFile(myRoot);
            
            System.Windows.MessageBox.Show("" + FolderItems.Count);

        }

        private void addFile(DirectoryInfo myDir)
        {
            string myPath = "";

            foreach (FileInfo file in myDir.GetFiles())
            {
                try
                {
                    myPath = file.ToString();

                    folderItems.Add(new FolderItem(myPath, fileImagePath));
                }
                catch(Exception c)
                {
                    Console.WriteLine("Error: " + c.Message);
                }
            }
        }

        private async Task recursiveLoop(string[] subdir)
        {
            foreach (string s in subdir)
            {
                try
                {
                    folderItems.Add(new FolderItem(s, folderImagePath)); // Folder HERE

                    string[] myDir = Directory.GetDirectories(s);
                    await recursiveLoop(myDir);

                    DirectoryInfo mySub = new DirectoryInfo(s);

                    string myPath = "";

                    foreach (FileInfo file in mySub.GetFiles())
                    {
                        try
                        {
                            myPath = file.ToString();

                            folderItems.Add(new FolderItem(myPath, fileImagePath));
                        }
                        catch (Exception c)
                        {
                            Console.WriteLine("Error: " + c.Message);
                        }
                    }
                }
                catch (Exception c)
                {
                    Console.WriteLine("Error: " + c.Message);
                }
            }
        }

        IEnumerable<string> GetDirs(string dir)
        {
            foreach (var d in Directory.EnumerateDirectories(dir, "*"))
            {
                yield return d;
            }
        }

        ///TODO : Tester avec un dossier avec beaucoup de fichier
        ///TODO : Rendre l'application asynchrone
        ///TODO : Ajouter un try/catch pour les dossiers sans permission

    }
}