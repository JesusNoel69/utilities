using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;

#if WINDOWS
using Windows.Storage.Pickers;
using WinRT.Interop;
#endif
namespace utilities.Views
{
    public partial class FileManagerPage
    {
        private string route = "C:\\Windows\\Temp";
        private List<string> errors = new();

        private List<string> filesToDelete = new();


        public FileManagerPage()
        {
            InitializeComponent();
            Route.Text=route;
            GetPaths();
        }
        public void GetPaths()
        {
            Paths.Text = "";
            filesToDelete=[];
            //obtain paths
            filesToDelete = [.. Directory.GetFiles(Route.Text, "*")];
            foreach (string path in filesToDelete)
            {
                Paths.Text+=path+"\n";    
            }
        }

        public void OnDeleteFiles_Clicked(object? sender, EventArgs e){
            route = Route.Text;
            if (string.IsNullOrWhiteSpace(route))
            {
                Console.WriteLine("Path should not be null");
                return;
            }
            GetPaths(); 
            foreach (string path in filesToDelete)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        Console.WriteLine("File deleted.");
                    }
                    else
                    {
                        Console.WriteLine("File does not exist.");
                    }
                        
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("No permissions for delete.");
                    AddError("No permissions");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"E/S Error: {ex.Message}");
                    AddError(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error has ocurred: {ex.Message}");
                    AddError(ex.Message);
                }
            }
            GetPaths();
            Errors.Text = string.Join("", errors);
            errors = new(); // limpiar después si quieres
        }
        public void AddError(string message)
        {
            errors.Add(message+"\n");
        }
        

        private async void OnFindFileButton_Clicked(object sender, EventArgs e)
        {
        #if WINDOWS
            var folderPicker = new FolderPicker();

            // necesario para WinUI
            var hwnd = WindowNative.GetWindowHandle(App.Current.Windows[0].Handler.PlatformView);
            InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.FileTypeFilter.Add("*");

            var folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                Route.Text = folder.Path;
                GetPaths();
            }
        #endif
        }
    }
}