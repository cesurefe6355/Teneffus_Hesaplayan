#nullable disable

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Tenefus_hesaplayan
{
    public class ScheduleItem
    {
        public string Name { get; set; }
        public string StartString { get; set; }
        public string EndString { get; set; }

        public TimeSpan Start
        {
            get { return TimeSpan.Parse(StartString); }
        }

        public TimeSpan End
        {
            get { return TimeSpan.Parse(EndString); }
        }
    }

    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private DispatcherTimer toastTimer;
        private bool isDarkMode = true;
        private bool isDynamicIsland = false;
        private bool isLeafClockMode = false;
        private bool skipCloseConfirmation = false;

        private bool isDraggingLeaf = false;
        private Point leafDragStart;
        private double leafStartLeft;
        private double leafStartTop;
        private bool wasDragged = false;

        private const double IslandDesignWidth = 260.0;
        private const double IslandDesignHeight = 42.0;

        private const double LeafClockWidth = 276.0;
        private const double LeafClockHeight = 108.0;

        // ============================================================
        //  WIN32 API
        // ============================================================
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        private void SetToolWindow(bool enable)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd == IntPtr.Zero) return;

            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            if (enable)
            {
                exStyle |= WS_EX_TOOLWINDOW;
                exStyle &= ~WS_EX_APPWINDOW;
            }
            else
            {
                exStyle &= ~WS_EX_TOOLWINDOW;
                exStyle |= WS_EX_APPWINDOW;
            }

            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
        }

        // ============================================================
        //  BAŞLANGIÇ / DOSYA
        // ============================================================
        private const string StartupRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string StartupAppName = "TenefusHesaplayan";

        private static readonly string ScheduleFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ders_programi.txt");

        private const double RingUnits = 59.69;

        public ObservableCollection<ScheduleItem> ScheduleList { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            if (!LoadScheduleFromFile())
            {
                LoadDefaultSchedule();
            }

            ScheduleListView.ItemsSource = ScheduleList;
            ApplyTheme(true);
            UpdateStartupButton();
            StartTimer();

            InitFlipTransforms();
            UpdateLeafClock(DateTime.Now);
        }

        // ============================================================
        //  YÖNETİCİ
        // ============================================================
        private static bool IsRunningAsAdmin()
        {
            try
            {
                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                var principal = new System.Security.Principal.WindowsPrincipal(identity);
                return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private void RestartAsAdmin()
        {
            try
            {
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                if (string.IsNullOrEmpty(exePath) || !File.Exists(exePath))
                {
                    ShowToast("Hata", "Uygulama yolu bulunamadı.", false);
                    return;
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    UseShellExecute = true,
                    Verb = "runas",
                    WorkingDirectory = Path.GetDirectoryName(exePath)
                };

                Process.Start(psi);

                skipCloseConfirmation = true;
                Application.Current.Shutdown();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                ShowToast("İptal Edildi", "Yönetici izni verilmedi.", false);
            }
            catch (Exception ex)
            {
                ShowToast("Hata", "Yeniden başlatılamadı: " + ex.Message, false);
            }
        }

        // ============================================================
        //  VARSAYILAN PROGRAM
        // ============================================================
        private void LoadDefaultSchedule()
        {
            ScheduleList = new ObservableCollection<ScheduleItem>();

            ScheduleList.Add(new ScheduleItem { Name = "1. Ders", StartString = "08:30", EndString = "09:10" });
            ScheduleList.Add(new ScheduleItem { Name = "1. Teneffüs", StartString = "09:10", EndString = "09:20" });
            ScheduleList.Add(new ScheduleItem { Name = "2. Ders", StartString = "09:20", EndString = "10:00" });
            ScheduleList.Add(new ScheduleItem { Name = "2. Teneffüs", StartString = "10:00", EndString = "10:10" });
            ScheduleList.Add(new ScheduleItem { Name = "3. Ders", StartString = "10:10", EndString = "10:50" });
            ScheduleList.Add(new ScheduleItem { Name = "3. Teneffüs", StartString = "10:50", EndString = "11:00" });
            ScheduleList.Add(new ScheduleItem { Name = "4. Ders", StartString = "11:00", EndString = "11:40" });
            ScheduleList.Add(new ScheduleItem { Name = "4. Teneffüs", StartString = "11:40", EndString = "11:50" });
            ScheduleList.Add(new ScheduleItem { Name = "5. Ders", StartString = "11:50", EndString = "12:30" });
            ScheduleList.Add(new ScheduleItem { Name = "Öğle Arası", StartString = "12:30", EndString = "13:15" });
            ScheduleList.Add(new ScheduleItem { Name = "6. Ders", StartString = "13:15", EndString = "13:55" });
            ScheduleList.Add(new ScheduleItem { Name = "6. Teneffüs", StartString = "13:55", EndString = "14:05" });
            ScheduleList.Add(new ScheduleItem { Name = "7. Ders", StartString = "14:05", EndString = "14:45" });
            ScheduleList.Add(new ScheduleItem { Name = "7. Teneffüs", StartString = "14:45", EndString = "14:55" });
            ScheduleList.Add(new ScheduleItem { Name = "8. Ders", StartString = "14:55", EndString = "15:35" });
        }

        // ============================================================
        //  KAYDET / YÜKLE
        // ============================================================
        private bool SaveScheduleToFile()
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("# Tenefüs Hesaplayan - Ders Programı");
                sb.AppendLine("# Format: EtkinlikAdı|Başlangıç|Bitiş");
                sb.AppendLine("# Bu dosyayı elle düzenleyebilirsiniz. Program her açılışta okur.");

                foreach (var item in ScheduleList)
                {
                    sb.AppendLine(item.Name + "|" + item.StartString + "|" + item.EndString);
                }

                File.WriteAllText(ScheduleFilePath, sb.ToString(), System.Text.Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool LoadScheduleFromFile()
        {
            try
            {
                if (!File.Exists(ScheduleFilePath)) return false;

                string[] lines = File.ReadAllLines(ScheduleFilePath, System.Text.Encoding.UTF8);
                var loaded = new ObservableCollection<ScheduleItem>();

                foreach (string raw in lines)
                {
                    if (string.IsNullOrWhiteSpace(raw)) continue;
                    if (raw.TrimStart().StartsWith("#")) continue;

                    string[] parts = raw.Split('|');
                    if (parts.Length < 3) continue;

                    string name = parts[0].Trim();
                    string start = parts[1].Trim();
                    string end = parts[2].Trim();

                    TimeSpan tmp;
                    if (!TimeSpan.TryParse(start, out tmp)) continue;
                    if (!TimeSpan.TryParse(end, out tmp)) continue;

                    loaded.Add(new ScheduleItem { Name = name, StartString = start, EndString = end });
                }

                if (loaded.Count == 0) return false;
                ScheduleList = loaded;
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ============================================================
        //  TOAST
        // ============================================================
        private void ShowToast(string title, string message, bool success)
        {
            toastTitle.Text = title;
            toastMessage.Text = message;

            if (success)
            {
                toastIcon.Text = "✓";
                toastIconBorder.Background = (Brush)FindResource("SuccessBrush");
                ToastPanel.BorderBrush = (Brush)FindResource("SuccessBrush");
            }
            else
            {
                toastIcon.Text = "✕";
                toastIconBorder.Background = (Brush)FindResource("DangerBrush");
                ToastPanel.BorderBrush = (Brush)FindResource("DangerBrush");
            }

            toastIcon.Foreground = new SolidColorBrush(Color.FromRgb(24, 24, 37));

            ToastPanel.Visibility = Visibility.Visible;
            toastTransform.Y = 20;
            ToastPanel.BeginAnimation(OpacityProperty, null);

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(260))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            ToastPanel.BeginAnimation(OpacityProperty, fadeIn);

            var slideIn = new DoubleAnimation(20, 0, TimeSpan.FromMilliseconds(320))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            toastTransform.BeginAnimation(TranslateTransform.YProperty, slideIn);

            if (toastTimer != null) { toastTimer.Stop(); toastTimer = null; }

            toastTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2.6) };
            toastTimer.Tick += (s, e) =>
            {
                toastTimer.Stop();
                toastTimer = null;

                var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300))
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                fadeOut.Completed += (s2, e2) => { ToastPanel.Visibility = Visibility.Collapsed; };
                ToastPanel.BeginAnimation(OpacityProperty, fadeOut);
            };
            toastTimer.Start();
        }

        // ============================================================
        //  ZAMANLAYICI
        // ============================================================
        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            TimeSpan nowT = now.TimeOfDay;

            txtCurrentTime.Text = now.ToString("HH:mm:ss");

            ScheduleItem current = null;
            ScheduleItem next = null;
            TimeSpan curStart = TimeSpan.Zero;
            TimeSpan curEnd = TimeSpan.Zero;

            foreach (var item in ScheduleList)
            {
                TimeSpan s;
                TimeSpan en;
                if (!TimeSpan.TryParse(item.StartString, out s)) continue;
                if (!TimeSpan.TryParse(item.EndString, out en)) continue;

                if (nowT >= s && nowT <= en)
                {
                    current = item;
                    curStart = s;
                    curEnd = en;
                    break;
                }

                if (nowT < s && next == null)
                {
                    next = item;
                }
            }

            if (current != null)
            {
                TimeSpan remaining = curEnd - nowT;
                double total = (curEnd - curStart).TotalSeconds;
                double passed = (nowT - curStart).TotalSeconds;
                double progress = total > 0 ? passed / total : 0;

                bool warn = remaining.TotalSeconds <= 60;

                txtStatus.Text = current.Name + " devam ediyor";
                txtStatusSub.Text = "Bitiş saati: " + curEnd.ToString(@"hh\:mm");
                txtCountdown.Text = remaining.ToString(@"mm\:ss");

                txtIslandStatus.Text = current.Name;
                txtIslandTime.Text = remaining.ToString(@"mm\:ss");
                prgIsland.Value = Math.Max(0, Math.Min(1, progress)) * 100;

                ScheduleItem nextAfter = null;
                foreach (var it in ScheduleList)
                {
                    TimeSpan ts;
                    if (!TimeSpan.TryParse(it.StartString, out ts)) continue;
                    if (ts > curEnd) { nextAfter = it; break; }
                }

                if (nextAfter != null)
                    txtNext.Text = nextAfter.Name + " • " + nextAfter.StartString;
                else
                    txtNext.Text = "Okul sonu";

                SetRing(progress, warn);
            }
            else if (next != null)
            {
                TimeSpan remaining = next.Start - nowT;

                txtStatus.Text = next.Name + " başlamasına kalan";
                txtStatusSub.Text = "Başlangıç saati: " + next.Start.ToString(@"hh\:mm");
                txtCountdown.Text = remaining.ToString(@"mm\:ss");

                txtIslandStatus.Text = next.Name + " (yaklaşan)";
                txtIslandTime.Text = remaining.ToString(@"mm\:ss");
                prgIsland.Value = 0;

                txtNext.Text = next.Name + " • " + next.StartString;

                SetRing(0, false);
            }
            else
            {
                txtStatus.Text = "Okul Saati Bitti";
                txtStatusSub.Text = "İyi akşamlar! 🌙";
                txtCountdown.Text = "00:00";

                txtIslandStatus.Text = "Okul Bitti";
                txtIslandTime.Text = "00:00";
                prgIsland.Value = 0;

                txtNext.Text = "Yarın görüşürüz";

                SetRing(1, false);
            }

            UpdateLeafClock(now);
        }

        private void SetRing(double fraction, bool warn)
        {
            if (fraction < 0.0005) fraction = 0.0005;
            if (fraction > 1) fraction = 1;

            ringProgress.StrokeDashArray = new DoubleCollection { fraction * RingUnits, RingUnits };

            Brush themeBrush = (Brush)FindResource(warn ? "WarnBrush" : "AccentBrush");
            ringProgress.Stroke = themeBrush;
            txtCountdown.Foreground = themeBrush;

            txtIslandTime.Foreground = themeBrush;
            prgIsland.Foreground = themeBrush;
            islandDot.Fill = themeBrush;
        }

        // ============================================================
        //  FLIP CLOCK
        // ============================================================
        private void UpdateLeafClock(DateTime now)
        {
            if (txtLeafH1 == null) return;

            string hh = now.ToString("HH");
            string mm = now.ToString("mm");

            SetFlipDigit(txtLeafH1, hh.Substring(0, 1));
            SetFlipDigit(txtLeafH2, hh.Substring(1, 1));
            SetFlipDigit(txtLeafM1, mm.Substring(0, 1));
            SetFlipDigit(txtLeafM2, mm.Substring(1, 1));
        }

        private void InitFlipTransforms()
        {
            TextBlock[] digits = { txtLeafH1, txtLeafH2, txtLeafM1, txtLeafM2 };
            foreach (var tb in digits)
            {
                if (tb == null) continue;
                tb.RenderTransformOrigin = new Point(0.5, 0.5);
                tb.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        private void SetFlipDigit(TextBlock tb, string newText)
        {
            if (tb == null) return;
            if (tb.Text == newText) return;

            ScaleTransform scale = tb.RenderTransform as ScaleTransform;
            if (scale == null)
            {
                tb.Text = newText;
                return;
            }

            var closeAnim = new DoubleAnimation
            {
                From = 1,
                To = 0.02,
                Duration = TimeSpan.FromMilliseconds(110),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            closeAnim.Completed += (s, e) =>
            {
                tb.Text = newText;

                var openAnim = new DoubleAnimation
                {
                    From = 0.02,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(210),
                    EasingFunction = new BackEase
                    {
                        EasingMode = EasingMode.EaseOut,
                        Amplitude = 0.45
                    }
                };
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, openAnim);
            };

            scale.BeginAnimation(ScaleTransform.ScaleYProperty, closeAnim);
        }

        // ============================================================
        //  TEMA
        // ============================================================
        private void ApplyTheme(bool dark)
        {
            isDarkMode = dark;

            if (dark)
            {
                Resources["BgBrush"] = new SolidColorBrush(Color.FromRgb(24, 24, 37));
                Resources["CardBrush"] = new SolidColorBrush(Color.FromRgb(30, 30, 46));
                Resources["SurfaceBrush"] = new SolidColorBrush(Color.FromRgb(49, 50, 68));
                Resources["TextBrush"] = new SolidColorBrush(Color.FromRgb(205, 214, 244));
                Resources["SubTextBrush"] = new SolidColorBrush(Color.FromRgb(166, 173, 200));
                Resources["AccentBrush"] = new SolidColorBrush(Color.FromRgb(137, 180, 250));
                Resources["AccentTextBrush"] = new SolidColorBrush(Color.FromRgb(24, 24, 37));
                Resources["TimeBrush"] = new SolidColorBrush(Color.FromRgb(243, 139, 168));
                Resources["WarnBrush"] = new SolidColorBrush(Color.FromRgb(249, 226, 175));
                Resources["DangerBrush"] = new SolidColorBrush(Color.FromRgb(243, 139, 168));
                Resources["SuccessBrush"] = new SolidColorBrush(Color.FromRgb(166, 227, 161));
                Resources["BorderBrushColor"] = new SolidColorBrush(Color.FromRgb(49, 50, 68));

                Resources["ButtonBgBrush"] = new SolidColorBrush(Color.FromRgb(49, 50, 68));
                Resources["ButtonHoverBrush"] = new SolidColorBrush(Color.FromRgb(69, 71, 90));
                Resources["ButtonPressedBrush"] = new SolidColorBrush(Color.FromRgb(88, 91, 112));
                Resources["ButtonTextBrush"] = new SolidColorBrush(Colors.White);

                Resources["IslandBgBrush"] = new LinearGradientBrush(
                    Color.FromRgb(0x1A, 0x1B, 0x26),
                    Color.FromRgb(0x0E, 0x0F, 0x16),
                    new Point(0, 0), new Point(0, 1));
                Resources["IslandBorderBrush"] = new LinearGradientBrush(
                    Color.FromRgb(0x4A, 0x51, 0x70),
                    Color.FromRgb(0x24, 0x28, 0x3B),
                    new Point(0, 0), new Point(1, 1));
                Resources["IslandTextBrush"] = new SolidColorBrush(Color.FromRgb(0xC0, 0xCA, 0xF5));
                Resources["IslandSeparatorBrush"] = new SolidColorBrush(Color.FromRgb(0x2F, 0x33, 0x49));
                Resources["IslandProgressBgBrush"] = new SolidColorBrush(Color.FromArgb(0x26, 0xFF, 0xFF, 0xFF));

                // ---- Yaprak Saat: karanlık tema ----
                Resources["LeafContainerBgBrush"] = new SolidColorBrush(Color.FromRgb(0x0A, 0x0A, 0x0A));
                Resources["LeafContainerBorderBrush"] = new SolidColorBrush(Color.FromRgb(0x24, 0x24, 0x24));
                Resources["LeafCardTopBrush"] = new SolidColorBrush(Color.FromRgb(0x2B, 0x2B, 0x2B));
                Resources["LeafCardBottomBrush"] = new SolidColorBrush(Color.FromRgb(0x16, 0x16, 0x16));
                Resources["LeafCardBorderBrush"] = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A));
                Resources["LeafDigitBrush"] = new SolidColorBrush(Color.FromRgb(0xF2, 0xF2, 0xF2));
                Resources["LeafCardDividerBrush"] = new SolidColorBrush(Colors.Black);
                Resources["LeafSeparatorDotBrush"] = new SolidColorBrush(Color.FromRgb(0x5A, 0x5A, 0x5A));

                if (btnTheme != null) btnTheme.Content = "🌙   Karanlık Mod";
            }
            else
            {
                Resources["BgBrush"] = new SolidColorBrush(Color.FromRgb(239, 241, 245));
                Resources["CardBrush"] = new SolidColorBrush(Colors.White);
                Resources["SurfaceBrush"] = new SolidColorBrush(Color.FromRgb(228, 232, 240));
                Resources["TextBrush"] = new SolidColorBrush(Color.FromRgb(76, 79, 105));
                Resources["SubTextBrush"] = new SolidColorBrush(Color.FromRgb(124, 127, 147));
                Resources["AccentBrush"] = new SolidColorBrush(Color.FromRgb(30, 102, 245));
                Resources["AccentTextBrush"] = new SolidColorBrush(Colors.White);
                Resources["TimeBrush"] = new SolidColorBrush(Color.FromRgb(210, 15, 57));
                Resources["WarnBrush"] = new SolidColorBrush(Color.FromRgb(223, 142, 29));
                Resources["DangerBrush"] = new SolidColorBrush(Color.FromRgb(210, 15, 57));
                Resources["SuccessBrush"] = new SolidColorBrush(Color.FromRgb(64, 160, 43));
                Resources["BorderBrushColor"] = new SolidColorBrush(Color.FromRgb(204, 208, 218));

                Resources["ButtonBgBrush"] = new SolidColorBrush(Color.FromRgb(228, 232, 240));
                Resources["ButtonHoverBrush"] = new SolidColorBrush(Color.FromRgb(214, 219, 230));
                Resources["ButtonPressedBrush"] = new SolidColorBrush(Color.FromRgb(198, 204, 218));
                Resources["ButtonTextBrush"] = new SolidColorBrush(Color.FromRgb(76, 79, 105));

                Resources["IslandBgBrush"] = new LinearGradientBrush(
                    Colors.White,
                    Color.FromRgb(0xE8, 0xEA, 0xF0),
                    new Point(0, 0), new Point(0, 1));
                Resources["IslandBorderBrush"] = new LinearGradientBrush(
                    Color.FromRgb(0xC8, 0xCC, 0xD8),
                    Color.FromRgb(0xA8, 0xAD, 0xBC),
                    new Point(0, 0), new Point(1, 1));
                Resources["IslandTextBrush"] = new SolidColorBrush(Color.FromRgb(76, 79, 105));
                Resources["IslandSeparatorBrush"] = new SolidColorBrush(Color.FromRgb(208, 212, 222));
                Resources["IslandProgressBgBrush"] = new SolidColorBrush(Color.FromArgb(0x1A, 0x00, 0x00, 0x00));

                // ---- Yaprak Saat: açık tema ----
                Resources["LeafContainerBgBrush"] = new SolidColorBrush(Color.FromRgb(0xE8, 0xEA, 0xF0));
                Resources["LeafContainerBorderBrush"] = new SolidColorBrush(Color.FromRgb(0xC0, 0xC6, 0xD2));
                Resources["LeafCardTopBrush"] = new SolidColorBrush(Colors.White);
                Resources["LeafCardBottomBrush"] = new SolidColorBrush(Color.FromRgb(0xE2, 0xE5, 0xEC));
                Resources["LeafCardBorderBrush"] = new SolidColorBrush(Color.FromRgb(0xB8, 0xBE, 0xCC));
                Resources["LeafDigitBrush"] = new SolidColorBrush(Color.FromRgb(0x2E, 0x30, 0x38));
                Resources["LeafCardDividerBrush"] = new SolidColorBrush(Color.FromRgb(0xB8, 0xBE, 0xCC));
                Resources["LeafSeparatorDotBrush"] = new SolidColorBrush(Color.FromRgb(0x90, 0x95, 0xA0));

                if (btnTheme != null) btnTheme.Content = "☀️   Açık Mod";
            }
        }

        // ============================================================
        //  OLAY YÖNETİCİLERİ
        // ============================================================
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsOverlayPanel.Visibility = Visibility.Visible;
        }

        private void CloseSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsOverlayPanel.Visibility = Visibility.Collapsed;
        }

        private void BtnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme(!isDarkMode);
            SettingsOverlayPanel.Visibility = Visibility.Collapsed;
        }

        // ============================================================
        //  BAŞLANGIÇTA ÇALIŞTIR
        // ============================================================
        private bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, false))
                {
                    if (key == null) return false;
                    return key.GetValue(StartupAppName) != null;
                }
            }
            catch { return false; }
        }

        private void SetStartup(bool enable)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, true))
                {
                    if (key == null) return;

                    if (enable)
                    {
                        string exePath = Assembly.GetExecutingAssembly().Location;
                        if (exePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                            exePath = exePath.Substring(0, exePath.Length - 4) + ".exe";
                        key.SetValue(StartupAppName, "\"" + exePath + "\"");
                    }
                    else
                    {
                        if (key.GetValue(StartupAppName) != null)
                            key.DeleteValue(StartupAppName, false);
                    }
                }
            }
            catch { ShowToast("Hata", "Başlangıç ayarı değiştirilemedi.", false); }
        }

        private void UpdateStartupButton()
        {
            if (btnStartup == null) return;
            bool enabled = IsStartupEnabled();
            btnStartup.Content = enabled
                ? "🚀   Başlangıçta Aç: Açık"
                : "🚀   Başlangıçta Aç: Kapalı";
        }

        private void BtnStartup_Click(object sender, RoutedEventArgs e)
        {
            bool current = IsStartupEnabled();
            SetStartup(!current);
            UpdateStartupButton();
            ShowToast("Başlangıç Ayarı",
                      !current ? "Program artık Windows ile birlikte başlayacak."
                               : "Program artık otomatik başlamayacak.",
                      true);
        }

        // ============================================================
        //  PENCERE MODLARI
        // ============================================================
        private static double GetIslandScale()
        {
            double screenW = SystemParameters.PrimaryScreenWidth;
            double scale = screenW / 1920.0;
            if (scale < 0.75) scale = 0.75;
            if (scale > 1.10) scale = 1.10;
            return scale;
        }

        private void HideMainUI()
        {
            MainUIGrid.Visibility = Visibility.Collapsed;
            DynamicIslandBorder.Visibility = Visibility.Collapsed;
            LeafClockBorder.Visibility = Visibility.Collapsed;

            this.ShowInTaskbar = false;
            SetToolWindow(true);
            this.Topmost = true;
            this.ResizeMode = ResizeMode.NoResize;

            if (WindowFrame != null)
            {
                WindowFrame.Background = Brushes.Transparent;
                WindowFrame.BorderBrush = Brushes.Transparent;
                WindowFrame.BorderThickness = new Thickness(0);
                WindowFrame.Effect = null;
            }
        }

        // -------------------- DİNAMİK ADA --------------------
        private void BtnDynamicIsland_Click(object sender, RoutedEventArgs e)
        {
            SettingsOverlayPanel.Visibility = Visibility.Collapsed;

            isLeafClockMode = false;
            isDynamicIsland = true;

            double screenW = SystemParameters.PrimaryScreenWidth;
            double scale = GetIslandScale();
            double islandW = IslandDesignWidth * scale;
            double islandH = IslandDesignHeight * scale;

            IslandViewbox.Width = islandW;
            IslandViewbox.Height = islandH;

            HideMainUI();

            this.Width = islandW + 20;
            this.Height = islandH;
            this.Left = (screenW - this.Width) / 2;
            this.Top = 0;

            DynamicIslandBorder.Visibility = Visibility.Visible;
        }

        private void DynamicIsland_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) SwitchToNormalMode();
        }

        // -------------------- YAPRAK SAAT / FLIP CLOCK --------------------
        private void BtnLeafClock_Click(object sender, RoutedEventArgs e)
        {
            SettingsOverlayPanel.Visibility = Visibility.Collapsed;

            isDynamicIsland = false;
            isLeafClockMode = true;

            HideMainUI();

            Rect wa = SystemParameters.WorkArea;

            this.Width = LeafClockWidth;
            this.Height = LeafClockHeight;
            this.Left = wa.Right - this.Width - 20;
            this.Top = wa.Bottom - this.Height - 20;

            UpdateLeafClock(DateTime.Now);

            LeafClockBorder.Visibility = Visibility.Visible;
        }

        // -------------------- HOVER EFEKTİ (büyüme) --------------------
        private void LeafClock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (leafHoverScale == null) return;

            var animX = new DoubleAnimation(1.04, TimeSpan.FromMilliseconds(180))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            var animY = new DoubleAnimation(1.04, TimeSpan.FromMilliseconds(180))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            leafHoverScale.BeginAnimation(ScaleTransform.ScaleXProperty, animX);
            leafHoverScale.BeginAnimation(ScaleTransform.ScaleYProperty, animY);
        }

        private void LeafClock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (leafHoverScale == null) return;

            var animX = new DoubleAnimation(1, TimeSpan.FromMilliseconds(180))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            var animY = new DoubleAnimation(1, TimeSpan.FromMilliseconds(180))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            leafHoverScale.BeginAnimation(ScaleTransform.ScaleXProperty, animX);
            leafHoverScale.BeginAnimation(ScaleTransform.ScaleYProperty, animY);
        }

        // -------------------- SÜRÜKLEME --------------------
        private void LeafClock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                SwitchToNormalMode();
                return;
            }

            isDraggingLeaf = true;
            wasDragged = false;

            leafDragStart = PointToScreen(e.GetPosition(this));
            leafStartLeft = this.Left;
            leafStartTop = this.Top;

            LeafClockBorder.CaptureMouse();
        }

        private void LeafClock_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggingLeaf) return;

            Point current = PointToScreen(e.GetPosition(this));
            double dx = current.X - leafDragStart.X;
            double dy = current.Y - leafDragStart.Y;

            if (Math.Abs(dx) > 3 || Math.Abs(dy) > 3)
                wasDragged = true;

            this.Left = leafStartLeft + dx;
            this.Top = leafStartTop + dy;
        }

        private void LeafClock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isDraggingLeaf) return;

            isDraggingLeaf = false;
            LeafClockBorder.ReleaseMouseCapture();

            if (wasDragged)
            {
                SnapLeafClockToNearestPosition();
            }
        }

        private void SnapLeafClockToNearestPosition()
        {
            Rect wa = SystemParameters.WorkArea;
            const double margin = 20;

            double[] xs = new double[3];
            xs[0] = wa.Left + margin;
            xs[1] = wa.Left + (wa.Width - this.Width) / 2;
            xs[2] = wa.Right - this.Width - margin;

            double[] ys = new double[3];
            ys[0] = wa.Top + margin;
            ys[1] = wa.Top + (wa.Height - this.Height) / 2;
            ys[2] = wa.Bottom - this.Height - margin;

            double cx = this.Left + this.Width / 2;
            double cy = this.Top + this.Height / 2;

            double bestDist = double.MaxValue;
            double targetLeft = this.Left;
            double targetTop = this.Top;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double tcx = xs[i] + this.Width / 2;
                    double tcy = ys[j] + this.Height / 2;
                    double d = (tcx - cx) * (tcx - cx) + (tcy - cy) * (tcy - cy);

                    if (d < bestDist)
                    {
                        bestDist = d;
                        targetLeft = xs[i];
                        targetTop = ys[j];
                    }
                }
            }

            AnimateWindowPosition(targetLeft, targetTop);
        }

        private void AnimateWindowPosition(double targetLeft, double targetTop)
        {
            double startLeft = this.Left;
            double startTop = this.Top;
            double dx = targetLeft - startLeft;
            double dy = targetTop - startTop;

            if (Math.Abs(dx) < 1 && Math.Abs(dy) < 1) return;

            const int totalFrames = 24;
            const int frameMs = 16;
            int frame = 0;

            var t = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(frameMs) };
            t.Tick += (s, e) =>
            {
                frame++;
                double p = (double)frame / totalFrames;

                double c1 = 1.70158;
                double c3 = c1 + 1;
                double eased = 1 + c3 * Math.Pow(p - 1, 3) + c1 * Math.Pow(p - 1, 2);

                this.Left = startLeft + dx * eased;
                this.Top = startTop + dy * eased;

                if (frame >= totalFrames)
                {
                    t.Stop();
                    this.Left = targetLeft;
                    this.Top = targetTop;
                }
            };
            t.Start();
        }

        // -------------------- NORMAL MOD --------------------
        private void SwitchToNormalMode()
        {
            isDynamicIsland = false;
            isLeafClockMode = false;

            MainUIGrid.Visibility = Visibility.Visible;
            DynamicIslandBorder.Visibility = Visibility.Collapsed;
            LeafClockBorder.Visibility = Visibility.Collapsed;

            this.Width = 500;
            this.Height = 640;
            this.ResizeMode = ResizeMode.CanResize;
            this.Topmost = false;
            this.ShowInTaskbar = true;
            SetToolWindow(false);

            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;

            if (WindowFrame != null)
            {
                WindowFrame.SetResourceReference(Border.BackgroundProperty, "BgBrush");
                WindowFrame.SetResourceReference(Border.BorderBrushProperty, "BorderBrushColor");
                WindowFrame.BorderThickness = new Thickness(1);

                DropShadowEffect shadow = new DropShadowEffect();
                shadow.Color = Colors.Black;
                shadow.BlurRadius = 26;
                shadow.ShadowDepth = 7;
                shadow.Opacity = 0.55;
                WindowFrame.Effect = shadow;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && !isDynamicIsland && !isLeafClockMode)
            {
                try { DragMove(); } catch { }
            }
        }

        // ============================================================
        //  PROGRAM DÜZENLEME
        // ============================================================
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ScheduleList.Add(new ScheduleItem
            {
                Name = "Yeni Etkinlik",
                StartString = "08:00",
                EndString = "08:40"
            });
            ShowToast("Yeni Etkinlik", "Listeye yeni bir satır eklendi.", true);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ScheduleItem selected = ScheduleListView.SelectedItem as ScheduleItem;
            if (selected != null)
            {
                string name = selected.Name;
                ScheduleList.Remove(selected);
                ShowToast("Silindi", "\"" + name + "\" programdan kaldırıldı.", true);
            }
            else
            {
                ShowToast("Silinemedi", "Lütfen önce silmek istediğiniz satırı seçin.", false);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            LoadDefaultSchedule();
            ScheduleListView.ItemsSource = ScheduleList;
            SaveScheduleToFile();
            ShowToast("Sıfırlandı", "Program varsayılan ayarlara döndü.", true);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (SaveScheduleToFile())
            {
                ShowToast("Kaydedildi", "Ders programı başarıyla diske yazıldı.", true);
                return;
            }

            if (IsRunningAsAdmin())
            {
                ShowToast("Kaydedilemedi", "Program zaten yönetici ama dosya yazılamadı.", false);
                return;
            }

            ShowAdminConfirmation();
        }

        // ============================================================
        //  YÖNETİCİ PANELİ
        // ============================================================
        private void ShowAdminConfirmation()
        {
            if (AdminConfirmPanel.Visibility == Visibility.Visible) return;
            SettingsOverlayPanel.Visibility = Visibility.Collapsed;
            ExitConfirmPanel.Visibility = Visibility.Collapsed;
            ToastPanel.Visibility = Visibility.Collapsed;
            ToastPanel.BeginAnimation(OpacityProperty, null);
            AdminConfirmPanel.Visibility = Visibility.Visible;
        }

        private void AdminConfirm_No_Click(object sender, RoutedEventArgs e)
        {
            AdminConfirmPanel.Visibility = Visibility.Collapsed;
        }

        private void AdminConfirm_Yes_Click(object sender, RoutedEventArgs e)
        {
            AdminConfirmPanel.Visibility = Visibility.Collapsed;
            RestartAsAdmin();
        }

        // ============================================================
        //  ÇIKIŞ ONAYI
        // ============================================================
        private void ShowExitConfirmation()
        {
            if (ExitConfirmPanel.Visibility == Visibility.Visible) return;
            SettingsOverlayPanel.Visibility = Visibility.Collapsed;
            AdminConfirmPanel.Visibility = Visibility.Collapsed;
            ToastPanel.Visibility = Visibility.Collapsed;
            ToastPanel.BeginAnimation(OpacityProperty, null);
            ExitConfirmPanel.Visibility = Visibility.Visible;
        }

        private void ExitConfirm_No_Click(object sender, RoutedEventArgs e)
        {
            ExitConfirmPanel.Visibility = Visibility.Collapsed;
        }

        private void ExitConfirm_Yes_Click(object sender, RoutedEventArgs e)
        {
            skipCloseConfirmation = true;
            ExitConfirmPanel.Visibility = Visibility.Collapsed;
            this.Close();
        }

        // ============================================================
        //  KAPANIŞ
        // ============================================================
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (skipCloseConfirmation || isDynamicIsland || isLeafClockMode)
            {
                SaveScheduleToFile();
                base.OnClosing(e);
                return;
            }

            e.Cancel = true;
            ShowExitConfirmation();
        }
    }
}