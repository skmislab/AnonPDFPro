using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

// Suppress spell-check warning for project name 'AnonPDF'
#pragma warning disable SPELL
namespace AnonPDF
{
    internal static class Program
    {
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
            EnsureContextMenuRegistration();
            string startupInputPath = ParseStartupInputPath(args);
            LicenseManager.Initialize(AppDomain.CurrentDomain.BaseDirectory);
            if (!ValidateRequiredLicenseFiles(out string licenseError))
            {
                MessageBox.Show(
                    licenseError,
                    Properties.Resources.Title_Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            bool hasStartupInputPath = !string.IsNullOrWhiteSpace(startupInputPath);
            SplashForm splash = hasStartupInputPath ? null : new SplashForm();
            var mainForm = new PDFForm(splash);
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
                mainForm.Shown += (_, __) => mainForm.OpenInputPath(startupInputPath, suppressTutorial: true);
            }

            mainForm.FormClosed += (_, __) =>
            {
                if (splash != null && !splash.IsDisposed)
                {
                    splash.Close();
                }
            };

            Application.Run(mainForm);
        }

        private static string ParseStartupInputPath(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return string.Empty;
            }

            foreach (string arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                {
                    continue;
                }

                string trimmed = arg.Trim();
                if (trimmed.StartsWith("--", StringComparison.Ordinal))
                {
                    continue;
                }

                string extension = Path.GetExtension(trimmed);
                if (string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(extension, ".app", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(extension, ".pap", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        return Path.GetFullPath(trimmed);
                    }
                    catch
                    {
                        return trimmed;
                    }
                }
            }

            return string.Empty;
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
                RegisterContextMenuForExtension(".app", commandValue, exePath);
                RegisterContextMenuForExtension(".pap", commandValue, exePath);
            }
            catch
            {
                // Best effort only. Lack of registry access must not block app startup.
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
