using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace ARQFlow
{
    public class App : IExternalApplication
    {
        // Esse é o "ajudante" que coloca a imagem no botão
        private void SetIcon(PushButton button, string iconPath)
        {
            if (!File.Exists(iconPath))
                return;

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(iconPath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze(); // Importante para não travar o arquivo no Windows

            button.LargeImage = bitmap; // Ícone grande (32x32)
            button.Image = bitmap;      // Ícone pequeno (16x16)
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // 1. Cria uma aba personalizada
            string tabName = "ARQFlow";
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch { /* Aba já existe */ }

            // 2. Cria um painel dentro da aba
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Ferramentas");

            // 3. Define os caminhos (Onde está a DLL e a pasta de ícones)
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string dllFolder = Path.GetDirectoryName(assemblyPath);

            // 4. Configura os dados do botão
            PushButtonData buttonData = new PushButtonData(
                "btnHelloWorld",
                "Hello\nWorld",
                assemblyPath,
                "ARQFlow.Commands.Command"
            );

            // 5. ADICIONA O BOTÃO AO PAINEL
            // Note que guardamos o botão na variável 'myButton' para poder colocar o ícone depois
            PushButton myButton = panel.AddItem(buttonData) as PushButton;

            // 6. COLOCA O ÍCONE
            // Ele vai procurar na pasta: %AppData%\Autodesk\Revit\Addins\2025\ResourcesARQFlow\hello-word.ico
            string iconFullPath = Path.Combine(dllFolder, "ResourcesARQFlow", "hello-word.ico");
            SetIcon(myButton, iconFullPath);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}