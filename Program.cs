using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;

// Suppress spell-check warning for project name 'AnonPDF'
#pragma warning disable SPELL
namespace AnonPDF
{
    internal static class Program
    {
        private const string AppProjectExtension = ".app";
        private const string AppProjectProgId = "AnonPDFPro.Project";
        private const int ShcneAssocChanged = 0x08000000;
        private const uint ShcnfIdList = 0x0000;
        private const uint AttachParentProcess = 0xFFFFFFFF;
        private const int StandardErrorHandle = -12;
        private static bool commandLineConsoleInitialized;
        private static CultureInfo commandLineCulture = CultureInfo.GetCultureInfo("en");

        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(int wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern uint GetConsoleOutputCP();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            // Global exception handler for all UI threads
            Application.ThreadException += (sender, e) =>
            {
                LogUnhandledException(e.Exception, "ThreadException");
                ShowError(e.Exception);
            };

            // Handler for unhandled exceptions in non‑UI threads and background tasks
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                string fallback = Properties.Resources.ResourceManager.GetString(
                    "Err_UnknownExceptionFallback",
                    Properties.Resources.Culture ?? System.Globalization.CultureInfo.CurrentUICulture) ?? "Err_UnknownExceptionFallback";
                Exception ex = e.ExceptionObject as Exception ?? new Exception(fallback);
                LogUnhandledException(ex, "UnhandledException");
                ShowError(ex);
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartupOptions startupOptions = ParseStartupOptions(args);
            ConfigureCommandLineCulture(ref startupOptions);
            if (!string.IsNullOrWhiteSpace(startupOptions.ParseError))
            {
                WriteCommandLineMessage(startupOptions.ParseError + Environment.NewLine + GetCommandLineHelp());
                Environment.ExitCode = 2;
                return;
            }
            if (startupOptions.ShowHelp)
            {
                WriteCommandLineMessage(GetCommandLineHelp());
                return;
            }
            if (!ValidateHeadlessOptions(startupOptions, out string startupError, out int startupExitCode))
            {
                WriteCommandLineMessage(startupError);
                Environment.ExitCode = startupExitCode;
                return;
            }

            EnsureContextMenuRegistration();
            LicenseManager.Initialize(AppDomain.CurrentDomain.BaseDirectory);
            if (!ValidateRequiredLicenseFiles(out string licenseError))
            {
                if (startupOptions.Headless)
                {
                    WriteCommandLineMessage(licenseError);
                    Environment.ExitCode = 5;
                    return;
                }
                MessageBox.Show(
                    licenseError,
                    Properties.Resources.Title_Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (startupOptions.Headless)
            {
                RunHeadlessTemplateBatch(startupOptions);
                return;
            }

            bool hasStartupInputPath = !string.IsNullOrWhiteSpace(startupOptions.InputPath);
            // Normal startup keeps SplashForm alive while PDFForm runs its first Load/Shown layout.
            // Keep the same DPI bootstrap for command-line startup without showing the splash window.
            SplashForm dpiBootstrapSplash = hasStartupInputPath ? new SplashForm() : null;
            SplashForm splash = hasStartupInputPath ? null : new SplashForm();
            var mainForm = new PDFForm(splash);
            if (hasStartupInputPath)
                mainForm.SuppressStartupUpdateCheck = true;
            if (splash != null)
            {
                splash.OpenPdfRequested += (_, __) => mainForm.OpenPdfFromSplash();
                splash.OpenProjectRequested += (_, __) => mainForm.OpenProjectFromSplash();
                splash.ResumeWorkRequested += (_, __) => mainForm.ResumeWorkFromSplash();
                splash.Owner = mainForm;
                splash.Show();
                Application.DoEvents();
            }

            if (hasStartupInputPath)
            {
                mainForm.Shown += (_, __) =>
                {
                    QueueStartupInputOpen(mainForm, startupOptions, dpiBootstrapSplash);
                };
            }

            mainForm.FormClosed += (_, __) =>
            {
                if (dpiBootstrapSplash != null && !dpiBootstrapSplash.IsDisposed)
                {
                    dpiBootstrapSplash.Dispose();
                }
                if (splash != null && !splash.IsDisposed)
                {
                    splash.Close();
                }
            };

            Application.Run(mainForm);
        }

        private static void QueueStartupInputOpen(PDFForm mainForm, StartupOptions startupOptions, SplashForm dpiBootstrapSplash)
        {
            // Match interactive startup: open the document after the first shown/layout pass has completed.
            EventHandler idleHandler = null;
            idleHandler = (_, __) =>
            {
                Application.Idle -= idleHandler;
                try
                {
                    OpenStartupInput(mainForm, startupOptions);
                }
                finally
                {
                    if (dpiBootstrapSplash != null && !dpiBootstrapSplash.IsDisposed)
                    {
                        dpiBootstrapSplash.Dispose();
                    }
                }
            };
            Application.Idle += idleHandler;
        }

        private static void OpenStartupInput(PDFForm mainForm, StartupOptions startupOptions)
        {
            if (mainForm == null || mainForm.IsDisposed || mainForm.Disposing)
            {
                return;
            }

            mainForm.OpenInputPath(startupOptions.InputPath, suppressTutorial: true);
            if (startupOptions.PageNumber > 0)
            {
                mainForm.GoToPage(startupOptions.PageNumber);
            }
            if (startupOptions.RotateDegrees != 0)
            {
                mainForm.RotateCurrentPageBy(startupOptions.RotateDegrees);
            }
            if (!string.IsNullOrWhiteSpace(startupOptions.PdfOutputPath))
            {
                mainForm.SetIntegrationMode(startupOptions.PdfOutputPath);
            }
            if (!string.IsNullOrWhiteSpace(startupOptions.PngOutputPath))
            {
                mainForm.SetAutoSavePngPath(startupOptions.PngOutputPath);
            }
        }

        private struct StartupOptions
        {
            public string InputPath;
            public string PdfOutputPath;  // --pdfout
            public string PngOutputPath;  // --pngout
            public int RotateDegrees;     // --rotate <0|90|180|270>
            public int PageNumber;        // --page <1-based page number>
            public string TemplatePath;   // --template <project.app>
            public bool OverwriteOutput;  // --overwrite
            public bool Headless;         // --headless
            public bool IndexText;        // --text
            public bool RunOcr;           // --ocr
            public bool PdfAOutput;       // --format pdfa
            public bool ShowHelp;         // --help
            public string Language;       // --lang <pl|en|de>
            public string ParseError;
        }

        private static StartupOptions ParseStartupOptions(string[] args)
        {
            var options = new StartupOptions();
            if (args == null || args.Length == 0) return options;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i]?.Trim();
                if (string.IsNullOrWhiteSpace(arg)) continue;

                if (string.Equals(arg, "--help", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(arg, "-h", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(arg, "/?", StringComparison.OrdinalIgnoreCase))
                {
                    options.ShowHelp = true;
                    continue;
                }
                if (string.Equals(arg, "--headless", StringComparison.OrdinalIgnoreCase))
                {
                    options.Headless = true;
                    continue;
                }
                if (string.Equals(arg, "--overwrite", StringComparison.OrdinalIgnoreCase))
                {
                    options.OverwriteOutput = true;
                    continue;
                }
                if (string.Equals(arg, "--text", StringComparison.OrdinalIgnoreCase))
                {
                    options.IndexText = true;
                    continue;
                }
                if (string.Equals(arg, "--ocr", StringComparison.OrdinalIgnoreCase))
                {
                    options.IndexText = true;
                    options.RunOcr = true;
                    continue;
                }
                if (string.Equals(arg, "--lang", StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 >= args.Length)
                    {
                        options.ParseError = CommandLineText("CommandLine_MissingLanguage");
                        break;
                    }
                    options.Language = args[++i]?.Trim();
                    continue;
                }
                if (string.Equals(arg, "--template", StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 >= args.Length)
                    {
                        options.ParseError = CommandLineText("CommandLine_MissingTemplate");
                        break;
                    }
                    options.TemplatePath = GetFullPathOrOriginal(args[++i]);
                    continue;
                }
                if (string.Equals(arg, "--format", StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 >= args.Length)
                    {
                        options.ParseError = CommandLineText("CommandLine_MissingFormat");
                        break;
                    }

                    string format = args[++i]?.Trim();
                    if (string.Equals(format, "pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        options.PdfAOutput = false;
                    }
                    else if (string.Equals(format, "pdfa", StringComparison.OrdinalIgnoreCase))
                    {
                        options.PdfAOutput = true;
                        options.IndexText = true;
                        options.RunOcr = true;
                    }
                    else
                    {
                        options.ParseError = CommandLineText("CommandLine_InvalidFormat");
                        break;
                    }
                    continue;
                }
                if (string.Equals(arg, "--pdfout", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    options.PdfOutputPath = GetFullPathOrOriginal(args[++i]);
                    continue;
                }
                if (string.Equals(arg, "--pngout", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    options.PngOutputPath = args[++i]?.Trim();
                    continue;
                }
                if (string.Equals(arg, "--rotate", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    if (int.TryParse(args[++i]?.Trim(), out int deg))
                        options.RotateDegrees = deg;
                    continue;
                }
                if (string.Equals(arg, "--page", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    if (int.TryParse(args[++i]?.Trim(), out int pg))
                        options.PageNumber = pg;
                    continue;
                }
                if (arg.StartsWith("--", StringComparison.Ordinal))
                {
                    options.ParseError = string.Format(CommandLineText("CommandLine_UnknownOption"), arg);
                    break;
                }

                string ext = Path.GetExtension(arg);
                if (string.IsNullOrEmpty(options.InputPath) &&
                    (string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(ext, ".app", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(ext, ".pap", StringComparison.OrdinalIgnoreCase)))
                {
                    options.InputPath = GetFullPathOrOriginal(arg);
                }
            }
            return options;
        }

        private static string GetFullPathOrOriginal(string path)
        {
            string value = path?.Trim();
            if (string.IsNullOrWhiteSpace(value)) return value;
            try { return Path.GetFullPath(value); } catch { return value; }
        }

        private static void ConfigureCommandLineCulture(ref StartupOptions options)
        {
            string language = options.Language?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(language))
            {
                commandLineCulture = CultureInfo.GetCultureInfo("en");
                Properties.Resources.Culture = commandLineCulture;
                return;
            }

            if (language != "pl" && language != "en" && language != "de")
            {
                commandLineCulture = CultureInfo.GetCultureInfo("en");
                options.ParseError = string.Format(CommandLineText("CommandLine_InvalidLanguage"), language);
                return;
            }

            commandLineCulture = CultureInfo.GetCultureInfo(language);
            Properties.Resources.Culture = commandLineCulture;
        }

        private static bool ValidateHeadlessOptions(StartupOptions options, out string error, out int exitCode)
        {
            error = null;
            exitCode = 2;
            if (!options.Headless)
            {
                if (!string.IsNullOrWhiteSpace(options.TemplatePath))
                {
                    error = CommandLineText("CommandLine_TemplateRequiresHeadless");
                    return false;
                }
                return true;
            }

            if (string.IsNullOrWhiteSpace(options.TemplatePath) || string.IsNullOrWhiteSpace(options.InputPath) || string.IsNullOrWhiteSpace(options.PdfOutputPath))
            {
                error = CommandLineText("CommandLine_HeadlessRequirements");
                return false;
            }
            if (!string.Equals(Path.GetExtension(options.InputPath), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                error = CommandLineText("CommandLine_InputMustBePdf");
                return false;
            }
            if (!File.Exists(options.InputPath))
            {
                error = string.Format(CommandLineText("CommandLine_InputNotFound"), options.InputPath);
                return false;
            }
            if (!File.Exists(options.TemplatePath))
            {
                error = string.Format(CommandLineText("CommandLine_TemplateNotFound"), options.TemplatePath);
                return false;
            }
            if (File.Exists(options.PdfOutputPath) && !options.OverwriteOutput)
            {
                error = string.Format(CommandLineText("CommandLine_OutputExists"), options.PdfOutputPath);
                exitCode = 4;
                return false;
            }
            if (string.Equals(options.InputPath, options.PdfOutputPath, StringComparison.OrdinalIgnoreCase))
            {
                error = CommandLineText("CommandLine_OutputSameAsInput");
                return false;
            }
            return true;
        }

        private static void RunHeadlessTemplateBatch(StartupOptions options)
        {
            using (var form = new PDFForm(null))
            {
                BatchRunResult result = form.RunHeadlessTemplateBatch(new BatchRunOptions
                {
                    InputPdfPath = options.InputPath,
                    TemplatePath = options.TemplatePath,
                    OutputPdfPath = options.PdfOutputPath,
                    OverwriteOutput = options.OverwriteOutput,
                    IndexText = options.IndexText,
                    RunOcr = options.RunOcr,
                    PdfAOutput = options.PdfAOutput,
                    Progress = WriteCommandLineMessage
                });
                WriteCommandLineMessage(result.Message);
                Environment.ExitCode = result.ExitCode;
            }
        }

        private static void WriteCommandLineMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                EnsureCommandLineConsole();
                Console.Error.WriteLine(message);
            }
        }

        private static void EnsureCommandLineConsole()
        {
            if (commandLineConsoleInitialized)
            {
                return;
            }

            commandLineConsoleInitialized = true;
            if (!AttachConsole(AttachParentProcess))
            {
                return;
            }

            IntPtr standardError = GetStdHandle(StandardErrorHandle);
            Encoding encoding = Encoding.UTF8;
            if (standardError != IntPtr.Zero && standardError != new IntPtr(-1) && GetConsoleMode(standardError, out _))
            {
                uint codePage = GetConsoleOutputCP();
                if (codePage != 0)
                {
                    encoding = Encoding.GetEncoding((int)codePage);
                }
            }
            var errorWriter = new StreamWriter(Console.OpenStandardError(), encoding) { AutoFlush = true };
            Console.SetError(errorWriter);
        }

        private static string CommandLineText(string key)
        {
            string value = Properties.Resources.ResourceManager.GetString(key, commandLineCulture);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Properties.Resources.ResourceManager.GetString(key, CultureInfo.GetCultureInfo("en"));
            }
            return string.IsNullOrWhiteSpace(value) ? key : value;
        }

        private static string GetCommandLineHelp()
        {
            return CommandLineText("CommandLine_Help");
        }

        private static void EnsureContextMenuRegistration()
        {
            try
            {
                string exePath = Application.ExecutablePath;
                if (string.IsNullOrWhiteSpace(exePath) || !File.Exists(exePath))
                {
                    return;
                }

                string commandValue = $"\"{exePath}\" \"%1\"";
                RegisterContextMenuForExtension(".pdf", commandValue, exePath);
                RegisterContextMenuForExtension(AppProjectExtension, commandValue, exePath);
                RegisterContextMenuForExtension(".pap", commandValue, exePath);
                RegisterAppProjectFileAssociation(commandValue, exePath);
                NotifyShellAssociationChanged();
            }
            catch
            {
                // Best effort only. Lack of registry access must not block app startup.
            }
        }

        private static void RegisterAppProjectFileAssociation(string commandValue, string exePath)
        {
            string description = GetAppProjectFileTypeDescriptionText();
            string iconValue = $"\"{exePath}\",0";

            using (RegistryKey extensionKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{AppProjectExtension}"))
            {
                if (extensionKey != null)
                {
                    extensionKey.SetValue(string.Empty, AppProjectProgId, RegistryValueKind.String);
                    extensionKey.SetValue("PerceivedType", "document", RegistryValueKind.String);

                    using (RegistryKey openWithKey = extensionKey.CreateSubKey("OpenWithProgids"))
                    {
                        openWithKey?.SetValue(AppProjectProgId, string.Empty, RegistryValueKind.String);
                    }
                }
            }

            using (RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{AppProjectProgId}"))
            {
                if (progIdKey == null)
                {
                    return;
                }

                progIdKey.SetValue(string.Empty, description, RegistryValueKind.String);
                progIdKey.SetValue("FriendlyTypeName", description, RegistryValueKind.String);

                using (RegistryKey defaultIconKey = progIdKey.CreateSubKey("DefaultIcon"))
                {
                    defaultIconKey?.SetValue(string.Empty, iconValue, RegistryValueKind.String);
                }

                using (RegistryKey shellKey = progIdKey.CreateSubKey("shell"))
                {
                    shellKey?.SetValue(string.Empty, "open", RegistryValueKind.String);
                }

                using (RegistryKey openKey = progIdKey.CreateSubKey(@"shell\open"))
                {
                    openKey?.SetValue(string.Empty, GetContextMenuOpenText(), RegistryValueKind.String);
                }

                using (RegistryKey commandKey = progIdKey.CreateSubKey(@"shell\open\command"))
                {
                    commandKey?.SetValue(string.Empty, commandValue, RegistryValueKind.String);
                }
            }
        }

        private static void NotifyShellAssociationChanged()
        {
            try
            {
                SHChangeNotify(ShcneAssocChanged, ShcnfIdList, IntPtr.Zero, IntPtr.Zero);
            }
            catch
            {
                // Best effort only. Explorer may refresh the association later.
            }
        }

        private static void RegisterContextMenuForExtension(string extension, string commandValue, string iconPath)
        {
            string shellKeyPath = $@"Software\Classes\SystemFileAssociations\{extension}\shell\AnonPDFPro";
            using (RegistryKey shellKey = Registry.CurrentUser.CreateSubKey(shellKeyPath))
            {
                if (shellKey == null)
                {
                    return;
                }

                shellKey.SetValue(string.Empty, GetContextMenuOpenText(), RegistryValueKind.String);
                shellKey.SetValue("Icon", iconPath, RegistryValueKind.String);
            }

            using (RegistryKey commandKey = Registry.CurrentUser.CreateSubKey(shellKeyPath + @"\command"))
            {
                if (commandKey == null)
                {
                    return;
                }

                commandKey.SetValue(string.Empty, commandValue, RegistryValueKind.String);
            }
        }

        private static string GetContextMenuOpenText()
        {
            string value = Properties.Resources.ResourceManager.GetString(
                "ContextMenu_OpenWithAnonPDFPro",
                Properties.Resources.Culture ?? System.Globalization.CultureInfo.CurrentUICulture);
            return string.IsNullOrWhiteSpace(value) ? "Open with AnonPDF Pro" : value;
        }

        private static string GetAppProjectFileTypeDescriptionText()
        {
            string value = Properties.Resources.ResourceManager.GetString(
                "FileAssociation_AppProjectDescription",
                Properties.Resources.Culture ?? System.Globalization.CultureInfo.CurrentUICulture);
            return string.IsNullOrWhiteSpace(value) ? "AnonPDF Pro project" : value;
        }

        private static bool ValidateRequiredLicenseFiles(out string errorMessage)
        {
            var issues = new List<string>();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var config = LicenseManager.Config;

            string configPath = config?.ConfigFilePath ?? Path.Combine(baseDir, "config.json");
            if (!File.Exists(configPath))
            {
                issues.Add(string.Format(Properties.Resources.License_ConfigMissing, configPath));
            }
            else
            {
                try
                {
                    JObject.Parse(File.ReadAllText(configPath));
                }
                catch (Exception ex)
                {
                    issues.Add(string.Format(Properties.Resources.License_ConfigInvalid, ex.Message));
                }
            }

            string licensePath = config?.ResolveLicensePath() ?? Path.Combine(baseDir, "license.json");
            if (!File.Exists(licensePath))
            {
                issues.Add(string.Format(Properties.Resources.License_FileMissing, licensePath));
            }

            string publicKeyPath = config?.ResolvePublicKeyPath() ?? Path.Combine(baseDir, "license_public.xml");
            if (!File.Exists(publicKeyPath))
            {
                issues.Add(string.Format(Properties.Resources.License_PublicKeyMissing, publicKeyPath));
            }

            var info = LicenseManager.Current;
            if (info == null || !info.IsSignatureValid || info.Payload == null)
            {
                string detail = info?.Error;
                if (string.IsNullOrWhiteSpace(detail))
                {
                    detail = "-";
                }
                issues.Add(string.Format(Properties.Resources.License_Invalid, detail));
            }

            if (issues.Count > 0)
            {
                errorMessage = string.Format(
                    Properties.Resources.License_StartupError,
                    string.Join(Environment.NewLine, issues));
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        private static string GetErrorLogDirectory()
        {
            string companyName = GetSafeDirectoryName(Application.CompanyName, "skmislab");
            string productName = GetSafeDirectoryName(Application.ProductName, "AnonPDFPro");

            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                companyName,
                productName);
        }

        private static string GetSafeDirectoryName(string value, string fallback)
        {
            string source = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
            var invalidChars = Path.GetInvalidFileNameChars();
            string normalized = new string(source.Select(ch => invalidChars.Contains(ch) ? '_' : ch).ToArray());
            return string.IsNullOrWhiteSpace(normalized) ? fallback : normalized;
        }

        // Log unhandled exceptions to AppData
        private static void LogUnhandledException(Exception ex, string exceptionType)
        {
            try
            {
                string appDataDir = GetErrorLogDirectory();
                Directory.CreateDirectory(appDataDir);

                string logPath = Path.Combine(appDataDir, "error.log");

                File.AppendAllText(
                    logPath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{exceptionType}]\r\n{ex}\r\n\r\n"
                );
            }
            catch
            {
                // Swallow logging failures to avoid blocking the application
            }
        }

        // Show an error dialog (includes log file location)
        private static void ShowError(Exception ex)
        {
            string appDataDir = GetErrorLogDirectory();
            string logPath = Path.Combine(appDataDir, "error.log");

            MessageBox.Show(
                string.Format(Properties.Resources.Err_UnhandledException, ex.Message, logPath),
                Properties.Resources.Title_CriticalAppError,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

    }
}
#pragma warning restore SPELL
